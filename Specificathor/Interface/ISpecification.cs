namespace SpecificaThor
{
    public interface ISpecification<TContract>
    {
        bool Validate(TContract contract);
    }
}
