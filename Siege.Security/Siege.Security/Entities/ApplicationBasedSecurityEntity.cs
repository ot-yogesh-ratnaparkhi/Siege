namespace Siege.Security.Entities
{
    public abstract class ApplicationBasedSecurityEntity<TID> : SecurityEntity<TID>
    {
        public virtual Application Application { get; set; }
    }
}