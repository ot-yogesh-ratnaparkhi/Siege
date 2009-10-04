namespace Siege.ServiceLocation.TypeGeneration
{
    public interface IPreProcessingAttribute : IAopAttribute
    {
        void Process();
    }
}