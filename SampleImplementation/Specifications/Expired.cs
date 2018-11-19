﻿using SpecificaThor;
using System;

namespace SampleImplementation
{
    public class Expired : ISpecification<Lot>, IHasErrorMessageWhenExpectingFalse<Lot>
    {
        public string GetErrorMessageWhenExpectingFalse(Lot contract)
            => $"Lot {contract.LotNumber} is expired and connot be used";

        public bool Validate(Lot contract)
            => contract.ExpirationDate.Date <= DateTime.Now.Date;
    }
}
