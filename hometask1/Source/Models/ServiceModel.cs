using hometask1.Source.Data;
using System.Collections.ObjectModel;

namespace hometask1.Source.Models
{
    internal class ServiceModel
    {
        public string Name { get; set; }
        private readonly Dictionary<long, PayerModel> _payers;
        public List<PayerModel> Payers => _payers.Select(x => x.Value).ToList();
        public decimal Total { get; set; }

        public ServiceModel(string name)
        {
            Name = name;
            _payers = new Dictionary<long, PayerModel>();
        }

        public void AddUpdateClient(PaymentInfo payment)
        {
            var account = payment.AccountNumber;
            if (!_payers.TryGetValue(account, out var payer))
            {
                payer = new PayerModel()
                {
                    Name = $"{payment.FirstName} {payment.LastName}",
                    Payment = payment.Payment,
                    Date = payment.Date,
                    AccountNumber = payment.AccountNumber
                };

                _payers.Add(payer.AccountNumber, payer);
            }
            else
                payer.Payment += payment.Payment;

            Total += payment.Payment;
        }
    }
}
