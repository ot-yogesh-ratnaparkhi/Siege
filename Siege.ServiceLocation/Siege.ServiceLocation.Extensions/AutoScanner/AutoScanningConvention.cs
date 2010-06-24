using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Siege.ServiceLocation.Extensions.Conventions;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Extensions.AutoScanner
{
    public class AutoScanningConvention : IConvention
    {
        private Assembly assembly;
        private readonly List<Type> baseTypes = new List<Type>();

        protected void FromAssemblyContaining<TType>()
        {
            assembly = typeof (TType).Assembly;
        }

        protected void ForTypesImplementing<TType>()
        {
            baseTypes.Add(typeof(TType));
        }

        public List<IUseCase> Build()
        {
            var useCases = new List<IUseCase>();

            foreach(Type type in assembly.GetExportedTypes())
            {
                if(baseTypes.Count > 0)
                {
                    useCases.AddRange((from baseType in baseTypes
                                       where baseType.IsAssignableFrom(type) && !type.IsInterface
                                       select new AutoScannedUseCase(baseType, type)).Cast<IUseCase>());
                }
                else
                {
                    useCases.Add(new AutoScannedUseCase(type.BaseType, type));
                }
            }

            return useCases;
        }
    }
}