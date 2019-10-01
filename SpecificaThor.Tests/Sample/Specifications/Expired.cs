using SpecificaThor;
using System;

namespace SpecificaThor.Tests.Sample
{
    public class Expired : ISpecification<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot candidate)
            => $"Lot {candidate.LotNumber} is expired and cannot be used";

        public bool Validate(Lot candidate)
            => candidate.ExpirationDate.Date <= DateTime.Now.Date;
    }
}
