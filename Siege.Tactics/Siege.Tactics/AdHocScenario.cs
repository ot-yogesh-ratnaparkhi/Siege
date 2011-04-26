using System;

namespace Siege.Tactics
{
    public class AdHocScenario : Scenario
    {
        private readonly Action<Scenario> scenario;

        public AdHocScenario(Action<Scenario> scenario)
        {
            this.scenario = scenario;
        }

        public override void Execute()
        {
            scenario(this);
        }
    }
}