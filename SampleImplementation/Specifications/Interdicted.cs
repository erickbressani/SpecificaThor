using SpecificaThor;

namespace SampleImplementation
{
    public class Interdicted : ISpecification<Lot>, IHasDefaultExpectingTrueErrorMessage<Lot>, IHasDefaultExpectingFalseErrorMessage<Lot>
    {
        public string GetErrorMessageExpectingFalse(Lot contract)
            => $"Lot {contract.LotNumber} is interdicted and needs to be replaced";

        public string GetErrorMessageExpectingTrue(Lot contract)
            => $"Lot {contract.LotNumber} is not interdicted";

        public bool Validate(Lot contract)
            => contract.IsInterdicted;
    }
}
