using System.Text;

namespace hometask1.Source
{
    internal class MetaStorage
    {
        public int ParsedFiles { get; private set; }
        public int ParsedLines { get; private set; }
        public int FoundErrors { get; private set; }

        private readonly List<string> InvalidFiles = new List<string>();
        private readonly object _lock = new object();

        private MetaStorage() { }

        private static MetaStorage _instance;

        public static MetaStorage GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MetaStorage();
            }
            return _instance;
        }

        public void Reset()
        {
            lock (_lock)
            {
                ParsedFiles = 0;
                ParsedLines = 0;
                FoundErrors = 0;
                InvalidFiles.Clear();
            }
        }

        public int UpdateCounters(int newFiles, int newLines, int newErrors)
        {
            lock (_lock)
            {
                ParsedFiles += newFiles;
                ParsedLines += newLines;
                FoundErrors += newErrors;

                return ParsedFiles;
            }
        }

        public void AddInvalidFile(string file)
        {
            lock (_lock)
            {
                InvalidFiles.Add(file);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            lock (_lock)
            {
                sb.AppendLine($"parsed_files: {ParsedFiles}");
                sb.AppendLine($"parsed_lines: {ParsedLines}");
                sb.AppendLine($"found_errors: {FoundErrors}");
                sb.AppendLine($"invalid_files: [{string.Join(",", InvalidFiles)}]");
            }

            return sb.ToString();
        }
    }
}
