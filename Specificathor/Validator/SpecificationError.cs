using System;

namespace SpecificaThor
{
    internal class SpecificationError<TContract>
    {
        private ISpecification<TContract> _specification;
        public string ErrorMessage { get; private set; }

        public SpecificationError(ISpecification<TContract> specificationType, string errorMessage)
        {
            _specification = specificationType;
            ErrorMessage = errorMessage;
        }

        public bool Is<TSpecification>() where TSpecification : ISpecification<TContract>
            => _specification is TSpecification;

        internal Type GetSpecificationType()
            => _specification.GetType();

        public override bool Equals(object obj)
            => GetSpecificationType() == (obj as SpecificationError<TContract>).GetSpecificationType();

        public override int GetHashCode()
            => base.GetHashCode();
    }
}
