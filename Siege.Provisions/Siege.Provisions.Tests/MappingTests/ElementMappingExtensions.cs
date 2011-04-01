using Siege.Provisions.Mapping;

namespace Siege.Provisions.Tests.MappingTests
{
    public static class ElementMappingExtensions
    {
        public static TType As<TType>(this IElementMapping mapping) where TType : IElementMapping
        {
            return (TType) mapping;
        }
    }
}