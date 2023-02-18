using hometask1.Source.Data;

namespace hometask1.Source.Models
{
    internal class CityModel
    {
        public string City { get; set; }
        private readonly Dictionary<string, ServiceModel> _services;
        public List<ServiceModel> Services => _services.Select(x => x.Value).ToList();
        public decimal Total { get; set; }

        public CityModel(string cityName)
        {
            City = cityName;
            _services = new Dictionary<string, ServiceModel>();
        }

        public void AddUpdateServices(PaymentInfo payment)
        {
            var serviceName = payment.Service;
            if (!_services.TryGetValue(serviceName, out var service))
            {
                service = new ServiceModel(serviceName);
                _services.Add(serviceName, service);
            }

            service.AddUpdateClient(payment);

            Total += payment.Payment;
        }
    }
}
