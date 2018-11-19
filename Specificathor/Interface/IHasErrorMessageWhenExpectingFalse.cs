namespace SpecificaThor
{
    public interface IHasErrorMessageWhenExpectingFalse<TContract>
    {
        string GetErrorMessageWhenExpectingFalse(TContract contract);
    }
}
