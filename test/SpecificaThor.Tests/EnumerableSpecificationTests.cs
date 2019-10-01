using SpecificaThor.Tests.Sample;
using SpecificaThor;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SpecificaThor.Tests
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

            IEnumerable<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .GetMatched();

            Assert.Equal(2, result.Count());
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

            IEnumerable<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .AndAre<Interdicted>()
                .AndAreNot<AvailableOnStock>()
                .GetMatched();

            Assert.Single(result);
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

            IEnumerable<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .AndAre<Interdicted>()
                .OrAre<AvailableOnStock>()
                .GetMatched();

            Assert.Equal(2, result.Count());
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

            IEnumerable<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .AndAre<Interdicted>()
                .AndAre<AvailableOnStock>()
                .GetMatched();

            Assert.False(result.Any());
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

            IEnumerable<Lot> result = lots
                .GetCandidates()
                .ThatAre<Expired>()
                .AndAre<Interdicted>()
                .GetMatched();

            Assert.Single(result);
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
                .AppendLine(new AvailableOnStock().GetErrorMessageWhenExpectingFalse(lot1))
                .Append(new Interdicted().GetErrorMessageWhenExpectingTrue(lot2))
                .ToString();

            ISpecificationResults<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .AndAre<Interdicted>()
                .AndAreNot<AvailableOnStock>()
                .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.False(result.ValidCandidates.Any());
            Assert.Equal(3, result.InvalidCandidates.Count());
            Assert.Equal(3, result.AllCandidates.Count());

            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());

            Assert.False(result.HasError<Expired>(lot1));
            Assert.False(result.HasError<Expired>(lot2));
            Assert.True(result.HasError<Expired>(lot3));

            Assert.True(result.HasError<Interdicted>(lot2));
            Assert.False(result.HasError<Interdicted>(lot1));
            Assert.False( result.HasError<Interdicted>(lot3));

            Assert.False(result.HasError<AvailableOnStock>(lot2));
            Assert.False(result.HasError<AvailableOnStock>(lot3));
            Assert.True(result.HasError<AvailableOnStock>(lot1));

            Assert.Equal(3, result.TotalOfErrors);
            Assert.Equal(0, result.TotalOfWarnings);

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

            ISpecificationResults<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAreNot<Expired>()
                .AndAre<Interdicted>()
                .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.Single(result.ValidCandidates);
            Assert.Equal(2, result.InvalidCandidates.Count());
            Assert.Equal(3, result.AllCandidates.Count());

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

            ISpecificationResults<Lot> result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .OrAre<Interdicted>()
                .GetResults();

            Assert.False(result.AreAllCandidatesValid);

            Assert.Single(result.ValidCandidates);
            Assert.Single(result.InvalidCandidates);
            Assert.Equal(2, result.AllCandidates.Count());

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

            var result = Specification
                .Create<Lot>(lots)
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

            Assert.Single(result.ValidCandidates);
            Assert.Single(result.InvalidCandidates);
            Assert.Equal(2, result.AllCandidates.Count());

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

            var result = Specification
                .Create<Lot>(lots)
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

            Assert.Single(result.ValidCandidates);
            Assert.Single(result.InvalidCandidates);
            Assert.Equal(2, result.AllCandidates.Count());

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

            var result = Specification
                .Create<Lot>(lots)
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

            Assert.Single(result.ValidCandidates);
            Assert.Single(result.InvalidCandidates);
            Assert.Equal(2, result.AllCandidates.Count());

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

            var result = Specification
                .Create<Lot>(lots)
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
            Assert.Equal(2, result.InvalidCandidates.Count());
            Assert.Equal(2, result.AllCandidates.Count());

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

            var result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .GetResults();

            Assert.Equal(string.Empty, result.ErrorMessages);
        }

        [Fact]
        public void NoErrorMessageWithCustomMessage()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                .NotExpired()
                .Build();

            var lot2 = new LotBuilder()
                .NotExpired()
                .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            const string customErrorMessage = "This is a Custom Error Message";
            string fullErrorMessage = new StringBuilder()
                .AppendLine(customErrorMessage)
                .Append(customErrorMessage)
                .ToString();

            var result = Specification
                .Create<Lot>(lots)
                .ThatAre<Expired>()
                .UseThisErrorMessageIfFails(customErrorMessage)
                .GetResults();

            Assert.Equal(fullErrorMessage, result.ErrorMessages);
        }

        [Fact]
        public void CustomMessage()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                .Expired()
                .Build();

            var lot2 = new LotBuilder()
                .Expired()
                .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            const string customErrorMessage = "This is a Custom Error Message";
            string fullErrorMessage = new StringBuilder()
                .AppendLine(customErrorMessage)
                .Append(customErrorMessage)
                .ToString();

            var result = Specification
                .Create<Lot>(lots)
                .ThatAreNot<Expired>()
                .UseThisErrorMessageIfFails(customErrorMessage)
                .GetResults();

            Assert.Equal(fullErrorMessage, result.ErrorMessages);
        }

        [Fact]
        public void AsWarning()
        {
            var lots = new List<Lot>();

            var lot1 = new LotBuilder()
                .Expired()
                .AvailableOnStock()
                .Build();

            var lot2 = new LotBuilder()
                .NotExpired()
                .AvailableOnStock()
                .Build();

            lots.Add(lot1);
            lots.Add(lot2);

            string expectedWarningMessage = new Expired().GetErrorMessageWhenExpectingFalse(lot2);

            var result = Specification
                .Create<Lot>(lots)
                .ThatAreNot<Expired>().AsWarning()
                .AndAre<AvailableOnStock>()
                .GetResults();

            Assert.True(result.AreAllCandidatesValid);
            Assert.Equal(0, result.TotalOfErrors);
            Assert.Equal(1, result.TotalOfWarnings);
            Assert.False(result.HasError<Expired>());
            Assert.True(result.HasWarning<Expired>());
            Assert.Equal(expectedWarningMessage, result.WarningMessages);
        }
    }
}
