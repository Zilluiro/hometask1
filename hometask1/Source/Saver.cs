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

        public async Task SaveDataFileAsync(string filename, List<CityModel> model)
        {
            var folder = DateTime.Now.ToString("MM-dd-yyyy");
            var path = $"{_output.Directory}{Path.DirectorySeparatorChar}{folder}";
            var filepath = $"{path}{Path.DirectorySeparatorChar}{filename}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using var stream = File.Create(filepath);
            await JsonSerializer.SerializeAsync(stream, model);
        }

        public async Task SaveMetaFileAsync()
        {
            var folder = DateTime.Now.ToString("MM-dd-yyyy");
            await SaveMetaFileAsync(folder);
        }

        public async Task SaveMetaFileAsync(string folder)
        {
            var path = $"{_output.Directory}{Path.DirectorySeparatorChar}{folder}";
            var filename = "meta.log";
            var filepath = $"{path}{Path.DirectorySeparatorChar}{filename}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var meta = MetaStorage.GetInstance();
            var serialized = meta.ToString();

            await File.WriteAllTextAsync(filepath, serialized);
        }

        public void RemoveDirectory()
        {
            var folder = DateTime.Now.ToString("MM-dd-yyyy");
            var path = $"{_output.Directory}{Path.DirectorySeparatorChar}{folder}";

            var directory = new DirectoryInfo(path);

            if (Directory.Exists(path))
                directory.Delete(true);
        }
    }
}
