using MasterDetails.Frameworks.ResponseFrameworks.Contracts;


namespace MasterDetails.Frameworks.ResponseFrameworks
{
    public class Response<T> : IResponse
    {
        public bool IsSuccess { get; set; }

        public ResponseStatus Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public string ErrorCode { get; set; } = ErrorCodes.None;

        public T? Data { get; set; }

        public List<ValidationError> ValidationErrors { get; set; } = new();
    }
}
