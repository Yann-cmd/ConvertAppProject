using ConvertAppProject.Model.Conversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAppProject.Model.Action
{
    internal class Group(FileConversion fileConversion, string defaultFileFormat) : Action(fileConversion, defaultFileFormat)
    {
        public override int Apply()
        {
            switch (FileFormat)
            {
                case "json":
                    IEnumerable<IGrouping<JToken, JToken>> valuesFiltered = [];
                    IEnumerable<JToken> propertyToFilter = fileConversion.GetJsonContext().GetJsonPropertieToFilter();

                    string groupFilter = _actionService.ChooseAtributeToApplyAction(fileConversion.GetJsonContext().GetJsonFileAttributes(), "\nGroup by :");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    valuesFiltered = from jsonValue in propertyToFilter
                                     select jsonValue into Json
                                     group Json by Json[groupFilter];

                    // if json empty, end the program
                    if (!valuesFiltered.Any()) return HandleBehaviorWhenSortedFileEmpty();

                    // if json not empty, do the update
                    fileConversion.GetJsonContext().UpdateJsonFile(valuesFiltered.SelectMany(group => group));
                    Console.Write("\n");
                    foreach (var valueFiltered in valuesFiltered) fileConversion.GetJsonContext().DisplayJsonResult(valueFiltered);
                    break;
                case "xml":
                    IEnumerable<IGrouping<string, XNode>> valuesFilteredXml = [];
                    IEnumerable<XElement> propertyToFilterXml = fileConversion.GetXmlContext().GetXmlPropertieToFilter();

                    string groupFilterXml = _actionService.ChooseAtributeToApplyAction(fileConversion.GetXmlContext().GetXmlFileAttributes(), "\nGroup by :");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    valuesFilteredXml = from xmlValue in propertyToFilterXml
                                     select xmlValue into Xml
                                     group Xml by Xml.Element(groupFilterXml)?.Value;

                    // if xml empty, end the program
                    if (!valuesFilteredXml.Any()) return HandleBehaviorWhenSortedFileEmpty();

                    // if xml not empty, do the update
                    fileConversion.GetXmlContext().UpdateXmlFile(valuesFilteredXml.SelectMany(group => group));
                    Console.Write("\n");
                    foreach (var valueFiltered in valuesFilteredXml) fileConversion.GetXmlContext().DisplayXmlResult(valueFiltered);
                    break;
            }

            return 1;
        }
    }
}
