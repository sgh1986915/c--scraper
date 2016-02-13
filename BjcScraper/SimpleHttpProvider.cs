using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BjcScraper
{
    public class SimpleHttpProvider: IHttpProvider
    {
        public string DownloadContent(string url)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
