using System;

namespace SpecificaThor
{
    internal class SpecificationFailure<TCandidate>
    {
        public string ValidationMessage { get; private set; }
        public FailureType FailureType { get; private set; }

        private ISpecification<TCandidate> _specification;

        internal SpecificationFailure(ISpecification<TCandidate> specificationType, string validationMessage, FailureType failureType)
        {
            ValidationMessage = validationMessage;
            FailureType = failureType;
            _specification = specificationType;
        }

        public bool Is<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _specification is TSpecification;

        internal Type GetSpecificationType()
            => _specification.GetType();

        public override bool Equals(object obj)
            => GetSpecificationType() == (obj as SpecificationFailure<TCandidate>).GetSpecificationType();

        public override int GetHashCode()
            => base.GetHashCode();
    }
}
