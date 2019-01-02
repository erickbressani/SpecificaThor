using System;

namespace SpecificaThor
{
    internal class SpecificationError<TCandidate>
    {
        private ISpecification<TCandidate> _specification;
        public string ErrorMessage { get; private set; }

        public SpecificationError(ISpecification<TCandidate> specificationType, string errorMessage)
        {
            _specification = specificationType;
            ErrorMessage = errorMessage;
        }

        public bool Is<TSpecification>() where TSpecification : ISpecification<TCandidate>
            => _specification is TSpecification;

        internal Type GetSpecificationType()
            => _specification.GetType();

        public override bool Equals(object obj)
            => GetSpecificationType() == (obj as SpecificationError<TCandidate>).GetSpecificationType();

        public override int GetHashCode()
            => base.GetHashCode();
    }
}
