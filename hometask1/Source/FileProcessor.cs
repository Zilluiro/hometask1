using hometask1.Source.Configurations;
using hometask1.Source.Exceptions;
using hometask1.Source.Handlers;

namespace hometask1.Source
{
    internal class FileProcessor
    {
        private readonly AppSettings _config;
        private readonly Saver _saver;
        private readonly Logger _logger;
        private readonly MetaStorage _metaStorage;

        public bool _stopped = false;
        private int _jobs = 0;
        private object _lock = new object();

        public FileProcessor(AppSettings config)
        {
            _config = config;
            _saver = new Saver(config.OutputConfiguration);
            _logger = new Logger();
            _metaStorage = MetaStorage.GetInstance();
        }

        public async Task Start()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = _config.InputConfiguration.Directory;

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

            _saver.RemoveDirectory();
            _logger.Log($"The service is running.");
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            _logger.Log($"New file has been spotted. Name: {e.Name}");
            if (_stopped)
            {
                _logger.Log($"The service cannot process new files due to shutdown.");
                return;
            }

            Task task = new Task(async () => await ProcessFile(e.FullPath));
            task.Start();
        }

        private async Task ProcessFile(string path)
        {
            lock (_lock)
            {
                _jobs += 1;
            }

            try
            {
                var handler = HandlerFactory.GetHandler(path);
                var result = handler.Handle(path);
                var totalFiles = _metaStorage.UpdateCounters(1, handler.ParsedLines, handler.FoundErrors);

                await _saver.SaveDataFileAsync($"output{totalFiles}.json", result);
            }
            catch(HandlerNotFound)
            {
                _metaStorage.AddInvalidFile(path);
            }

            lock (_lock)
            {
                _jobs -= 1;
            }
        }

        public void Reset()
        {
            _metaStorage.Reset();
            _saver.RemoveDirectory();

            _logger.Log("The service has been reset.");
        }

        public async Task Stop()
        {
            while(_jobs > 0)
            {
                _logger.Log($"Wait until all tasks are completed ({_jobs}).");
                await Task.Delay(3000);
            }

            await _saver.SaveMetaFileAsync();
            _logger.Log("The service has stopped.");
        }
    }
}
