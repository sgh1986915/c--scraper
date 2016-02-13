using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace BjcScraper
{
    public class Scraper
    {
        private readonly IHttpProvider _httpProvider;
        private const string RootUrl = "http://www.bjs.com/";

        public delegate void ProcessEndEventHandler(object sender);
        public event ProcessEndEventHandler ProcessEnd;
        protected virtual void RaiseProcessEnd()
        {
            if (ProcessEnd != null)
                ProcessEnd(this);
        }

        public delegate void StatusChangeEventHandler(object sender, string e);
        public event StatusChangeEventHandler StatusChange;
        protected virtual void RaiseStatusChange(string args)
        {
            if (StatusChange != null)
                StatusChange(this, args);
        }

        public delegate void NewItemEventHandler(object sender, NewItemEventArgs e);
        public event NewItemEventHandler NewItem;
        protected virtual void RaiseNewItem(NewItemEventArgs args)
        {
            if (NewItem != null)
                NewItem(this, args);
        }

        private ConcurrentQueue<ScrapItemTask> _itemTasks = new ConcurrentQueue<ScrapItemTask>();
        private int TotalTasks { get; set; }
        private int CompletedTasks { get; set; }
        private readonly Object _lock = new object();
        private Thread _linkParserThread;
        private bool _stopFlag;
        private bool _categoryParsingInProgress;

        public Scraper(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
            _linkParserThread = new Thread(LinksParserProcess);
        }

        public List<Category> LoadCategories()
        {
            var result = new List<Category>();
            var mainPageContent = _httpProvider.DownloadContent(RootUrl);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(mainPageContent);
            var menuCategories = document.DocumentNode.SelectNodes("//*[contains(@class, 'newHeaderBox')]/ul/li");
            foreach (var mainCategoryNode in menuCategories)
            {
                try
                {
                    var topCategory = new Category();
                    var link = mainCategoryNode.SelectSingleNode("a");
                    if (link == null) continue;
                    topCategory.Title = Regex.Replace(link.InnerText, @"\t|\n|\r", "");
                    var categoryBlocks = mainCategoryNode.SelectNodes(".//ul[contains(@class, 'dropdownList')]");
                    foreach (var categoryBlock in categoryBlocks)
                    {
                        //get header
                        var mainLink = categoryBlock.SelectSingleNode(".//h1/a");
                        if (mainLink == null) continue;
                        var subCategory = new Category
                        {
                            Title = Regex.Replace(mainLink.InnerText, @"\t|\n|\r", ""),
                            Url = mainLink.Attributes["href"].Value
                        };
                        topCategory.Childs.Add(subCategory);
                        //get other links
                        var links = categoryBlock.SelectNodes(".//li/a");
                        if (links == null) continue;
                        foreach (var sublink in links)
                        {
                            var subSubCategory = new Category
                            {
                                Title = Regex.Replace(sublink.InnerText, @"\t|\n|\r", ""),
                                Url = sublink.Attributes["href"].Value
                            };
                            subCategory.Childs.Add(subSubCategory);
                        }
                    }
                    result.Add(topCategory);
                }
                catch (Exception ex)
                {
                    
                } 
            }
            return result;
        }

        public void StartProcess(List<Category> categories)
        {
            _stopFlag = false;
            var categoryParserProcess = new Thread(() => CategoryParserProcess(categories));
            categoryParserProcess.Start();
            _linkParserThread.Start();
            RaiseStatusChange("Started");
        }

        public void StopProcess()
        {
            _stopFlag = true;
        }

        public void CategoryParserProcess(List<Category> categories)
        {
            _categoryParsingInProgress = true;
            var processedCats = 0;
            //get by 30
            var skip = 0;
            var take = 30;
            while (true)
            {
                var categoriesChunk = categories.Skip(skip).Take(take).ToList();
                if(!categoriesChunk.Any()) break;
                Parallel.For(0, categoriesChunk.Count(), new ParallelOptions {MaxDegreeOfParallelism = 10}, (i, loopState) =>
                    {
                        try
                        {
                            RaiseStatusChange(string.Format("Begin parsing category {0}", categoriesChunk[i].Title));
                            var url = string.Format("{0}{1}", RootUrl, categoriesChunk[i].Url);
                            var pageContent = _httpProvider.DownloadContent(url);
                            HtmlDocument document = new HtmlDocument();
                            document.LoadHtml(pageContent);
                            AddLinks(ParseItemLinks(document), categoriesChunk[i].Title);

                            var pagerBoxNode =
                                document.DocumentNode.SelectSingleNode("//*[contains(@class, 'pagerbox')]");
                            if (pagerBoxNode == null) return;
                            var pageLinks = pagerBoxNode.SelectNodes(".//a").Select(x => x.InnerText);
                            foreach (var pageLinkStr in pageLinks)
                            {
                                int pageLink = 0;
                                var parseResult = int.TryParse(pageLinkStr, NumberStyles.Any,
                                    CultureInfo.InvariantCulture,
                                    out pageLink);
                                if (!parseResult || pageLink == 1) continue;
                                var lastPointPosition = url.LastIndexOf('.');
                                var nextUrl = string.Format("{0}.{1}", url.Substring(0, lastPointPosition), pageLink);
                                var nextPageContent = _httpProvider.DownloadContent(nextUrl);
                                HtmlDocument nextPagedoc = new HtmlDocument();
                                nextPagedoc.LoadHtml(nextPageContent);
                                AddLinks(ParseItemLinks(nextPagedoc), categoriesChunk[i].Title);
                            }
                            processedCats++;
                            RaiseStatusChange(string.Format("End parsing category {0}", categoriesChunk[i].Title));
                        }
                        catch
                        {
                            
                        }
                    });
                skip += take;
                if(_stopFlag) return;
            }
            _categoryParsingInProgress = false;
        }

        private BjsItem ParseItem(string url)
        {

            var itemUrl = string.Format("{0}{1}", RootUrl, url);
            var pageContent = _httpProvider.DownloadContent(itemUrl);
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pageContent);
            var item = new BjsItem();
            try
            {
                var titleNode = document.DocumentNode.SelectSingleNode("//h1[contains(@id,'itemNameID')]");
                if (titleNode != null)
                    item.Title = titleNode.InnerText.Trim();
                var itemNumberNode = document.DocumentNode.SelectSingleNode("//p[contains(@id,'productModel')]");
                if (itemNumberNode != null)
                {
                    var itemNumberParts = itemNumberNode.InnerText.Trim().Split('|');
                    if (itemNumberParts.Length > 1)
                    {
                        item.ItemNumber = itemNumberParts[0].Replace("Item:", "").Trim();
                    }
                }
                var priceNode = document.DocumentNode.SelectSingleNode("//input[contains(@name,'addToCartPrice')]");
                if (priceNode != null)
                {
                    double price;
                    var priceStr = priceNode.Attributes["value"].Value;
                    double.TryParse(priceStr, NumberStyles.Any, CultureInfo.InvariantCulture, out price);
                    item.Price = price;
                }
                var descriptionNode = document.DocumentNode.SelectSingleNode("//p[contains(@itemprop,'description')]");
                if (descriptionNode != null)
                {
                    item.Description = Regex.Replace(descriptionNode.InnerText, @"\t|\n|\r", "");
                }
                var estimatedNode = document.DocumentNode.SelectSingleNode("//div[contains(@id,'estimatedDelivery')]");
                if (estimatedNode != null)
                {
                    item.EstimatedDelivery = estimatedNode.InnerText.Replace("Estimated Delivery: ", "");
                    item.EstimatedDelivery = Regex.Replace(item.EstimatedDelivery, @"\t|\n|\r", "");
                    item.EstimatedDelivery = item.EstimatedDelivery.Replace(" <!-- <td> --><!-- </td> -->", "");
                }

                var categoryNode = document.DocumentNode.SelectSingleNode("//li[contains(@id,'pagepath')]");
                if (categoryNode != null)
                {
                    item.Category = categoryNode.InnerText;
                    item.Category = Regex.Replace(item.Category, @"\t|\n|\r", "");
                    item.Category = item.Category.Replace("&gt; ", ">");
                    item.Category = item.Category.Replace("&amp;", "&");
                    item.Category = item.Category.Replace("&#034;", "'");
                }
            }
            catch
            {
                
            }
            return item;
        }

        private List<string> ParseItemLinks(HtmlDocument document)
        {
            var links = document.DocumentNode.SelectNodes("//div[contains(@id,'prodtitle')]/a");
            if(links==null) return new List<string>();
            return links.Select(x => x.Attributes["href"].Value).ToList();
        }


        private void AddLinks(List<string> links, string categoryTitle)
        {
            lock (_lock)
            {
                var items =
                    links.Select(
                        x => new ScrapItemTask {Link = x, Title = categoryTitle}).ToList();
                items.ForEach(_itemTasks.Enqueue);
                TotalTasks += links.Count;
            }
        }

        private List<ScrapItemTask> Dequeue()
        {
            var result = new List<ScrapItemTask>();
            while (true)
            {
                ScrapItemTask scrapItemTask;
                var itemExists = _itemTasks.TryDequeue(out scrapItemTask);
                if (itemExists)
                    result.Add(scrapItemTask);
                else break;
                if(result.Count>=30) break;
            }
            return result;
        }

        private void LinksParserProcess()
        {
            while (true)
            {
                List<ScrapItemTask> pendingTasks = Dequeue();
                if (pendingTasks.Any())
                {
                    Parallel.For(0, pendingTasks.Count, new ParallelOptions { MaxDegreeOfParallelism = 100 }, (i, loopState) =>
                    {
                        try
                        {
                            RaiseStatusChange(string.Format("Begin parsing item {0}", pendingTasks[i].Link));
                            var item = ParseItem(pendingTasks[i].Link);
                            RaiseNewItem(new NewItemEventArgs {Item = item});
                            CompletedTasks++;
                            RaiseStatusChange(string.Format("End parsing item {0}", pendingTasks[i].Link));
                        }
                        catch
                        {
                            
                        }
                    });
                    RaiseStatusChange("Idle");
                }
                else
                {
                    if(!_categoryParsingInProgress) RaiseProcessEnd();
                }
                if (_stopFlag) return;
                Thread.Sleep(500);
            }
        }
    }
}
