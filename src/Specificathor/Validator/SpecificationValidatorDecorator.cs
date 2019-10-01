namespace SpecificaThor
{
    internal sealed class SpecificationValidatorDecorator<TCandidate>
    {
        public string CustomErrorMessage { get; set; }
        public FailureType FailureType { get; set; }

        private readonly Expecting _expecting;
        private readonly ISpecification<TCandidate> _specification;

        private SpecificationValidatorDecorator(ISpecification<TCandidate> specification, Expecting expecting)
        {
            _specification = specification;
            _expecting = expecting;
            FailureType = FailureType.Error;
        }

        public static SpecificationValidatorDecorator<TCandidate> CreateWithSpecification<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TCandidate>, new()
            => new SpecificationValidatorDecorator<TCandidate>(new TSpecification(), expecting);

        public bool Validate(TCandidate candidate)
        {
            bool expecting = _expecting == Expecting.True;
            return _specification.Validate(candidate) == expecting;
        }

        public SpecificationFailure<TCandidate> GetSpecificationError(TCandidate candidate)
        {
            string errorMessage = GetErrorMessage(candidate);
            return new SpecificationFailure<TCandidate>(_specification, errorMessage, FailureType);
        }

        private string GetErrorMessage(TCandidate candidate)
        {
            if (!string.IsNullOrEmpty(CustomErrorMessage))
                return CustomErrorMessage;
            else if (_expecting == Expecting.True)
                return (_specification as IHasErrorMessageWhenExpectingTrue<TCandidate>)?.GetErrorMessageWhenExpectingTrue(candidate);

            return (_specification as IHasErrorMessageWhenExpectingFalse<TCandidate>)?.GetErrorMessageWhenExpectingFalse(candidate);
        }
    }
}
