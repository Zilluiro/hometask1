using hometask1.Source.Data;
using hometask1.Source.Models;
using System.Globalization;

namespace hometask1.Source.Handlers.Implementations
{
    internal abstract class BaseHandler
    {
        protected readonly Dictionary<string, CityModel> _cities;

        protected BaseHandler()
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

        protected bool ProcessName(string name, PaymentInfo payment)
        {
            var normalizedName = name.Trim();
            if (string.IsNullOrEmpty(normalizedName))
                return false;

            payment.FirstName = normalizedName;
            return true;
        }

        protected bool ProcessSurname(string name, PaymentInfo payment)
        {
            var normalizedName = name.Trim();
            if (string.IsNullOrEmpty(normalizedName))
                return false;

            payment.LastName = normalizedName;
            return true;
        }

        protected bool ProcessAddress(string address, PaymentInfo payment)
        {
            var normalizedAddress = address.Trim();
            if (string.IsNullOrEmpty(normalizedAddress))
                return false;

            payment.Address = normalizedAddress;
            return true;
        }

        protected bool ProcessPayment(string paymentString, PaymentInfo paymentObject)
        {
            var result = decimal.TryParse(paymentString, out var payment);
            if (result)
                paymentObject.Payment = payment;

            return result;
        }

        protected bool ProcessDate(string dateString, PaymentInfo payment)
        {
            var result = DateTime.TryParseExact(dateString, "yyyy-dd-MM", new CultureInfo("en-US"),
                DateTimeStyles.None, out var date);

            if (result)
                payment.Date = date;

            return result;
        }

        protected bool ProcessAccountNumber(string accountNumberString, PaymentInfo payment)
        {
            var result = long.TryParse(accountNumberString, out var accountNumber);
            if (result)
                payment.AccountNumber = accountNumber;

            return result;
        }

        protected bool ProcessService(string serviceString, PaymentInfo payment)
        {
            var normalizedService = serviceString.Trim();
            if (string.IsNullOrEmpty(normalizedService))
                return false;

            payment.Service = normalizedService;
            return true;
        }
    }
}
