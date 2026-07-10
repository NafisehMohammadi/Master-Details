namespace MasterDetails.Frameworks.ResponseFrameworks
{
    public  static class ErrorCodes
    {
        public const string None = "";

        public const string CustomerNotFound = "ORD001";

        public const string ProductNotFound = "ORD002";

        public const string QuantityInvalid = "ORD003";

        public const string ValidationFailed = "ORD004";

        public const string DatabaseError = "ORD005";

        public const string OrderNotFound = "ORD006";

        public const string UnexpectedError = "ORD999";

        public const string InvalidInput = "ORD007";

        public const string InvalidOrderId = "ORD008";
    }
}
