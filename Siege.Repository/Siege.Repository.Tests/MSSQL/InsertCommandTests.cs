using System;
using NUnit.Framework;
using Siege.Repository.MSSQL;
using Siege.Repository.Mapping;
using Siege.Repository.Tests.MappingTests;

namespace Siege.Repository.Tests.MSSQL
{
    [TestFixture]
    public class InsertCommandTests
    {
        private ICommand insertCommand;
        private DomainMap domainMap;

        [SetUp]
        public void SetUp()
        {
            domainMap = new DomainMap();
            domainMap.Create(mapper =>
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

                    convention.ForPrimaryKeys(key => key.FormatAs(type => type.Name + "ID"));

                    convention.ForForeignKeys(key =>
                    {
                        key.FormatAs(property => 
                                    property.PropertyType.IsGenericType ?
                                    property.DeclaringType.Name + "ID" :
                                    property.PropertyType.Name + "ID");
                    });
                });

                mapper.Add<Product>();
                mapper.Add<OrderItem>();
                mapper.Add<Order>();
                mapper.Add<Customer>();
            });

            insertCommand = new InsertCommand(domainMap);
        }

        [Test]
        public void ShouldInsertObjectWithoutRelationships()
        {
            var product = new Product {Name = "Test Product", Price = 1M};

            var sql = insertCommand.GenerateFor(product);

            Assert.AreEqual(1, sql.Parameters[0].Value);
            Assert.AreEqual("Test Product", sql.Parameters[1].Value);
            Assert.AreEqual("@Products_ProductID", sql.Parameters[2].ParameterName);
            Assert.AreEqual("INSERT INTO siege.Products ([Price], [Name]) VALUES (@Products_Price, @Products_Name); SELECT @Products_ProductID = Scope_Identity(); SELECT @Products_ProductID;", sql.CommandText);
        }

        [Test]
        public void ShouldInsertObjectWithComponents()
        {
            var customer = new Customer { DateCreated = DateTime.Today, Name = new Name { First = "Test", Last = "Last" } };

            var sql = insertCommand.GenerateFor(customer);

            Assert.AreEqual(DateTime.Today, sql.Parameters[0].Value);
            Assert.AreEqual("Test", sql.Parameters[1].Value);
            Assert.AreEqual("Last", sql.Parameters[2].Value);
            Assert.AreEqual("@Customers_CustomerID", sql.Parameters[3].ParameterName);

            Assert.AreEqual("INSERT INTO siege.Customers ([DateCreated], [CustomerFirstName], [CustomerLastName]) VALUES (@Customers_DateCreated, @Customers_CustomerFirstName, @Customers_CustomerLastName); SELECT @Customers_CustomerID = Scope_Identity(); SELECT @Customers_CustomerID;", sql.CommandText);
        }

        [Test]
        public void ShouldInsertObjectWithNewEntityReference()
        {
            var orderItem = new OrderItem {Product = new Product {Name = "Test Product", Price = 1M}};

            var sql = insertCommand.GenerateFor(orderItem);

            Assert.AreEqual(4, sql.Parameters.Count);
            Assert.AreEqual(1, sql.Parameters[0].Value);
            Assert.AreEqual("Test Product", sql.Parameters[1].Value);
            Assert.AreEqual("@Products_ProductID", sql.Parameters[2].ParameterName);
            Assert.AreEqual("@OrderItems_OrderItemID", sql.Parameters[3].ParameterName);
            Assert.AreEqual("INSERT INTO siege.Products ([Price], [Name]) VALUES (@Products_Price, @Products_Name); SELECT @Products_ProductID = Scope_Identity(); INSERT INTO siege.OrderItems ([ProductID]) VALUES (@Products_ProductID); SELECT @OrderItems_OrderItemID = Scope_Identity(); SELECT @Products_ProductID, @OrderItems_OrderItemID;", sql.CommandText);
        }
    }
}