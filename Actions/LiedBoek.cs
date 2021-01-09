using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Actions.HelperClasses;
using System.IO;
using General;
using System.Configuration;
using System.Threading;

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

        public Boolean SearchSong(String song,List<String> songNumbers, String downloadDirectory, String directoryName,String order,String waitToDownloadFile)
        {
            if (!Directory.Exists(downloadDirectory))
            {
                throw new InvalidOperationException("Specify an existing root directory");

            }
            else
            {

            }
            foreach (String songNumber in songNumbers)
            {
                GeneralFunctions.DeleteFiles("Standaard_liedlijst*.zip", downloadDirectory);
                String extractDirectory = downloadDirectory + @"\Standaard_liedlijst";
                if (Directory.Exists(extractDirectory))
                {
                    GeneralFunctions.DeleteFiles("*.*", extractDirectory);
                    Directory.Delete(extractDirectory);
                }

                currentWebDriver.FindElement(By.Name("txtSearch")).SendKeys(song+ (songNumber.Equals("")?"":":") + songNumber);
                currentWebDriver.FindElement(By.XPath("//a[@class='focusButton btnSearch']")).Click();
                Thread.Sleep(3000);
                currentWebDriver.FindElement(By.Id("selectsong")).Click();
                currentWebDriver.FindElement(By.XPath("//a[@href='/site/nl/mijnliedboek/Default.aspx']")).Click();
                currentWebDriver.FindElement(By.XPath("//a[@href='/site/nl/mijnliedboek/Download/default.aspx']")).Click();
                currentWebDriver.FindElement(By.XPath("//a[@class='focusButton dark-style btnDownload']")).Click();
                currentWebDriver.FindElement(By.XPath("//a[@href='https://liedboek.liedbundels.nu/download/liedlijsten/l12991/Standaard_liedlijst.zip']")).Click();
                //Wait for downloading file
                Thread.Sleep(Convert.ToInt32(waitToDownloadFile) *1000);
                GeneralFunctions.UnzipFiles(downloadDirectory+ @"\"+ "Standaard_liedlijst.zip", extractDirectory);
                GeneralFunctions.DeleteFiles(@"Liedboek-licentie.txt", extractDirectory);
                //File.Copy
                foreach (String file in Directory.GetFiles(extractDirectory))
                    File.Copy(file, Path.Combine(directoryName, order + "_" + Path.GetFileName(file)));

            }
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
