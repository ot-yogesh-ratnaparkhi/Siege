using System.Collections;

namespace Siege.ServiceLocation
{
    public class KeyBasedUseCase<TBaseService> : GenericUseCase<TBaseService>, IKeyBasedUseCase<TBaseService>
    {
        private readonly string key;

        public KeyBasedUseCase(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return key; }
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new KeyBasedActivationStrategy(Key);
        }

        private class KeyBasedActivationStrategy : IActivationStrategy
        {
            private readonly string key;

            public KeyBasedActivationStrategy(string key)
            {
                this.key = key;
            }

            public TBaseService Resolve(IMinimalServiceLocator locator, IDictionary constructorArguments)
            {
                return locator.GetInstance<TBaseService>(key, constructorArguments);
            }
        }
    }
}