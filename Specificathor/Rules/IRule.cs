namespace SpecificaThor
{
    public interface IRule<TContract>
    {
        string GetErrorMessage(TContract contract);
        bool Validate(TContract contract);
    }
}
