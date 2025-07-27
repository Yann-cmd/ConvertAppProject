using ConvertAppProject.Model.Conversion;
using ConvertAppProject.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAppProject.Contexts
{
    internal class JsonContext
    {
        private JObject StoredJsonFile { get; set; }

        private IEnumerable<string> StoredJsonFileAttributes { get; set; }

        private string FirstAttributeName { get; set; }

        public JsonContext(string path) 
        {
            FirstAttributeName = "";
            StoredJsonFile = JObject.Parse(File.ReadAllText(path));
            StoredJsonFileAttributes = GetAttributesInJsonFile();

        }

        public JObject GetJsonFile()
        {
            return StoredJsonFile;
        }

        public void SetJsonFile(JObject newJson)
        {
            StoredJsonFile = newJson;
        }

        public IEnumerable<string> GetJsonFileAttributes()
        {
            return StoredJsonFileAttributes;
        }

        public void SetJsonFileAttributes(IEnumerable<string> attributes)
        {
            StoredJsonFileAttributes = attributes;
        }

        public string GetFirstAttributeName()
        {
            return FirstAttributeName;
        }

        public void SetFirstAttributeName(string newName)
        {
            FirstAttributeName = newName;
        }

        // check if first attribute of JSON file is an array
        public bool IsFirstJsonAttributeIsArray()
        {
            return StoredJsonFile.Properties().First().Value.Type == JTokenType.Array;
        }

        // return the propertie (json object) of the JSON file
        public IEnumerable<JToken> GetJsonPropertieToFilter()
        {
            return IsFirstJsonAttributeIsArray()
                        ? StoredJsonFile.Properties().First().Value.Children()
                        : new List<JToken> { StoredJsonFile };
        }

        // get the attribute of each propertie of the JSON file
        private IEnumerable<string> GetAttributesInJsonFile()
        {
            IEnumerable<string> arrayValues = [];
            foreach (var property in StoredJsonFile.Properties())
            {
                // handle case when we have an array in json
                if (IsFirstJsonAttributeIsArray())
                {
                    SetFirstAttributeName(property.Name);
                    arrayValues = from props in ((JObject)property.Value.First()).Properties()
                                  select props.Name;
                }
            }
            if (arrayValues.Any()) return arrayValues;

            // default case, means we have an object
            return from prop in StoredJsonFile.Properties()
                   select prop.Name;
        }

        public void UpdateJsonFile(IEnumerable<JToken> valuesFiltered)
        {
            var updatedJsonFile = IsFirstJsonAttributeIsArray() ? new JObject { { GetFirstAttributeName(), new JArray(valuesFiltered) } } : (JObject)valuesFiltered.First();
            SetJsonFile(updatedJsonFile);
            SetJsonFileAttributes(GetAttributesInJsonFile());
        }

        public void DisplayJsonResult(IEnumerable<JToken>  valuesFiltered)
        {
            foreach (var valueFiltered in valuesFiltered)
            {
                string sentence = "";
                foreach (var attribute in GetJsonFileAttributes())
                {
                    sentence += $"|{valueFiltered[attribute]}  ";
                }
                Console.WriteLine(sentence);
            }
        }

        public void Update(IEnumerable<JToken> valuesFiltered)
        {

            // update the json
            UpdateJsonFile(valuesFiltered);
            Console.Write("\n");
            // display the values
            DisplayJsonResult(valuesFiltered);
        }

        public void RemoveUnwantedPropertiesInJsonFile()
        {
            string allJsonAttributes = string.Join(" - ", GetJsonFileAttributes());
            Console.WriteLine("\nChoose the fields you want to remove for your export. I you want several of them, write their name followed by a space. After your choice, tap 'enter' (if you want to keep everything, enter 'NONE') : ");
            Assets.WriteColoredLine(allJsonAttributes, ConsoleColor.Blue);
            string propertieToRemove = Console.ReadLine()!;
            if (propertieToRemove.ToLower() != "none")
            {
                string[] propsToRemove = propertieToRemove.Split(' ');
                if (IsFirstJsonAttributeIsArray())
                {
                    foreach (var propertie in GetJsonFile()[GetFirstAttributeName()])
                    {
                        foreach (var props in propsToRemove)
                        {
                            if (propertie[props] != null) propertie[props]?.Parent.Remove();
                        }
                    }
                }
                else
                {
                    foreach (var props in propsToRemove)
                    {
                        if (GetJsonFile()[props] != null) GetJsonFile()[props]?.Parent.Remove();
                    }
                }
            }
        }
    }
}
