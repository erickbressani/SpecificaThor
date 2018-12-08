using System.Collections.Generic;
using System.Linq;
using SpecificaThor.Enums;
using SpecificaThor.Extensions;
using SpecificaThor.Structure;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static IValidationSpecification<TContract> Create<TContract>(TContract contract)
            => new ValidationSpecification<TContract>(contract);

        internal sealed class ValidationSpecification<TContract> : IValidationSpecification<TContract>
        {
            private readonly TContract _contract;

            internal ValidationSpecification(TContract contract)
                => _contract = contract;

            public IContractOperator<TContract> Is<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                => CreateOperator<TSpecification>(Expecting.True);

            public IContractOperator<TContract> IsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                => CreateOperator<TSpecification>(Expecting.False);

            private IContractOperator<TContract> CreateOperator<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
            {
                var contractOperator = new ContractOperator(_contract);
                contractOperator.AddToGroup<TSpecification>(expecting);
                return contractOperator;
            }

            internal sealed class ContractOperator : IContractOperator<TContract>
            {
                private readonly TContract _contract;
                private readonly SpecificationResult<TContract> _result;
                private readonly List<ValidationGroup<TContract>> _validationGroups;

                internal ContractOperator(TContract contract)
                {
                    _contract = contract;
                    _result = new SpecificationResult<TContract>();
                    _validationGroups = new List<ValidationGroup<TContract>>();
                    _validationGroups.AddGroup();
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TContract>, new()
                    => _validationGroups.AddToGroup<TSpecification, TContract>(expecting);

                public IContractOperator<TContract> OrIs<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.True);
                    return this;
                }

                public IContractOperator<TContract> AndIs<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.True);
                    return this;
                }

                public IContractOperator<TContract> OrIsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.False);
                    return this;
                }

                public IContractOperator<TContract> AndIsNot<TSpecification>() where TSpecification : ISpecification<TContract>, new()
                {
                    _validationGroups.AddToGroup<TSpecification, TContract>(Expecting.False);
                    return this;
                }

                public IContractOperator<TContract> UseThisErrorMessageIfFails(string errorMessage)
                {
                    _validationGroups.GetLastAddedValidator().CustomErrorMessage = errorMessage;
                    return this;
                }

                public ISpecificationResult<TContract> GetResult()
                {
                    foreach (ValidationGroup<TContract> validationGroup in _validationGroups)
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