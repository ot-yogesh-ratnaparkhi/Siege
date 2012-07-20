using System;

namespace Siege.Repository.Mapping.Conventions.Identifiers
{
    public class InlineIdentifier<T> : IIdentifier<T>
    {
        private readonly Predicate<T> matcher;

        public InlineIdentifier(Predicate<T> matcher)
        {
            this.matcher = matcher;
        }

        public bool Matches(T item)
        {
            return matcher(item);
        }
    }
}