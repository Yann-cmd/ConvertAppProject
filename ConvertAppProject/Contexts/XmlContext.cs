using ConvertAppProject.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAppProject.Contexts
{
    internal class XmlContext
    {
        private XElement StoredXmlFile { get; set; }

        private IEnumerable<string> StoredXmlFileAttributes { get; set; }

        private string FirstAttributeName { get; set; }

        public XmlContext(string path)
        {
            FirstAttributeName = "";
            StoredXmlFile = XElement.Load(path);
            StoredXmlFileAttributes = GetAttributesInXmlFile();
        }

        public XElement GetXmlFile()
        {
            return StoredXmlFile;
        }

        public void SetXmlFile(XElement newXml)
        {
            StoredXmlFile = newXml;
        }

        public IEnumerable<string> GetXmlFileAttributes()
        {
            return StoredXmlFileAttributes;
        }

        public void SetXmlFileAttributes(IEnumerable<string> attributes)
        {
            StoredXmlFileAttributes = attributes;
        }

        public string GetFirstAttributeName()
        {
            return FirstAttributeName;
        }

        public void SetFirstAttributeName(string newName)
        {
            FirstAttributeName = newName;
        }

        // return the propertie (XElement object) of the XML file
        public IEnumerable<XElement> GetXmlPropertieToFilter()
        {
            return StoredXmlFile.Descendants()
                       .Where(e => e.HasElements && e.Elements().All(child => child.HasElements == false));
        }

        // get the attribute of each propertie of the XML file
        private IEnumerable<string> GetAttributesInXmlFile()
        {
            SetFirstAttributeName(StoredXmlFile.Name.ToString());
            var firstNode = StoredXmlFile.Descendants().FirstOrDefault(e => e.HasElements);
            if(firstNode is not null)
            {
                return from value in firstNode.Elements()
                       select value.Name.LocalName;

            }
            return [];
        }

        public void UpdateXmlFile(IEnumerable<XNode> valuesFiltered)
        {
            var updatedXmlFile = new XElement(GetFirstAttributeName(), valuesFiltered);
            SetXmlFile(updatedXmlFile);
            SetXmlFileAttributes(GetAttributesInXmlFile());
        }

        public void DisplayXmlResult(IEnumerable<XNode> valuesFiltered)
        {
            foreach (var valueFiltered in valuesFiltered)
            {
                string sentence = "";
                foreach (var attribute in GetXmlFileAttributes())
                {
                    sentence += $"|{((XElement)valueFiltered).Element(attribute)?.Value}  ";
                }
                Console.WriteLine(sentence);
            }
        }

        public void Update(IEnumerable<XNode> valuesFiltered)
        {

            // update the xml
            UpdateXmlFile(valuesFiltered);
            Console.Write("\n");
            // display the values
            DisplayXmlResult(valuesFiltered);
        }

        public void RemoveUnwantedPropertiesInXmlFile()
        {
            string allXmlAttributes = string.Join(" - ", GetXmlFileAttributes());
            Console.WriteLine("\nChoose the fields you want to remove for your export. I you want several of them, write their name followed by a space. After your choice, tap 'enter' (if you want to keep everything, enter 'NONE') : ");
            Assets.WriteColoredLine(allXmlAttributes, ConsoleColor.Blue);
            string propertieToRemove = Console.ReadLine()!;
            if (propertieToRemove.ToLower() != "none")
            {
                string[] propsToRemove = propertieToRemove.Split(' ');
                GetXmlFile().Descendants(propertieToRemove).Remove();
            }
        }
    }
}
