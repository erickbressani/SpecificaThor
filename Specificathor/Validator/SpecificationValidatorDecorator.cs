namespace SpecificaThor
{
    internal class SpecificationValidatorDecorator<TContract>
    {
        public string CustomErrorMessage { get; set; }

        private Expecting _expecting;
        private ISpecification<TContract> _specification;

        private SpecificationValidatorDecorator(ISpecification<TContract> specification, Expecting expecting)
        {
            _specification = specification;
            _expecting = expecting;
        }

        public static SpecificationValidatorDecorator<TContract> CreateWithSpecification<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
            => new SpecificationValidatorDecorator<TContract>(new TSpecification(), expecting);

        public bool Validate(TContract contract)
        {
            bool expecting = _expecting == Expecting.True;
            return _specification.Validate(contract) == expecting;
        }

        public SpecificationError<TContract> GetSpecificationError(TContract contract)
        {
            string errorMessage = GetErrorMessage(contract);
            return new SpecificationError<TContract>(_specification, errorMessage);
        }

        private string GetErrorMessage(TContract contract)
        {
            if (!string.IsNullOrEmpty(CustomErrorMessage))
                return CustomErrorMessage;
            else if (_expecting == Expecting.True)
                return (_specification as IHasErrorMessageWhenExpectingTrue<TContract>)?.GetErrorMessageWhenExpectingTrue(contract);

            return (_specification as IHasErrorMessageWhenExpectingFalse<TContract>)?.GetErrorMessageWhenExpectingFalse(contract);
        }
    }
}
