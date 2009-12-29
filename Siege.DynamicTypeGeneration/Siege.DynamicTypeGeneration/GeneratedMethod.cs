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
using System.Reflection.Emit;
using Siege.DynamicTypeGeneration.Actions;

namespace Siege.DynamicTypeGeneration
{
    public class Local
    {
        public object Entry { get; set; }
        public int Index { get; set; }
    }

    public class GeneratedMethod
    {
        private readonly MethodBuilderBundle bundle;
        private IList<ITypeGenerationAction> actions;
        private List<Local> locals = new List<Local>();
        internal int LocalCount { get; set; }
        internal MethodBuilderBundle MethodBuilder { get { return bundle; } }
        public List<Local> Locals
        {
            get { return locals; }
        }


        public GeneratedMethod(MethodBuilderBundle bundle, IList<ITypeGenerationAction> actions)
        {
            this.bundle = bundle;
            this.actions = actions;
        }

        public int Instantiate(Type type, Type[] constructorArguments)
        {
            actions.Add(new InstantiationAction(bundle, type, constructorArguments));

            return LocalCount - 1;
        }

        public CallAction Call(MethodInfo method)
        {
            var action = new CallAction(bundle, method, actions, this);
            actions.Add(action);

            return action;
        }

        public CompletedMethod ReturnFrom(Func<MethodBuilder> method)
        {
            actions.Add(new ReturnAction(bundle, method, this));

            return new CompletedMethod(bundle, actions);
        }

        public CallBaseAction CallBase(MethodInfo method, Type baseType)
        {
            if (method.ReturnType != typeof(void)) AddLocal(method.ReturnType);

            var action = new CallBaseAction(bundle, method, actions, baseType, this, LocalCount - 1);
            actions.Add(action);

            return action;
        }

        public CreateFuncAction UsingFunc(Type type)
        {
            var action = new CreateFuncAction(bundle, type, actions, this);
            return action;
        }

        public FieldAssignmentAction Assign(FieldInfo value)
        {
            var action = new FieldAssignmentAction(bundle, value);
            actions.Add(action);

            return action;
        }

        internal void AddLocal(Type item)
        {
            LocalCount++;
            var local = new AddLocalAction(bundle, item);

            Locals.Add(new Local { Entry = item, Index = LocalCount });
            actions.Add(local);
        }

        internal void AddLocal(Func<MethodInfo> item)
        {
            LocalCount++;
            var local = new AddLocalAction(bundle, item().ReturnType);

            Locals.Add(new Local { Entry = item, Index = LocalCount });
            actions.Add(local);
        }
    }
}