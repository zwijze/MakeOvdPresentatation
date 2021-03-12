using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using General;
using log4net.Config;
using log4net;
using Actions;
using General.Config;
using System.Linq;

namespace DetermineActions
{
    public class ActionsToExecute
    {
        private static int order = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneralFunctions));
        public static void ActionsToExecuteWrapper(object data)
        {
            List<String> ActionsListParameters = (List<String>)data;
            String line = ActionsListParameters[0];
            String linePrevious = ActionsListParameters[1];
            String lineNext = ActionsListParameters[2];
            String directoryName = ActionsListParameters[3];

            Boolean actionExecuted=false;
            //Download Youtube

            actionExecuted =DownLoadYouTube(line, directoryName);

            /*if (actionExecuted == false)
            {
                actionExecuted = DownLoadSongs(order, line, lineNext, directoryName);
            }*/


        }

        public static void intialActions(String directoryName)
        {
            //Create Quick access item for directory of presentation
            String script = @"powershell Scripts\quickAccessAdd.ps1 -directoryName '" + directoryName + "'";
            //String script = @"powershell -executionpolicy -bypass";
            GeneralFunctions.executeCmdCommand(script);
            log.Info("Created Quick Access link:" + Regex.Match(directoryName, @".*\\(.*)").Groups[1].Value);

            //Copy First Powerpoint of presentation
            DateTime previousSunday = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            String directoryNamePreviousSunday = Regex.Match(directoryName, @"(.*)\\(.*)").Groups[1].Value + "\\" + previousSunday.Year.ToString() + "-" + previousSunday.Month.ToString().PadLeft(2, '0') + "-" + previousSunday.Day.ToString().PadLeft(2, '0');
            String fileCopyFrom = directoryNamePreviousSunday + "\\0-Voor de dienst.ppt";
            String fileCopyTo = directoryName + "\\0-Voor de dienst.ppt";
            if (Directory.Exists(directoryName) && File.Exists(fileCopyFrom) && !File.Exists(fileCopyTo))
            {
                log.Info("Copying file 0-Voor de dienst.ppt from previous service...");
                File.Copy(fileCopyFrom, fileCopyTo);
                System.Diagnostics.Process.Start(fileCopyTo);
            }
        }

        public static bool DownLoadSongs(String line,String lineNext, String directoryName)
        {
            //Retrieve Liedboek  song
            if (!line.ToLower().Contains("youtube.com") && !lineNext.ToLower().Contains("youtube.com") && Regex.Match(line.ToLower(), @"lied\s+\d+").Success)
            {
                order = order + 10;
                var section = (General.Config.Section)ConfigurationManager.GetSection("SongsSection");
                IEnumerable<SectionCollectionElement> sectionCollectionMembers = section.SectionCollectionMembers.Cast<SectionCollectionElement>();
                List<Add> addCollectionMembersList =GetaddCollectionMembersList(sectionCollectionMembers, "LiedBoek");
                String url = addCollectionMembersList.First(k => k.key.Equals("url")).value;
                LiedBoek _liedBoek = new LiedBoek();
                _liedBoek.Start(url);
                String loginName = addCollectionMembersList.First(k => k.key.Equals("loginName")).value;
                String password = addCollectionMembersList.First(k => k.key.Equals("password")).value;
                _liedBoek.Login(loginName,password);
                String regexSearchStringForSongs = addCollectionMembersList.First(k => k.key.Equals("regexSearchStringForSongs")).value;
                String song = Regex.Match(line.ToLower(), @regexSearchStringForSongs).Groups[1].Value;
                String regexSearchStringForSongsNumbers = addCollectionMembersList.First(k => k.key.Equals("regexSearchStringForSongsNumbers")).value;
                List<String> songNumbers = Regex.Match(line.ToLower(), @regexSearchStringForSongsNumbers).Groups[1].Value.Replace(" " ,"" ).Replace("-", ",").Split(',').ToList();
                String downloadDirectory = addCollectionMembersList.First(k => k.key.Equals("downloadDirectory")).value;
                String waitToDownloadFile = addCollectionMembersList.First(k => k.key.Equals("waitToDownloadFile")).value;
                _liedBoek.SearchSong(song, songNumbers, downloadDirectory, directoryName,order.ToString(),waitToDownloadFile);
                _liedBoek.Quit();
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool DownLoadYouTube(String line,String directoryName)
        {
            if (!line.ToLower().Contains("youtube.com")) return false;
            order = order + 10;
            String youtubeDownloadApplication = ConfigurationManager.AppSettings["youtubeDownloadApplication"];
            String url = line.ToLower().Contains(@"https://")? @"https://" + Regex.Match(line, @".*?(?i)https(?-i)://(.*?)(\s|$).*").Groups[1].Value : "www" + Regex.Match(line, @".*?(?i)www(?-i)(.*?)(\s|$).*").Groups[1].Value;
            String script = youtubeDownloadApplication + " -f best --output \"" + directoryName + "\\"  + order + "_%(title)s.%(ext)s\" " + url;
            log.Info("Downloading url: " + url + "  ...");
            GeneralFunctions.executeCmdCommand(script);
            return true;
        }

        private static List<Add> GetaddCollectionMembersList(IEnumerable<SectionCollectionElement> sectionCollectionMembers,String keyValue)
        {
            SectionCollectionElement sectionCollectionMember = sectionCollectionMembers.First(m => m.key.Equals(keyValue));
            IEnumerable<Add> addCollectionMembers = sectionCollectionMember.AddCollectionMembers.Cast<Add>();
            return addCollectionMembers.ToList();
        }
    }
}
