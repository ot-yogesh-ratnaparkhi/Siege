using System.Collections.Generic;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation.Extensions.Conventions
{
    public interface IConvention
    {
        List<IUseCase> Build();
    }
}