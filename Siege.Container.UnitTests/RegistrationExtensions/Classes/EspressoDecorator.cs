namespace Siege.Container.UnitTests.RegistrationExtensions.Classes
{
    public class EspressoShotDecorator : IngredientDecorator
    {
        public EspressoShotDecorator(ICoffee decoratedCoffee) : base(decoratedCoffee)
        {
        }

        public override string Name
        {
            get { return string.Format("{0}, shot of espresso", base.Name); }
        }

        public override decimal Total
        {
            get { return base.Total + 0.50m; }
        }
    }
}
