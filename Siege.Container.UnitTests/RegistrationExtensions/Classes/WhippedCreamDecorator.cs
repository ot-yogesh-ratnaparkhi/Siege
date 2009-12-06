namespace Siege.Container.UnitTests.RegistrationExtensions.Classes
{
    public class WhippedCreamDecorator : IngredientDecorator
    {
        public WhippedCreamDecorator(ICoffee decoratedCoffee): base(decoratedCoffee)
        {
        }

        public override string Name
        {
            get { return string.Format("{0}, whip cream", base.Name); }
        }

        public override decimal Total
        {
            get { return base.Total + 0.25m; }
        }
    }
}
