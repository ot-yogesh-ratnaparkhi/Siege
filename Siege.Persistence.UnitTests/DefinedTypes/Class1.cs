using Siege.QueryAbstractions;
using Siege.QueryAbstractions.Queries;

namespace Siege.Persistence.UnitTests.DefinedTypes
{
    public class Class1
    {
        public void Test()
        {
            //QuerySyntax.InitializeWith(new NHibernateAdapter());

            IQuery query = new SelectQuery<Customer>()
                .Join<Order>()
                .Join<Order, OrderItem>((order, orderItem) => order.Id == orderItem.Id)
                .Where<OrderItem>(orderItem => orderItem.Id == 3);
        }
    }


    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
    }
}