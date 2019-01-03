using SampleImplementation;
using SpecificaThor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SampleTests
{
    public class EnumerableSpecificationTests
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
                                                   .AndAreNot<AvailableOnStock>()
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

            IEnumerable<Lot> result = lots.GetCandidates()
                                          .ThatAre<Expired>()
                                          .AndAre<Interdicted>()
                                          .GetMatched();

            Assert.True(result.Count() == 1);
            Assert.Contains(lot1, result);
            Assert.DoesNotContain(lot2, result);
            Assert.DoesNotContain(lot3, result);
        }

        [Fact]
        public void AllFaling()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .LotNumber("Lot1")
                            .Expired()
                            .Interdicted()
                            .AvailableOnStock()
                            .Build();

            var lot2 = new LotBuilder()
                            .LotNumber("Lot2")
                            .Expired()
                            .NotInterdicted()
                            .NotAvailableOnStock()
                            .Build();

            var lot3 = new LotBuilder()
                            .LotNumber("Lot3")
                            .NotExpired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            string fullErrorMessages = new StringBuilder()
                                        .Append(new AvailableOnStock().GetErrorMessageWhenExpectingFalse(lot1))
                                        .Append($"\n{new Interdicted().GetErrorMessageWhenExpectingTrue(lot2)}")
                                        .ToString();

            ISpecificationResults<Lot> result = Specification.Create<Lot>(lots)
                                                             .ThatAre<Expired>()
                                                             .AndAre<Interdicted>()
                                                             .AndAreNot<AvailableOnStock>()
                                                             .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.False(result.ValidCandidates.Any());
            Assert.True(result.InvalidCandidates.Count() == 3);
            Assert.True(result.AllCandidates.Count() == 3);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.False(result.HasError<Expired>(lot1) || result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Expired>(lot3));

            Assert.True(result.HasError<Interdicted>(lot2));
            Assert.False(result.HasError<Interdicted>(lot1) || result.HasError<Interdicted>(lot3));

            Assert.False(result.HasError<AvailableOnStock>(lot2) || result.HasError<AvailableOnStock>(lot3));
            Assert.True(result.HasError<AvailableOnStock>(lot1));

            Assert.Equal(fullErrorMessages, result.ErrorMessages);
        }

        [Fact]
        public void TwoFailuresOneSuccess()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .NotInterdicted()
                            .Build();

            var lot3 = new LotBuilder()
                            .NotExpired()
                            .NotInterdicted()
                            .Build();
            lots.Add(lot1);
            lots.Add(lot2);
            lots.Add(lot3);

            ISpecificationResults<Lot> result = Specification.Create<Lot>(lots)
                                                             .ThatAreNot<Expired>()
                                                             .AndAre<Interdicted>()
                                                             .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.True(result.ValidCandidates.Count() == 1);
            Assert.True(result.InvalidCandidates.Count() == 2);
            Assert.True(result.AllCandidates.Count() == 3);

            Assert.Contains(lot1, result.ValidCandidates);
            Assert.Contains(lot2, result.InvalidCandidates);
            Assert.Contains(lot3, result.InvalidCandidates);

            Assert.False(result.HasError<AvailableOnStock>());
            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());

            Assert.True(result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Interdicted>(lot2));

            Assert.False(result.HasError<Expired>(lot3));
            Assert.True(result.HasError<Interdicted>(lot3));
        }

        [Fact]
        public void SimpleOr()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .Interdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .NotExpired()
                            .NotInterdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            ISpecificationResults<Lot> result = Specification.Create<Lot>(lots)
                                                             .ThatAre<Expired>()
                                                             .OrAre<Interdicted>()
                                                             .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.True(result.ValidCandidates.Count() == 1);
            Assert.True(result.InvalidCandidates.Count() == 1);
            Assert.True(result.AllCandidates.Count() == 2);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());

            Assert.False(result.HasError<Expired>(lot1));
            Assert.True(result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Interdicted>(lot2));
        }

        [Fact]
        public void OrFirstGroupIsTrue()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .AvailableOnStock()
                            .NotInterdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Interdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            var result = Specification.Create<Lot>(lots)
                                      .ThatAreNot<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAreNot<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.True(result.ValidCandidates.Count() == 1);
            Assert.True(result.InvalidCandidates.Count() == 1);
            Assert.True(result.AllCandidates.Count() == 2);

            Assert.Contains(lot1, result.ValidCandidates);
            Assert.Contains(lot2, result.InvalidCandidates);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.False(result.HasError<Expired>(lot1));
            Assert.False(result.HasError<Interdicted>(lot1));
            Assert.False(result.HasError<AvailableOnStock>(lot1));

            Assert.True(result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Interdicted>(lot2));
            Assert.True(result.HasError<AvailableOnStock>(lot2));
        }

        [Fact]
        public void OrMiddleGroupIsTrue()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .AvailableOnStock()
                            .NotInterdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Interdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            var result = Specification.Create<Lot>(lots)
                                      .ThatAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAre<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAreNot<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.True(result.ValidCandidates.Count() == 1);
            Assert.True(result.InvalidCandidates.Count() == 1);
            Assert.True(result.AllCandidates.Count() == 2);

            Assert.Contains(lot2, result.ValidCandidates);
            Assert.Contains(lot1, result.InvalidCandidates);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.True(result.HasError<Expired>(lot1));
            Assert.True(result.HasError<Interdicted>(lot1));
            Assert.True(result.HasError<AvailableOnStock>(lot1));

            Assert.False(result.HasError<Expired>(lot2));
            Assert.False(result.HasError<Interdicted>(lot2));
            Assert.False(result.HasError<AvailableOnStock>(lot2));
        }

        [Fact]
        public void OrLastGroupIsTrue()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .AvailableOnStock()
                            .NotInterdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Interdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            var result = Specification.Create<Lot>(lots)
                                      .ThatAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAreNot<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAre<Interdicted>()
                                      .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.True(result.ValidCandidates.Count() == 1);
            Assert.True(result.InvalidCandidates.Count() == 1);
            Assert.True(result.AllCandidates.Count() == 2);

            Assert.Contains(lot2, result.ValidCandidates);
            Assert.Contains(lot1, result.InvalidCandidates);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.True(result.HasError<Expired>(lot1));
            Assert.True(result.HasError<Interdicted>(lot1));
            Assert.True(result.HasError<AvailableOnStock>(lot1));

            Assert.False(result.HasError<Expired>(lot2));
            Assert.False(result.HasError<Interdicted>(lot2));
            Assert.False(result.HasError<AvailableOnStock>(lot2));
        }

        [Fact]
        public void OrAllGroupsAreFalse()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .LotNumber("Lot1")
                            .NotExpired()
                            .AvailableOnStock()
                            .NotInterdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .LotNumber("Lot2")
                            .Expired()
                            .AvailableOnStock()
                            .Interdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            var result = Specification.Create<Lot>(lots)
                                      .ThatAre<Expired>()
                                      .AndAre<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAre<Expired>()
                                      .AndAreNot<AvailableOnStock>()
                                      .AndAreNot<Interdicted>()
                                      .OrAreNot<Expired>()
                                      .AndAreNot<AvailableOnStock>()
                                      .AndAre<Interdicted>()
                                      .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.False(result.ValidCandidates.Any());
            Assert.True(result.InvalidCandidates.Count() == 2);
            Assert.True(result.AllCandidates.Count() == 2);

            Assert.Contains(lot2, result.InvalidCandidates);
            Assert.Contains(lot1, result.InvalidCandidates);

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.True(result.HasError<Expired>(lot1));
            Assert.True(result.HasError<Interdicted>(lot1));
            Assert.True(result.HasError<AvailableOnStock>(lot1));

            Assert.True(result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Interdicted>(lot2));
            Assert.True(result.HasError<AvailableOnStock>(lot2));
        }

        [Fact]
        public void NoErrorMessage()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                            .NotExpired()
                            .AvailableOnStock()
                            .NotInterdicted()
                            .Build();

            var lot2 = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Interdicted()
                            .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            var result = Specification.Create<Lot>(lots)
                                      .ThatAre<Expired>()
                                      .GetResults();

            Assert.True(string.IsNullOrEmpty(result.ErrorMessages));
        }
    }
}
