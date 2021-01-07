using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Actions
{
    public class LiedBoek:IGetSong
    {
        HttpClient client;
        public LiedBoek()
        {
            client = new HttpClient();

        }

        public Boolean Login()
        {
            return true;
        }

        public Boolean SearchSong()
        {
            return true;
        }
        public Boolean DownloadSong()
        {
            return true;
        }
        public Boolean ExtractSong()
        {
            return true;
        }

        private async Task createTask()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://www.contoso.com/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}
