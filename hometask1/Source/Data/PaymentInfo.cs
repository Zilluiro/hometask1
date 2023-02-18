namespace hometask1.Source.Data
{
    internal class PaymentInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public decimal Payment { get; set; }

        public DateTime Date { get; set; }

        public long AccountNumber { get; set; }

        public string Service { get; set; }

        public string GetCityName()
        {
            if (!string.IsNullOrEmpty(Address))
                return Address.Split(',').FirstOrDefault();

            return default;
        }
    }
}
