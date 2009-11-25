using NUnit.Framework;
using Siege.Container.StructureMapAdapter;
using Siege.ServiceLocation;
using Siege.Workflow.Framework.Activities;
using Siege.Workflow.Framework.UnitTests.TestActivities;
using Siege.Workflow.Framework.UnitTests.TestContracts;
using Siege.Workflow.Framework.UnitTests.TestWorkflows;

namespace Siege.Workflow.Framework.UnitTests
{
    [TestFixture]
    public class WorkflowTests
    {
        protected IContextualServiceLocator locator;
        
        [SetUp]
        public void SetUp()
        {
            locator = new SiegeContainer(new StructureMapAdapter());

            locator.Register(Given<IApplyCouponsActivity>.Then<ApplyCouponsActivity>());
            locator.Register(Given<IApplyGiftCardsActivity>.Then<ApplyGiftCardsActivity>());
            locator.Register(Given<IChargeCreditCardActivity>.Then<ChargeCreditCardActivity>());
            locator.Register(Given<ICreateInvoiceActivity>.Then<CreateInvoiceActivity>());
            locator.Register(Given<IEmailBackOrderNotificationActivity>.Then<EmailBackOrderNotificationActivity>());
            locator.Register(Given<IEmailOrderConfirmationActivity>.Then<EmailOrderConfirmationActivity>());
            locator.Register(Given<IFulfillOrderActivity>.Then<FulfillOrderActivity>());
            locator.Register(Given<IRefundGiftCardsActivity>.Then<RefundGiftCardsActivity>());

            locator.Register(Given<ExceptionActivity>.Then<ExceptionActivity>());
            locator.Register(Given<BreakActivity>.Then<BreakActivity>());
            locator.Register(Given<ConditionalActivity>.Then<ConditionalActivity>());

            locator.Register(Given<WorkflowActivity<IApplyCouponsActivity>>.Then<WorkflowActivity<IApplyCouponsActivity>>());
            locator.Register(Given<WorkflowActivity<IApplyGiftCardsActivity>>.Then<WorkflowActivity<IApplyGiftCardsActivity>>());
            locator.Register(Given<WorkflowActivity<IChargeCreditCardActivity>>.Then<WorkflowActivity<IChargeCreditCardActivity>>());
            locator.Register(Given<WorkflowActivity<ICreateInvoiceActivity>>.Then<WorkflowActivity<ICreateInvoiceActivity>>());
            locator.Register(Given<WorkflowActivity<IEmailBackOrderNotificationActivity>>.Then<WorkflowActivity<IEmailBackOrderNotificationActivity>>());
            locator.Register(Given<WorkflowActivity<IEmailOrderConfirmationActivity>>.Then<WorkflowActivity<IEmailOrderConfirmationActivity>>());
            locator.Register(Given<WorkflowActivity<IFulfillOrderActivity>>.Then<WorkflowActivity<IFulfillOrderActivity>>());
            locator.Register(Given<WorkflowActivity<IRefundGiftCardsActivity>>.Then<WorkflowActivity<IRefundGiftCardsActivity>>());
            locator.Register(Given<CaptureResultActivity<IFulfillOrderActivity>>.Then<CaptureResultActivity<IFulfillOrderActivity>>());
        }

        [Test]
        public void Should_Run_Checkout_Workflow()
        {
            Checkoutcontract checkoutcontract = new Checkoutcontract();
            CheckoutWorkflow workflow = new CheckoutWorkflow(locator, checkoutcontract);

            workflow.Process();
        }
    }
}
