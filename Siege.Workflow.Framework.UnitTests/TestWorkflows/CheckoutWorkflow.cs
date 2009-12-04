using Siege.ServiceLocation;
using Siege.Workflow.Framework.UnitTests.TestActivities;
using Siege.Workflow.Framework.UnitTests.TestExceptions;

namespace Siege.Workflow.Framework.UnitTests.TestWorkflows
{
    public class CheckoutWorkflow : Workflow
    {
        private OrderStatus orderStatus;

        public CheckoutWorkflow(IContextualServiceLocator serviceLocator) : base(serviceLocator)
        {
            First<IApplyCouponsActivity>()
                .Then<IApplyGiftCardsActivity>()
                .Then<IChargeCreditCardActivity>()
                    .OnException<DeniedChargeException>()
                        .Break
                        (
                            Create
                            (
                                workflow => workflow.First<IRefundGiftCardsActivity>()
                            )
                        )
                .Then<ICreateInvoiceActivity>()
                .Then<IFulfillOrderActivity>().CaptureResult(order => orderStatus = order.Status)
                    .If(IsOnBackOrder)
                        .Then
                        (
                            Create
                            (
                                workflow => workflow.First<IEmailBackOrderNotificationActivity>()
                            )
                        )
                    .Else
                        .Then<IEmailOrderConfirmationActivity>();
                    
        }
        
        public bool IsOnBackOrder()
        {
            return orderStatus == OrderStatus.BackOrder;
        }
    }
}
