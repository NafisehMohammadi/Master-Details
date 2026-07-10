CREATE OR ALTER PROCEDURE dbo.sp_Order_GetById
(
    @Id UNIQUEIDENTIFIER
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT

        OH.Id,

        OH.OrderNumber,

        OH.CustomerId,

        CONCAT(C.FirstName,' ',C.LastName) AS CustomerName,

        OH.OrderDate,

        OH.TotalAmount,

        OH.Description,


        OD.Id AS DetailId,

        OD.ProductId,

        P.Title AS ProductName,

        OD.Quantity,

        OD.UnitPrice,

        OD.LineTotal


    FROM OrderHeader OH

    INNER JOIN Customer C
        ON OH.CustomerId = C.Id


    LEFT JOIN OrderDetail OD
        ON OD.OrderHeaderId = OH.Id
        AND OD.IsDeleted = 0


    LEFT JOIN Product P
        ON OD.ProductId = P.Id


    WHERE OH.Id = @Id;

END