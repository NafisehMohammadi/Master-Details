namespace MasterDetails.Frameworks.ResponseFrameworks
{
    public static class ResponseMessages
    {
        #region CRUD Success

        public const string CreatedSuccessfully =
            "Record created successfully.";

        public const string UpdatedSuccessfully =
            "Record updated successfully.";

        public const string DeletedSuccessfully =
            "Record deleted successfully.";

        public const string RetrievedSuccessfully =
            "Record retrieved successfully.";

        public const string ListRetrievedSuccessfully =
            "Records retrieved successfully.";

        #endregion

        #region Not Found

        public const string OrderNotFound =
            "Order not found.";

        public const string CustomerNotFound =
            "Customer not found.";

        public const string ProductNotFound =
            "Product not found.";

        #endregion

        #region Validation

        public const string ValidationFailed =
            "Validation failed.";

        public const string InvalidInput =
            "Invalid request.";

        public const string InvalidOrderId =
            "Order Id is invalid.";
        public const string InvalidCustomerId=
             "Customer Id is invalid.";

        public const string OrderMustHaveAtLeastOneItem =
            "An order must contain at least one item.";

        #endregion

        #region Errors

        public const string DatabaseError =
            "A database error occurred.";

        public const string UnexpectedError =
            "An unexpected error occurred.";

        #endregion
    }
}