CREATE FUNCTION [dbo].[GetTablePrimaryKeyColumn](@nameOfTable varchar(max))
    RETURNS NVARCHAR(50)
    AS
    BEGIN

RETURN (SELECT COL_NAME(ic.OBJECT_ID,
ic.column_id) AS ColumnName FROM sys.indexes AS i
 INNER JOIN sys.index_columns AS ic ON i.OBJECT_ID = ic.OBJECT_ID AND 
 i.index_id = ic.index_id 
 INNER JOIN sys.tables t ON (ic.OBJECT_ID=t.object_id)WHERE i.is_primary_key = 1 AND  
 UPPER(SCHEMA_NAME(t.schema_id)+'.'+OBJECT_NAME(ic.OBJECT_ID))=UPPER(@nameOfTable));
    END
    ;


