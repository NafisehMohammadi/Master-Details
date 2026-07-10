CREATE OR ALTER PROCEDURE dbo.sp_OrderDetail_Create
(
    @OrderHeaderId UNIQUEIDENTIFIER,
    @Details dbo.OrderDetailType READONLY
)
AS
BEGIN

    SET NOCOUNT ON;


    INSERT INTO OrderDetail
    (
        Id,
        OrderHeaderId,
        ProductId,
        Quantity,
        UnitPrice,
        LineTotal,
        IsDeleted,
        CreatedDate
    )

    SELECT
        Id,
        @OrderHeaderId,
        ProductId,
        Quantity,
        UnitPrice,
        LineTotal,
        0,
        GETDATE()

    FROM @Details;

END