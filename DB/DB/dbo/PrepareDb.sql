CREATE PROCEDURE [dbo].[PrepareDb]
AS
BEGIN
 SET NOCOUNT ON;
 if EXISTS(select s.name +'.'+t.name, 0 from [UmanyiSMS].[sys].[tables] t inner JOIN 
 UmanyiSMS.sys.schemas s on (t.schema_id=s.schema_id))
 BEGIN
 delete from dbo.sysIDs;
 
 DECLARE tables_cursor CURSOR
   FOR
   SELECT s.name,t.name FROM sys.tables AS t
 INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id 
 order by (s.name+'.'+t.name )
 
OPEN tables_cursor;
DECLARE @nameOfTable varchar(50);
DECLARE @schema_name varchar(50);
DECLARE @table_name varchar(50);
FETCH NEXT FROM tables_cursor INTO @schema_name,@table_name;
WHILE (@@FETCH_STATUS <> -1)
BEGIN;
set @nameOfTable = @schema_name+'.'+@table_name;

declare @lastID int;
declare @sql nvarchar(max);
declare @pkey varchar(50)=dbo.GetTablePrimaryKeyColumn(@nameOfTable);
    if @pkey is null
    set @pkey=(select  Top 1 name from sys.columns where object_id=object_id(@nameOfTable))
    
    set @sql='select @lastID=ISNULL(MAX('+@pkey+'),0) from ['+@schema_name+'].['+@table_name+']';    
    exec sp_executesql @sql, N'@lastID int output', @lastID output;    
declare @sql2 nvarchar(200)='INSERT INTO dbo.sysIDs(table_name,last_id) VALUES(''' + @nameOfTable + ''','+CONVERT(varchar(50),@lastID)+')';
   EXEC (@sql2);
   FETCH NEXT FROM tables_cursor INTO @schema_name,@table_name
END;
CLOSE tables_cursor;
DEALLOCATE tables_cursor;
END

EXEC dbo.RefreshSubjects
EXEC dbo.InsertDefaultValues;

END
GO
GRANT CONTROL
    ON OBJECT::[dbo].[PrepareDb] TO [SystemAdmin]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PrepareDb] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PrepareDb] TO [Deputy]
    AS [dbo];

