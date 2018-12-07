namespace SpecificaThor
{
    public interface IHasErrorMessageWhenExpectingTrue<TContract>
    {
        string GetErrorMessageWhenExpectingTrue(TContract contract);
    }
}
