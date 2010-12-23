using System;

namespace Siege.Courier.Web.ViewEngine
{
    public interface ITemplateSelector
    {
        string Path { get; }
        void When(Func<bool> condition);
        bool IsValid();
    }
}