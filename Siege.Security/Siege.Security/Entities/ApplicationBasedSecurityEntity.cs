namespace Siege.Security.Entities
{
    public abstract class ConsumerBasedSecurityEntity : SecurityEntity
    {
        public virtual Consumer Consumer { get; set; }
    }
}