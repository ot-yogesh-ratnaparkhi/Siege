using System;

namespace Siege.Tactics
{
    public class Workflow : IDisposable
    {
        private readonly ITestAdapter adapter;

        public Workflow(ITestAdapter adapter)
        {
            this.adapter = adapter;
        }

        public Scenario Execute(Action<Scenario> action)
        {
            Scenario scenario = new AdHocScenario(action);
            scenario.WithAdapter(adapter);
            scenario.Execute();
            
            return scenario;
        }

        public Scenario Execute(Scenario scenario)
        {
            scenario.WithAdapter(adapter);
            scenario.Execute();
            return scenario;
        }

        public void Dispose()
        {
            this.adapter.Dispose();
        }
    }
}