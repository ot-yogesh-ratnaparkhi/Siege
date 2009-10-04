namespace Siege.ServiceLocation.TypeGeneration
{
    public interface IPostProcessingAttribute : IAopAttribute
    {
        void Process();
    }
}