using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public class ValidationGroup<TContract>
    {
        private List<Validator<TContract>> _validations;

        public ValidationGroup()
        {
            _validations = new List<Validator<TContract>>();
        }

        public void AddToGroup<TRule>(bool expected) where TRule : IRule<TContract>, new()
        {
            var validator = Validator<TContract>.CreateWithRule<TRule>(expected);
            _validations.Add(validator);
        }

        public bool IsGroupValid(TContract contract)
        {
            return _validations.TrueForAll(validator => validator.Validate(contract));
        }

        public IEnumerable<string> GetFailures(TContract contract)
        {
            var failures = _validations.FindAll(validator => !validator.Validate(contract));

            foreach (Validator<TContract> validator in failures)
                yield return validator.GetErrorMessage(contract);
        }
    }

    public static class ValidationGroupExtensions
    {
        public static void AddGroup<TContract>(this List<ValidationGroup<TContract>> source)
        {
            source.Add(new ValidationGroup<TContract>());
        }

        public static void AddToGroup<TRule, TContract>(this List<ValidationGroup<TContract>> source, bool expected) where TRule : IRule<TContract>, new()
        {
            source.Last().AddToGroup<TRule>(expected);
        }
    }
}
