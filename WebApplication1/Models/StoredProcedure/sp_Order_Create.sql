CREATE or Alter PROCEDURE dbo.sp_Order_Create
(
    @OrderId UNIQUEIDENTIFIER,
    @OrderNumber NVARCHAR(40),
    @CustomerId UNIQUEIDENTIFIER,
    @OrderDate DATETIME2,
    @Description NVARCHAR(1000),
    @TotalAmount DECIMAL(18,2),
    @Details dbo.OrderDetailType READONLY
)
AS
BEGIN

    INSERT INTO OrderHeader
    (
        Id,
        OrderNumber,
        CustomerId,
        OrderDate,
        Description,
        TotalAmount,
        CreatedDate,
        IsDeleted
    )
    VALUES
    (
        @OrderId,
        @OrderNumber,
        @CustomerId,
        @OrderDate,
        @Description,
        @TotalAmount,
        GETDATE(),
        0
    );


    INSERT INTO OrderDetail
    (
        Id,
        OrderHeaderId,
        ProductId,
        Quantity,
        UnitPrice,
        LineTotal,
        CreatedDate,
        IsDeleted
    )
    SELECT
        Id,
        @OrderId,
        ProductId,
        Quantity,
        UnitPrice,
        LineTotal,
        GETDATE(),
        0

    FROM @Details;

END