namespace Siege.ServiceLocation
{
    public interface IActivationRule
    {
        bool Evaluate(object context);
    }
}
