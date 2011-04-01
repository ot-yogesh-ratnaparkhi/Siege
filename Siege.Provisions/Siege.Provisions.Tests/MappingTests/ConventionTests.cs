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
                    convention.WithSchema("test");
                    convention.WithSuffix("s");
                    
                    convention.IdentifyEntitiesWith<EntityIdentifier>();
                    convention.IdentifyComponentsWith<ComponentIdentifier>();
                    convention.IdentifyIDsWith<IdIdentifier>();
                    
                    convention.ForComponents(component =>
                    {
                        component.PrefixWith((type, propertyName) => type.Name + "_");
                        component.SuffixWith((type, propertyName) => "_" + type.Name);
                    });

                    convention.ForForeignKeys(key => {});
                });

                mapper.Add<Customer>();
                mapper.Add<Order>();
                mapper.Add<OrderItem>();
                mapper.Add<Product>();
            });

            Assert.AreEqual("Customers", map.Mappings.For<Customer>().Table.Name);
            Assert.AreEqual("test", map.Mappings.For<Customer>().Table.Schema);
        }

        [Test]
        public void ShouldAllowOverriding()
        {
            map.Create(mapper =>
            {
                mapper.UseConvention(convention =>
                {
                    convention.WithSchema("test");
                    convention.WithSuffix("s");
                });

                mapper.Add<Customer>();

            });
        }
    }
}