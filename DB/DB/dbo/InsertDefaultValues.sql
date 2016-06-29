CREATE PROCEDURE [dbo].[InsertDefaultValues]
AS
BEGIN
 SET NOCOUNT ON;
 
 DECLARE @id int = dbo.GetNewID('Sales.ItemCategory');
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'ASSETS') 
 INSERT INTO [Sales].[ItemCategory] ([ItemCategoryID],[Description]) VALUES (@id,'ASSETS')
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'CASH')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('CASH',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'ACCOUNTS RECEIVABLE')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('ACCOUNTS RECEIVABLE',@id)
 
 SET @id = dbo.GetNewID('Sales.ItemCategory');
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'LIABILITIES')
 INSERT INTO [Sales].[ItemCategory] ([ItemCategoryID],[Description]) VALUES (@id,'LIABILITIES')
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'ACCOUNTS PAYABLE')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('ACCOUNTS PAYABLE',@id)
  
 SET @id = dbo.GetNewID('Sales.ItemCategory');     
  
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'CAPITAL')
 INSERT INTO [Sales].[ItemCategory] ([ItemCategoryID],[Description]) VALUES (@id,'CAPITAL')
 
 SET @id = dbo.GetNewID('Sales.ItemCategory'); 
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'REVENUE')
 INSERT INTO [Sales].[ItemCategory] ([ItemCategoryID],[Description]) VALUES (@id,'REVENUE')
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'FEES PAID')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('FEES PAID',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'DONATIONS')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('DONATIONS',@id)

SET @id = dbo.GetNewID('Sales.ItemCategory');

 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'EXPENSES')
 INSERT INTO [Sales].[ItemCategory] ([ItemCategoryID],[Description]) VALUES (@id,'EXPENSES')              
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'B.E.S.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('B.E.S.',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'E.W.C.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('E.W.C.',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'ADMINISTRATION & TUITION')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('ADMINISTRATION & TUITION',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'P.E.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('P.E.',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'CONTINGENCIES')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('CONTINGENCIES',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'MEDICAL FEE')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('MEDICAL FEE',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'ACTIVITY FEE')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('ACTIVITY FEE',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'L.T. & T.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('LT & T',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'R.M.I.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('R.M.I.',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'PTA DEV FUND')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('PTA DEV FUND',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'V.M. & F.')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('V.M. & F.',@id)
 
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemCategory] WHERE [Description] = 'PAYROLL EXPENSES')
 INSERT INTO [Sales].[ItemCategory] ([Description],[ParentCategoryID]) VALUES ('PAYROLL EXPENSES',@id)
 
 
 IF NOT EXISTS (SELECT * FROM [Sales].[Vat] WHERE [Description] = 'VAT 16pc')
 INSERT INTO [Sales].[Vat] ([Description],[Rate]) VALUES ('VAT 16pc',16)
 IF NOT EXISTS (SELECT * FROM [Sales].[Vat] WHERE [Description] = 'VAT Zero')
 INSERT INTO [Sales].[Vat] ([Description],[Rate]) VALUES ('VAT Zero',0)
 
 IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange0')
INSERT [Institution].[Settings] ([Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange0', N'85', N'100')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange1')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange1', N'80', N'84')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange2')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange2', N'75', N'79')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange3')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange3', N'70', N'74')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange4')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange4', N'65', N'69')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange5')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange5', N'60', N'64')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange6')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange6', N'55', N'59')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange7')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange7', N'50', N'54')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange8')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange8', N'45', N'49')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange9')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange9', N'40', N'44')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange10')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange10', N'35', N'39')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRange11')
INSERT [Institution].[Settings] ( [Type], [Key], [Value],  [Value2]) VALUES ( N'ExamSettings', N'GradeRange11', N'0', N'34')

IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark0')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark0', N'EXCELLENT')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark1')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark1', N'VERY GOOD')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark2')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark2', N'VERY GOOD')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark3')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark3', N'GOOD')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark4')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark4', N'ABOVE AVERAGE')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark5')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark5', N'AVERAGE')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark6')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark6', N'AVERAGE')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark7')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark7', N'FAIR')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark8')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark8', N'BELOW AVERAGE')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark9')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark9', N'POOR')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark10')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark10', N'VERY POOR')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='GradeRemark11')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'GradeRemark11', N'WAKE UP')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='Best7Subjects')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'Best7Subjects', N'3')
IF NOT EXISTS (SELECT * FROM [Institution].[Settings] WHERE [Type] = 'ExamSettings' AND [Key]='MeanGradeCalculation')
INSERT [Institution].[Settings] ( [Type], [Key], [Value]) VALUES ( N'ExamSettings', N'MeanGradeCalculation', N'1')

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertDefaultValues] TO [Principal]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertDefaultValues] TO [Deputy]
    AS [dbo];

