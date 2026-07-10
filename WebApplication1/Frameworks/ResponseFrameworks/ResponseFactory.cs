using MasterDetail.Frameworks.ResponseFrameworks.Contracts;

namespace MasterDetails.Frameworks.ResponseFrameworks
{
    public class ResponseFactory : IResponseFactory
    {
       
        public Response<T> Success<T>(T? data, string message)
        {
            return new Response<T>
            {
                IsSuccess = true,

                Status = ResponseStatus.Success,

                Message = message,

                ErrorCode = ErrorCodes.None,

                Data = data
            };
        }
        public Response<T> Fail<T>( string message, ResponseStatus status,string errorCode)
        {
            return new Response<T>
            {
                IsSuccess = false,

                Status = status,

                Message = message,

                ErrorCode = errorCode,

                Data = default
            };
        }
        public Response<T> NotFound<T>(string message, string errorCode)
        {
            return new Response<T>
            {
                IsSuccess = false,

                Status = ResponseStatus.NotFound,

                Message = message,

                ErrorCode = errorCode,

                Data = default
            };
        }

        public Response<T> ValidationFail<T>(List<ValidationError> validationErrors)
        {
            return new Response<T>
            {
                IsSuccess = false,

                Status = ResponseStatus.ValidationError,

                Message = ResponseMessages.ValidationFailed,

                ErrorCode = ErrorCodes.ValidationFailed,

                ValidationErrors = validationErrors
            };
        }
        public Response<T> BadRequest<T>(string message, string errorCode)
        {
            return new Response<T>
            {
                IsSuccess = false,

                Status = ResponseStatus.BadRequest,

                Message = message,

                ErrorCode = errorCode,

                Data = default
            };
        }
        public Response<T> Exception<T>(string message,string errorCode)
        {
            return new Response<T>
            {
                IsSuccess = false,

                Status = ResponseStatus.Exception,

                Message = message,

                ErrorCode = errorCode,

                Data = default
            };
        }
    }
}
