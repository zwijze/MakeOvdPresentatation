using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions.HelperClasses
{
    class BrowserContext
    {
        private static Lazy<IWebDriver> webDriver = CreateLazyWebDriver();

        private static Lazy<IWebDriver> CreateLazyWebDriver()
        {
            return new Lazy<IWebDriver>(CreateWebDriver);
        }

        private static IWebDriver CreateWebDriver()
        {
            if ((ConfigurationManager.AppSettings["browserForRetrievingDataFromInternet"] ?? "Chrome") == "InternetExplorer")
            {
                var ieOption = new InternetExplorerOptions()
                {
                    IntroduceInstabilityByIgnoringProtectedModeSettings=true,
                    IgnoreZoomLevel=true,
                    EnableNativeEvents=true,
                    EnsureCleanSession=true
                };
                return new InternetExplorerDriver();
            }
            else
            {
                string ChromePath = Environment.GetEnvironmentVariable("ChromeWebDriver");
                if (ChromePath == null) ChromePath = AppDomain.CurrentDomain.BaseDirectory;
                //Console.WriteLine(ChromePath);
                var service = ChromeDriverService.CreateDefaultService(ChromePath);
                //service.LogPath = $"{ChromePath}\\chromedriver.log";
                service.EnableVerboseLogging =false;
                service.SuppressInitialDiagnosticInformation = false;
                service.LogPath = null;
                var options = new ChromeOptions();
                options.AddArgument("no-sandbox");
                options.AddArgument("disable-extensions");
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-logging");
                if (Environment.GetEnvironmentVariable("ChromeBinaryPath") != null) options.BinaryLocation = Environment.GetEnvironmentVariable("ChromeBinaryPath");
                var currentWebDriver = new ChromeDriver(service, options);
                return currentWebDriver;

            }
        }

        public IWebDriver WebDriver
        {
            get { return webDriver.Value;}
        }

        public void Quit()
        {
            if (GetIsInitialized())
            {
                webDriver.Value.Quit();
                webDriver = CreateLazyWebDriver();
            }
        }

        private static bool GetIsInitialized()
        {
            return webDriver.IsValueCreated;
        }
    }


}
