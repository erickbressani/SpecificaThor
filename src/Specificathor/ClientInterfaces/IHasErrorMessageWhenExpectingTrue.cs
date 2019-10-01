namespace SpecificaThor
{
    public interface IHasErrorMessageWhenExpectingTrue<TCandidate>
    {
        string GetErrorMessageWhenExpectingTrue(TCandidate candidate);
    }
}
