USE MasterDetailDb;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Order_Update
(
    @Id UNIQUEIDENTIFIER,

    @GuidKey UNIQUEIDENTIFIER,

    @CustomerId UNIQUEIDENTIFIER,

    @OrderDate DATETIME2,

    @Description NVARCHAR(1000),

    @TotalAmount DECIMAL(18,2),

    @Details dbo.OrderDetailUpdateType READONLY
)
AS
BEGIN

    SET NOCOUNT ON;


    BEGIN TRY

        BEGIN TRANSACTION;



        --------------------------------------------------
        -- Validate LineTotal
        --------------------------------------------------

        IF EXISTS
        (
            SELECT 1
            FROM @Details
            WHERE LineTotal <> Quantity * UnitPrice
        )
        BEGIN
            THROW 50001,
            'Invalid LineTotal',
            1;
        END



        --------------------------------------------------
        -- Validate TotalAmount
        --------------------------------------------------

        IF @TotalAmount <>
        (
            SELECT ISNULL(SUM(LineTotal),0)
            FROM @Details
        )
        BEGIN
            THROW 50002,
            'Invalid TotalAmount',
            1;
        END



        --------------------------------------------------
        -- Update Header
        --------------------------------------------------

        UPDATE OrderHeader

        SET

            GuidKey = @GuidKey,

            CustomerId = @CustomerId,

            OrderDate = @OrderDate,

            Description = @Description,

            TotalAmount = @TotalAmount,

            ModifiedDate = GETDATE()


        WHERE Id = @Id;



        --------------------------------------------------
        -- Update Existing Details
        --------------------------------------------------

        UPDATE D

        SET

            D.GuidKey = @GuidKey,

            D.ProductId = T.ProductId,

            D.Quantity = T.Quantity,

            D.UnitPrice = T.UnitPrice,

            D.LineTotal = T.LineTotal,

            D.ModifiedDate = GETDATE(),

            D.IsDeleted = 0


        FROM OrderDetail D

        INNER JOIN @Details T

            ON D.Id = T.Id


        WHERE D.OrderHeaderId = @Id;



        --------------------------------------------------
        -- Insert New Details
        --------------------------------------------------

        INSERT INTO OrderDetail
        (
            Id,

            OrderHeaderId,

            GuidKey,

            ProductId,

            Quantity,

            UnitPrice,

            LineTotal,

            CreatedDate,

            IsDeleted
        )

        SELECT

            NEWID(),

            @Id,

            @GuidKey,

            T.ProductId,

            T.Quantity,

            T.UnitPrice,

            T.LineTotal,

            GETDATE(),

            0


        FROM @Details T

        WHERE T.Id IS NULL;



        --------------------------------------------------
        -- Soft Delete Removed Details
        --------------------------------------------------

        UPDATE D

        SET

            D.IsDeleted = 1,

            D.ModifiedDate = GETDATE()


        FROM OrderDetail D


        WHERE

            D.OrderHeaderId = @Id

            AND D.IsDeleted = 0

            AND NOT EXISTS
            (
                SELECT 1

                FROM @Details T

                WHERE T.Id = D.Id
            );



        COMMIT TRANSACTION;


    END TRY


    BEGIN CATCH


        IF @@TRANCOUNT > 0

            ROLLBACK TRANSACTION;


        THROW;


    END CATCH

END
GO


