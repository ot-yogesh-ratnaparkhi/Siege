/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class Local
    {
        public object Entry { get; set; }
        public int Index { get; set; }
    }

    public class LocalIndexer : ILocalIndexer
    {
        private readonly int localIndex;

        public LocalIndexer(int localIndex)
        {
            this.localIndex = localIndex;
        }

        public Func<int> LocalIndex
        {
            get { return () => localIndex; }
        }
    }

    public class GeneratedMethod
    {
        private readonly Func<MethodBuilderBundle> bundle;
        private IList<ITypeGenerationAction> actions;
        private List<Local> locals = new List<Local>();
        internal Func<int> LocalCount { get { return () => locals.Count;} }
        internal Func<MethodBuilderBundle> MethodBuilder { get { return bundle; } }
        
        public Func<List<Local>> Locals
        {
            get { return () => locals; }
        }

        public GeneratedMethod(Func<MethodBuilderBundle> bundle, IList<ITypeGenerationAction> actions)
        {
            this.bundle = bundle;
            this.actions = actions;
        }

        public int Instantiate(Type type, Type[] constructorArguments)
        {
            actions.Add(new InstantiationAction(bundle, type, constructorArguments));

            return LocalCount() - 1;
        }

        public GeneratedVariable Call(Func<MethodInfo> method, Func<List<IGeneratedParameter>> parameters)
        {
            var action = new CallAction(bundle, method, actions, this, parameters);
            actions.Add(action);

            return new GeneratedVariable(action.LocalIndex, actions, this);
        }

        public void ReturnFrom()
        {
            Return(() => new LocalIndexer(LocalCount() - 1));
        }

        public void Return(Func<ILocalIndexer> localIndex)
        {
            actions.Add(new ReturnAction(bundle, localIndex));
        }

        public void TargettingSelf()
        {
            actions.Add(new LoadThisAction(this));
        }

        public GeneratedVariable CallBase(MethodInfo method, Type baseType)
        {
            var action = new CallBaseAction(bundle, () => method, actions, baseType, this);
            actions.Add(action);

            return new GeneratedVariable(action.LocalIndex, actions, this);
        }

        public void CreateDelegate(Type type)
        {
            var action = new CreateDelegateAction(bundle, type);

            AddLocal(action.DelegateType);
            actions.Add(action);
        }

        internal void AddLocal(Type item)
        {
            var local = new AddLocalAction(bundle, item);

            locals.Add(new Local { Entry = item, Index = LocalCount() });
            actions.Add(local);
        }

        internal void AddLocal(Func<MethodInfo> item)
        {
            var local = new AddLocalAction(bundle, item().ReturnType);

            locals.Add(new Local { Entry = item, Index = LocalCount() });
            actions.Add(local);
        }

        internal void AddLocal(Local item)
        {
            locals.Add(item);
        }

        public void Target(Func<GeneratedMethod> method)
        {
            actions.Add(new LoadFunctionAction(() => this, method));
        }

        public void Target(GeneratedVariable variable, MethodInfo method)
        {
            actions.Add(new LoadVariableFunctionAction(() => this, variable.LocalIndex(), method));
        }
    }
}