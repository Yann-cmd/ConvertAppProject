using ConvertAppProject.Model.Conversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAppProject.Model.Action
{
    internal class Search(FileConversion fileConversion, string fileFormat) : Action(fileConversion, fileFormat)
    {

        public override int Apply()
        {
            switch (FileFormat)
            {
                case "json":
                    IEnumerable<JToken> valuesFiltered = [];
                    IEnumerable<JToken> propertyToFilter = fileConversion.GetJsonContext().GetJsonPropertieToFilter();

                    string searchFilter = _actionService.ChooseAtributeToApplyAction(fileConversion.GetJsonContext().GetJsonFileAttributes(), "\nSearch by :");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"\nEnter the value to search by {searchFilter} : ");
                    string valueToFilter = Console.ReadLine();
                    valuesFiltered = from jsonValue in propertyToFilter
                                     where jsonValue[searchFilter].ToString().Contains(valueToFilter!, StringComparison.OrdinalIgnoreCase)
                                     select jsonValue;

                    // if json empty, end the program
                    if (!valuesFiltered.Any()) return HandleBehaviorWhenSortedFileEmpty();

                    // if json not empty, do the update
                    fileConversion.GetJsonContext().Update(valuesFiltered);
                    break;
                case "xml":
                    IEnumerable<XNode> valuesFilteredXml = [];
                    IEnumerable<XElement> propertyToFilterXml = fileConversion.GetXmlContext().GetXmlPropertieToFilter();

                    string searchFilterXml = _actionService.ChooseAtributeToApplyAction(fileConversion.GetXmlContext().GetXmlFileAttributes(), "\nSearch by :"); Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"\nEnter the value to search by {searchFilterXml} : ");
                    string valueToFilterXml = Console.ReadLine();

                    valuesFilteredXml = from xmlValue in propertyToFilterXml
                                        where xmlValue.Element(searchFilterXml).ToString().Contains(valueToFilterXml!, StringComparison.OrdinalIgnoreCase)
                                        select xmlValue;

                    // if xml empty, end the program
                    if (!valuesFilteredXml.Any()) return HandleBehaviorWhenSortedFileEmpty();

                    //if xml not empty, do the update
                    fileConversion.GetXmlContext().Update(valuesFilteredXml);
                    break;
            }

            return 1;
        }
    }
}
