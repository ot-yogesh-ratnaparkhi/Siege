using System.Collections.Generic;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Extensions.Conventions
{
    public abstract class Using
    {
        public static List<IUseCase> Convention<TConvention>() where TConvention : IConvention, new()
        {
            return new TConvention().Build();
        }

        public static List<IUseCase> Convention(IConvention convention)
        {
            return convention.Build();
        }
    }
}