using SampleImplementation;
using SpecificaThor;
using System;
using Xunit;

namespace SampleTests
{
    public class SpecificationTests
    {
        [Fact]
        public void TestLot()
        {
            var lot = new Lot()
            {
                ExpirationDate = DateTime.Now.AddDays(-5),
                LotNumber = "aaaa2323",
                Id = 1,
                AvailableQuantity = 2,
                IsInterdicted = true
            };

            var result = Specification.Create<Lot>(lot)
                                      .IsNot<Expired>()
                                      .UseThisErrorMessageIfFails("aaa")
                                      .AndIsNot<Interdicted>()
                                      .AndIs<AvailableOnStock>()
                                      .IsSatisfied();
        }
    }
}
