CREATE PROCEDURE [dbo].[InsertDefaultValues]
AS
BEGIN
 SET NOCOUNT ON;
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'FOOD')
 INSERT INTO [Sales].[ItemCategory] ([Description]) VALUES ('FOOD')
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'FURNITURE')
 INSERT INTO [Sales].[ItemCategory] ([Description]) VALUES ('FURNITURE')
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'OFFICE SUPPLIES')
 INSERT INTO [Sales].[ItemCategory] ([Description]) VALUES ('OFFICE SUPPLIES')
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'EQUIPMENT')
 INSERT INTO [Sales].[ItemCategory] ([Description]) VALUES ('EQUIPMENT')
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'OTHER')
 INSERT INTO [Sales].[ItemCategory] ([Description]) VALUES ('OTHER')
 
 IF NOT EXISTS (SELECT * FROM [Sales].[Vat] WHERE [Description] = 'VAT 16pc')
 INSERT INTO [Sales].[Vat] ([Description],[Rate]) VALUES ('VAT 16pc',16)
 IF NOT EXISTS (SELECT * FROM [Sales].[Vat] WHERE [Description] = 'VAT Zero')
 INSERT INTO [Sales].[Vat] ([Description],[Rate]) VALUES ('VAT Zero',0)
END