using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public delegate void HTTPClientEventHandler(HTTPClientResponse result);
    public class HTTPClientResponse
    {
        public string raw { get; set; }
        public HtmlDocument html { get; set; }
        public HtmlNode node { get; set; }
    }

    public class HttpUtil
    {
        private WebClient client = null;
        private static HttpUtil _instance;
        public static HttpUtil Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new HttpUtil();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public HttpUtil()
        {
            client = new WebClient();
            client.Encoding = Encoding.UTF8;
        }

        public void RequestGet(string url, HTTPClientEventHandler handler, DownloadStringCompletedEventHandler callback)
        {
            if (client.IsBusy)
            {
                client.CancelAsync();
            }
            string u = url;
            client.DownloadStringCompleted -= callback;
            client.DownloadStringCompleted += callback;
            client.DownloadStringAsync(new Uri(u), handler);
        }
        private HTTPClientResponse ResponseCallback(HTTPClientEventHandler handler, string html, string error)
        {
            HTTPClientResponse response = new HTTPClientResponse();
            response.raw = html;
            response.html = new HtmlDocument();
            response.html.LoadHtml(html);
            response.node = response.html.DocumentNode;
            return response;
        }
        private void RequestResponseCallback(Object sender, DownloadStringCompletedEventArgs e)
        {
            HTTPClientEventHandler handler = e.UserState as HTTPClientEventHandler;
            handler(ResponseCallback(handler, e.Result, null));
            WebClient client = sender as WebClient;
            client.Dispose();
        }
        public void RequestGetAsync(string url, HTTPClientEventHandler handler)
        {
            Instance.RequestGet(url, handler, new DownloadStringCompletedEventHandler(RequestResponseCallback));
        }
    }
}
