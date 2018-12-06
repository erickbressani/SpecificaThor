using System.Collections.Generic;
using System.Linq;

namespace SpecificaThor
{
    public static partial class Specification
    {
        public static EnumerableSpecification<TContract> Create<TContract>(IEnumerable<TContract> subjects)
            => new EnumerableSpecification<TContract>(subjects);

        public sealed class EnumerableSpecification<TEnumerableContract>
        {
            private readonly IEnumerable<TEnumerableContract> _subjects;

            public EnumerableSpecification(IEnumerable<TEnumerableContract> subjects)
                => _subjects = subjects;

            public EnumerableOperator<TEnumerableContract> ThatAre<TSpecification>() where TSpecification : ISpecification<TEnumerableContract>, new()
                => EnumerableOperator<TEnumerableContract>.Create<TSpecification>(_subjects, Expecting.True);

            public EnumerableOperator<TEnumerableContract> ThatAreNot<TSpecification>() where TSpecification : ISpecification<TEnumerableContract>, new()
                => EnumerableOperator<TEnumerableContract>.Create<TSpecification>(_subjects, Expecting.False);

            public sealed class EnumerableOperator<TOperatorContract>
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