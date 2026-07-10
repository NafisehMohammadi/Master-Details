CREATE OR ALTER PROCEDURE dbo.sp_OrderDetail_Update
(
    @OrderHeaderId UNIQUEIDENTIFIER,
    @Details dbo.OrderDetailType READONLY
)
AS
BEGIN

    SET NOCOUNT ON;


    BEGIN TRY

        BEGIN TRANSACTION;



        -- حذف منطقی Detail های قبلی

        UPDATE OrderDetail

        SET
            IsDeleted = 1,
            ModifiedDate = GETDATE()

        WHERE OrderHeaderId = @OrderHeaderId
        AND IsDeleted = 0;



        -- درج Detail های جدید

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



        COMMIT TRANSACTION;


    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;


        THROW;

    END CATCH

END