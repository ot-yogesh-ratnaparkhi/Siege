namespace Siege.Container.UnitTests.RegistrationExtensions.Classes
{
    public abstract class IngredientDecorator : ICoffee
    {
        protected ICoffee _decoratedCoffee;

        protected IngredientDecorator(ICoffee decoratedCoffee)
        {
            _decoratedCoffee = decoratedCoffee;
        }

        public virtual string Name
        {
            get { return _decoratedCoffee.Name; }
        }

        public virtual decimal Total
        {
            get { return _decoratedCoffee.Total; }
        }
    }
}
