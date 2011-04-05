using NUnit.Framework;
using Siege.Provisions.Mapping;

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
                mapper.UseConvention(convention =>
                {
                    convention.WithSchema("siege");
                    convention.WithSuffix("s");
                    
                    convention.IdentifyEntitiesWith<EntityIdentifier>();
                    convention.IdentifyComponentsWith<ComponentIdentifier>();
                    convention.IdentifyIDsWith<IdIdentifier>();
                    
                    convention.ForComponents(component =>
                    {
                        component.PrefixWith((parentType, objectType, propertyName) => parentType.Name);
                        component.SuffixWith((parentType, objectType, propertyName) => objectType.Name);
                    });

                    convention.ForPrimaryKeys(key =>
                    {
                        key.FormatAs(type => type.Name + "ID");
                    });

                    convention.ForForeignKeys(key => 
                    { 
                        key.FormatAs(property => property.Name + "ID");
                    });
                });

                mapper.Add<Customer>();
                mapper.Add<Order>();
                mapper.Add<OrderItem>();
                mapper.Add<Product>();
            });

            Assert.AreEqual("Customers", map.Mappings.For<Customer>().Table.Name);
            Assert.AreEqual("siege", map.Mappings.For<Customer>().Table.Schema);
        }

        [Test]
        public void ShouldAllowOverriding()
        {
            map.Create(mapper =>
            {
                mapper.UseConvention(convention =>
                {
                    convention.WithSchema("siege");
                    convention.WithSuffix("s");
                });

                mapper.Add<Customer>();

            });
        }
    }
}