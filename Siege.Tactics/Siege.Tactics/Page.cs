using System;

namespace Siege.Tactics
{
    public abstract class Page : IDisposable
    {
        protected ITestAdapter adapter;
        public abstract string Url { get; }

        public void WithAdapter(ITestAdapter adapter)
        {
            this.adapter = adapter;
        }

        public void Open()
        {
            this.adapter.Init(this);
        }

        public void Dispose()
        {
            adapter.Dispose();
        }
    }
}