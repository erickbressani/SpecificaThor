using SampleImplementation;
using System;

namespace SampleTests
{
    public class LotBuilder
    {
        private Lot _lot;

        public LotBuilder()
        {
            _lot = new Lot() { LotNumber = "FooBar123" };
        }

        public LotBuilder Expired()
        {
            _lot.ExpirationDate = DateTime.Now.AddDays(-5);
            return this;
        }

        public LotBuilder NotExpired()
        {
            _lot.ExpirationDate = DateTime.Now.AddDays(5);
            return this;
        }

        public LotBuilder AvailableOnStock()
        {
            _lot.AvailableQuantity = 10;
            return this;
        }

        public LotBuilder NotAvailableOnStock()
        {
            _lot.AvailableQuantity = 0;
            return this;
        }

        public LotBuilder Interdicted()
        {
            _lot.IsInterdicted = true;
            return this;
        }

        public LotBuilder NotInterdicted()
        {
            _lot.IsInterdicted = false;
            return this;
        }

        public Lot Build()
        {
            return _lot;
        }
    }
}
