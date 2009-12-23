namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Classes
{
    public interface ICoffee
    {
        string Name { get; }
        decimal Total { get; }
    }

    public class Coffee : ICoffee
    {
        public string Name
        {
            get { return "Coffee"; }
        }

        public decimal Total
        {
            get { return 0.75m; }
        }
    }
}
