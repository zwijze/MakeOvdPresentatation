using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Actions
{
    public class CreateDirectory
    {

        public static String CreateDirectoryIfNotExists(String directoryName){
            if (directoryName.Equals("")){
                throw new InvalidOperationException("Specify a non-empty DirectoryCreatePresentation");

            }
            if (!Regex.IsMatch(directoryName, @".*\d4-\d2-\d2"))
            {
                if (!Directory.Exists(directoryName))
                {
                    throw new InvalidOperationException("Specify an existing root directory");

                }
                DateTime sunday = DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek);
                directoryName = directoryName + "\\" + sunday.Year.ToString() + "-" + sunday.Month.ToString().PadLeft(2, '0') + "-" + sunday.Day.ToString().PadLeft(2, '0');
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            else
            {
                if (!Directory.Exists(Regex.Match(directoryName, @"(.*)\d4-\d2-\d2").Groups[1].Value))
                {
                    throw new InvalidOperationException("Specify an existing root directory");

                }
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
            }
            return directoryName;
        }
    }
}
