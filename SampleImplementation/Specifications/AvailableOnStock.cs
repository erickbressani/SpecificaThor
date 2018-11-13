using SpecificaThor;
using System;

namespace SampleImplementation
{
    public class AvailableOnStock : ISpecification<Lot>, IHasDefaultExpectingTrueErrorMessage<Lot>, IHasDefaultExpectingFalseErrorMessage<Lot>
    {
        public string GetErrorMessageExpectingFalse(Lot contract)
            => $"Lot {contract.LotNumber} is available on stock";

        public string GetErrorMessageExpectingTrue(Lot contract)
            => $"Lot {contract.LotNumber} is not available on stock. Current Quantity: {contract.AvailableQuantity}";

        public bool Validate(Lot contract)
            => contract.AvailableQuantity > 0;
    }
}
