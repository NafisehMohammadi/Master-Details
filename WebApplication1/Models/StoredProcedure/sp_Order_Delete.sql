create Or ALTER PROCEDURE sp_Order_Delete
(
    @Id UNIQUEIDENTIFIER
)
AS
BEGIN

    BEGIN TRANSACTION;


    UPDATE OrderHeader
    SET
        IsDeleted = 1,
        ModifiedDate = GETDATE()

    WHERE Id = @Id;



    UPDATE OrderDetail
    SET
        IsDeleted = 1

    WHERE OrderHeaderId = @Id;



    COMMIT TRANSACTION;

END