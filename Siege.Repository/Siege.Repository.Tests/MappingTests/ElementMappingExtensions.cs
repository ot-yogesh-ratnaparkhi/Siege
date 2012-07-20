using Siege.Repository.Mapping;

namespace Siege.Repository.Tests.MappingTests
{
    public static class ElementMappingExtensions
    {
        public static TType As<TType>(this IElementMapping mapping) where TType : IElementMapping
        {
            return (TType) mapping;
        }
    }
}