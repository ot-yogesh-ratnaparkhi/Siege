using System;
using NUnit.Framework;

namespace Siege.Tactics
{
    public abstract class Scenario : IDisposable
    {
        private ITestAdapter adapter;
        public abstract void Execute();

        public TPage NavigateTo<TPage>(Action<TPage> action) where TPage : Page, new()
        {
            TPage page = new TPage();
            page.WithAdapter(adapter);
            page.Open();
            action(page);

            return page;
        }

        internal void WithAdapter(ITestAdapter adapter)
        {
            this.adapter = adapter;
        }

        internal TPage Open<TPage>() where TPage : Page, new()
        {
            return NavigateTo<TPage>(page => { });
        }

        public void Dispose()
        {
            adapter.Dispose();
        }

        public void ShouldShow<TPage>() where TPage : Page, new()
        {
            TPage page = new TPage();
            page.WithAdapter(this.adapter);

            Assert.AreEqual(page.Url, adapter.GetCurrentUrl());
        }

        public void ShouldShow<TPage>(Action<TPage> validations) where TPage : Page, new()
        {
            TPage page = new TPage();
            page.WithAdapter(this.adapter);

            validations(page);
         
            Assert.AreEqual(page.Url, adapter.GetCurrentUrl());
        }

        public void Expect(bool condition)
        {
            Assert.IsTrue(condition);
        }
    }
}