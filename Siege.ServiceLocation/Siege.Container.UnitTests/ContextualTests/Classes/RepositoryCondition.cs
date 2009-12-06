namespace Siege.Container.UnitTests.ContextualTests.Classes
{
    public class RepositoryCondition : IRepositoryCondition
    {
        public RepositoryCondition(Conditions conditions)
        {
            Condition = conditions;
        }

        public Conditions Condition
        {
            get; set;
        }
    }
}
