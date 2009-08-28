namespace Siege.Workflow.Framework.UnitTests.TestContracts
{
    public interface ICheckoutContract : IInvoiceContract
    {
    }

    public class Checkoutcontract : Request, ICheckoutContract
    {
        
    }
}
