using SpecificaThor;
using System;

namespace SampleImplementation
{
    public class AvailableOnStock : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot contract)
            => $"Lot {contract.LotNumber} is available on stock";

        public string GetErrorMessageWhenExpectingTrue(Lot contract)
            => $"Lot {contract.LotNumber} is not available on stock. Current Quantity: {contract.AvailableQuantity}";

        public bool Validate(Lot contract)
            => contract.AvailableQuantity > 0;
    }
}
