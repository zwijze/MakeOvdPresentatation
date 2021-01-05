using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net.Config;
using log4net;

namespace General
{
    public static class GeneralFunctions
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneralFunctions));

        public static void executeCmdCommand(String command)
        {
            //Start the command without showing it in a screen
            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            if (!command.Contains("powershell"))
            {
                startInfo.FileName = "cmd.exe";
                //Important is that the argument begins with /C otherwise it won't work. It "Carries out the command specified by the string and then terminates."
                startInfo.Arguments = "/C " + command;
            }
            else
            {
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = command.Replace("powershell ", " ");
            }
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (System.InvalidOperationException e)
            {
                log.Error($"Error running command {command}:{e.Message}");
                Console.ReadLine().ToLower();
                Environment.Exit(1);
            }
        }
    }
}
