using hometask1.Source.Configurations;
using System.Text.Json;

namespace hometask1.Source
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileContent = File.ReadAllText("appsettings.json");
            var appSettings = JsonSerializer.Deserialize<AppSettings>(fileContent);
            Console.WriteLine(appSettings.OutputConfiguration.Directory);

            var fileProcessor = new FileProcessor(appSettings);
            fileProcessor.Start();

            Console.ReadLine();
        }
    }
}