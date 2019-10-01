using SpecificaThor;

namespace SpecificaThor.Tests.Sample
{
    public class AvailableOnStock : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot candidate)
            => $"Lot {candidate.LotNumber} is available on stock";

        public string GetErrorMessageWhenExpectingTrue(Lot candidate)
            => $"Lot {candidate.LotNumber} is not available on stock. Current Quantity: {candidate.AvailableQuantity}";

        public bool Validate(Lot candidate)
            => candidate.AvailableQuantity > 0;
    }
}
