using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Actions.HelperClasses;

namespace Actions
{
    public class LiedBoek:IGetSong
    {
        IWebDriver currentWebDriver;
        BrowserContext browserContext;
        public LiedBoek()
        {
            browserContext = new BrowserContext();
            currentWebDriver = browserContext.WebDriver;
        }

        public Boolean Start(String url)
        {
            currentWebDriver.Url = url;
            return true;
        }
        public Boolean Login(String loginName,String password)
        {
            currentWebDriver.FindElement(By.Name("txtEmail")).SendKeys(loginName);
            currentWebDriver.FindElement(By.Name("txtPassword")).SendKeys(password);
            currentWebDriver.FindElement(By.XPath("//input[@value='Inloggen']")).Click();
            return true;
        }

        public Boolean SearchSong()
        {
            return true;
        }
        public Boolean DownloadSong()
        {
            return true;
        }
        public Boolean ExtractSong()
        {
            return true;
        }

        public Boolean Quit()
        {
            browserContext.Quit();
            return true;
        }
    }
}
