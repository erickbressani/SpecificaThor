using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public sealed class Specification
    {
        public static ValidationSpecification<TContract> Create<TContract>(TContract contract)
        {
            return new ValidationSpecification<TContract>(contract);
        }

        public static EnumerableSpecification<TContract> Create<TContract>(IEnumerable<TContract> subjects)
        {
            return new EnumerableSpecification<TContract>(subjects);
        }

        public sealed class ValidationSpecification<TContract>
        {
            private readonly TContract _contract;

            internal ValidationSpecification(TContract contract)
            {
                _contract = contract;
            }

            public ContractOperator<TContract> Is<TRule>() where TRule : IRule<TContract>, new()
            {
                return CreateOperator<TRule>(expected: true);
            }

            public ContractOperator<TContract> IsNot<TRule>() where TRule : IRule<TContract>, new()
            {
                return CreateOperator<TRule>(expected: false);
            }

            private ContractOperator<TContract> CreateOperator<TRule>(bool expected) where TRule : IRule<TContract>, new()
            {
                var contractOperator = new ContractOperator<TContract>(_contract);
                contractOperator.AddToGroup<TRule>(expected);
                return contractOperator;
            }

            public sealed class ContractOperator<TContract>
            {
                private readonly TContract _contract;
                private readonly SpecificationResult _result;
                private readonly List<ValidationGroup<TContract>> _validationGroups;

                internal ContractOperator(TContract contract)
                {
                    _contract = contract;
                    _result = new SpecificationResult();
                    _validationGroups = new List<ValidationGroup<TContract>>();
                    _validationGroups.AddGroup();
                }

                internal void AddToGroup<TRule>(bool expected) where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddToGroup<TRule, TContract>(expected);
                }

                public ContractOperator<TContract> OrIs<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TRule, TContract>(expected: true);
                    return this;
                }

                public ContractOperator<TContract> AndIs<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddToGroup<TRule, TContract>(expected: true);
                    return this;
                }

                public ContractOperator<TContract> OrIsNot<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TRule, TContract>(expected: false);
                    return this;
                }

                public ContractOperator<TContract> AndIsNot<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddToGroup<TRule, TContract>(expected: false);
                    return this;
                }

                public SpecificationResult IsSatisfied()
                {
                    foreach (ValidationGroup<TContract> validationGroup in _validationGroups)
                    {
                        var errors = validationGroup.GetFailures(_contract);

                        if (errors.Any())
                        {
                            _result.AddErrors(errors);
                            _result.IsValid = false;
                        }
                        else
                        {
                            _result.IsValid = true;
                            break;
                        }
                    }

                    return _result;
                }
            }
        }

        public sealed class EnumerableSpecification<TContract>
        {
            private readonly IEnumerable<TContract> _subjects;

            public EnumerableSpecification(IEnumerable<TContract> subjects)
            {
                _subjects = subjects;
            }

            public EnumerableOperator<TContract> ThatAre<TRule>() where TRule : IRule<TContract>, new()
            {
                return EnumerableOperator<TContract>.Create<TRule>(_subjects, expected: true);
            }

            public EnumerableOperator<TContract> ThatAreNot<TRule>() where TRule : IRule<TContract>, new()
            {
                return EnumerableOperator<TContract>.Create<TRule>(_subjects, expected: false);
            }

            public class EnumerableOperator<TContract>
            {
                private readonly IEnumerable<TContract> _subjects;
                private readonly List<ValidationGroup<TContract>> _validationGroups;

                private EnumerableOperator(IEnumerable<TContract> subjects)
                {
                    _subjects = subjects;
                    _validationGroups = new List<ValidationGroup<TContract>>();
                    _validationGroups.AddGroup();
                }

                internal static EnumerableOperator<TContract> Create<TRule>(IEnumerable<TContract> subjects, bool expected) where TRule : IRule<TContract>, new()
                {
                    var enumerableOperator = new EnumerableOperator<TContract>(subjects);
                    enumerableOperator.AddToGroup<TRule>(expected);
                    return enumerableOperator;
                }

                internal void AddToGroup<TRule>(bool expected) where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddToGroup<TRule, TContract>(expected);
                }

                public EnumerableOperator<TContract> AndAre<TRule>() where TRule : IRule<TContract>, new()
                {
                    AddToGroup<TRule>(expected: true);
                    return this;
                }

                public EnumerableOperator<TContract> OrAre<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TRule, TContract>(expected: true);
                    return this;
                }

                public EnumerableOperator<TContract> AndAreNot<TRule>() where TRule : IRule<TContract>, new()
                {
                    AddToGroup<TRule>(expected: false);
                    return this;
                }

                public EnumerableOperator<TContract> OrAreNot<TRule>() where TRule : IRule<TContract>, new()
                {
                    _validationGroups.AddGroup();
                    _validationGroups.AddToGroup<TRule, TContract>(expected: false);
                    return this;
                }

                public IEnumerable<TContract> GetMatch()
                {
                    return _subjects.Where(subject => _validationGroups.Any(
                                                group => group.IsGroupValid(subject)));
                }
            }
        }
    }
}