namespace Siege.ServiceLocation
{
    public class Context<T> : IContext<T>
    {
        public T Value { get; set; }

        public object GetValue()
        {
            return Value;
        }
    }

    public class With
    {
        public static Context<T> Context<T>(T context)
        {
            return new Context<T> {Value = context};
        }
    }
}
