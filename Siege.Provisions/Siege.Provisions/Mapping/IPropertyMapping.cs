namespace Siege.Provisions.Mapping
{
    public interface IPropertyMapping : IElementMapping
    {
        string ColumnName { get; }
    }
}