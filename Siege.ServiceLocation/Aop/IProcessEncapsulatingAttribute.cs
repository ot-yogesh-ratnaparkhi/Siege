using System;
using Siege.ServiceLocation.TypeGeneration;

namespace Siege.ServiceLocation.Aop
{
    public interface IProcessEncapsulatingAttribute : IAopAttribute
    {
        TResponseType Process<TResponseType>(Func<TResponseType> func);
        void Process(Action action);
    }
}