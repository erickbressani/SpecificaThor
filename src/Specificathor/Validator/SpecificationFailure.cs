using System;
using System.Diagnostics.CodeAnalysis;

namespace SpecificaThor
{
    internal class SpecificationFailure<TCandidate>
    {
        public string ValidationMessage { get; }
        public FailureType FailureType { get; }

        private readonly ISpecification<TCandidate> _specification;

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

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
            => GetSpecificationType() == (obj as SpecificationFailure<TCandidate>)?.GetSpecificationType();

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
            => _specification.GetHashCode();
    }
}
