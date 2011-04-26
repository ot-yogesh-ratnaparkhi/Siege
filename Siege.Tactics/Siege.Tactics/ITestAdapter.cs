using System;

namespace Siege.Tactics
{
    public interface ITestAdapter : IDisposable
    {
        void Init(Page page);
        string GetText(string controlName);
        void SetText(string controlName, string value);
        void ClickButton(string controlName);
        void ClickImage(string controlName);
        string GetCurrentUrl();
        string GetByPath(string xPath);
    }
}