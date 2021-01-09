using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net.Config;
using log4net;
using System.Text.RegularExpressions;
using System.IO.Compression;

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
                startInfo.RedirectStandardOutput = false;
                startInfo.RedirectStandardError = true;
            }
            else
            {
                //Run this command under a cmd box as administrator otherwise you get a Not digitally signed error:
                //Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Scope CurrentUser
                //Check with:Get-ExecutionPolicy -List
                startInfo.FileName = "powershell.exe";
                startInfo.Arguments = Regex.Match(command, @".*powershell\s(.*)").Groups[1].Value;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;

            }
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                process.WaitForExit();
                String errors = process.StandardError.ReadToEnd();
                if (!errors.Equals(""))
                {
                    Console.WriteLine("errors:" + process.StandardError.ReadToEnd());
                    Console.ReadLine().ToLower();
                    Environment.Exit(1);
                }
                //Console.WriteLine("Standoutput:" + process.StandardOutput.ReadToEnd());
            }
            catch (System.InvalidOperationException e)
            {
                log.Error($"Error running command {command}:{e.Message}");
                Console.ReadLine().ToLower();
                Environment.Exit(1);
            }
        }

        public static void DeleteFiles(String filePattern, String directoryName)
        {
            String[] files = Directory.GetFiles(directoryName, filePattern);

            foreach (String file in files)
            {
                File.Delete(file);
            }
        }

        public static void UnzipFiles(String file, String directoryName)
        {
            ZipFile.ExtractToDirectory(file, directoryName);
        }
    }
}
