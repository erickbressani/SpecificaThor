using System;


namespace SpecificaThor
{
    public class Validator<TContract>
    {
        private bool _expected;
        private IRule<TContract> _rule;

        private Validator(IRule<TContract> rule, bool expected)
        {
            _rule = rule;
            _expected = expected;
        }

        public static Validator<TContract> CreateWithRule<TRule>(bool expected) where TRule : IRule<TContract>, new()
        {
            return new Validator<TContract>(new TRule(), expected);
        }

        public bool Validate(TContract contract)
        {
            return _rule.Validate(contract) == _expected;
        }

        public string GetErrorMessage(TContract contract)
        {
            return _rule.GetErrorMessage(contract);
        }
    }
}
