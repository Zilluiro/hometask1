using hometask1.Source.Configurations.Interfaces;
using hometask1.Source.Models;
using System.Text.Json;

namespace hometask1.Source
{
    internal class Saver
    {
        private readonly IConfiguration _output;

        public Saver(IConfiguration output)
        {
            _output = output;
        }

        public async Task SaveAsync(string filename, List<CityModel> model)
        {
            var folder = DateTime.Now.ToString("MM-dd-yyyy");
            var path = $"{_output.Directory}{Path.DirectorySeparatorChar}{folder}";
            var filepath = $"{path}{Path.DirectorySeparatorChar}{filename}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using var stream = File.Create(filepath);
            await JsonSerializer.SerializeAsync(stream, model);
        }

        public void RemoveDirectory()
        {
            var folder = DateTime.Now.ToString("MM-dd-yyyy");
            var path = $"{_output.Directory}{Path.DirectorySeparatorChar}{folder}";

            if (!IsDirectoryEmpty(path))
            {
                var directory = new DirectoryInfo(path);
                directory.Delete();
            }
        }

        private bool IsDirectoryEmpty(string path)
        {
            var items = Directory.EnumerateFileSystemEntries(path);
            using var enumerator = items.GetEnumerator();
            return !enumerator.MoveNext();
        }
    }
}
