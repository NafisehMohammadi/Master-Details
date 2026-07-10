CREATE OR ALTER PROCEDURE dbo.sp_Order_GetAll
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
        OD.Quantity,
        OD.UnitPrice,
        OD.LineTotal

    FROM OrderHeader AS OH

    INNER JOIN Customer AS C
        ON OH.CustomerId = C.Id

    LEFT JOIN OrderDetail AS OD
        ON OD.OrderHeaderId = OH.Id
       AND OD.IsDeleted = 0

    WHERE OH.IsDeleted = 0

    ORDER BY OH.CreatedDate DESC;
END