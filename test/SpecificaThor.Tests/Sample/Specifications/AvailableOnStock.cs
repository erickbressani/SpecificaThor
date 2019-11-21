using System.Diagnostics.CodeAnalysis;

namespace SpecificaThor.Tests.Sample
{
    [ExcludeFromCodeCoverage]
    public class AvailableOnStock : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>
    {
        public string GetErrorMessageWhenExpectingTrue(Lot candidate)
            => $"Lot {candidate.LotNumber} is not available on stock. Current Quantity: {candidate.AvailableQuantity}";

        public bool Validate(Lot candidate)
            => candidate.AvailableQuantity > 0;
    }
}
