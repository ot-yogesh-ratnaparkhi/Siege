namespace Siege.ServiceLocation.AOP
{
    public interface IPreProcessingAttribute : IAopAttribute
    {
        void Process();
    }
}