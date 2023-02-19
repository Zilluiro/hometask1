using hometask1.Source.Configurations;
using System.Text.Json;

namespace hometask1.Source
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var fileContent = File.ReadAllText("appsettings.json");
            var appSettings = JsonSerializer.Deserialize<AppSettings>(fileContent);

            var fileProcessor = new FileProcessor(appSettings);
            await fileProcessor.Start();
            while (true)
            {
                Console.WriteLine("Please select an action:");
                Console.WriteLine("1.reset\n2.stop");
                var command = Console.ReadLine();

                switch (command)
                {
                    case "1": { fileProcessor.Reset(); break; }
                    case "2": { await fileProcessor.Stop(); return; }
                }
            }
        }
    }
}