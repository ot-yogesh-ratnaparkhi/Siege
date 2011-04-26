using OpenQA.Selenium;

namespace Siege.Tactics.Selenium
{
    public class SeleniumAdapter<TWebDriver> : ITestAdapter where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;

        public void Dispose()
        {
            this.driver.Quit();
            this.driver.Dispose();
        }

        public void Init(Page page)
        {
            this.driver = new TWebDriver();
            this.driver.Navigate().GoToUrl(page.Url);
        }

        public string GetText(string controlName)
        {
            return this.driver.FindElement(By.Name(controlName)).Value;
        }

        public void SetText(string controlName, string value)
        {
            this.driver.FindElement(By.Name(controlName)).SendKeys(value);
        }

        public void ClickButton(string controlName)
        {
            this.driver.FindElement(By.Name(controlName)).Click();
        }

        public void ClickImage(string controlName)
        {
            ClickButton(controlName);
        }

        public string GetCurrentUrl()
        {
            return this.driver.Url;
        }

        public string GetByPath(string xPath)
        {
            return this.driver.FindElement(By.XPath(xPath)).Text;
        }
    }
}