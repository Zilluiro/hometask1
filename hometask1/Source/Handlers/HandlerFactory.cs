using hometask1.Source.Handlers.Implementations;
using hometask1.Source.Handlers.Interfaces;

namespace hometask1.Source.Handlers
{
    internal static class HandlerFactory
    {
        public static IDataHandler GetHandler(string file)
        {
            if (file.EndsWith(".txt"))
                return new TXTHandler();

            if (file.EndsWith(".csv"))
                return new CSVHandler();

            throw new ArgumentException("Wrong filetype");
        }
    }
}
