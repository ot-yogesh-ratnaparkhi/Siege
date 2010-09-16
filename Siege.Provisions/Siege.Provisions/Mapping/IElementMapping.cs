using System.Reflection;

namespace Siege.Provisions.Mapping
{
    public interface IElementMapping
    {
        PropertyInfo Property { get; }
    }
}