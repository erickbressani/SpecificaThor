namespace SpecificaThor
{
    public interface IHasDefaultExpectingTrueErrorMessage<TContract>
    {
        string GetErrorMessageExpectingTrue(TContract contract);
    }
}
