﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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
            //Only one thread should download it's own song, but when same threads want to download at the same time both browser will have both downloads.
            try
            {
                if (!Directory.Exists(downloadDirectory))
                {
                    throw new InvalidOperationException("Specify an existing root directory");
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

                    var wait = new WebDriverWait(currentWebDriver, TimeSpan.FromSeconds(5));

                    IWebElement txtSearch = wait.Until(drv => drv.FindElement(By.Name("txtSearch")));
                    txtSearch.SendKeys(song + (songNumber.Equals("") ? "" : ":") + songNumber);
                    currentWebDriver.FindElement(By.XPath("//a[@class='focusButton btnSearch']")).Click();

                    IWebElement selectsong = wait.Until(drv => drv.FindElement(By.Id("selectsong")));
                    selectsong.Click();

                    Thread.Sleep(5000);//Extra wait otherwise not alle songs are in the list en when selecting 'Alles' not all songs will be selected

                    IWebElement defaultSite = wait.Until(drv => drv.FindElement(By.XPath("//a[@href='/site/nl/mijnliedboek/Default.aspx']")));
                    defaultSite.Click();

                    IWebElement downloadSite = wait.Until(drv => drv.FindElement(By.XPath("//a[@href='/site/nl/mijnliedboek/Download/default.aspx']")));
                    downloadSite.Click();

                    try
                    {
                        IWebElement btnDownload = wait.Until(drv => drv.FindElement(By.XPath("//a[@class='focusButton dark-style btnDownload']")));
                        btnDownload.Click();
                    }
                    catch (NoSuchElementException)
                    {
                        currentWebDriver.FindElement(By.XPath("//a[@class='focusButton dark-style btnStartDownload']")).Click();
                        currentWebDriver.FindElement(By.XPath("//a[@class='focusButton dark-style btnDownload']")).Click();
                    }

                    IWebElement liedlijst = wait.Until(drv => drv.FindElement(By.XPath("//a[@href='https://liedboek.liedbundels.nu/download/liedlijsten/l12991/Standaard_liedlijst.zip']")));
                    liedlijst.Click();
                    //Wait for downloading file
                    Thread.Sleep(Convert.ToInt32(waitToDownloadFile) * 1000);
                    GeneralFunctions.UnzipFiles(downloadDirectory + @"\" + "Standaard_liedlijst.zip", extractDirectory);
                    GeneralFunctions.DeleteFiles(@"Liedboek-licentie.txt", extractDirectory);
                    //File.Copy
                    foreach (String file in Directory.GetFiles(extractDirectory))
                        File.Copy(file, Path.Combine(directoryName, order + "_" + Path.GetFileName(file)));
                }

                return true;
            } catch (Exception e)
            {
                Console.WriteLine("Song " + song + " not downloaded:" + e.Message);
                return false;
            }
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
