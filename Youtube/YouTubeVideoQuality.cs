using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.IO;

namespace YouTube_Getter
{
    public partial class mainForm : Form
    {
        //internal mainForm()
        public mainForm()
        {
            InitializeComponent();
        }
        private byte[] downloadedDataStream;
        private Video_Downloader_DLL.VideoDownloader downloader = new Video_Downloader_DLL.VideoDownloader();
        private void Button1_Operation(object sender, System.EventArgs e)
        {
            string videoHeader = "";
            //string thumbnailurl = "";
            string outputStr = "";
            string fileinputurl = TextBox1.Text;
            downloader.GetvideoHeader(fileinputurl, ref videoHeader);
            System.String tempVar = "";
            System.Int32 tempVar2 = 0;
            downloader.MakeDownloadURL(fileinputurl, ref outputStr, ref tempVar, ref tempVar2);
            //downloader.GetPreviewThumbnail(fileinputurl, ref thumbnailurl);
            OutLink.Text = (videoHeader);
            LinkText.Text = (outputStr);
            //PicImage.Text = (thumbnailurl);
            MessageBox.Show("To Download " + videoHeader + Environment.NewLine + "Please click Download now, When finished Downloading select Save To File");
        }
        private void downloadData(string url)
        {
            progressBar1.Value = 0;
            downloadedDataStream = new byte[0];
            try
            {
                //Optional
                this.Text = "Connecting...";
                Application.DoEvents();
                //Get a data stream from the url
                WebRequest req = WebRequest.Create(url);
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();
                //Download in chuncks
                byte[] buffer = new byte[1024];
                //Get Total Size
                int dataLength = (int)response.ContentLength;
                //With the total data we can set up our progress indicators
                progressBar1.Maximum = dataLength;
                lbProgress.Text = "0/" + dataLength.ToString();

                this.Text = "Downloading...";
                Application.DoEvents();

                //Download to memory
                //Note: adjust the streams here to download directly to the hard drive
                MemoryStream memStream = new MemoryStream();
                while (true)
                {
                    //Try to read the data
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        //Finished downloading
                        progressBar1.Value = progressBar1.Maximum;
                        lbProgress.Text = dataLength.ToString() + "/" + dataLength.ToString();

                        Application.DoEvents();
                        break;
                    }
                    else
                    {
                        //Write the downloaded data
                        memStream.Write(buffer, 0, bytesRead);

                        //Update the progress bar
                        if (progressBar1.Value + bytesRead <= progressBar1.Maximum)
                        {
                            progressBar1.Value += bytesRead;
                            lbProgress.Text = progressBar1.Value.ToString() + "/" + dataLength.ToString();

                            progressBar1.Refresh();
                            Application.DoEvents();
                        }
                    }
                }

                //Convert the downloaded stream to a byte array
                downloadedDataStream = memStream.ToArray();

                //Clean up
                stream.Close();
                memStream.Close();
            }
            catch (Exception)
            {
                //May not be connected to the internet
                //Or the URL might not exist
                MessageBox.Show("There was an error accessing the URL.");
            }

            txtData.Text = downloadedDataStream.Length.ToString();
            this.Text = "YT Snatcher";
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            downloadData(LinkText.Text);

            //Get the last part of the url, ie the file name
            if (downloadedDataStream != null && downloadedDataStream.Length != 0)
            {
                string ytdata = OutLink.Text;
                string urlName = LinkText.Text;
                if (urlName.EndsWith("/"))
                    urlName = urlName.Substring(0, urlName.Length - 1); //Chop off the last '/'

                urlName = urlName.Substring(urlName.LastIndexOf('/') + 1);

                saveDiag1.FileName = ytdata + ".flv";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (downloadedDataStream != null && downloadedDataStream.Length != 0)
            {
                if (saveDiag1.ShowDialog() == DialogResult.OK)
                {
                    this.Text = "Saving your file ...";
                    Application.DoEvents();

                    //Write the bytes to a file
                    FileStream newFile = new FileStream(saveDiag1.FileName, FileMode.Create);
                    newFile.Write(downloadedDataStream, 0, downloadedDataStream.Length);
                    newFile.Close();

                    this.Text = "Download Data";
                    MessageBox.Show("Saved the file Successfully");
                }
            }
            else
                MessageBox.Show("No File was Downloaded !");
        }
    }
}
