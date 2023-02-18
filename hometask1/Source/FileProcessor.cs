using hometask1.Source.Configurations;
using hometask1.Source.Handlers;
using hometask1.Source.Handlers.Implementations;

namespace hometask1.Source
{
    internal class FileProcessor
    {
        private readonly AppSettings _config;
        private readonly Saver _saver;
        private readonly int _fileCounter = 1;

        public FileProcessor(AppSettings config)
        {
            _config = config;
            _saver = new Saver(config.OutputConfiguration);
        }

        public void Start()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = _config.InputConfiguration.Directory;
            watcher.Filters.Add("*.txt");
            watcher.Filters.Add("*.csv");

            watcher.NotifyFilter = NotifyFilters.Attributes
                     | NotifyFilters.CreationTime
                     | NotifyFilters.DirectoryName
                     | NotifyFilters.FileName
                     | NotifyFilters.LastAccess
                     | NotifyFilters.LastWrite
                     | NotifyFilters.Security
                     | NotifyFilters.Size;

            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            await ProcessFile(e.FullPath);
        }

        private async Task ProcessFile(string path)
        {
            Console.WriteLine(path);
            var handler = HandlerFactory.GetHandler(path);

            var result = handler.Handle(path);
            await _saver.SaveAsync($"output{_fileCounter}.json", result);
        }
    }
}
