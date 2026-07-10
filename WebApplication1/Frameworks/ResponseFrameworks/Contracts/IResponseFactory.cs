using MasterDetails.Frameworks.ResponseFrameworks;

namespace MasterDetail.Frameworks.ResponseFrameworks.Contracts;

public interface IResponseFactory
{
    Response<T> Success<T>(T? data, string message);

    Response<T> Fail<T>
    (
        string message,
        ResponseStatus status,
        string errorCode
    );

    Response<T> ValidationFail<T>
    (
        List<ValidationError> validationErrors
    );

    Response<T> NotFound<T>
    (
        string message,
        string errorCode
    );
    Response<T> BadRequest<T>(
      string message,
      string errorCode);

    Response<T> Exception<T>(
        string message,
        string errorCode);
}