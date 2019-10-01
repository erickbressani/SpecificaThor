using SpecificaThor;

namespace SpecificaThor.Tests.Sample
{
    public class Interdicted : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot candidate)
            => $"Lot {candidate.LotNumber} is interdicted and needs to be replaced";

        public string GetErrorMessageWhenExpectingTrue(Lot candidate)
            => $"Lot {candidate.LotNumber} is not interdicted";

        public bool Validate(Lot candidate)
            => candidate.IsInterdicted;
    }
}
