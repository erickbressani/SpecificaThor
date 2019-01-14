using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SpecificaThor
{
    [DebuggerDisplay("SpecificationResults: AreAllCandidatesValid = {AreAllCandidatesValid}")]
    internal class SpecificationResults<TCandidate> : ISpecificationResults<TCandidate>
    {
        public string ErrorMessages => _errorMessageBuilder.ToString();
        public string WarningMessages => _warningMessageBuilder.ToString();
        public int TotalOfErrors => _resultsPerCandidate.Sum(result => result.TotalOfErrors);
        public int TotalOfWarnings => _resultsPerCandidate.Sum(result => result.TotalOfWarnings);
        public bool AreAllCandidatesValid => !InvalidCandidates.Any();
        public IEnumerable<TCandidate> ValidCandidates => _validCandidates;
        public IEnumerable<TCandidate> InvalidCandidates => _invalidCandidates;
        public IEnumerable<TCandidate> AllCandidates => _allCandidates;

        private readonly List<ISpecificationResult<TCandidate>> _resultsPerCandidate;
        private readonly List<TCandidate> _validCandidates;
        private readonly List<TCandidate> _invalidCandidates;
        private readonly List<TCandidate> _allCandidates;
        private readonly StringBuilder _errorMessageBuilder;
        private readonly StringBuilder _warningMessageBuilder;

        internal SpecificationResults()
        {
            _validCandidates = new List<TCandidate>();
            _invalidCandidates = new List<TCandidate>();
            _allCandidates = new List<TCandidate>();
            _resultsPerCandidate = new List<ISpecificationResult<TCandidate>>();
            _errorMessageBuilder = new StringBuilder();
            _warningMessageBuilder = new StringBuilder();
        }

        public bool HasError<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _resultsPerCandidate.Any(result => result.HasError<TSpecification>());

        public bool HasWarning<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _resultsPerCandidate.Any(result => result.HasWarning<TSpecification>());

        public bool HasError<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>
        {
            var result = _resultsPerCandidate.FirstOrDefault(resultPerCandidate => resultPerCandidate.Candidate.Equals(candidate));

            if (result == null)
                return false;

            return result.HasError<TSpecification>();
        }

        public bool HasWarning<TSpecification>(TCandidate candidate) where TSpecification : ISpecification<TCandidate>
        {
            var result = _resultsPerCandidate.FirstOrDefault(resultPerCandidate => resultPerCandidate.Candidate.Equals(candidate));

            if (result == null)
                return false;

            return result.HasWarning<TSpecification>();
        }

        internal void Add(ISpecificationResult<TCandidate> result)
        {
            if (result.IsValid)
                _validCandidates.Add(result.Candidate);
            else
            {
                _invalidCandidates.Add(result.Candidate);
                _errorMessageBuilder.AppendMessage(result.ErrorMessage);
            }

            if (!string.IsNullOrEmpty(result.WarningMessage))
                _warningMessageBuilder.AppendMessage(result.WarningMessage);

            _allCandidates.Add(result.Candidate);
            _resultsPerCandidate.Add(result);
        }
    }
}
