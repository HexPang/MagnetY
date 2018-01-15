using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Util
    {
        public static List<RuleItemModel> LoadRule()
        {
            string ruleJson = File.ReadAllText(@"res\rule.json");
            List<RuleItemModel> list = JsonConvert.DeserializeObject<List<RuleItemModel>>(ruleJson);
            return list;
        }
    }
}
