using hometask1.Source.Models;

namespace hometask1.Source.Handlers.Interfaces
{
    internal interface IDataHandler
    {
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }

        public List<CityModel> Handle(string filepath);
    }
}
