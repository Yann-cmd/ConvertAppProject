using ConvertAppProject.Services;
using ConvertAppProject.Model.Action;
using ConvertAppProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ConvertAppProject.Contexts;

namespace ConvertAppProject.Model.Conversion
{
    internal class FileConversion
    {
        protected readonly ConversionService _conversionService = new();

        private JsonContext? JsonContext { get; set; }

        private XmlContext? XmlContext { get; set; }

        private string FileToConvertPath { get; set; }

        public FileConversion()
        {
            JsonContext = null;
            XmlContext = null;
            FileToConvertPath = "";
        }

        public string GetFileName()
        {
            return Path.GetFileName(FileToConvertPath);
        }

        public string GetPath()
        {
            return FileToConvertPath;
        }

        public void SetPath(string path)
        {
            FileToConvertPath = path;
        }

        public JsonContext GetJsonContext()
        {
            return JsonContext ?? new("");
        }

        public XmlContext GetXmlContext()
        {
            return XmlContext ?? new("");
        }

        public void LaunchConversion()
        {
            ChooseFileToConvert();
            ApplyActions();
        }

        public void ChooseFileToConvert()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] filesPathToConvert = _conversionService.GetFilesPathFrom("/FilesToConvert");
            if (filesPathToConvert.Length <= 0)
            {
                Console.WriteLine("No files found. Be sure that you have upload a file or that your path is good.");
                return;
            }
            SetPath(_conversionService.ChooseFileToConvert(filesPathToConvert));
        }

        public void InitContextBasedOnFormat(string path, string fileToConvertFormat)
        {
            switch (fileToConvertFormat)
            {
                case "json":
                    JsonContext = new(path);
                    break;
                case "xml":
                    XmlContext = new(path);
                    break;
            }
        }

        public void ApplyActions()
        {
            int status = -1;
            Console.ForegroundColor = ConsoleColor.Yellow;
            string fileToConvertFormat = _conversionService.GetFormatFromPath(GetPath());
            InitContextBasedOnFormat(GetPath(), fileToConvertFormat);
            Console.WriteLine("");
            while (status != 0) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Type action = _conversionService.ChooseAction(GetFileName());
                Action.Action? actionChoose = (Action.Action?)Activator.CreateInstance(action, this, fileToConvertFormat);
                if (actionChoose is null)
                {
                    Assets.WriteColoredLine("\nError while creating actions.\n", ConsoleColor.Red);
                    return;
                }
                status = actionChoose.Apply();
                Console.Write("\n");
            }
        }
    }
}
