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

namespace Siege.Foundry.Actions
{
    public class AddFieldAction : ITypeGenerationAction
    {
        private readonly Func<BuilderBundle> bundle;
        private readonly Func<string> fieldName;
        private readonly Func<BuilderBundle> fieldBundle;
        private readonly Func<Type> fieldType;
        private FieldBuilder fieldBuilder;

        public FieldBuilder Field
        {
            get { return fieldBuilder; }
        }

        public AddFieldAction(Func<BuilderBundle> bundle, Func<string> fieldName, Func<Type> fieldType)
        {
            this.bundle = bundle;
            this.fieldName = fieldName;
            this.fieldType = fieldType;
        }

        public AddFieldAction(Func<BuilderBundle> bundle, Func<string> fieldName, Func<BuilderBundle> fieldBundle)
        {
            this.bundle = bundle;
            this.fieldName = fieldName;
            this.fieldBundle = fieldBundle;
        }

        public void Execute()
        {
            if(fieldType != null)
            {
                fieldBuilder = bundle().TypeBuilder.DefineField(fieldName(), fieldType(), FieldAttributes.Family);
            }
            else if(fieldBundle != null)
            {
                fieldBuilder = bundle().TypeBuilder.DefineField(fieldName(), fieldBundle().TypeBuilder, FieldAttributes.Family);
            }
        }
    }
}