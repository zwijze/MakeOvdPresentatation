using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextProcessing
{
    public class TextInput
    {
        private static Dictionary<int, String> _TextLines=new Dictionary<int, String>();

        public static  Dictionary<int, String> TextLines=>_TextLines;

        public static void readText(String fileName)
        {
            List<String> tempTextLines =new List<String>();
            if (fileName.Equals("")) //Get from Clipboard
            {
                if (Clipboard.ContainsText(TextDataFormat.Text))
                {
                    string clipboardText = Clipboard.GetText(TextDataFormat.Text);
                    tempTextLines = clipboardText.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
                } 
            } else //get from file
            {
                tempTextLines = File.ReadAllLines(fileName).ToList();
            }
            int i = 1;
            foreach (String tempTextLine in tempTextLines)
            {
                _TextLines.Add(i, tempTextLine);
                i++;
            }
        }
    }
}
