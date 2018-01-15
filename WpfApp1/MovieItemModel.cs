using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class MovieItemModel
    {
        public string name { get; set; }
        public string size { get; set; }
        public string count { get; set; }
        public string source { get; set; }
        public string magnet { get; set; }

        public static MovieItemModel ModelWithDocument(HtmlNode pNode,RuleItemModel ruleItem)
        {
            
            MovieItemModel movieItem = new MovieItemModel();
            var node = pNode.SelectNodes(ruleItem.magnet)[0];
            string firstMagnet = node.Attributes["href"].Value;
            if (firstMagnet.StartsWith(".html") || firstMagnet.EndsWith(".html"))
            {
                firstMagnet = firstMagnet.Replace(".html", "");
            }
            if (firstMagnet.Split('&').Length > 1)
            {
                firstMagnet = firstMagnet.Split('&')[0];
            }
            string magnet = firstMagnet.Substring(firstMagnet.Length - 40, 40);
            movieItem.magnet = String.Format("magnet:?xt=urn:btih:{0}", magnet);
            movieItem.name = node.InnerText;
            node = FirstChildWIthXPath(pNode, ruleItem.size);
            movieItem.size = node.InnerText;
            movieItem.count = FirstChildWIthXPath(pNode, ruleItem.count).InnerText;
            movieItem.source = ruleItem.site;
            return movieItem;
        }

        private static HtmlNode FirstChildWIthXPath(HtmlNode pNode,string xPath)
        {
            return pNode.SelectNodes(xPath)[0];
        }
    }
}
