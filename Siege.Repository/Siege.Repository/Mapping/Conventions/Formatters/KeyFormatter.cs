using System;

namespace Siege.Repository.Mapping.Conventions.Formatters
{
    public class Formatter<T>
    {
        private Func<T, string> formatter;

        public void FormatAs(Func<T, string> formatter)
        {
            this.formatter = formatter;
        }

        public string Format(T property)
        {
            return this.formatter(property);
        }
    }
}