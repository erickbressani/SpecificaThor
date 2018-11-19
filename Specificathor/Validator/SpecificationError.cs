using System;

namespace SpecificaThor
{
    internal class SpecificationError<TContract>
    {
        private ISpecification<TContract> _specificationType;
        public string ErrorMessage { get; private set; }

        public SpecificationError(ISpecification<TContract> specificationType, string errorMessage)
        {
            _specificationType = specificationType;
            ErrorMessage = errorMessage;
        }

        public bool Is<TSpecification>() where TSpecification : ISpecification<TContract>
            => _specificationType is TSpecification;

        internal Type GetSpecificationType()
            => _specificationType.GetType();

        public override bool Equals(object obj)
            => GetSpecificationType() == (obj as SpecificationError<TContract>).GetSpecificationType();

        public override int GetHashCode()
            => base.GetHashCode();
    }
}
