using SampleImplementation;
using SpecificaThor;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SampleTests
{
    public class SpecificationFilterTests
    {
        [Fact]
        public void FilterOneCondition()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                                   .ThatAre<Expired>()
                                                   .GetMatched();

            Assert.True(result.Count() == 2);
            Assert.Contains(lot1, result);
            Assert.Contains(lot2, result);
            Assert.DoesNotContain(lot3, result);
        }

        [Fact]
        public void AndCondition()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .NotInterdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                                   .ThatAre<Expired>()
                                                   .AndAre<Interdicted>()
                                                   .GetMatched();

            Assert.True(result.Count() == 1);
            Assert.Contains(lot1, result);
            Assert.DoesNotContain(lot2, result);
            Assert.DoesNotContain(lot3, result);
        }

        [Fact]
        public void OrCondition()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .NotInterdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .AvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                                   .ThatAre<Expired>()
                                                   .AndAre<Interdicted>()
                                                   .OrAre<AvailableOnStock>()
                                                   .GetMatched();

            Assert.True(result.Count() == 2);
            Assert.Contains(lot1, result);
            Assert.DoesNotContain(lot2, result);
            Assert.Contains(lot3, result);
        }

        [Fact]
        public void ReturnsEmpty()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .NotInterdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .AvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            IEnumerable<Lot> result = Specification.Create<Lot>(lots)
                                                   .ThatAre<Expired>()
                                                   .AndAre<Interdicted>()
                                                   .AndAre<AvailableOnStock>()
                                                   .GetMatched();

            Assert.True(result.Count() == 0);
        }

        [Fact]
        public void EnumerableExtension()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .NotInterdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            IEnumerable<Lot> result = lots.GetSubjects()
                                          .ThatAre<Expired>()
                                          .AndAre<Interdicted>()
                                          .GetMatched();

            Assert.True(result.Count() == 1);
            Assert.Contains(lot1, result);
            Assert.DoesNotContain(lot2, result);
            Assert.DoesNotContain(lot3, result);
        }
    }
}
