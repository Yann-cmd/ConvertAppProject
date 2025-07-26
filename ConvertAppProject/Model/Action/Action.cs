using ConvertAppProject.Model.Conversion;
using ConvertAppProject.Services;
using ConvertAppProject.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAppProject.Model.Action
{
    internal abstract class Action(FileConversion fileConversion, string fileFormat)
    {
        protected readonly ActionService _actionService = new();

        protected readonly string[] ALL_FORMAT = ["json", "xml"];

        protected FileConversion FileConversion { get; } = fileConversion;

        protected string FileFormat { get; } = fileFormat;

        public abstract int Apply();

        protected int HandleBehaviorWhenSortedFileEmpty()
        {
            Assets.WriteColoredLine("\nYour file is empty after applying your action. End of program.", ConsoleColor.Red);
            return 0;
        }
    }
}
