using System;

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

        public override Type GetUseCaseBindingType()
        {
            return typeof(IKeyBasedUseCaseBinding<>);
        }
    }
}