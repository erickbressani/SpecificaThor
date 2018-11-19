using SpecificaThor;

namespace SampleImplementation
{
    public class Interdicted : ISpecification<Lot>, IHasErrorMessageWhenExpectingTrue<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot contract)
            => $"Lot {contract.LotNumber} is interdicted and needs to be replaced";

        public string GetErrorMessageWhenExpectingTrue(Lot contract)
            => $"Lot {contract.LotNumber} is not interdicted";

        public bool Validate(Lot contract)
            => contract.IsInterdicted;
    }
}
