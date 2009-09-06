namespace Siege.ServiceLocation
{
    public class KeyBasedImplementationUseCase<TBaseType> : DefaultImplementationUseCase<TBaseType>
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