using System;

namespace Siege.Persistence.UnitTests.AnonymousTypes
{
    public class Class1
    {
        public void Test()
        {
            IQuery query = From<Customer>
                .Select(customer => customer.FirstName, customer => customer.LastName)
                .Where(customer => customer.ID == 3);

            var result = query.Execute();
        }
    }

    public class From<T>
    {
        public static SelectStatement<T> Select(params Func<T, object>[] items)
        {
            return new SelectStatement<T>(items);
        }
    }

    public class SelectStatement<T>
    {
        private Func<T, object>[] selections;

        public SelectStatement(Func<T, object>[] selections)
        {
            this.selections = selections;
        }

        public IQuery Where(Func<T, bool> condition)
        {
            return new SelectQuery();
        }
    }

    public interface IQuery
    {
        object Execute();
    }

    public class SelectQuery : IQuery
    {
        public object Execute()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Customer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}