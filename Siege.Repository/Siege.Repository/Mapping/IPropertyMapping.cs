namespace Siege.Repository.Mapping
{
    public interface IPropertyMapping : IElementMapping
    {
        string ColumnName { get; }
    }
}