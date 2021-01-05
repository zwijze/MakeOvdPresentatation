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

            //Execute intial actions like copying the forst Powerpoint sheet
            ActionsToExecute.intialActions(directoryName);

            //For each line determine if the lines fullfills the requirement to execute an action is needed and if so execute the action
            int order = 10;
            List<Thread> threadList = new List<Thread>();
            foreach (KeyValuePair<int, string> textLine in textLines)
            {
                order = order + 10;
                String textLineValue = textLine.Value;
                int textLineKey = textLine.Key;
                String previousTextLineValue = textLineKey==1?"":textLines[textLineKey -1];
                String nextTextLineValue = textLines.Count== textLineKey?"":textLines[textLineKey + 1];
                Thread newThread = new Thread(ActionsToExecute.ActionsToExecuteWrapper);
                List<String> ActionsToExecuteWrapperParameters = new List<String>();
                ActionsToExecuteWrapperParameters.Add(order.ToString());
                ActionsToExecuteWrapperParameters.Add(textLineValue);
                ActionsToExecuteWrapperParameters.Add(previousTextLineValue);
                ActionsToExecuteWrapperParameters.Add(nextTextLineValue);
                ActionsToExecuteWrapperParameters.Add(directoryName);
                newThread.Start(ActionsToExecuteWrapperParameters);
                threadList.Add(newThread);
            }
            foreach (Thread thread in threadList)
            {
                thread.Join();
            }
        }

        private static void ExecuteScripts(object data)
        {
            int i = 1;
        }
    }
}
