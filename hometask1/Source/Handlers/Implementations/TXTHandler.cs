using hometask1.Source.Data;
using hometask1.Source.Handlers.Interfaces;
using hometask1.Source.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace hometask1.Source.Handlers.Implementations
{
    internal class TXTHandler : BaseHandler, IDataHandler
    {
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }

        public List<CityModel> Handle(string filepath)
        {
            // process a file line by line
            var lines = File.ReadLines(filepath);
            foreach(var line in lines)
            {
                if (!ProcessLine(line, out var paymentInfo))
                {
                    FoundErrors++;
                    continue;
                }

                var cityName = paymentInfo.GetCityName();
                if (!TryStoreInformation(cityName, paymentInfo))
                {
                    FoundErrors++;
                    continue;
                }

                ParsedLines++;
            }

            return _cities.Select(x => x.Value).ToList();
        }

        // the order of processing is <first_name: string>, <last_name: string>,
        // <address: string>, <payment: decimal>, <date: date>,
        // <account_number: long>, <service: string>
        private bool ProcessLine(string line, out PaymentInfo paymentInfo)
        {
            paymentInfo = new PaymentInfo();

            var pattern = "^(\\w+), (\\w+), “(.*?)”, ([\\d.]+), (\\d{4}-\\d{2}-\\d{2}), (\\d+), (\\w+)$";
            var match = Regex.Match(line, pattern);
            if (!match.Success)
                return false;

            var processors = new List<Func<string, PaymentInfo, bool>>() {ProcessName, ProcessSurname, ProcessAddress,
                ProcessPayment, ProcessDate, ProcessAccountNumber, ProcessService };

            for(int i = 1; i < match.Groups.Count; i++)
            {
                var chunk = match.Groups[i].Value;
                var processor = processors[i - 1];
                var success = processor(chunk, paymentInfo);

                if (!success)
                    return false;
            }

            return true;
        }

        private bool ProcessName(string name, PaymentInfo payment)
        {
            var normalizedName = name.Trim();
            if (string.IsNullOrEmpty(normalizedName))
                return false;

            payment.FirstName = normalizedName;
            return true;
        }

        private bool ProcessSurname(string name, PaymentInfo payment)
        {
            var normalizedName = name.Trim();
            if (string.IsNullOrEmpty(normalizedName))
                return false;

            payment.LastName = normalizedName;
            return true;
        }

        private bool ProcessAddress(string address, PaymentInfo payment)
        {
            var normalizedAddress = address.Trim();
            if (string.IsNullOrEmpty(normalizedAddress))
                return false;

            payment.Address = normalizedAddress;
            return true;
        }

        private bool ProcessPayment(string paymentString, PaymentInfo paymentObject)
        {
            var result = decimal.TryParse(paymentString, out var payment);
            if (result)
                paymentObject.Payment = payment;

            return result;
        }

        private bool ProcessDate(string dateString, PaymentInfo payment)
        {
            var result = DateTime.TryParseExact(dateString, "yyyy-dd-MM", new CultureInfo("en-US"),
                DateTimeStyles.None, out var date);

            if (result)
                payment.Date = date;

            return result;
        }

        private bool ProcessAccountNumber(string accountNumberString, PaymentInfo payment)
        {
            var result = long.TryParse(accountNumberString, out var accountNumber);
            if (result)
                payment.AccountNumber = accountNumber;

            return result;
        }

        private bool ProcessService(string serviceString, PaymentInfo payment)
        {
            var normalizedService = serviceString.Trim();
            if (string.IsNullOrEmpty(normalizedService))
                return false;

            payment.Service = normalizedService;
            return true;
        }
    }
}
