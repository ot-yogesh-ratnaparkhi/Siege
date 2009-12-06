using System;

namespace Siege.ServiceLocation
{
    public class KeyBasedInstanceUseCase<TBaseService> : InstanceUseCase<TBaseService>, IKeyBasedUseCase<TBaseService>
    {
        private readonly string key;

        public KeyBasedInstanceUseCase(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return key; }
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (IKeyBasedUseCaseBinding<>);
        }
    }
}