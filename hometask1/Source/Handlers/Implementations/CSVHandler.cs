﻿using hometask1.Source.Data;
using hometask1.Source.Handlers.Interfaces;
using hometask1.Source.Models;
using System.Text.RegularExpressions;

namespace hometask1.Source.Handlers.Implementations
{
    internal class CSVHandler : BaseHandler, IDataHandler
    {
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }

        public List<CityModel> Handle(string filepath)
        {
            // skip a header and process a file line by line
            var lines = File.ReadLines(filepath).Skip(1);
            foreach (var line in lines)
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

            for (int i = 1; i < match.Groups.Count; i++)
            {
                var chunk = match.Groups[i].Value;
                var processor = processors[i - 1];
                var success = processor(chunk, paymentInfo);

                if (!success)
                    return false;
            }

            return true;
        }
    }
}
