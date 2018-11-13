namespace SpecificaThor
{
    internal class SpecificationValidatorDecorator<TContract>
    {
        public string CustomErrorMessage { get; set; }

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
            if (!string.IsNullOrEmpty(CustomErrorMessage))
                return CustomErrorMessage;
            else if (_expecting == Expecting.True)
                return (_specification as IHasDefaultExpectingTrueErrorMessage<TContract>)?.GetErrorMessageExpectingTrue(contract);

            return (_specification as IHasDefaultExpectingFalseErrorMessage<TContract>)?.GetErrorMessageExpectingFalse(contract);
        }
    }
}
