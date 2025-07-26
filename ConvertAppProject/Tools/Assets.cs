using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConvertAppProject.Tools
{
    public static class Assets
    {

        /**
        * Return an enumerable containing all the class that are subclass of T
        * <typeparam name="T"> The parent class </typeparam>
        * <returns> An IEnumerable<Type> containing all the subclass type of T </returns>
        */
        public static IEnumerable<Type> GetSubclassesOf<T>()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsClass && type.IsSubclassOf(typeof(T)));
        }

        /**
         * Display a customizable menu and return the number corresponding to the choice
         * <param name="startSentence"> The title menu sentence </param>
         * <param name="mustBeGreatherThan"> The minimum index choice authorized </param>
         * <param name="mustBeLowerThan"> The maximum index choice authorized </param>
         * <returns> The index choice </returns>
         */
        public static int DisplayMenuAndGetChoice(string startSentence, int mustBeGreatherThan, int mustBeLowerThan)
        {
            int choix;
            Console.WriteLine(startSentence);

            while (true)
            {
                string? choice = Console.ReadLine();
                if (int.TryParse(choice, out choix) && choix > mustBeGreatherThan && choix <= mustBeLowerThan)
                {
                    break;
                }
                else
                {
                    WriteColoredLine("\nIncorrect input. Enter the corresponding number.\n", ConsoleColor.Red);
                }
                Console.WriteLine(startSentence);

            }
            return choix;
        }

        /**
         * Display a customizable colored line and colored only the line
         * <param name="text"> The text to be colored </param>
         * <param name="color"> The color to apply </param>
         * <param name="backLine"> Optionnal : boolean to define if the text need to apply a backline at the end or not </param>
         * <returns> Display the text colored by the designated color </returns>
         */
        public static void WriteColoredLine(string text, ConsoleColor color, bool backLine = true)
        {
            // actual color
            ConsoleColor previousColor = Console.ForegroundColor;

            // Change do desired color
            Console.ForegroundColor = color;
            if (backLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }

            // get back to actual color
            Console.ForegroundColor = previousColor;
        }
    }
}
