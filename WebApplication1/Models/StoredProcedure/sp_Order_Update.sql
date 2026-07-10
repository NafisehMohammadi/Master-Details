Create or ALTER PROCEDURE sp_Order_Update
(
    @Id UNIQUEIDENTIFIER,
    @CustomerId UNIQUEIDENTIFIER,
    @OrderDate DATETIME2,
    @Description NVARCHAR(1000),
    @TotalAmount DECIMAL(18,2),
    @Details dbo.OrderDetailType READONLY
)
AS
BEGIN

BEGIN TRANSACTION;


UPDATE OrderHeader
SET
    CustomerId = @CustomerId,
    OrderDate = @OrderDate,
    Description = @Description,
    TotalAmount = @TotalAmount,
    ModifiedDate = GETDATE()

WHERE Id = @Id;



UPDATE OrderDetail
SET IsDeleted = 1

WHERE OrderHeaderId = @Id;



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
    @Id,
    ProductId,
    Quantity,
    UnitPrice,
    LineTotal,
    GETDATE(),
    0

FROM @Details;



COMMIT TRANSACTION;

END