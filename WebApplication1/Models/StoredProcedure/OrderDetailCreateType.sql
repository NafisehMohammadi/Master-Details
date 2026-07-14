DROP TYPE IF EXISTS dbo.OrderDetailCreateType;
GO

CREATE TYPE dbo.OrderDetailCreateType AS TABLE
(
   GuidKey UNIQUEIDENTIFIER,

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
WHERE tt.name = 'OrderDetailCreateType';*/

/*SELECT 
    name,
    type_desc
FROM sys.objects
WHERE name = 'sp_Order_Update';*/

