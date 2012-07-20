namespace Siege.Repository.Mapping.Conventions.Identifiers
{
    public interface IIdentifier<in T>
    {
        bool Matches(T item);
    }
}