using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using General;
using log4net.Config;
using log4net;

namespace DetermineActions
{
    public class ActionsToExecute
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneralFunctions));
        public static void ActionsToExecuteWrapper(object data)
        {
            List<String> ActionsListParameters = (List<String>)data;
            String order = ActionsListParameters[0];
            String line = ActionsListParameters[1];
            String linePrevious = ActionsListParameters[2];
            String lineNext = ActionsListParameters[3];
            String directoryName = ActionsListParameters[4];

            Boolean actionExecuted;
            //Download Youtube
            actionExecuted=DownLoadYouTube(order, line, directoryName);
        }

        public static void intialActions(String directoryName)
        {
            //Copy First Powerpoint of presentation
            DateTime previousSunday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            String directoryNamePreviousSunday = Regex.Match(directoryName, @"(.*)\\(.*)").Groups[1].Value + "\\" + previousSunday.Year.ToString() + "-" + previousSunday.Month.ToString().PadLeft(2, '0') + "-" + previousSunday.Day.ToString().PadLeft(2, '0');
            String fileToCopy = directoryNamePreviousSunday + "\\0-Voor de dienst.ppt";
            if (Directory.Exists(directoryName) && File.Exists(fileToCopy))
            {
                log.Info("Copying file 0-Voor de dienst.ppt from previous service...");
                File.Copy(fileToCopy, directoryName + "\\0-Voor de dienst.ppt");
            }
        }

        private static bool DownLoadYouTube(String order,String line,String directoryName)
        {
            if (line.Contains("youtube.com"))
            {
                String youtubeDownloadApplication = ConfigurationManager.AppSettings["youtubeDownloadApplication"];
                String url = line.ToLower().Contains(@"https://")? @"https://" + Regex.Match(line, @".*?(?i)https(?-i)://(.*?)(\s|$).*").Groups[1].Value : "www" + Regex.Match(line, @".*?(?i)www(?-i)(.*?)(\s|$).*").Groups[1].Value;
                String script = youtubeDownloadApplication + " -f best --output \"" + directoryName + "\\"  + order + "_%(title)s.%(ext)s\" " + url;
                log.Info("Downloading url: " + url + "  ...");
                GeneralFunctions.executeCmdCommand(script);
                return true;
            } else
            {
                return false;
            }
        }
    }
}
