using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public static class Specification
    {
        public static ValidationSpecification<TContract> Create<TContract>(TContract contract)
            => new ValidationSpecification<TContract>(contract);

        public static EnumerableSpecification<TContract> Create<TContract>(IEnumerable<TContract> subjects)
            => new EnumerableSpecification<TContract>(subjects);

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
                private readonly SpecificationResult _result;
                private readonly List<ValidationGroup<TOperatorContract>> _validationGroups;

                internal ContractOperator(TOperatorContract contract)
                {
                    _contract = contract;
                    _result = new SpecificationResult();
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

                public SpecificationResult IsSatisfied()
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

        public sealed class EnumerableSpecification<TEnumerableContract>
        {
            private readonly IEnumerable<TEnumerableContract> _subjects;

            public EnumerableSpecification(IEnumerable<TEnumerableContract> subjects)
            {
                _subjects = subjects;
            }

            public EnumerableOperator<TEnumerableContract> ThatAre<TSpecification>() where TSpecification : ISpecification<TEnumerableContract>, new()
                => EnumerableOperator<TEnumerableContract>.Create<TSpecification>(_subjects, Expecting.True);

            public EnumerableOperator<TEnumerableContract> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TEnumerableContract>, new()
                => EnumerableOperator<TEnumerableContract>.Create<TSpecification>(_subjects, Expecting.False);

            public class EnumerableOperator<TOperatorContract>
            {
                private readonly IEnumerable<TOperatorContract> _subjects;
                private readonly List<ValidationGroup<TOperatorContract>> _validationGroups;

                private EnumerableOperator(IEnumerable<TOperatorContract> subjects)
                {
                    _subjects = subjects;
                    _validationGroups = new List<ValidationGroup<TOperatorContract>>();
                    _validationGroups.AddGroup();
                }

                internal static EnumerableOperator<TOperatorContract> Create<TSpecification>(IEnumerable<TOperatorContract> subjects, Expecting expecting) where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    var enumerableOperator = new EnumerableOperator<TOperatorContract>(subjects);
                    enumerableOperator.AddToGroup<TSpecification>(expecting);
                    return enumerableOperator;
                }

                internal void AddToGroup<TSpecification>(Expecting expecting) where TSpecification : ISpecification<TOperatorContract>, new()
                    => _validationGroups.AddToGroup<TSpecification, TOperatorContract>(expecting);

                public EnumerableOperator<TOperatorContract> AndAre<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    AddToGroup<TSpecification>(Expecting.True);
                    return this;
                }

                public EnumerableOperator<TOperatorContract> OrAre<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.True);
                    return this;
                }

                public EnumerableOperator<TOperatorContract> AndAreNot<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    AddToGroup<TSpecification>(Expecting.False);
                    return this;
                }

                public EnumerableOperator<TOperatorContract> OrAreNot<TSpecification>() where TSpecification : ISpecification<TOperatorContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TSpecification, TOperatorContract>(Expecting.False);
                    return this;
                }

                public IEnumerable<TOperatorContract> GetMatched()
                    => _subjects.Where(subject => _validationGroups.Any(
                                            group => group.IsGroupValid(subject)));
            }
        }
    }
}