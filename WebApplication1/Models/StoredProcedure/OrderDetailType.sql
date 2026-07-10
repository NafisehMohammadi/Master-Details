IF TYPE_ID('dbo.OrderDetailType') IS NOT NULL
BEGIN
    DROP TYPE dbo.OrderDetailType;
END
GO


CREATE TYPE dbo.OrderDetailType AS TABLE
(
    Id UNIQUEIDENTIFIER,
    ProductId UNIQUEIDENTIFIER,
    Quantity DECIMAL(18,2),
    UnitPrice DECIMAL(18,2),
    LineTotal DECIMAL(18,2)
);
GO


/*SELECT 
    c.name AS ColumnName,
    t.name AS DataType
FROM sys.table_types tt
INNER JOIN sys.columns c
ON tt.type_table_object_id = c.object_id
INNER JOIN sys.types t
ON c.user_type_id = t.user_type_id
WHERE tt.name = 'OrderDetailType';*/

