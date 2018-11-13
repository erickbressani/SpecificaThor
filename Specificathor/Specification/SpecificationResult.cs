using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public class SpecificationResult
    {
        private readonly List<string> _errors;
        public bool IsValid { get; set; }

        public string ErrorMessage
        {
            get
            {
                return string.Join("\n", _errors.Where(error => !string.IsNullOrEmpty(error))
                                                .Distinct());
            }
        }

        public SpecificationResult()
        {
            _errors = new List<string>();
            IsValid = true;
        }

        internal void AddErrors(IEnumerable<string> errors)
            => _errors.AddRange(errors);
    }
}
