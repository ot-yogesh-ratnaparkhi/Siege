using System.Collections;
using System.Collections.Generic;
using Ninject;
using Ninject.Parameters;
using Siege.ServiceLocation;

namespace Siege.Container.NinjectAdapter
{
    public class NinjectFactory<TBaseType> : IGenericFactory<TBaseType>
    {
        private readonly IContextualServiceLocator locator;
        private readonly IKernel outerKernel;
        private readonly List<IConditionalUseCase<TBaseType>> conditionalUseCases = new List<IConditionalUseCase<TBaseType>>();
        private readonly List<IDefaultUseCase<TBaseType>> defaultCases = new List<IDefaultUseCase<TBaseType>>();

        public NinjectFactory(IContextualServiceLocator locator, IKernel outerKernel)
        {
            this.locator = locator;
            this.outerKernel = outerKernel;
        }

        public List<IConditionalUseCase<TBaseType>> ConditionalUseCases
        {
            get { return conditionalUseCases; }
        }

        public List<IDefaultUseCase<TBaseType>> DefaultUseCases
        {
            get { return defaultCases; }
        }

        public void AddCase(IConditionalUseCase<TBaseType> useCase)
        {
            ConditionalUseCases.Add(useCase);
        }

        public void AddCase(IDefaultUseCase<TBaseType> useCase)
        {
            DefaultUseCases.Add(useCase);
        }

        public TBaseType Build(IDictionary constructorArguments)
        {
            foreach (IConditionalUseCase<TBaseType> useCase in ConditionalUseCases)
            {
                TBaseType result = useCase.Resolve(locator, locator.Context, constructorArguments);

                if (!Equals(result, default(TBaseType))) return result;
            }

            foreach (IDefaultUseCase<TBaseType> useCase in defaultCases)
            {
                if (constructorArguments == null || constructorArguments.Count == 0) return (TBaseType)outerKernel.Get(useCase.GetBoundType());

                List<ConstructorArgument> args = new List<ConstructorArgument>();

                foreach (string key in constructorArguments.Keys)
                {
                    ConstructorArgument argument = new ConstructorArgument(key, constructorArguments[key]);
                    args.Add(argument);
                }

                return (TBaseType)outerKernel.Get(useCase.GetBoundType(), args.ToArray());
            }

            return locator.GetInstance<TBaseType>();
        }
    }
}
