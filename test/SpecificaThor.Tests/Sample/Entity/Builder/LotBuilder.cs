using System;
using System.Diagnostics.CodeAnalysis;

namespace SpecificaThor.Tests.Sample
{
    [ExcludeFromCodeCoverage]
    public class LotBuilder
    {
        private readonly Lot _lot;

        public LotBuilder()
            => _lot = new Lot() { LotNumber = "FooBar123" };

        public LotBuilder LotNumber(string lotNumber)
        {
            _lot.LotNumber = lotNumber;
            return this;
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
            => _lot;
    }
}
