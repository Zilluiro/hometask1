using hometask1.Source.Handlers.Interfaces;
using hometask1.Source.Models;

namespace hometask1.Source.Handlers.Implementations
{
    internal class CSVHandler : IDataHandler
    {
        public int ParsedLines { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int FoundErrors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        List<CityModel> IDataHandler.Handle(string filepath)
        {
            throw new NotImplementedException();
        }
    }
}
