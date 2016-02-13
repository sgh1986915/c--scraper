using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BjcScraper
{
    public interface IHttpProvider
    {
        string DownloadContent(string url);
    }
}
