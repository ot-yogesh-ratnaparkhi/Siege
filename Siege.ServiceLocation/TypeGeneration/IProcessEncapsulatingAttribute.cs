using System;

namespace Siege.ServiceLocation.TypeGeneration
{
    public interface IProcessEncapsulatingAttribute : IAopAttribute
    {
        TResponseType Process<TResponseType>(Func<TResponseType> func);
        void Process(Action action);
    }
}