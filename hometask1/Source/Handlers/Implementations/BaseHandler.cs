using hometask1.Source.Data;
using hometask1.Source.Models;

namespace hometask1.Source.Handlers.Implementations
{
    internal abstract class BaseHandler
    {
        protected readonly Dictionary<string, CityModel> _cities;

        public BaseHandler()
        {
            _cities = new Dictionary<string, CityModel>();
        }

        protected bool TryStoreInformation(string cityName, PaymentInfo paymentInfo)
        {
            if (!string.IsNullOrEmpty(cityName))
            {
                if (!_cities.TryGetValue(cityName, out var cityModel))
                {
                    cityModel = new CityModel(cityName);
                    _cities.Add(cityName, cityModel);
                }

                cityModel.AddUpdateServices(paymentInfo);
            }
            else
                return false;

            return true;
        }
    }
}
