namespace SpecificaThor
{
    public interface IHasDefaultExpectingFalseErrorMessage<TContract>
    {
        string GetErrorMessageExpectingFalse(TContract contract);
    }
}
