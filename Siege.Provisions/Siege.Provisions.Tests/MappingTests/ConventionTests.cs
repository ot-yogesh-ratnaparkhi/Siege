using NUnit.Framework;
using Siege.Provisions.Mapping;
using Siege.Provisions.Mapping.Conventions;
using Siege.Provisions.Mapping.PropertyMappings;

namespace Siege.Provisions.Tests.MappingTests
{
    [TestFixture]
    public class ConventionTests
    {
        private DomainMap map;

        [SetUp]
        public void SetUp()
        {
            map = new DomainMap();
        }

        [Test]
        public void ShouldAutoMap()
        {
            map.Create(mapper =>
            {
                mapper.UseConvention<ClassConvention>(convention =>
                {
                    convention.WithSchema("test");
                    convention.WithSuffix("s");
                    convention.ForComponents(component =>
                    {
                        component.PrefixWith((type, propertyName) => type.Name + "_");
                        component.SuffixWith((type, propertyName) => "_" + type.Name);
                    });
                });

                mapper.Add<Customer>();
            });

            Assert.AreEqual("Customers", map.Mappings.For<Customer>().Table.Name);
            Assert.AreEqual("test", map.Mappings.For<Customer>().Table.Schema);

            foreach(IElementMapping mapping in map.Mappings.For<Customer>().SubMappings)
            {
                Assert.AreEqual(mapping.As<PropertyMapping>().ColumnName, mapping.As<PropertyMapping>().Property.Name);   
            }
        }

        [Test]
        public void ShouldAllowOverriding()
        {
            map.Create(mapper =>
            {
                mapper.UseConvention<ClassConvention>(convention =>
                {
                    convention.WithSchema("test");
                    convention.WithSuffix("s");
                });

                mapper.Add<Customer>();

            });
        }
    }

    public static class ElementMappingExtensions
    {
        public static TType As<TType>(this IElementMapping mapping) where TType : IElementMapping
        {
            return (TType) mapping;
        }
    }
}