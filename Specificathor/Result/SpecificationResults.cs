using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpecificaThor
{
    [DebuggerDisplay("SpecificationResult: AreAllCandidatesValid = {AreAllCandidatesValid}")]
    internal class SpecificationResults<TCandidate> : ISpecificationResults<TCandidate>
    {
        public string ErrorMessages => _errorMessageBuilder.ToString();
        public bool AreAllCandidatesValid => !InvalidCandidates.Any();
        public IEnumerable<TCandidate> ValidCandidates => _validCandidates;
        public IEnumerable<TCandidate> InvalidCandidates => _invalidCandidates;
        public IEnumerable<TCandidate> AllCandidates => _allCandidates;

        private readonly List<KeyValuePair<TCandidate, ISpecificationResult<TCandidate>>> _resultsPerCandidate;
        private readonly List<TCandidate> _validCandidates;
        private readonly List<TCandidate> _invalidCandidates;
        private readonly List<TCandidate> _allCandidates;
        private readonly StringBuilder _errorMessageBuilder;

        internal SpecificationResults()
        {
            _validCandidates = new List<TCandidate>();
            _invalidCandidates = new List<TCandidate>();
            _allCandidates = new List<TCandidate>();
            _resultsPerCandidate = new List<KeyValuePair<TCandidate, ISpecificationResult<TCandidate>>>();
            _errorMessageBuilder = new StringBuilder();
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _resultsPerCandidate.Any(result => result.Value.HasError<TSpecification>());

        public bool HasError<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>
        {
            var result = _resultsPerCandidate.Where(resultPerCandidate => resultPerCandidate.Key.Equals(candidate))
                                             .Select(resultPerCandidate => resultPerCandidate.Value)
                                             .FirstOrDefault();

            if (result == null)
                return false;

            return result.HasError<TSpecification>();
        }

        internal void Add(TCandidate candidate, ISpecificationResult<TCandidate> result)
        {
            if (result.IsValid)
                _validCandidates.Add(candidate);
            else
            {
                _invalidCandidates.Add(candidate);
                _errorMessageBuilder.AppendMessage(result.ErrorMessage);
            }

            _allCandidates.Add(candidate);
            _resultsPerCandidate.Add(new KeyValuePair<TCandidate, ISpecificationResult<TCandidate>>(candidate, result));
        }
    }
}
