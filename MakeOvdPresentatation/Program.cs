using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextProcessing;
using Actions;
using log4net.Config;
using log4net.Repository.Hierarchy;
using log4net;
using System.Threading;
using DetermineActions;
using System.IO;
using System.Text.RegularExpressions;

namespace MakeOvdPresentatation
{
    class Program
    {
        // Define a static logger variable so that it references the
        // Logger instance named "MyApp".
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        [STAThread] //Needed to read from Clipboard
        static void Main(string[] args)
        {
            //log4net
            XmlConfigurator.Configure();
            log.Info("Starting application.");

            //Create directory to put presentation atifacts
            String directoryName = CreateDirectory.CreateDirectoryIfNotExists(ConfigurationManager.AppSettings["DirectoryCreatePresentation"]);
            log.Info($"Directory where presentation artifacts are stored: " + directoryName);

            //Read all lines from the Ovd
            TextInput.readText(ConfigurationManager.AppSettings["filenameReadingText"]);
            Dictionary<int, String> textLines = TextInput.TextLines;

            String goOnYN;
            if (textLines.Count <= 1)
            {
                Console.WriteLine("Not many Liturgy text is being selected. Do you want to continue (y/n)?");
                goOnYN = Console.ReadLine().ToLower();
                if (goOnYN.Equals("n"))
                {
                    ExitProgram();
                }
            }

            //Execute intial actions like copying the forst Powerpoint sheet
            ActionsToExecute.intialActions(directoryName);

            //For each line determine if the lines fullfills the requirement to execute an action is needed and if so execute the action
            List<Thread> threadList = new List<Thread>();
            foreach (KeyValuePair<int, string> textLine in textLines)
            {
                String textLineValue = textLine.Value;
                int textLineKey = textLine.Key;
                String previousTextLineValue = textLineKey == 1 ? "" : textLines[textLineKey - 1];
                String nextTextLineValue = textLines.Count == textLineKey ? "" : textLines[textLineKey + 1];
                //Downloading songs online should not be done parallel, because otherwise the 2 downaloads will interfere
                if (!textLineValue.ToLower().Contains("youtube.com") && !nextTextLineValue.ToLower().Contains("youtube.com") && Regex.Match(textLineValue.ToLower(), @"lied\s+\d+").Success)
                {
                    ActionsToExecute.DownLoadSongs(textLineValue, nextTextLineValue, directoryName);
                } else
                {
                    Thread newThread = new Thread(ActionsToExecute.ActionsToExecuteWrapper);
                    List<String> ActionsToExecuteWrapperParameters = new List<String>();
                    ActionsToExecuteWrapperParameters.Add(textLineValue);
                    ActionsToExecuteWrapperParameters.Add(previousTextLineValue);
                    ActionsToExecuteWrapperParameters.Add(nextTextLineValue);
                    ActionsToExecuteWrapperParameters.Add(directoryName);
                    newThread.Start(ActionsToExecuteWrapperParameters);
                    threadList.Add(newThread);
                }

            }
            foreach (Thread thread in threadList)
            {
                thread.Join();
            }

            //Manually add Powerpoint presentation file
            goOnYN = "y";
            String powerpointTemplateFile= ConfigurationManager.AppSettings["powerpointTemplateFile"];
            while (goOnYN.ToLower().Equals("y")) {
                Console.WriteLine("Do you want to add more powerpoint presentation files to the presentation directory (y/n)?");
                goOnYN = Console.ReadLine().ToLower();
                if (goOnYN.Equals("y"))
                {
                    Console.WriteLine("Specify filename of the Powerpoint presentation file:");
                    String fileName = Console.ReadLine();
                    File.Copy(powerpointTemplateFile, directoryName + @"\" + fileName);
                    Console.WriteLine("Copied Powerpoint presentation file (do manually edit yourselve): " + fileName);
                    System.Diagnostics.Process.Start(directoryName + @"\" + fileName);
                }
                else
                {
                    ExitProgram();
                }
            }
        }
        private static void ExitProgram()
        {
            Console.WriteLine("Program will stop. Press enter to exit.");
            Console.ReadLine().ToLower();
            Environment.Exit(1);
        }
    }
}
