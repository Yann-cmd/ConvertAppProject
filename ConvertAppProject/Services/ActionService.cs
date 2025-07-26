using ConvertAppProject.Model.Conversion;
using ConvertAppProject.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAppProject.Services
{
    internal class ActionService
    {
        public ActionService() { }

        public string ChooseAtributeToApplyAction(IEnumerable<string> attributesToSearch, string sentence)
        {
            foreach (var (item, index) in attributesToSearch.Select((value, index) => (value, index))) sentence += $"\n{index + 1}. {item}";
            int choice = Assets.DisplayMenuAndGetChoice(sentence, 0, attributesToSearch.Count());
            return attributesToSearch.ElementAt(choice - 1);
        }

        public XElement JsonToXml(JToken token, string name)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var properties = from propertie in ((JObject)token).Properties()
                                select JsonToXml(propertie.Value, propertie.Name);
                    return new XElement(name, properties);
                case JTokenType.Array:
                    var items = from item in ((JArray)token)
                                select JsonToXml(item, "Item");
                    return new XElement(name, items);
                default:
                    return new XElement(name, ((JValue)token).Value);

            }
        }
    }
}
