namespace MasterDetails.Frameworks.ResponseFrameworks.Contracts
{
    public interface IResponse
    {
        bool IsSuccess { get; }

        string Message { get; }

        ResponseStatus Status { get; }
    }
}
