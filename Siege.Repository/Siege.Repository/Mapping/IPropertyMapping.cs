using System.Reflection;

namespace Siege.Repository.Mapping
{
    public interface IPropertyMapping : IElementMapping
    {
        string ColumnName { get; }
        PropertyInfo Property { get;  }
    }
}