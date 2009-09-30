namespace Siege.ServiceLocation
{
    public class KeyBasedInstanceUseCase<TBaseType> : InstanceUseCase<TBaseType>, IKeyBasedUseCase<TBaseType>
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
    }
}