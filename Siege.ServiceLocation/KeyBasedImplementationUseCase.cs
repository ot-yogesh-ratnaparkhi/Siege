namespace Siege.ServiceLocation
{
    public class KeyBasedImplementationUseCase<TBaseType> : ImplementationUseCase<TBaseType>, IKeyBasedUseCase<TBaseType>
    {
        private readonly string key;

        public KeyBasedImplementationUseCase(string key)
        {
            this.key = key;
        }

        public string Key
        {
            get { return key; }
        }
    }
}