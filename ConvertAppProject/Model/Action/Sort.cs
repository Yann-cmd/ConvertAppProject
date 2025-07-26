using ConvertAppProject.Model.Conversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAppProject.Model.Action
{
    internal class Sort(FileConversion fileConversion, string defaultFileFormat) : Action(fileConversion, defaultFileFormat)
    {
        private IEnumerable<JToken> ConditionalJsonSort(string valueToSort, IEnumerable<JToken> propertyToFilter, string searchFilter)
        {
            if(valueToSort.ToLower() == "asc")
            {
                return from jsonValue in propertyToFilter
                       select jsonValue into Json
                       orderby Json[searchFilter] ascending
                       select Json;
            }
            if (valueToSort.ToLower() == "desc")
            {
                return from jsonValue in propertyToFilter
                       select jsonValue into Json
                       orderby Json[searchFilter] descending
                       select Json;
            }
            return [];
        }

        public override int Apply()
        {
            switch (FileFormat)
            {
                case "json":
                    IEnumerable<JToken> valuesFiltered = [];
                    IEnumerable<JToken> propertyToFilter = fileConversion.GetJsonContext().GetJsonPropertieToFilter();
                    string sortFilter = _actionService.ChooseAtributeToApplyAction(fileConversion.GetJsonContext().GetJsonFileAttributes(), "\nSort by :");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write($"\nEnter the value to sort by (ASC or DESC) : ");
                    string valueToSort = Console.ReadLine();

                    valuesFiltered = ConditionalJsonSort(valueToSort, propertyToFilter, sortFilter);

                    // if json empty, end the program
                    if (!valuesFiltered.Any()) return HandleBehaviorWhenSortedFileEmpty();

                    // if json not empty, do the update
                    fileConversion.GetJsonContext().Update(valuesFiltered);
                    break;
                case "xml":

                    break;
            }

            return 1;
        }
    }
}
