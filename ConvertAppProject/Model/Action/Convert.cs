using ConvertAppProject.Model.Conversion;
using ConvertAppProject.Services;
using ConvertAppProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConvertAppProject.Model.Action
{
    internal class Convert(FileConversion fileConversion, string defaultFileFormat) : Action(fileConversion, defaultFileFormat)
    {

        private readonly ConversionService _conversionService = new();

        protected readonly string CONVERTED_FILE_ENDPATH = "/FilesConverted";

        public string ChooseFormatToConvert(IEnumerable<string> formatListWithoutActualOne)
        {
            string sentence = $"\nChoose the format you want to convert your file (from {defaultFileFormat} to) :";

            foreach (var (item, index) in formatListWithoutActualOne.Select((value, index) => (value, index))) sentence += $"\n{index + 1}. {item}";
            int choice = Assets.DisplayMenuAndGetChoice(sentence, 0, formatListWithoutActualOne.Count());
            return formatListWithoutActualOne.ElementAt(choice - 1);
        }

        public override int Apply()
        {
            var formatListWithoutActualOne = from format in ALL_FORMAT where format != defaultFileFormat select format;
            string convertInto = ChooseFormatToConvert(formatListWithoutActualOne);

            switch (convertInto)
            {
                case "json":
                    if (defaultFileFormat == "xml")
                    {
                        fileConversion.GetXmlContext().RemoveUnwantedPropertiesInXmlFile();

                        string xmlToJson = _actionService.XmlToJson(fileConversion.GetXmlContext().GetXmlFile());
                        //Console.WriteLine(fileConversion.GetXmlContext().GetXmlFile());
                        //Console.WriteLine(xmlToJson);
                        string destinationFolderPath = _conversionService.GetAbsolutePathFrom(CONVERTED_FILE_ENDPATH);
                        Console.Write($"\nChoose a name for your {convertInto} file : ");
                        string convertedFileName = Console.ReadLine()!;
                        string finalFilePath = Path.Combine(destinationFolderPath, $"{convertedFileName}.{convertInto}");
                        File.WriteAllText(finalFilePath, xmlToJson);
                        Console.WriteLine($"\nFile created here : {finalFilePath}");
                    }
                    break;
                case "xml":
                    if (defaultFileFormat == "json")
                    {
                        fileConversion.GetJsonContext().RemoveUnwantedPropertiesInJsonFile();

                        XElement jsonToXml = _actionService.JsonToXml(fileConversion.GetJsonContext().GetJsonFile(), "Root");
                        string destinationFolderPath = _conversionService.GetAbsolutePathFrom(CONVERTED_FILE_ENDPATH);
                        Console.Write($"\nChoose a name for your {convertInto} file : ");
                        string convertedFileName = Console.ReadLine()!;
                        string finalFilePath = Path.Combine(destinationFolderPath, $"{convertedFileName}.{convertInto}");
                        jsonToXml.Save(finalFilePath);
                        Console.WriteLine($"\nFile created here : {finalFilePath}");
                    }
                    break;
            }
            return 0;
        }
    }
}
