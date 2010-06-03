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

using System.Collections.Generic;
using Siege.ServiceLocation.Resolution;

namespace Siege.ServiceLocation.ExtensionMethods
{
    public static class ResolutionArgumentExtensions
    {
        public static TResult[] OfType<TResult, TInput>(this List<TInput> input) where TResult : TInput
        {
            int inputCount = 0;
            for (int i = 0; i < input.Count; i++)
            {
                TInput argument = input[i];
                if (argument is TResult) inputCount++;
            }

            var output = new TResult[inputCount];
            int outputIndex = 0;

            for (int i = 0; i < input.Count; i++)
            {
                TInput argument = input[i];
                if (argument is TResult)
                {
                    output[outputIndex] = (TResult)argument;
                    outputIndex++;
                }
            }

            return output;
        }

        public static TResult[] OfType<TResult, TInput>(this TInput[] input) where TResult : TInput
        {
            int inputCount = 0;
            for (int i = 0; i < input.Length; i++)
            {
                TInput argument = input[i];
                if (argument is TResult) inputCount++;
            }

            var output = new TResult[inputCount];
            int outputCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                TInput argument = input[i];
                if (argument is ConstructorParameter)
                {
                    output[outputCount] = (TResult)argument;
                    outputCount++;
                }
            }

            return output;
        }
    }
}