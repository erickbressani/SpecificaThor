using SampleImplementation;
using SpecificaThor;
using System;
using System.Text;
using Xunit;

namespace Tests
{
    public class SingleSpecificationTests
    {
        [Fact]
        public void AllFaling()
        {
            var lot = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();

            string fullErrorMessage = new StringBuilder()
                                        .Append(new Expired().GetErrorMessageWhenExpectingFalse(lot))
                                        .Append("\n")
                                        .Append(new Interdicted().GetErrorMessageWhenExpectingFalse(lot))
                                        .Append("\n")
                                        .Append(new AvailableOnStock().GetErrorMessageWhenExpectingTrue(lot))
                                        .ToString();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIsNot<Interdicted>()
                                      .AndIs<AvailableOnStock>()
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.True(string.Equals(result.ErrorMessage, fullErrorMessage, StringComparison.InvariantCultureIgnoreCase));
            Assert.True(result.TotalOfErrors == 3);
            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<AvailableOnStock>());
        }

        [Fact]
        public void TwoFailuresOneSuccess()
        {
            var lot = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .AvailableOnStock()
                            .Build();

            string fullErrorMessage = new StringBuilder()
                                        .Append(new Expired().GetErrorMessageWhenExpectingFalse(lot))
                                        .Append("\n")
                                        .Append(new Interdicted().GetErrorMessageWhenExpectingFalse(lot))
                                        .ToString();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIsNot<Interdicted>()
                                      .AndIs<AvailableOnStock>()
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.Equal(result.ErrorMessage, fullErrorMessage);
            Assert.True(result.TotalOfErrors == 2);
            Assert.True(result.HasError<Expired>());
            Assert.True(result.HasError<Interdicted>());
            Assert.False(result.HasError<AvailableOnStock>());
        }

        [Fact]
        public void OrFirstFalse()
        {
            var lot = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .OrIs<AvailableOnStock>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<Expired>());
        }

        [Fact]
        public void OrFirstTrue()
        {
            var lot = new LotBuilder()
                            .Expired()
                            .AvailableOnStock()
                            .Build();

            var result = Specification.Create(lot)
                                      .Is<Expired>()
                                      .OrIsNot<AvailableOnStock>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<AvailableOnStock>());
        }

        [Fact]
        public void OrFirstGroupIsTrue()
        {
            var lot = new LotBuilder()
                             .NotExpired()
                             .NotInterdicted()
                             .AvailableOnStock()
                             .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIsNot<AvailableOnStock>()
                                      .AndIs<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<AvailableOnStock>());
            Assert.False(result.HasError<Interdicted>());
            Assert.False(result.HasError<Expired>());
        }

        [Fact]
        public void OrMiddleGroupIsTrue()
        {
            var lot = new LotBuilder()
                             .Expired()
                             .Interdicted()
                             .NotAvailableOnStock()
                             .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIsNot<AvailableOnStock>()
                                      .AndIs<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<AvailableOnStock>());
            Assert.False(result.HasError<Interdicted>());
            Assert.False(result.HasError<Expired>());
        }

        [Fact]
        public void OrLastGroupIsTrue()
        {
            var lot = new LotBuilder()
                             .Expired()
                             .NotInterdicted()
                             .AvailableOnStock()
                             .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIsNot<AvailableOnStock>()
                                      .AndIs<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<AvailableOnStock>());
            Assert.False(result.HasError<Interdicted>());
            Assert.False(result.HasError<Expired>());
        }

        [Fact]
        public void OrAllGroupsAreFalse()
        {
            var lot = new LotBuilder()
                             .Expired()
                             .AvailableOnStock()
                             .Interdicted()
                             .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIsNot<AvailableOnStock>()
                                      .AndIs<Interdicted>()
                                      .OrIs<Expired>()
                                      .AndIs<AvailableOnStock>()
                                      .AndIsNot<Interdicted>()
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.True(result.TotalOfErrors == 3);
            Assert.True(result.HasError<AvailableOnStock>());
            Assert.True(result.HasError<Interdicted>());
            Assert.True(result.HasError<Expired>());
        }

        [Fact]
        public void NoErrorMessage()
        {
            var lot = new LotBuilder()
                             .NotExpired()
                             .Build();

            var result = Specification.Create(lot)
                                      .Is<Expired>()
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.True(result.TotalOfErrors == 1);
            Assert.True(result.HasError<Expired>());
            Assert.Equal(result.ErrorMessage, string.Empty);
        }

        [Fact]
        public void CustomMessage()
        {
            const string customErrorMessage = "This is a custom error message";

            var lot = new LotBuilder()
                              .Expired()
                              .Build();

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .UseThisErrorMessageIfFails(customErrorMessage)
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.Equal(result.ErrorMessage, customErrorMessage);
            Assert.True(result.TotalOfErrors == 1);
            Assert.True(result.HasError<Expired>());
        }

        [Fact]
        public void NoErrorMessageWithCustomMessage()
        {
            const string customErrorMessage = "This is a custom error message";

            var lot = new LotBuilder()
                             .NotExpired()
                             .Build();

            var result = Specification.Create(lot)
                                      .Is<Expired>()
                                      .UseThisErrorMessageIfFails(customErrorMessage)
                                      .GetResult();

            Assert.False(result.IsValid);
            Assert.Equal(result.ErrorMessage, customErrorMessage);
            Assert.True(result.TotalOfErrors == 1);
            Assert.True(result.HasError<Expired>());
        }

        [Fact]
        public void AsWarning()
        {
            var lot = new LotBuilder()
                              .Expired()
                              .AvailableOnStock()
                              .Build();

            string expectedWarningMessage = new Expired().GetErrorMessageWhenExpectingFalse(lot);

            var result = Specification.Create(lot)
                                      .IsNot<Expired>().AsWarning()
                                      .AndIs<AvailableOnStock>()
                                      .GetResult();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.True(result.TotalOfWarnings == 1);
            Assert.False(result.HasError<Expired>());
            Assert.True(result.HasWarning<Expired>());
            Assert.Equal(result.WarningMessage, expectedWarningMessage);
        }
    }
}
