namespace MasterDetails.Frameworks.ResponseFrameworks
{
    public enum ResponseStatus
    {
        Success = 1,

        BadRequest = 2,

        ValidationError = 3,

        NotFound = 4,

        DatabaseError = 5,

        Exception = 6
    }
}
