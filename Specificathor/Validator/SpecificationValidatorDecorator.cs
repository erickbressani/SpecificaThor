namespace SpecificaThor
{
    internal class SpecificationValidatorDecorator<TContract>
    {
        private Expecting _expecting;
        private ISpecification<TContract> _specification;

        private SpecificationValidatorDecorator(ISpecification<TContract> rule, Expecting expecting)
        {
            _specification = rule;
            _expecting = expecting;
        }

        public static SpecificationValidatorDecorator<TContract> CreateWithRule<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
        {
            return new SpecificationValidatorDecorator<TContract>(new TSpecification(), expecting);
        }

        public bool Validate(TContract contract)
        {
            return _specification.Validate(contract) && _expecting == Expecting.True;
        }

        public string GetErrorMessage(TContract contract)
        {
            if (_expecting == Expecting.True && _specification is IHasDefaultExpectingTrueErrorMessage<TContract>)
            {
                var specification = (_specification as IHasDefaultExpectingTrueErrorMessage<TContract>);
                return specification.GetErrorMessageExpectingTrue(contract);
            }
            else if (_expecting == Expecting.False && _specification is IHasDefaultExpectingFalseErrorMessage<TContract>)
            {
                var specification = (_specification as IHasDefaultExpectingFalseErrorMessage<TContract>);
                return specification.GetErrorMessageExpectingFalse(contract);
            }

            return null;
        }
    }
}
