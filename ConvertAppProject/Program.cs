// See https://aka.ms/new-console-template for more information
using ConvertAppProject.Model.Conversion;
using ConvertAppProject.Contexts;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to your conversion tool !\n");
        FileConversion conversion = new();
        conversion.LaunchConversion();
    }
}