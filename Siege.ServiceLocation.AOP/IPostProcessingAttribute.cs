namespace Siege.ServiceLocation.AOP
{
    public interface IPostProcessingAttribute : IAopAttribute
    {
        void Process();
    }
}