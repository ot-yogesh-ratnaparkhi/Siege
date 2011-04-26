namespace Siege.Tactics.Tests
{
    public class LoginPage : Page
    {
        public string UserNameError
        {
            get { return adapter.GetByPath("/html/body/div/div/div[2]/form/table/tbody/tr/td[2]/div"); }
        }

        public string PasswordError
        {
            get { return adapter.GetByPath("/html/body/div/div/div[2]/form/table/tbody/tr[2]/td[2]/div"); }
        }

        public string UserName
        {
            get { return adapter.GetText("Username"); }
            set { adapter.SetText("Username", value); }
        }

        public string Password
        {
            get { return adapter.GetText("Password"); }
            set { adapter.SetText("Password", value); }
        }

        public void Submit()
        {
            adapter.ClickImage("Submit");
        }

        public override string Url
        {
            get { return "https://members.scoresense.com/Authenticate.mvc/Login"; }
        }
    }
}