CREATE OR ALTER PROCEDURE dbo.sp_OrderDetail_Delete
(
    @OrderHeaderId UNIQUEIDENTIFIER
)
AS
BEGIN

    SET NOCOUNT ON;


    UPDATE OrderDetail

    SET
        IsDeleted = 1,
        ModifiedDate = GETDATE()

    WHERE OrderHeaderId = @OrderHeaderId
    AND IsDeleted = 0;

END