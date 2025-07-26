using ConvertAppProject;
using ConvertAppProject.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAppProject.Services
{
    internal class ConversionService
    {
        private readonly string? ROOT_PATH = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;

        public ConversionService() { }

        public string GetAbsolutePathFrom(string path)
        {
            return ROOT_PATH + path;
        }

        public string[] GetFilesPathFrom(string path)
        {
            var filesToConvertPath = GetAbsolutePathFrom(path);
            return Directory.Exists(filesToConvertPath) ? Directory.GetFiles(filesToConvertPath) : [];
        }

        public string ChooseFileToConvert(string[] files)
        {
            string sentence = "Choose the file you want to convert :";

            foreach (var (item, index) in files.Select((value, index) => (value, index))) sentence += $"\n{index + 1}. {Path.GetFileName(item)}";
            int choice = Assets.DisplayMenuAndGetChoice(sentence, 0, files.Length);
            return files.ElementAt(choice - 1);
        }

        public string GetFormatFromPath(string path)
        {
            string fileName = Path.GetFileName(path);
            return fileName.Split('.')[1];
        }

        public Type ChooseAction(string fileName)
        {
            var allSubclasses = Assets.GetSubclassesOf<Model.Action.Action>();
            string sentence = $"Choose the actions you want to perform on your file ({fileName}) :";
            foreach (var (item, index) in allSubclasses.Select((value, index) => (value, index))) sentence += $"\n{index + 1}. {item.Name}";
            int choice = Assets.DisplayMenuAndGetChoice(sentence, 0, allSubclasses.Count());
            return allSubclasses.ElementAt(choice - 1);
        }
    }
}
