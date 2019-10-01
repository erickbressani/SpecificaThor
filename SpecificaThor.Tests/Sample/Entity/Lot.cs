using System;

namespace SpecificaThor.Tests.Sample
{
    public class Lot
    {
        public long Id { get; set; }
        public string LotNumber { get; set; }
        public bool IsInterdicted { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
