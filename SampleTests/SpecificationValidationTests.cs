using SampleImplementation;
using SpecificaThor;
using System;
using Xunit;

namespace SampleTests
{
    public class SpecificationValidationTests
    {
        [Fact]
        public void AllFaling()
        {
            var lot = new LotBuilder()
                            .Expired()
                            .Interdicted()
                            .NotAvailableOnStock()
                            .Build();


            string fullErrorMessage = string.Join("\n",
                                        new Expired().GetErrorMessageWhenExpectingFalse(lot),
                                        new Interdicted().GetErrorMessageWhenExpectingFalse(lot),
                                        new AvailableOnStock().GetErrorMessageWhenExpectingTrue(lot));

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIsNot<Interdicted>()
                                      .AndIs<AvailableOnStock>()
                                      .IsSatisfied();

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

            string fullErrorMessage = string.Join("\n",
                                        new Expired().GetErrorMessageWhenExpectingFalse(lot),
                                        new Interdicted().GetErrorMessageWhenExpectingFalse(lot));

            var result = Specification.Create(lot)
                                      .IsNot<Expired>()
                                      .AndIsNot<Interdicted>()
                                      .AndIs<AvailableOnStock>()
                                      .IsSatisfied();

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
                                      .IsSatisfied();

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
                                      .IsSatisfied();

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
                                      .IsSatisfied();

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
                                      .IsSatisfied();

            Assert.True(result.IsValid);
            Assert.True(result.TotalOfErrors == 0);
            Assert.False(result.HasError<AvailableOnStock>());
            Assert.False(result.HasError<Interdicted>());
            Assert.False(result.HasError<Expired>());
        }

        [Fact]
        public void OrFinalGroupIsTrue()
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
                                      .IsSatisfied();

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
                                      .IsSatisfied();


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
                                      .IsSatisfied();

            Assert.False(result.IsValid);
            Assert.True(result.TotalOfErrors == 1);
            Assert.True(result.HasError<Expired>());
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
                                      .IsSatisfied();

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
                                      .IsSatisfied();

            Assert.False(result.IsValid);
            Assert.Equal(result.ErrorMessage, customErrorMessage);
            Assert.True(result.TotalOfErrors == 1);
            Assert.True(result.HasError<Expired>());
        }
    }
}