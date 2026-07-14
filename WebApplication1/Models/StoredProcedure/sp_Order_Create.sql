USE MasterDetailDb;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Order_Create
(
    @GuidKey UNIQUEIDENTIFIER,

    @OrderNumber NVARCHAR(40),

    @CustomerId UNIQUEIDENTIFIER,

    @OrderDate DATETIME2,

    @Description NVARCHAR(1000),

    @TotalAmount DECIMAL(18,2),

    @Details dbo.OrderDetailCreateType READONLY
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
        -- Create Header Id
        --------------------------------------------------

        DECLARE @OrderHeaderId UNIQUEIDENTIFIER =
        NEWID();



        --------------------------------------------------
        -- Insert Header
        --------------------------------------------------

        INSERT INTO OrderHeader
        (
            Id,

            GuidKey,

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
            @OrderHeaderId,

            @GuidKey,

            @OrderNumber,

            @CustomerId,

            @OrderDate,

            @Description,

            @TotalAmount,

            GETDATE(),

            0
        );



        --------------------------------------------------
        -- Insert Details
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

            @OrderHeaderId,

            D.GuidKey,

            D.ProductId,

            D.Quantity,

            D.UnitPrice,

            D.LineTotal,

            GETDATE(),

            0

        FROM @Details D

        WHERE D.GuidKey = @GuidKey;



        COMMIT TRANSACTION;


    END TRY


    BEGIN CATCH


        IF @@TRANCOUNT > 0

            ROLLBACK TRANSACTION;


        THROW;


    END CATCH


END
GO