using System;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.Provisions.Mapping;
using Siege.Provisions.Mapping.PropertyMappings;

namespace Siege.Provisions.Tests.MappingTests
{
    [TestFixture]
    public class DomainMapTests
    {
        private DomainMap map;

        [SetUp]
        public void SetUp()
        {
            map = new DomainMap();
        }

        [Test]
        public void ShouldMapIdWithoutName()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapID(customer => customer.ID);
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);
            
            IElementMapping elementMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<PropertyMapping<Customer, int>>(elementMapping);
            Assert.AreEqual("ID", ((PropertyMapping<Customer, int>)elementMapping).ColumnName);
            Assert.AreEqual("ID", ((PropertyMapping<Customer, int>)elementMapping).Property.Name);
        }

        [Test]
        public void ShouldMapIdWithName()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapID(customer => customer.ID, "CustomerID");
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);
            IElementMapping elementMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<PropertyMapping<Customer, int>>(elementMapping);
            Assert.AreEqual("CustomerID", ((PropertyMapping<Customer, int>)elementMapping).ColumnName);
            Assert.AreEqual("ID", ((PropertyMapping<Customer, int>)elementMapping).Property.Name);
        }

        [Test]
        public void ShouldMapTableByName()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.ToTable("Customers");
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual("Customers", map.Mappings.For<Customer>().Table.Name);
        }

        [Test]
        public void ShouldMapPropertyWithoutName()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapProperty(customer => customer.DateCreated);
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);
            IElementMapping elementMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<PropertyMapping<Customer, DateTime>>(elementMapping);
            Assert.AreEqual("DateCreated", ((PropertyMapping<Customer, DateTime>)elementMapping).ColumnName);
            Assert.AreEqual("DateCreated", ((PropertyMapping<Customer, DateTime>)elementMapping).Property.Name);
        }

        [Test]
        public void ShouldMapPropertyWithName()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapProperty(customer => customer.DateCreated, "CreatedOn");
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);
            IElementMapping elementMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<PropertyMapping<Customer, DateTime>>(elementMapping);
            Assert.AreEqual("CreatedOn", ((PropertyMapping<Customer, DateTime>)elementMapping).ColumnName);
            Assert.AreEqual("DateCreated", ((PropertyMapping<Customer, DateTime>)elementMapping).Property.Name);
        }

        [Test]
        public void ShouldMapComponent()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapComponent(customer => customer.Name, nameMap => 
                    {
                        nameMap.MapProperty(name => name.First, "FirstName");
                        nameMap.MapProperty(name => name.Last, "LastName");
                    });
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);

            IElementMapping nameMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<ComponentMapping<Customer, Name>>(nameMapping);
            Assert.AreEqual(2, ((ComponentMapping<Customer, Name>)nameMapping).SubMappings.Count);

            Assert.AreEqual("Name", ((ComponentMapping<Customer, Name>)nameMapping).Property.Name);

            IElementMapping firstNameMapping = ((ComponentMapping<Customer,Name>)nameMapping).SubMappings[0];

            Assert.IsInstanceOf<PropertyMapping<Name, string>>(firstNameMapping);
            Assert.AreEqual("FirstName", ((PropertyMapping<Name, string>)firstNameMapping).ColumnName);

            Assert.AreEqual("First", ((PropertyMapping<Name, string>)firstNameMapping).Property.Name);

            IElementMapping lastNameMapping = ((ComponentMapping<Customer, Name>)nameMapping).SubMappings[1];

            Assert.IsInstanceOf<PropertyMapping<Name, string>>(lastNameMapping);
            Assert.AreEqual("LastName", ((PropertyMapping<Name, string>)lastNameMapping).ColumnName);

            Assert.AreEqual("Last", ((PropertyMapping<Name, string>)lastNameMapping).Property.Name);
        }

        [Test]
        public void ShouldMapList()
        {
            map.Create(mapper =>
            {
                mapper.Add<Customer>(customerMap =>
                {
                    customerMap.MapList(customer => customer.Orders);
                });
            });

            Assert.AreEqual(1, map.Mappings.Count);
            Assert.AreEqual(1, map.Mappings.For<Customer>().SubMappings.Count);
            IElementMapping elementMapping = map.Mappings.For<Customer>().SubMappings[0];

            Assert.IsInstanceOf<ListMapping<Customer, List<Order>>>(elementMapping);
            Assert.AreEqual("Orders", ((ListMapping<Customer, List<Order>>)elementMapping).Property.Name);
        }
    }

    public class Customer
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public Name Name { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int ID { get; set; }
        public DateTime DateCreated { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class Name
    {
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class OrderItem
    {
        public int ID { get; set; }
        public Product Product { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}