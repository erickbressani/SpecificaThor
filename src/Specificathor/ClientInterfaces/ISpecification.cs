namespace SpecificaThor
{
    public interface ISpecification<TCandidate>
    {
        bool Validate(TCandidate candidate);
    }
}
