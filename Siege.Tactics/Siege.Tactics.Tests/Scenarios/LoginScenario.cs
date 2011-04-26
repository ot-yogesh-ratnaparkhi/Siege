namespace Siege.Tactics.Tests.Scenarios
{
    public class LoginScenario : Scenario
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginScenario()
        {
            this.UserName = "";
            this.Password = "";
        }

        public override void Execute()
        {
            this.NavigateTo<LoginPage>(login =>
            {
                login.UserName = this.UserName;
                login.Password = this.Password;

                login.Submit();
            });
        }
    }
}