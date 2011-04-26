using NUnit.Framework;
using OpenQA.Selenium.IE;
using Siege.Tactics.Selenium;
using Siege.Tactics.Tests.Scenarios;

namespace Siege.Tactics.Tests
{
    [TestFixture, Ignore]
    public class LoginPageTests
    {
        private Workflow workflow;

        [SetUp]
        public void SetUp()
        {
            workflow = new Workflow(new SeleniumAdapter<InternetExplorerDriver>());
        }

        [Test]
        public void ShouldLogIn()
        {
            var scenario = new LoginScenario { UserName = "########", Password = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<DashboardPage>();
        }

        [Test]
        public void ShouldPromptForPassword()
        {
            var scenario = new LoginScenario { UserName = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.PasswordError == "Please enter your password.");
            });
        }

        [Test]
        public void ShouldPromptForUserName()
        {
            var scenario = new LoginScenario { Password = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.UserNameError == "Please enter a username.");
            });
        }

        [Test]
        public void PasswordsRequireAtLeastOneNumber()
        {
            var scenario = new LoginScenario { Password = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.UserNameError == "Password must contain 1 number or non-alphanumeric character.");
            });
        }
        
        [Test]
        public void PasswordsRequireAtLeastOneLetter()
        {
            var scenario = new LoginScenario { Password = "########" };
            workflow.Execute(scenario);
           
            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.UserNameError == "Password must contain 1 number or non-alphanumeric character.");
            });
        }

        [Test]
        public void PasswordsMustBeAtLeastSixCharacters()
        {
            var scenario = new LoginScenario { Password = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.UserNameError == "Password must be between 6 and 50 characters long.");
            });            
        }

        [Test]
        public void PasswordsMustBeLessThanFiftyCharacters()
        {
            var scenario = new LoginScenario { Password = "########" };
            workflow.Execute(scenario);

            scenario.ShouldShow<LoginPage>(login =>
            {
                scenario.Expect(login.UserNameError == "Password must be between 6 and 50 characters long.");
            });
        }

        [TearDown]
        public void TearDown()
        {
            workflow.Dispose();
        }
    }
}