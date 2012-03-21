namespace Siege.Security.Entities
{
    public abstract class SecurityEntity<TID>
    {
        public virtual TID ID { get; set; }
    }
}