using NUnit.Framework;
using Siege.Repository.Mapping;
using Siege.Repository.Mapping.PropertyMappings;

namespace Siege.Repository.Tests.MappingTests
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
                        key.FormatAs(property =>
                                         {
                                             return property.PropertyType.IsGenericType ? 
                                                    property.DeclaringType.Name  + "ID" :
                                                    property.PropertyType.Name + "ID";
                                         });
                    });
                });

                mapper.Add<Product>();
                mapper.Add<OrderItem>();
                mapper.Add<Order>();
                mapper.Add<Customer>();
            });

            var customerMapping = map.Mappings.For<Customer>();
            var orderMapping = map.Mappings.For<Order>();
            var orderItemMapping = map.Mappings.For<OrderItem>();
            var productMapping = map.Mappings.For<Product>();

            Assert.AreEqual("Customers", customerMapping.Table.Name);
            Assert.AreEqual("siege", customerMapping.Table.Schema);
            Assert.AreEqual(4, customerMapping.SubMappings.Count);

            Assert.AreEqual("CustomerID", customerMapping.SubMappings[0].As<IdMapping>().ColumnName);
            Assert.AreEqual("DateCreated", customerMapping.SubMappings[1].As<PropertyMapping>().ColumnName);
            Assert.AreEqual("CustomerFirstName", customerMapping.SubMappings[2].As<ComponentMapping>().SubMappings[0].As<PropertyMapping>().ColumnName);
            Assert.AreEqual("CustomerLastName", customerMapping.SubMappings[2].As<ComponentMapping>().SubMappings[1].As<PropertyMapping>().ColumnName);
            Assert.AreEqual(typeof(Order), customerMapping.SubMappings[3].As<ListMapping>().Type);
            Assert.AreEqual("CustomerID", customerMapping.SubMappings[3].As<ListMapping>().ForeignRelationshipMapping.ColumnName);

            Assert.AreEqual("Orders", orderMapping.Table.Name);
            Assert.AreEqual("siege", orderMapping.Table.Schema);
            Assert.AreEqual(3, orderMapping.SubMappings.Count);

            Assert.AreEqual("OrderID", orderMapping.SubMappings[0].As<IdMapping>().ColumnName);
            Assert.AreEqual("DateCreated", orderMapping.SubMappings[1].As<PropertyMapping>().ColumnName);
            Assert.AreEqual(typeof(OrderItem), orderMapping.SubMappings[2].As<ListMapping>().Type);
            Assert.AreEqual("OrderID", orderMapping.SubMappings[2].As<ListMapping>().ForeignRelationshipMapping.ColumnName);

            Assert.AreEqual("OrderItems", orderItemMapping.Table.Name);
            Assert.AreEqual("siege", orderItemMapping.Table.Schema);
            Assert.AreEqual(2, orderItemMapping.SubMappings.Count);

            Assert.AreEqual("OrderItemID", orderItemMapping.SubMappings[0].As<IdMapping>().ColumnName);
            Assert.AreEqual("ProductID", orderItemMapping.SubMappings[1].As<ForeignRelationshipMapping>().ColumnName);

            Assert.AreEqual("Products", productMapping.Table.Name);
            Assert.AreEqual("siege", productMapping.Table.Schema);
            Assert.AreEqual(3, productMapping.SubMappings.Count);

            Assert.AreEqual("ProductID", productMapping.SubMappings[0].As<IdMapping>().ColumnName);
            Assert.AreEqual("Price", productMapping.SubMappings[1].As<PropertyMapping>().ColumnName);
            Assert.AreEqual("Name", productMapping.SubMappings[2].As<PropertyMapping>().ColumnName);
        }

        [Test, Ignore]
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

            map.Override<Customer>(customerMap => customerMap.MapID(customer => customer.ID, "ID"));
        }
    }
}