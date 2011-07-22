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
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Siege.TypeGenerator
{
    public class TypeGenerator
    {
        private static readonly AssemblyBuilder assemblyBuilder;
        private static readonly ModuleBuilder module;

        static TypeGenerator()
        {
            var assemblyName = new AssemblyName { Name = "Siege.DynamicTypes" };
            AppDomain thisDomain = Thread.GetDomain();

            assemblyBuilder = thisDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            module = assemblyBuilder.DefineDynamicModule(assemblyBuilder.GetName().Name,
                                                         assemblyBuilder.GetName().Name +
                                                         ".dll");
        }

        public Type CreateType(Action<TypeGenerationContext> nestedClosure)
        {

            var bundle = new BuilderBundle
                                       {
                                           ModuleBuilder = module
                                       };

            var context = new TypeGenerationContext(this, () => bundle, nestedClosure);
            
            var type = new GeneratedType(bundle, context);

            var returnType = type.Create();

            return returnType;
        }

        public void Save()
        {
            assemblyBuilder.Save(assemblyBuilder.GetName().Name + ".dll");
        }
    }
}
