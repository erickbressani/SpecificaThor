namespace SpecificaThor
{
    public interface IHasErrorMessageWhenExpectingFalse<TCandidate>
    {
        string GetErrorMessageWhenExpectingFalse(TCandidate candidate);
    }
}
