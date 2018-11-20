using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static ValidationSpecification<TContract> Create<TContract>(TContract contract)
            => new ValidationSpecification<TContract>(contract);

        public sealed class ValidationSpecification<TValidationContract>
        {
            private readonly TValidationContract _contract;

            internal ValidationSpecification(TValidationContract contract)
            {
                _contract = contract;
            }

            public ContractOperator<TValidationContract> Is<TSpecification>() where TSpecification : ISpecification<TValidationContract>, new()
                => CreateOperator<TSpecification>(Expecting.True);

            public ContractOperator<TValidationContract> IsNot<TSpecification>() where TSpecification : ISpecification<TValidationContract>, new()
                => CreateOperator<TSpecification>(Expecting.False);

            private ContractOperator<TValidationContract> CreateOperator<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TValidationContract>, new()
            {
                var contractOperator = new ContractOperator<TValidationContract>(_contract);
                contractOperator.AddToGroup<TSpecification>(expecting);
                return contractOperator;
            }

            public sealed class ContractOperator<TOperatorContract>
            {
                private readonly TOperatorContract _contract;
                private readonly SpecificationResult<TOperatorContract> _result;
                private readonly List<ValidationGroup<TOperatorContract>> _validationGroups;

                internal ContractOperator(TOperatorContract contract)
                {
                    _contract = contract;
                    _result = new SpecificationResult<TOperatorContract>();
                    _validationGroups = new List<ValidationGroup<TOperatorContract>>();
                    _validationGroups.AddGroup();
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TOperatorContract>, new()
                    => _validationGroups.AddToGroup<TSpecification, TOperatorContract>(expecting);

                public ContractOperator<TOperatorContract> OrIs<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.True);
                    return this;
                }

                public ContractOperator<TOperatorContract> AndIs<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.True);
                    return this;
                }

                public ContractOperator<TOperatorContract> OrIsNot<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.False);
                    return this;
                }

                public ContractOperator<TOperatorContract> AndIsNot<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.False);
                    return this;
                }

                public ContractOperator<TOperatorContract> UseThisErrorMessageIfFails(string errorMessage)
                {
                    _validationGroups.GetLastAddedValidator().CustomErrorMessage = errorMessage;
                    return this;
                }

                public SpecificationResult<TOperatorContract> GetResult()
                {
                    foreach (ValidationGroup<TOperatorContract> validationGroup in _validationGroups)
                    {
                        var errors = validationGroup.GetFailures(_contract);
                        _result.IsValid = !errors.Any();

                        if (_result.IsValid)
                            break;
                        else
                            _result.AddErrors(errors);
                    }

                    return _result;
                }
            }
        }
    }
}