USE [UmanyiSMS]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentBalance]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetCurrentBalance]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectSelection]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetSubjectSelection]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTermClassPosition]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetTermClassPosition]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTermOverAllPosition]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetTermOverAllPosition]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemReceiptHeader_IsCancelled]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptHeader] DROP CONSTRAINT [DF_ItemReceiptHeader_IsCancelled]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PurchaseOrderHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptHeader] DROP CONSTRAINT [DF_PurchaseOrderHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PurchaseOrderHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptHeader] DROP CONSTRAINT [DF_PurchaseOrderHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemReceiptHeader]') AND type in (N'U'))
DROP TABLE [Sales].[ItemReceiptHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SaleHeader_PaymentID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleHeader] DROP CONSTRAINT [DF_SaleHeader_PaymentID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SalesOrderHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleHeader] DROP CONSTRAINT [DF_SalesOrderHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SalesOrderHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleHeader] DROP CONSTRAINT [DF_SalesOrderHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SaleHeader]') AND type in (N'U'))
DROP TABLE [Sales].[SaleHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Student_PreviousBalance]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Student] DROP CONSTRAINT [DF_Student_PreviousBalance]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Student_IsBoarder]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Student] DROP CONSTRAINT [DF_Student_IsBoarder]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Student_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Student] DROP CONSTRAINT [DF_Student_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Student_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Student] DROP CONSTRAINT [DF_Student_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Student]') AND type in (N'U'))
DROP TABLE [Institution].[Student]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StockTakingDetail_StockTakingDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[StockTakingDetail] DROP CONSTRAINT [DF_StockTakingDetail_StockTakingDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StockTakingDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[StockTakingDetail] DROP CONSTRAINT [DF_StockTakingDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StockTakingDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[StockTakingDetail] DROP CONSTRAINT [DF_StockTakingDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[StockTakingDetail]') AND type in (N'U'))
DROP TABLE [Sales].[StockTakingDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentSubjectSelectionHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentSubjectSelectionHeader] DROP CONSTRAINT [DF_StudentSubjectSelectionHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentSubjectSelectionHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentSubjectSelectionHeader] DROP CONSTRAINT [DF_StudentSubjectSelectionHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentSubjectSelectionHeader]') AND type in (N'U'))
DROP TABLE [Institution].[StudentSubjectSelectionHeader]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUnreturnedCopies]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetUnreturnedCopies]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWeightedExamSubjectScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetWeightedExamSubjectScore]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWeightedExamTotalScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetWeightedExamTotalScore]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectsTakenByStudent]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetSubjectsTakenByStudent]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPurchaseTotal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetPurchaseTotal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSaleTotal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetSaleTotal]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStudentIsActive]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetStudentIsActive]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentClass]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetCurrentClass]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentQuantity]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetCurrentQuantity]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExamSubjectScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetExamSubjectScore]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExamTotalScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetExamTotalScore]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserDetail_UserDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[UserDetail] DROP CONSTRAINT [DF_UserDetail_UserDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[UserDetail] DROP CONSTRAINT [DF_UserDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[UserDetail] DROP CONSTRAINT [DF_UserDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[UserDetail]') AND type in (N'U'))
DROP TABLE [Users].[UserDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vat_VatID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Vat] DROP CONSTRAINT [DF_Vat_VatID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vat_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Vat] DROP CONSTRAINT [DF_Vat_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vat_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Vat] DROP CONSTRAINT [DF_Vat_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Vat]') AND type in (N'U'))
DROP TABLE [Sales].[Vat]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Event_EventID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Event] DROP CONSTRAINT [DF_Event_EventID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Event_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Event] DROP CONSTRAINT [DF_Event_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Event_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Event] DROP CONSTRAINT [DF_Event_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Event]') AND type in (N'U'))
DROP TABLE [Institution].[Event]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamClassDetail_ExamClassDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamClassDetail] DROP CONSTRAINT [DF_ExamClassDetail_ExamClassDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamClassDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamClassDetail] DROP CONSTRAINT [DF_ExamClassDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamClassDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamClassDetail] DROP CONSTRAINT [DF_ExamClassDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamClassDetail]') AND type in (N'U'))
DROP TABLE [Institution].[ExamClassDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamDetail_ExamDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamDetail] DROP CONSTRAINT [DF_ExamDetail_ExamDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamDetail] DROP CONSTRAINT [DF_ExamDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamDetail] DROP CONSTRAINT [DF_ExamDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamDetail]') AND type in (N'U'))
DROP TABLE [Institution].[ExamDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultDetail_ExamResultDetail]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultDetail] DROP CONSTRAINT [DF_ExamResultDetail_ExamResultDetail]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultDetail] DROP CONSTRAINT [DF_ExamResultDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultDetail] DROP CONSTRAINT [DF_ExamResultDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamResultDetail]') AND type in (N'U'))
DROP TABLE [Institution].[ExamResultDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Book_BookID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Book] DROP CONSTRAINT [DF_Book_BookID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Book_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Book] DROP CONSTRAINT [DF_Book_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Book_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Book] DROP CONSTRAINT [DF_Book_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Book]') AND type in (N'U'))
DROP TABLE [Institution].[Book]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssueDetail_BookIssueDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueDetail] DROP CONSTRAINT [DF_BookIssueDetail_BookIssueDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssueDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueDetail] DROP CONSTRAINT [DF_BookIssueDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssueDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueDetail] DROP CONSTRAINT [DF_BookIssueDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookIssueDetail]') AND type in (N'U'))
DROP TABLE [Institution].[BookIssueDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssueHeader_BookIssueID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueHeader] DROP CONSTRAINT [DF_BookIssueHeader_BookIssueID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssue_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueHeader] DROP CONSTRAINT [DF_BookIssue_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookIssue_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookIssueHeader] DROP CONSTRAINT [DF_BookIssue_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookIssueHeader]') AND type in (N'U'))
DROP TABLE [Institution].[BookIssueHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnDetail_BookReturnDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnDetail] DROP CONSTRAINT [DF_BookReturnDetail_BookReturnDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnDetail] DROP CONSTRAINT [DF_BookReturnDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnDetail] DROP CONSTRAINT [DF_BookReturnDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookReturnDetail]') AND type in (N'U'))
DROP TABLE [Institution].[BookReturnDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnHeader_BookReturnID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnHeader] DROP CONSTRAINT [DF_BookReturnHeader_BookReturnID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnHeader] DROP CONSTRAINT [DF_BookReturnHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_BookReturnHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[BookReturnHeader] DROP CONSTRAINT [DF_BookReturnHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookReturnHeader]') AND type in (N'U'))
DROP TABLE [Institution].[BookReturnHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassGroupDetail_ClassGroupDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassGroupDetail] DROP CONSTRAINT [DF_ClassGroupDetail_ClassGroupDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassGroupDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassGroupDetail] DROP CONSTRAINT [DF_ClassGroupDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassGroupDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassGroupDetail] DROP CONSTRAINT [DF_ClassGroupDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassGroupDetail]') AND type in (N'U'))
DROP TABLE [Institution].[ClassGroupDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassSetupDetail_ClassSetupDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupDetail] DROP CONSTRAINT [DF_ClassSetupDetail_ClassSetupDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassSetupDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupDetail] DROP CONSTRAINT [DF_ClassSetupDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassSetupDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupDetail] DROP CONSTRAINT [DF_ClassSetupDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassSetupDetail]') AND type in (N'U'))
DROP TABLE [Institution].[ClassSetupDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CurrentClass_CurrentClassID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[CurrentClass] DROP CONSTRAINT [DF_CurrentClass_CurrentClassID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CurrentClass_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[CurrentClass] DROP CONSTRAINT [DF_CurrentClass_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CurrentClass_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[CurrentClass] DROP CONSTRAINT [DF_CurrentClass_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CurrentClass_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[CurrentClass] DROP CONSTRAINT [DF_CurrentClass_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[CurrentClass]') AND type in (N'U'))
DROP TABLE [Institution].[CurrentClass]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Discipline_DisciplineID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Discipline] DROP CONSTRAINT [DF_Discipline_DisciplineID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Discipline_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Discipline] DROP CONSTRAINT [DF_Discipline_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Discipline_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Discipline] DROP CONSTRAINT [DF_Discipline_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Discipline]') AND type in (N'U'))
DROP TABLE [Institution].[Discipline]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Dormitory_DormitoryID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Dormitory] DROP CONSTRAINT [DF_Dormitory_DormitoryID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Dormitory_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Dormitory] DROP CONSTRAINT [DF_Dormitory_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Dormitory_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Dormitory] DROP CONSTRAINT [DF_Dormitory_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Dormitory]') AND type in (N'U'))
DROP TABLE [Institution].[Dormitory]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureDetail_FeesStructureDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureDetail] DROP CONSTRAINT [DF_FeesStructureDetail_FeesStructureDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureDetail] DROP CONSTRAINT [DF_FeesStructureDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureDetail] DROP CONSTRAINT [DF_FeesStructureDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesStructureDetail]') AND type in (N'U'))
DROP TABLE [Institution].[FeesStructureDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Gallery_GalleryID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Gallery] DROP CONSTRAINT [DF_Gallery_GalleryID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Gallery_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Gallery] DROP CONSTRAINT [DF_Gallery_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Gallery_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Gallery] DROP CONSTRAINT [DF_Gallery_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Gallery]') AND type in (N'U'))
DROP TABLE [Institution].[Gallery]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemCategory_ItemCategoryID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemCategory] DROP CONSTRAINT [DF_ItemCategory_ItemCategoryID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ProductCategory_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemCategory] DROP CONSTRAINT [DF_ProductCategory_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ProductCategory_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemCategory] DROP CONSTRAINT [DF_ProductCategory_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemCategory]') AND type in (N'U'))
DROP TABLE [Sales].[ItemCategory]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemIssueDetail_ItemIssueDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemIssueDetail] DROP CONSTRAINT [DF_ItemIssueDetail_ItemIssueDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemIssueDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemIssueDetail] DROP CONSTRAINT [DF_ItemIssueDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemIssueDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemIssueDetail] DROP CONSTRAINT [DF_ItemIssueDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemIssueDetail]') AND type in (N'U'))
DROP TABLE [Sales].[ItemIssueDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemReceiptDetail_ItemReceiptDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptDetail] DROP CONSTRAINT [DF_ItemReceiptDetail_ItemReceiptDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PurchaseOrderDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptDetail] DROP CONSTRAINT [DF_PurchaseOrderDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PurchaseOrderDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemReceiptDetail] DROP CONSTRAINT [DF_PurchaseOrderDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemReceiptDetail]') AND type in (N'U'))
DROP TABLE [Sales].[ItemReceiptDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LeavingCertificate_LeavingCertificateID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[LeavingCertificate] DROP CONSTRAINT [DF_LeavingCertificate_LeavingCertificateID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LeavingCertificate_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[LeavingCertificate] DROP CONSTRAINT [DF_LeavingCertificate_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_LeavingCertificate_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[LeavingCertificate] DROP CONSTRAINT [DF_LeavingCertificate_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[LeavingCertificate]') AND type in (N'U'))
DROP TABLE [Institution].[LeavingCertificate]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SaleDetail_SalesOrderDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleDetail] DROP CONSTRAINT [DF_SaleDetail_SalesOrderDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SalesOrderDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleDetail] DROP CONSTRAINT [DF_SalesOrderDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SalesOrderDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SaleDetail] DROP CONSTRAINT [DF_SalesOrderDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SaleDetail]') AND type in (N'U'))
DROP TABLE [Sales].[SaleDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableDetail_TimeTableDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableDetail] DROP CONSTRAINT [DF_TimeTableDetail_TimeTableDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableDetail] DROP CONSTRAINT [DF_TimeTableDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableDetail] DROP CONSTRAINT [DF_TimeTableDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableDetail]') AND type in (N'U'))
DROP TABLE [Institution].[TimeTableDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTransfer_StudentTransferID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTransfer] DROP CONSTRAINT [DF_StudentTransfer_StudentTransferID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTransfer_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTransfer] DROP CONSTRAINT [DF_StudentTransfer_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTransfer_riwguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTransfer] DROP CONSTRAINT [DF_StudentTransfer_riwguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentTransfer]') AND type in (N'U'))
DROP TABLE [Institution].[StudentTransfer]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupDetail_SubjectSetupDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupDetail] DROP CONSTRAINT [DF_SubjectSetupDetail_SubjectSetupDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupDetail] DROP CONSTRAINT [DF_SubjectSetupDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupDetail] DROP CONSTRAINT [DF_SubjectSetupDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[SubjectSetupDetail]') AND type in (N'U'))
DROP TABLE [Institution].[SubjectSetupDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentClearance_StudentClearanceID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentClearance] DROP CONSTRAINT [DF_StudentClearance_StudentClearanceID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentClearance_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentClearance] DROP CONSTRAINT [DF_StudentClearance_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentClearance_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentClearance] DROP CONSTRAINT [DF_StudentClearance_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentClearance]') AND type in (N'U'))
DROP TABLE [Institution].[StudentClearance]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentSubjectSelectionDetail_StudentSubjectSelectionDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentSubjectSelectionDetail] DROP CONSTRAINT [DF_StudentSubjectSelectionDetail_StudentSubjectSelectionDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentSubjectSelectionDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentSubjectSelectionDetail] DROP CONSTRAINT [DF_StudentSubjectSelectionDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentSubjectSelectionDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentSubjectSelectionDetail] DROP CONSTRAINT [DF_StudentSubjectSelectionDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentSubjectSelectionDetail]') AND type in (N'U'))
DROP TABLE [Institution].[StudentSubjectSelectionDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayoutDetail_PayoutDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[PayoutDetail] DROP CONSTRAINT [DF_PayoutDetail_PayoutDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayoutDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[PayoutDetail] DROP CONSTRAINT [DF_PayoutDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayoutDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[PayoutDetail] DROP CONSTRAINT [DF_PayoutDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[PayoutDetail]') AND type in (N'U'))
DROP TABLE [Institution].[PayoutDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Supplier_SupplierID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Supplier] DROP CONSTRAINT [DF_Supplier_SupplierID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vendor_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Supplier] DROP CONSTRAINT [DF_Vendor_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Vendor_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Supplier] DROP CONSTRAINT [DF_Vendor_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Supplier]') AND type in (N'U'))
DROP TABLE [Sales].[Supplier]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierDetail_SupplierDetailID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierDetail] DROP CONSTRAINT [DF_SupplierDetail_SupplierDetailID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierDetail_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierDetail] DROP CONSTRAINT [DF_SupplierDetail_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierDetail_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierDetail] DROP CONSTRAINT [DF_SupplierDetail_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SupplierDetail]') AND type in (N'U'))
DROP TABLE [Sales].[SupplierDetail]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierPayment_SupplierPaymentID]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierPayment] DROP CONSTRAINT [DF_SupplierPayment_SupplierPaymentID]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierPayment_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierPayment] DROP CONSTRAINT [DF_SupplierPayment_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SupplierPayment_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[SupplierPayment] DROP CONSTRAINT [DF_SupplierPayment_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SupplierPayment]') AND type in (N'U'))
DROP TABLE [Sales].[SupplierPayment]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Link_GetNewID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[Link_GetNewID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemIssueHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemIssueHeader] DROP CONSTRAINT [DF_ItemIssueHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ItemIssueHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[ItemIssueHeader] DROP CONSTRAINT [DF_ItemIssueHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemIssueHeader]') AND type in (N'U'))
DROP TABLE [Sales].[ItemIssueHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Product_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Item] DROP CONSTRAINT [DF_Product_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Product_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[Item] DROP CONSTRAINT [DF_Product_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Item]') AND type in (N'U'))
DROP TABLE [Sales].[Item]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetNewID]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureHeader] DROP CONSTRAINT [DF_FeesStructureHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureHeader] DROP CONSTRAINT [DF_FeesStructureHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesStructureHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesStructureHeader] DROP CONSTRAINT [DF_FeesStructureHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesStructureHeader]') AND type in (N'U'))
DROP TABLE [Institution].[FeesStructureHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EmployeePayment_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[EmployeePayment] DROP CONSTRAINT [DF_EmployeePayment_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EmployeePayment_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[EmployeePayment] DROP CONSTRAINT [DF_EmployeePayment_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[EmployeePayment]') AND type in (N'U'))
DROP TABLE [Institution].[EmployeePayment]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassSetupHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupHeader] DROP CONSTRAINT [DF_ClassSetupHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetup_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupHeader] DROP CONSTRAINT [DF_SubjectSetup_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetup_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassSetupHeader] DROP CONSTRAINT [DF_SubjectSetup_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassSetupHeader]') AND type in (N'U'))
DROP TABLE [Institution].[ClassSetupHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassGroup_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassGroupHeader] DROP CONSTRAINT [DF_ClassGroup_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ClassGroup_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ClassGroupHeader] DROP CONSTRAINT [DF_ClassGroup_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassGroupHeader]') AND type in (N'U'))
DROP TABLE [Institution].[ClassGroupHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Class_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Class] DROP CONSTRAINT [DF_Class_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Class_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Class] DROP CONSTRAINT [DF_Class_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Class]') AND type in (N'U'))
DROP TABLE [Institution].[Class]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultHeader] DROP CONSTRAINT [DF_ExamResultHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultHeader] DROP CONSTRAINT [DF_ExamResultHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamResultHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamResultHeader] DROP CONSTRAINT [DF_ExamResultHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamResultHeader]') AND type in (N'U'))
DROP TABLE [Institution].[ExamResultHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesPayment_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesPayment] DROP CONSTRAINT [DF_FeesPayment_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FeesPayment_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[FeesPayment] DROP CONSTRAINT [DF_FeesPayment_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesPayment]') AND type in (N'U'))
DROP TABLE [Institution].[FeesPayment]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamHeader_ExamDatetime]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamHeader] DROP CONSTRAINT [DF_ExamHeader_ExamDatetime]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamHeader_Modifieddate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamHeader] DROP CONSTRAINT [DF_ExamHeader_Modifieddate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ExamHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[ExamHeader] DROP CONSTRAINT [DF_ExamHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamHeader]') AND type in (N'U'))
DROP TABLE [Institution].[ExamHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableHeader] DROP CONSTRAINT [DF_TimeTableHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableHeader] DROP CONSTRAINT [DF_TimeTableHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableHeader] DROP CONSTRAINT [DF_TimeTableHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableHeader]') AND type in (N'U'))
DROP TABLE [Institution].[TimeTableHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableSettings_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableSettings] DROP CONSTRAINT [DF_TimeTableSettings_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableSettings_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableSettings] DROP CONSTRAINT [DF_TimeTableSettings_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_TimeTableSettings_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[TimeTableSettings] DROP CONSTRAINT [DF_TimeTableSettings_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableSettings]') AND type in (N'U'))
DROP TABLE [Institution].[TimeTableSettings]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayoutHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[PayoutHeader] DROP CONSTRAINT [DF_PayoutHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_PayoutHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[PayoutHeader] DROP CONSTRAINT [DF_PayoutHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[PayoutHeader]') AND type in (N'U'))
DROP TABLE [Institution].[PayoutHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_QBSync_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[QBSync] DROP CONSTRAINT [DF_QBSync_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_QBSync_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[QBSync] DROP CONSTRAINT [DF_QBSync_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[QBSync]') AND type in (N'U'))
DROP TABLE [Institution].[QBSync]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetUniqueIDs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ResetUniqueIDs]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Staff_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Staff] DROP CONSTRAINT [DF_Staff_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Staff_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Staff] DROP CONSTRAINT [DF_Staff_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Staff]') AND type in (N'U'))
DROP TABLE [Institution].[Staff]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupHeader] DROP CONSTRAINT [DF_SubjectSetupHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupHeader] DROP CONSTRAINT [DF_SubjectSetupHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_SubjectSetupHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[SubjectSetupHeader] DROP CONSTRAINT [DF_SubjectSetupHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[SubjectSetupHeader]') AND type in (N'U'))
DROP TABLE [Institution].[SubjectSetupHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Subject_IsOptional]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Subject] DROP CONSTRAINT [DF_Subject_IsOptional]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Subject_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Subject] DROP CONSTRAINT [DF_Subject_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Subject_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[Subject] DROP CONSTRAINT [DF_Subject_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Subject]') AND type in (N'U'))
DROP TABLE [Institution].[Subject]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTranscriptHeader_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTranscriptHeader] DROP CONSTRAINT [DF_StudentTranscriptHeader_IsActive]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTranscriptHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTranscriptHeader] DROP CONSTRAINT [DF_StudentTranscriptHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StudentTranscriptHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Institution].[StudentTranscriptHeader] DROP CONSTRAINT [DF_StudentTranscriptHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentTranscriptHeader]') AND type in (N'U'))
DROP TABLE [Institution].[StudentTranscriptHeader]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StockTakingHeader_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[StockTakingHeader] DROP CONSTRAINT [DF_StockTakingHeader_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_StockTakingHeader_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Sales].[StockTakingHeader] DROP CONSTRAINT [DF_StockTakingHeader_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[StockTakingHeader]') AND type in (N'U'))
DROP TABLE [Sales].[StockTakingHeader]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sysIDs]') AND type in (N'U'))
DROP TABLE [dbo].[sysIDs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGrade]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetGrade]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTablePrimaryKeyColumn]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetTablePrimaryKeyColumn]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddValuesIgnoringNull]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[AddValuesIgnoringNull]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_User_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[User] DROP CONSTRAINT [DF_User_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_User_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[User] DROP CONSTRAINT [DF_User_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[User]') AND type in (N'U'))
DROP TABLE [Users].[User]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserRole_ModifiedDate]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[UserRole] DROP CONSTRAINT [DF_UserRole_ModifiedDate]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserRole_rowguid]') AND type = 'D')
BEGIN
ALTER TABLE [Users].[UserRole] DROP CONSTRAINT [DF_UserRole_rowguid]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[UserRole]') AND type in (N'U'))
DROP TABLE [Users].[UserRole]
GO
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Users')
DROP SCHEMA [Users]
GO
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Sales')
DROP SCHEMA [Sales]
GO
IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'Institution')
DROP SCHEMA [Institution]
GO
DECLARE @RoleName sysname
set @RoleName = N'Accounts'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Accounts' AND type = 'R')
DROP ROLE [Accounts]
GO
DECLARE @RoleName sysname
set @RoleName = N'Deputy'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Deputy' AND type = 'R')
DROP ROLE [Deputy]
GO
DECLARE @RoleName sysname
set @RoleName = N'None'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'None' AND type = 'R')
DROP ROLE [None]
GO
DECLARE @RoleName sysname
set @RoleName = N'Principal'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Principal' AND type = 'R')
DROP ROLE [Principal]
GO
DECLARE @RoleName sysname
set @RoleName = N'SystemAdmin'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'SystemAdmin' AND type = 'R')
DROP ROLE [SystemAdmin]
GO
DECLARE @RoleName sysname
set @RoleName = N'Teacher'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Teacher' AND type = 'R')
DROP ROLE [Teacher]
GO
DECLARE @RoleName sysname
set @RoleName = N'User'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
Begin
	DECLARE @RoleMemberName sysname
	DECLARE Member_Cursor CURSOR FOR
	select [name]
	from sys.database_principals 
	where principal_id in ( 
		select member_principal_id 
		from sys.database_role_members 
		where role_principal_id in (
			select principal_id
			FROM sys.database_principals where [name] = @RoleName  AND type = 'R' ))

	OPEN Member_Cursor;

	FETCH NEXT FROM Member_Cursor
	into @RoleMemberName

	WHILE @@FETCH_STATUS = 0
	BEGIN

		exec sp_droprolemember @rolename=@RoleName, @membername= @RoleMemberName

		FETCH NEXT FROM Member_Cursor
		into @RoleMemberName
	END;

	CLOSE Member_Cursor;
	DEALLOCATE Member_Cursor;
End
GO
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'User' AND type = 'R')
DROP ROLE [User]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Accounts' AND type = 'R')
CREATE ROLE [Accounts] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Deputy' AND type = 'R')
CREATE ROLE [Deputy] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'None' AND type = 'R')
CREATE ROLE [None] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Principal' AND type = 'R')
CREATE ROLE [Principal] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'SystemAdmin' AND type = 'R')
CREATE ROLE [SystemAdmin] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Teacher' AND type = 'R')
CREATE ROLE [Teacher] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'User' AND type = 'R')
CREATE ROLE [User] AUTHORIZATION [dbo]
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Users')
EXEC sys.sp_executesql N'CREATE SCHEMA [Users] AUTHORIZATION [dbo]'
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Sales')
EXEC sys.sp_executesql N'CREATE SCHEMA [Sales] AUTHORIZATION [dbo]'
GO
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Institution')
EXEC sys.sp_executesql N'CREATE SCHEMA [Institution] AUTHORIZATION [dbo]'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[UserRole]') AND type in (N'U'))
BEGIN
CREATE TABLE [Users].[UserRole](
	[UserRoleID] [int] NOT NULL,
	[Description] [varchar](20) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_UserRole_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserRole_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [Users].[User](
	[UserID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_User_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_User_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddValuesIgnoringNull]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[AddValuesIgnoringNull](@score1 decimal,@score2 decimal,@score3 decimal,@score4 decimal,@score5 decimal)
    RETURNS decimal
    AS
    BEGIN
    DECLARE @total decimal;

    set @total = NULL;

	if NOT @score1 IS NULL
	set @total = @score1;
	if NOT @score2 IS NULL
	set @total = @total+@score2;
   if NOT @score3 IS NULL
	set @total = @total+@score3;
	if NOT @score4 IS NULL
	set @total = @total+@score4;
	if NOT @score5 IS NULL
	set @total = @total+@score5;

    RETURN @total
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTablePrimaryKeyColumn]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTablePrimaryKeyColumn](@nameOfTable varchar(max))
    RETURNS NVARCHAR(50)
    AS
    BEGIN

RETURN (SELECT COL_NAME(ic.OBJECT_ID,
ic.column_id) AS ColumnName FROM sys.indexes AS i
 INNER JOIN sys.index_columns AS ic ON i.OBJECT_ID = ic.OBJECT_ID AND 
 i.index_id = ic.index_id 
 INNER JOIN sys.tables t ON (ic.OBJECT_ID=t.object_id)WHERE i.is_primary_key = 1 AND  
 UPPER(SCHEMA_NAME(t.schema_id)+''.''+OBJECT_NAME(ic.OBJECT_ID))=UPPER(@nameOfTable));
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetGrade]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetGrade](@score decimal)
    RETURNS varchar(2)
    AS
    BEGIN
    DECLARE @grade varchar(2)

    set @grade = (SELECT 
   CASE 
     WHEN (@score >= 80) AND( @score <= 100) THEN ''A''

            WHEN (@score >= 75 AND @score <= 79)
                THEN ''A-''
            WHEN (@score >= 70 AND @score <= 74)
                THEN ''B+''
            WHEN (@score >= 65 AND @score <= 69)
                THEN ''B''
            WHEN (@score >= 60 AND @score <= 64)
                THEN ''B-''
            WHEN (@score >= 55 AND @score <= 59)
                THEN ''C+''
            WHEN (@score >= 50 AND @score <= 54)
                THEN ''C''
            WHEN (@score >= 45 AND @score <= 49)
                THEN ''C-''
            WHEN (@score >= 40 AND @score <= 44)
                THEN ''D+''
            WHEN (@score >= 35 AND @score <= 39)
                THEN ''D''
            WHEN (@score >= 30 AND @score <= 34)
                THEN ''D-''
            WHEN (@score >= 0 AND @score <= 29)
                THEN ''E'' 
			WHEN (@score < 0 or @score > 100)
                THEN ''-'' 
   END)

    RETURN @grade
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sysIDs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[sysIDs](
	[table_name] [varchar](50) NOT NULL,
	[last_id] [int] NOT NULL,
 CONSTRAINT [PK_sysIDs] PRIMARY KEY CLUSTERED 
(
	[table_name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[StockTakingHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[StockTakingHeader](
	[StockTakingID] [int] NOT NULL,
	[DateTaken] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StockTakingHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StockTakingHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StockTakingHeader] PRIMARY KEY CLUSTERED 
(
	[StockTakingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentTranscriptHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[StudentTranscriptHeader](
	[StudentTranscriptID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[Exam1ID] [int] NULL,
	[Exam2ID] [int] NULL,
	[Exam3ID] [int] NULL,
	[IsActive] [bit] NULL CONSTRAINT [DF_StudentTranscriptHeader_IsActive]  DEFAULT ((1)),
	[Responsibilities] [varchar](50) NULL,
	[ClubsAndSport] [varchar](50) NULL,
	[Boarding] [varchar](50) NULL,
	[ClassTeacher] [varchar](50) NULL,
	[ClassTeacherComments] [varchar](50) NULL,
	[Principal] [varchar](50) NULL,
	[PrincipalComments] [varchar](50) NULL,
	[OpeningDay] [datetime] NULL,
	[ClosingDay] [datetime] NULL,
	[Term1Pos] [varchar](50) NULL,
	[Term2Pos] [varchar](50) NULL,
	[Term3Pos] [varchar](50) NULL,
	[DateSaved] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StudentTranscriptHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StudentTranscriptHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StudentTranscriptHeader] PRIMARY KEY CLUSTERED 
(
	[StudentTranscriptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Subject]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Subject](
	[SubjectID] [int] NOT NULL,
	[NameOfSubject] [varchar](50) NOT NULL,
	[Code] [int] NULL,
	[Tutor] [varchar](50) NULL,
	[MaximumScore] [varchar](50) NOT NULL,
	[IsOptional] [bit] NOT NULL CONSTRAINT [DF_Subject_IsOptional]  DEFAULT ((0)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Subject_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Subject_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[SubjectSetupHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[SubjectSetupHeader](
	[SubjectSetupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_SubjectSetupHeader_IsActive]  DEFAULT ((1)),
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SubjectSetupHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SubjectSetupHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SubjectSetup] PRIMARY KEY CLUSTERED 
(
	[SubjectSetupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Staff]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Staff](
	[StaffID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[NationalID] [varchar](50) NULL,
	[DateOfAdmission] [datetime] NULL,
	[PhoneNo] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[PostalCode] [varchar](50) NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Staff_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Staff_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetUniqueIDs]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[ResetUniqueIDs]
AS
BEGIN
 SET NOCOUNT ON;
 if EXISTS(select s.name +''.''+t.name, 0 from [UmanyiSMS].[sys].[tables] t inner JOIN 
 UmanyiSMS.sys.schemas s on (t.schema_id=s.schema_id))
 BEGIN
 delete from dbo.sysIDs;
 
 DECLARE tables_cursor CURSOR
   FOR
   SELECT s.name,t.name FROM sys.tables AS t
 INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id 
 order by (s.name+''.''+t.name )
 
OPEN tables_cursor;
DECLARE @nameOfTable varchar(50);
DECLARE @schema_name varchar(50);
DECLARE @table_name varchar(50);
FETCH NEXT FROM tables_cursor INTO @schema_name,@table_name;
WHILE (@@FETCH_STATUS <> -1)
BEGIN;
set @nameOfTable = @schema_name+''.''+@table_name;

declare @lastID int;
declare @sql nvarchar(max);
declare @pkey varchar(50)=dbo.GetTablePrimaryKeyColumn(@nameOfTable);
    if @pkey is null
    set @pkey=(select  Top 1 name from sys.columns where object_id=object_id(@nameOfTable))
    
    set @sql=''select @lastID=ISNULL(MAX(''+@pkey+''),0) from [''+@schema_name+''].[''+@table_name+'']'';    
    exec sp_executesql @sql, N''@lastID int output'', @lastID output;    
declare @sql2 nvarchar(200)=''INSERT INTO dbo.sysIDs(table_name,last_id) VALUES('''''' + @nameOfTable + '''''',''+CONVERT(varchar(50),@lastID)+'')'';
   EXEC (@sql2);
   FETCH NEXT FROM tables_cursor INTO @schema_name,@table_name
END;
CLOSE tables_cursor;
DEALLOCATE tables_cursor;

 
END
END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[QBSync]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[QBSync](
	[QBSyncID] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[TransactionID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_QBSync_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_QBSync_rowguid]  DEFAULT (newid())
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[PayoutHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[PayoutHeader](
	[PayoutID] [int] NOT NULL,
	[Payee] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[TotalPaid] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_PayoutHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PayoutHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_PayoutHeader] PRIMARY KEY CLUSTERED 
(
	[PayoutID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableSettings]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[TimeTableSettings](
	[TimeTableSettingsID] [int] NOT NULL,
	[NoOfLessons] [int] NOT NULL,
	[LessonDuration] [int] NOT NULL,
	[LessonsStartTime] [time](7) NOT NULL,
	[BreakIndices] [varchar](50) NOT NULL,
	[BreakDuration] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_TimeTableSettings_IsActive]  DEFAULT ((1)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_TimeTableSettings_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_TimeTableSettings_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_TimeTableSettings] PRIMARY KEY CLUSTERED 
(
	[TimeTableSettingsID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[TimeTableHeader](
	[TimeTableID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_TimeTableHeader_IsActive]  DEFAULT ((1)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_TimeTableHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_TimeTableHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_TimeTableHeader] PRIMARY KEY CLUSTERED 
(
	[TimeTableID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ExamHeader](
	[ExamID] [int] NOT NULL,
	[NameOfExam] [varchar](50) NOT NULL,
	[OutOf] [decimal](18, 0) NULL,
	[ExamDatetime] [datetime] NOT NULL CONSTRAINT [DF_ExamHeader_ExamDatetime]  DEFAULT (sysdatetime()),
	[Modifieddate] [datetime] NOT NULL CONSTRAINT [DF_ExamHeader_Modifieddate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ExamHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ExamHeader_1] PRIMARY KEY CLUSTERED 
(
	[ExamID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[FeesPayment](
	[FeesPaymentID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[AmountPaid] [varchar](50) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_FeesPayment_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_FeesPayment_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_FeesPayment] PRIMARY KEY CLUSTERED 
(
	[FeesPaymentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamResultHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ExamResultHeader](
	[ExamResultID] [int] NOT NULL,
	[ExamID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ExamResultHeader_IsActive]  DEFAULT ((1)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ExamResultHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ExamResultHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ExamResultHeader] PRIMARY KEY CLUSTERED 
(
	[ExamResultID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Class]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Class](
	[ClassID] [int] NOT NULL,
	[NameOfClass] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Class_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Class_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassGroupHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ClassGroupHeader](
	[ClassGroupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ClassGroup_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ClassGroup_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ClassGroup] PRIMARY KEY CLUSTERED 
(
	[ClassGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassSetupHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ClassSetupHeader](
	[ClassSetupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_ClassSetupHeader_IsActive]  DEFAULT ((1)),
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SubjectSetup_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SubjectSetup_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SubjectSetup_1] PRIMARY KEY CLUSTERED 
(
	[ClassSetupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[EmployeePayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[EmployeePayment](
	[EmployeePaymentID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[AmountPaid] [decimal](18, 0) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[Notes] [varchar](max) NULL,
	[ModifiedDate] [nchar](10) NOT NULL CONSTRAINT [DF_EmployeePayment_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_EmployeePayment_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_EmployeePayment] PRIMARY KEY CLUSTERED 
(
	[EmployeePaymentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesStructureHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[FeesStructureHeader](
	[FeesStructureID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NULL CONSTRAINT [DF_FeesStructureHeader_IsActive]  DEFAULT ((1)),
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_FeesStructureHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_FeesStructureHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_FeesStructureHeader] PRIMARY KEY CLUSTERED 
(
	[FeesStructureID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetNewID](@nameOfTable varchar(max))
    RETURNS int
    AS
    BEGIN

    DECLARE @lastID int
 if (exists(SELECT [last_id] FROM [dbo].[sysids] where UPPER(table_name) = UPPER(@nameOfTable)))
    set @lastID = (SELECT [last_id]+1 FROM [dbo].[sysids] where UPPER(table_name) = UPPER(@nameOfTable))

    RETURN @lastID
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Item]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[Item](
	[ItemID] [bigint] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ItemCategoryID] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Cost] [decimal](18, 0) NOT NULL,
	[StartQuantity] [decimal](18, 0) NOT NULL,
	[VatID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Product_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Product_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemIssueHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[ItemIssueHeader](
	[ItemIssueID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[IsCancelled] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ItemIssueHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ItemIssueHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ItemIssueHeader] PRIMARY KEY CLUSTERED 
(
	[ItemIssueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_StockTakingHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_StockTakingHeader_UpdateID]
 ON [Sales].[StockTakingHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.StockTakingHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Staff_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Staff_UpdateID]
 ON [Institution].[Staff] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Staff'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_QBSync_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_QBSync_UpdateID]
 ON [Institution].[QBSync] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.QBSync'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_PayoutHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_PayoutHeader_UpdateID]
 ON [Institution].[PayoutHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.PayoutHeader'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_Item_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_Item_UpdateID]
 ON [Sales].[Item] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.Item'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_ItemIssueHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_ItemIssueHeader_UpdateID]
 ON [Sales].[ItemIssueHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.ItemIssueHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentTranscriptHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentTranscriptHeader_UpdateID]
 ON [Institution].[StudentTranscriptHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.StudentTranscriptHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Subject_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Subject_UpdateID]
 ON [Institution].[Subject] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Subject'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_TimeTableSettings_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_TimeTableSettings_UpdateID]
 ON [Institution].[TimeTableSettings] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.TimeTableSettings'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_TimeTableSettings_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_TimeTableSettings_SetActive_EndDate]
 ON [Institution].[TimeTableSettings] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN 
     UPDATE [Institution].[TimeTableSettings] SET IsActive = 0 WHERE TimeTableSettingsID NOT IN 
  (SELECT TimeTableSettingsID FROM inserted)
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_TimeTableHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_TimeTableHeader_UpdateID]
 ON [Institution].[TimeTableHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.TimeTableHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_TimeTableHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_TimeTableHeader_SetActive_EndDate]
 ON [Institution].[TimeTableHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN 
     UPDATE [Institution].[TimeTableHeader] SET IsActive = 0 WHERE TimeTableID NOT IN 
  (SELECT TimeTableID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_SubjectSetupHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_UpdateID]
 ON [Institution].[SubjectSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.SubjectSetupHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_SubjectSetupHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_SetActive_EndDate]
 ON [Institution].[SubjectSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[SubjectSetupHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND SubjectSetupID NOT IN (SELECT SubjectSetupID FROM inserted) 
  AND ClassID IN (SELECT ClassID FROM inserted) 
     UPDATE [Institution].[SubjectSetupHeader] SET IsActive = 0 WHERE SubjectSetupID NOT IN 
  (SELECT SubjectSetupID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Link_GetNewID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Link_GetNewID](@nameOfTable varchar(max))
    RETURNS int
    AS
    BEGIN

    RETURN dbo.GetNewID(@nameOfTable);
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_EmployeePayment_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_EmployeePayment_UpdateID]
 ON [Institution].[EmployeePayment] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.EmployeePayment'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamHeader_UpdateID]
 ON [Institution].[ExamHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ExamHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_FeesPayment_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_FeesPayment_UpdateID]
 ON [Institution].[FeesPayment] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.FeesPayment'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamResultHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamResultHeader_UpdateID]
 ON [Institution].[ExamResultHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ExamResultHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamResultHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamResultHeader_SetActive_EndDate]
 ON [Institution].[ExamResultHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
   UPDATE [Institution].[ExamResultHeader] SET IsActive=0 WHERE ExamResultID IN(
     SELECT ExamResultID FROM [Institution].[ExamResultHeader] WHERE ExamResultID NOT IN 
  (SELECT ExamResultID FROM inserted) AND StudentID IN 
  (SELECT StudentID FROM inserted) AND ExamID IN
  (SELECT ExamID FROM inserted))
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassSetupHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassSetupHeader_UpdateID]
 ON [Institution].[ClassSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ClassSetupHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassSetupHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassSetupHeader_SetActive_EndDate]
 ON [Institution].[ClassSetupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[ClassSetupHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND ClassSetupID NOT IN (SELECT ClassSetupID FROM inserted)
     UPDATE [Institution].[ClassSetupHeader] SET IsActive = 0 WHERE ClassSetupID NOT IN 
  (SELECT ClassSetupID FROM inserted)
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_FeesStructureHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_FeesStructureHeader_UpdateID]
 ON [Institution].[FeesStructureHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.FeesStructureHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_FeesStructureHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_FeesStructureHeader_SetActive_EndDate]
 ON [Institution].[FeesStructureHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
  UPDATE [Institution].[FeesStructureHeader] SET EndDate = SYSDATETIME() WHERE IsActive=1 
  AND FeesStructureID NOT IN (SELECT FeesStructureID FROM inserted) 
  AND ClassID IN (SELECT ClassID FROM inserted) 
     UPDATE [Institution].[FeesStructureHeader] SET IsActive = 0 WHERE FeesStructureID NOT IN 
  (SELECT FeesStructureID FROM inserted) AND ClassID IN (SELECT ClassID FROM inserted) 
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Class_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Class_UpdateID]
 ON [Institution].[Class] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Class'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassGroupHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassGroupHeader_UpdateID]
 ON [Institution].[ClassGroupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ClassGroupHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassGroupHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassGroupHeader_SetActive_EndDate]
 ON [Institution].[ClassGroupHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[ClassGroupHeader] SET IsActive = 0 WHERE ClassGroupID NOT IN 
  (SELECT ClassGroupID FROM inserted)
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SupplierPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[SupplierPayment](
	[SupplierPaymentID] [int] NOT NULL CONSTRAINT [DF_SupplierPayment_SupplierPaymentID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.SupplierPayment')),
	[SupplierID] [int] NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[AmountPaid] [decimal](18, 0) NOT NULL,
	[Notes] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SupplierPayment_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SupplierPayment_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SupplierPayment] PRIMARY KEY CLUSTERED 
(
	[SupplierPaymentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SupplierDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[SupplierDetail](
	[SupplierDetailID] [int] NOT NULL CONSTRAINT [DF_SupplierDetail_SupplierDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.SupplierDetail')),
	[SupplierID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SupplierDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SupplierDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SupplierDetail] PRIMARY KEY CLUSTERED 
(
	[SupplierDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Supplier]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[Supplier](
	[SupplierID] [int] NOT NULL CONSTRAINT [DF_Supplier_SupplierID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.Supplier')),
	[NameOfSupplier] [varchar](50) NOT NULL,
	[PhoneNo] [varchar](50) NOT NULL,
	[AltPhoneNo] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[PostalCode] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[PINNo] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Vendor_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Vendor_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[PayoutDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[PayoutDetail](
	[PayoutDetailID] [int] NOT NULL CONSTRAINT [DF_PayoutDetail_PayoutDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.PayoutDetail')),
	[PayoutID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[Amount] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_PayoutDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PayoutDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_PayoutDetail] PRIMARY KEY CLUSTERED 
(
	[PayoutDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentSubjectSelectionDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[StudentSubjectSelectionDetail](
	[StudentSubjectSelectionDetailID] [int] NOT NULL CONSTRAINT [DF_StudentSubjectSelectionDetail_StudentSubjectSelectionDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentSubjectSelectionDetail')),
	[StudentSubjectSelectionID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StudentSubjectSelectionDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StudentSubjectSelectionDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StudentSubjectSelectionDetail] PRIMARY KEY CLUSTERED 
(
	[StudentSubjectSelectionDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentClearance]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[StudentClearance](
	[StudentClearanceID] [int] NOT NULL CONSTRAINT [DF_StudentClearance_StudentClearanceID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentClearance')),
	[StudentID] [int] NOT NULL,
	[DateCleared] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StudentClearance_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StudentClearance_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StudentClearance] PRIMARY KEY CLUSTERED 
(
	[StudentClearanceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[SubjectSetupDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[SubjectSetupDetail](
	[SubjectSetupDetailID] [int] NOT NULL CONSTRAINT [DF_SubjectSetupDetail_SubjectSetupDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.SubjectSetupDetail')),
	[SubjectSetupID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SubjectSetupDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SubjectSetupDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SubjectSetupDetail] PRIMARY KEY CLUSTERED 
(
	[SubjectSetupDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentTransfer]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[StudentTransfer](
	[StudentTransferID] [int] NOT NULL CONSTRAINT [DF_StudentTransfer_StudentTransferID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.StudentTransfer')),
	[StudentID] [int] NOT NULL,
	[DateTransferred] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StudentTransfer_ModifiedDate]  DEFAULT (sysdatetime()),
	[riwguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StudentTransfer_riwguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StudentTransfer] PRIMARY KEY CLUSTERED 
(
	[StudentTransferID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[TimeTableDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[TimeTableDetail](
	[TimeTableDetailID] [int] NOT NULL CONSTRAINT [DF_TimeTableDetail_TimeTableDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.TimeTableDetail')),
	[TimeTableID] [int] NOT NULL,
	[SubjectIndex] [int] NOT NULL,
	[NameOfSubject] [varchar](50) NOT NULL,
	[Tutor] [varchar](50) NULL,
	[Day] [varchar](50) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_TimeTableDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_TimeTableDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_TimeTableDetail] PRIMARY KEY CLUSTERED 
(
	[TimeTableDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SaleDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[SaleDetail](
	[SalesOrderDetailID] [int] NOT NULL CONSTRAINT [DF_SaleDetail_SalesOrderDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.SaleDetail')),
	[SaleID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SalesOrderDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SalesOrderDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SalesOrderDetail] PRIMARY KEY CLUSTERED 
(
	[SalesOrderDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[LeavingCertificate]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[LeavingCertificate](
	[LeavingCertificateID] [int] NOT NULL CONSTRAINT [DF_LeavingCertificate_LeavingCertificateID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.LeavingCertificate')),
	[StudentID] [int] NOT NULL,
	[DateOfIssue] [datetime] NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[DateOfAdmission] [datetime] NOT NULL,
	[DateOfLeaving] [datetime] NOT NULL,
	[Nationality] [varchar](50) NOT NULL,
	[ClassEntered] [varchar](50) NOT NULL,
	[ClassLeft] [varchar](50) NOT NULL,
	[Remarks] [varchar](1000) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_LeavingCertificate_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_LeavingCertificate_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_LeavingCertificate] PRIMARY KEY CLUSTERED 
(
	[LeavingCertificateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemReceiptDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[ItemReceiptDetail](
	[ItemReceiptDetailID] [int] NOT NULL CONSTRAINT [DF_ItemReceiptDetail_ItemReceiptDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemreceiptDetail')),
	[ItemReceiptID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[UnitCost] [decimal](18, 0) NOT NULL,
	[LineTotal] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_PurchaseOrderDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PurchaseOrderDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_PurchaseOrderDetail] PRIMARY KEY CLUSTERED 
(
	[ItemReceiptDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemIssueDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[ItemIssueDetail](
	[ItemIssueDetailID] [int] NOT NULL CONSTRAINT [DF_ItemIssueDetail_ItemIssueDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemIssueDetail')),
	[ItemIssueID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ItemIssueDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ItemIssueDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ItemIssueDetail] PRIMARY KEY CLUSTERED 
(
	[ItemIssueDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemCategory]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[ItemCategory](
	[ItemCategoryID] [int] NOT NULL CONSTRAINT [DF_ItemCategory_ItemCategoryID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.ItemCategory')),
	[Description] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ProductCategory_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ProductCategory_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ItemCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Gallery]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Gallery](
	[GalleryID] [int] NOT NULL CONSTRAINT [DF_Gallery_GalleryID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.Gallery')),
	[Name] [varchar](255) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Gallery_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Gallery_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Gallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[FeesStructureDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[FeesStructureDetail](
	[FeesStructureDetailID] [int] NOT NULL CONSTRAINT [DF_FeesStructureDetail_FeesStructureDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.FeesStructureDetail')),
	[FeesStructureID] [int] NULL,
	[Name] [varchar](50) NULL,
	[Amount] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_FeesStructureDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_FeesStructureDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_FeesStructureDetail] PRIMARY KEY CLUSTERED 
(
	[FeesStructureDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Dormitory]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Dormitory](
	[DormitoryID] [int] NOT NULL CONSTRAINT [DF_Dormitory_DormitoryID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.Dormitory')),
	[NameOfDormitory] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Dormitory_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Dormitory_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Dormitory] PRIMARY KEY CLUSTERED 
(
	[DormitoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Discipline]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Discipline](
	[DisciplineID] [int] NOT NULL CONSTRAINT [DF_Discipline_DisciplineID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.Discipline')),
	[StudentID] [int] NOT NULL,
	[Issue] [varchar](50) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[SPhoto] [varbinary](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Discipline_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Discipline_rowguid]  DEFAULT (newid())
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[CurrentClass]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[CurrentClass](
	[CurrentClassID] [int] NOT NULL CONSTRAINT [DF_CurrentClass_CurrentClassID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.CurrentClass')),
	[StudentID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_CurrentClass_IsActive]  DEFAULT ((1)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_CurrentClass_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_CurrentClass_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_CurrentClass] PRIMARY KEY CLUSTERED 
(
	[CurrentClassID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassSetupDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ClassSetupDetail](
	[ClassSetupDetailID] [int] NOT NULL CONSTRAINT [DF_ClassSetupDetail_ClassSetupDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.ClassSetupDetail')),
	[ClassSetupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ClassSetupDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ClassSetupDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ClassSetupDetail] PRIMARY KEY CLUSTERED 
(
	[ClassSetupDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ClassGroupDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ClassGroupDetail](
	[ClassGroupDetailID] [int] NOT NULL CONSTRAINT [DF_ClassGroupDetail_ClassGroupDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.ClassGroupDetail')),
	[ClassGroupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ClassGroupDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ClassGroupDetail_rowguid]  DEFAULT (newid())
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookReturnHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[BookReturnHeader](
	[BookReturnID] [int] NOT NULL CONSTRAINT [DF_BookReturnHeader_BookReturnID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.BookReturnHeader')),
	[StudentID] [int] NOT NULL,
	[DateReturned] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BookReturnHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_BookReturnHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_BookReturnHeader] PRIMARY KEY CLUSTERED 
(
	[BookReturnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookReturnDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[BookReturnDetail](
	[BookReturnDetailID] [int] NOT NULL CONSTRAINT [DF_BookReturnDetail_BookReturnDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.BookReturnDetail')),
	[BookReturnID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BookReturnDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_BookReturnDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_BookReturnDetail] PRIMARY KEY CLUSTERED 
(
	[BookReturnDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookIssueHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[BookIssueHeader](
	[BookIssueID] [int] NOT NULL CONSTRAINT [DF_BookIssueHeader_BookIssueID]  DEFAULT ([dbo].[Link_GetNewID]('BookIssueHeader')),
	[StudentID] [int] NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BookIssue_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_BookIssue_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_BookIssue] PRIMARY KEY CLUSTERED 
(
	[BookIssueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[BookIssueDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[BookIssueDetail](
	[BookIssueDetailID] [int] NOT NULL CONSTRAINT [DF_BookIssueDetail_BookIssueDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.BookIssueDetail')),
	[BookIssueID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BookIssueDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_BookIssueDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_BookIssueDetail] PRIMARY KEY CLUSTERED 
(
	[BookIssueDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Book]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Book](
	[BookID] [int] NOT NULL CONSTRAINT [DF_Book_BookID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.Book')),
	[ISBN] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Author] [varchar](50) NOT NULL,
	[Publisher] [varchar](50) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Book_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Book_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamResultDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ExamResultDetail](
	[ExamResultDetail] [int] NOT NULL CONSTRAINT [DF_ExamResultDetail_ExamResultDetail]  DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamResultDetail')),
	[ExamResultID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[Score] [decimal](18, 0) NOT NULL,
	[Grade]  AS ([dbo].[GetGrade](CONVERT([decimal],[Score],(0)))),
	[Remarks] [varchar](50) NULL,
	[Tutor] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ExamResultDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ExamResultDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ExamResultDetail] PRIMARY KEY CLUSTERED 
(
	[ExamResultDetail] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ExamDetail](
	[ExamDetailID] [int] NOT NULL CONSTRAINT [DF_ExamDetail_ExamDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamDetail')),
	[ExamID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ExamDateTime] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ExamDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ExamDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ExamDetail] PRIMARY KEY CLUSTERED 
(
	[ExamDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[ExamClassDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[ExamClassDetail](
	[ExamClassDetailID] [int] NOT NULL CONSTRAINT [DF_ExamClassDetail_ExamClassDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.ExamClassDetail')),
	[ExamID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ExamClassDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ExamClassDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_ExamClassDetail] PRIMARY KEY CLUSTERED 
(
	[ExamClassDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Event]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Event](
	[EventID] [int] NOT NULL CONSTRAINT [DF_Event_EventID]  DEFAULT ([dbo].[Link_GetNewID]('Institution.Event')),
	[Name] [varchar](50) NULL,
	[StartDateTime] [varchar](50) NULL,
	[EndDateTime] [varchar](50) NULL,
	[Location] [varchar](50) NULL,
	[Subject] [varchar](50) NULL,
	[Message] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Event_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Event_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[Vat]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[Vat](
	[VatID] [int] NOT NULL CONSTRAINT [DF_Vat_VatID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.Vat')),
	[Description] [varchar](50) NOT NULL,
	[Rate] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Vat_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Vat_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Vat] PRIMARY KEY CLUSTERED 
(
	[VatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Users].[UserDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Users].[UserDetail](
	[UserDetailID] [int] NOT NULL CONSTRAINT [DF_UserDetail_UserDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Users.UserDetail')),
	[UserID] [varchar](50) NOT NULL,
	[UserRoleID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_UserDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[UserDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_SaleDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_SaleDetail_UpdateID]
 ON [Sales].[SaleDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.SaleDetail'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_ItemIssueDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_ItemIssueDetail_UpdateID]
 ON [Sales].[ItemIssueDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.ItemIssueDetail'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_ItemCategory_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_ItemCategory_UpdateID]
 ON [Sales].[ItemCategory] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.ItemCategory'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Gallery_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Gallery_UpdateID]
 ON [Institution].[Gallery] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Gallery'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_PayoutDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_PayoutDetail_UpdateID]
 ON [Institution].[PayoutDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.PayoutDetail'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_LeavingCertificate_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_LeavingCertificate_UpdateID]
 ON [Institution].[LeavingCertificate] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.LeavingCertificate'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_ItemReceiptDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_ItemReceiptDetail_UpdateID]
 ON [Sales].[ItemReceiptDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.ItemReceiptDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentSubjectSelectionDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionDetail_UpdateID]
 ON [Institution].[StudentSubjectSelectionDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.StudentSubjectSelectionDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentClearance_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentClearance_UpdateID]
 ON [Institution].[StudentClearance] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.StudentClearance'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_Vat_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_Vat_UpdateID]
 ON [Sales].[Vat] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.Vat'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Users].[TR_UserDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Users].[TR_UserDetail_UpdateID]
 ON [Users].[UserDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Users.UserDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_SubjectSetupDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_SubjectSetupDetail_UpdateID]
 ON [Institution].[SubjectSetupDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.SubjectSetupDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_TimeTableDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_TimeTableDetail_UpdateID]
 ON [Institution].[TimeTableDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.TimeTableDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_SupplierPayment_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_SupplierPayment_UpdateID]
 ON [Sales].[SupplierPayment] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.SupplierPayment'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_Supplier_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_Supplier_UpdateID]
 ON [Sales].[Supplier] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.Supplier'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentTransfer_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentTransfer_UpdateID]
 ON [Institution].[StudentTransfer] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.StudentTransfer'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExamTotalScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetExamTotalScore](@studentID int, @examID int)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @total = (SELECT SUM(ISNULL(erd.Score,0)) FROM [Institution].[ExamResultHeader] erh
	LEFT OUTER JOIN [Institution].[ExamResultDetail] erd ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1)
    
	RETURN @total;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetExamSubjectScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetExamSubjectScore](@studentID int, @examID int, @subjectID int)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @score decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @score = (SELECT erd.Score FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh
	ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1 AND erd.SubjectID=@subjectID)
    
	RETURN @score;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentQuantity]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCurrentQuantity](@itemID bigint)
    RETURNS decimal
    AS
    BEGIN

    DECLARE @startQuantity decimal;
 DECLARE @totalSold decimal;
 DECLARE @totalBought decimal;
 DECLARE @currentQty decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[Item] WHERE ItemID=@itemID)
 RETURN 0
 ELSE
 BEGIN 
    set @startQuantity = ISNULL((SELECT StartQuantity FROM [Sales].[Item] where ItemID = @itemID),0)
 set @totalSold = ISNULL((SELECT SUM(Quantity) FROM [Sales].[ItemIssueDetail] where ItemID = @itemID),0)
 set @totalBought = ISNULL((SELECT SUM(Quantity) FROM [Sales].[ItemReceiptDetail] where ItemID = @itemID),0)

    set @currentQty=@startQuantity+@totalBought-@totalSold;
    END
 RETURN @currentQty
    END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentClass]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCurrentClass](@studentID int)
    RETURNS int
    AS
    BEGIN
	DECLARE @currentClassID int;
 IF NOT EXISTS (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND IsActive=1)
 RETURN 0
 ELSE
    set @currentClassID = (SELECT ClassID FROM [Institution].[CurrentClass] WHERE StudentID=@studentID AND IsActive=1);
    
	RETURN @currentClassID;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStudentIsActive]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetStudentIsActive](@studentID int)
    RETURNS bit
    AS
    BEGIN
 IF EXISTS (SELECT * FROM [Institution].[StudentClearance] WHERE StudentID=@studentID)
 RETURN 0;
  IF EXISTS (SELECT * FROM [Institution].[StudentTransfer] WHERE StudentID=@studentID)
 RETURN 0;    
	RETURN 1;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSaleTotal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSaleTotal](@saleID int)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[SaleDetail] WHERE SaleID=@saleID)
 RETURN 0
 ELSE
    set @total = (SELECT SUM(Amount) FROM [Sales].[SaleDetail] WHERE SaleID=@saleID);
    
	RETURN @total;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPurchaseTotal]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetPurchaseTotal](@purchaseID int)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Sales].[ItemReceiptDetail] WHERE ItemReceiptID=@purchaseID)
 RETURN 0
 ELSE
    set @total = (SELECT SUM(LineTotal) FROM [Sales].[ItemReceiptDetail] WHERE ItemReceiptID=@purchaseID);
    
	RETURN @total;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectsTakenByStudent]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSubjectsTakenByStudent](@studentSubjectSelectionID int)
    RETURNS int
    AS
    BEGIN

    DECLARE @noOfSubjects int;
 IF NOT EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@studentSubjectSelectionID)
 RETURN 0
 ELSE
 BEGIN 
    set @noOfSubjects = (SELECT COUNT(*) FROM [Institution].[StudentSubjectSelectionDetail] WHERE StudentSubjectSelectionID=@studentSubjectSelectionID)
 END
 RETURN @noOfSubjects
    END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWeightedExamTotalScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetWeightedExamTotalScore](@studentID int, @examID int,@weight decimal)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @total decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 ELSE
    set @total = (SELECT SUM(ISNULL(erd.Score,0)) FROM [Institution].[ExamResultHeader] erh
	LEFT OUTER JOIN [Institution].[ExamResultDetail] erd ON(erh.ExamResultID= erd.ExamResultID) 
	WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1)
    
	IF  NOT @total IS NULL
	SET @total= @total*(@weight/(SELECT OutOf FROM [Institution].[ExamHeader] WHERE ExamID=@examID))
	RETURN @total;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetWeightedExamSubjectScore]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetWeightedExamSubjectScore](@studentID int, @examID int, @subjectID int,@weight decimal)
    RETURNS decimal
    AS
    BEGIN
	DECLARE @score decimal;
 IF NOT EXISTS (SELECT * FROM [Institution].[ExamResultHeader] WHERE StudentID=@studentID)
 RETURN NULL
 IF @weight=0
 RETURN 0
 ELSE
	
    set @score = (SELECT erd.Score FROM [Institution].[ExamResultDetail] erd LEFT OUTER JOIN [Institution].[ExamResultHeader] erh
	ON(erh.ExamResultID= erd.ExamResultID) WHERE erh.StudentID=@studentID AND erh.ExamID=@examID AND erh.IsActive=1 AND erd.SubjectID=@subjectID)
    
	IF  NOT @score IS NULL
	SET @score= @score*(@weight/(SELECT OutOf FROM [Institution].[ExamHeader] WHERE ExamID=@examID))
	RETURN @score;
	END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUnreturnedCopies]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetUnreturnedCopies](@bookID int)
    RETURNS int
    AS
    BEGIN
 DECLARE @unreturned int=(SELECT COUNT(*) FROM ((SELECT bid.BookID FROM [Institution].[BookIssueDetail] bid INNER JOIN [Institution].[BookIssueHeader] bih ON(bid.BookIssueID=bih.BookIssueID) WHERE NOT EXISTS(SELECT brd.BookID FROM [Institution].[BookReturnDetail] brd INNER JOIN [Institution].[BookReturnHeader] brh ON(brd.BookReturnID=brh.BookReturnID) WHERE brh.DateReturned>bih.DateIssued AND brd.BookID=bid.BookID)) x LEFT OUTER JOIN [Institution].[Book] b ON (x.BookID=b.BookID)) WHERE x.BookID=@bookID);
 
 RETURN @unreturned
    END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_FeesStructureDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_FeesStructureDetail_UpdateID]
 ON [Institution].[FeesStructureDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.FeesStructureDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassSetupDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassSetupDetail_UpdateID]
 ON [Institution].[ClassSetupDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ClassSetupDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamResultDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamResultDetail_UpdateID]
 ON [Institution].[ExamResultDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ExamResultDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamDetail_UpdateID]
 ON [Institution].[ExamDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ExamDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ExamClassDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ExamClassDetail_UpdateID]
 ON [Institution].[ExamClassDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ExamClassDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Event_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Event_UpdateID]
 ON [Institution].[Event] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Event'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Dormitory_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Dormitory_UpdateID]
 ON [Institution].[Dormitory] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Dormitory'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Discipline_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Discipline_UpdateID]
 ON [Institution].[Discipline] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Discipline'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_CurrentClass_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_CurrentClass_UpdateID]
 ON [Institution].[CurrentClass] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.CurrentClass'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_CurrentClass_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_CurrentClass_SetActive_EndDate]
 ON [Institution].[CurrentClass] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[CurrentClass] SET IsActive = 0 WHERE CurrentClassID NOT IN 
  (SELECT CurrentClassID FROM inserted) AND StudentID IN (SELECT StudentID FROM inserted) 
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_ClassGroupDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_ClassGroupDetail_UpdateID]
 ON [Institution].[ClassGroupDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.ClassGroupDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_BookReturnHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_BookReturnHeader_UpdateID]
 ON [Institution].[BookReturnHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.BookReturnHeader'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_BookReturnDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_BookReturnDetail_UpdateID]
 ON [Institution].[BookReturnDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.BookReturnDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_BookIssueHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_BookIssueHeader_UpdateID]
 ON [Institution].[BookIssueHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.BookIssueHeader'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_BookIssueDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_BookIssueDetail_UpdateID]
 ON [Institution].[BookIssueDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.BookIssueDetail'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Book_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Book_UpdateID]
 ON [Institution].[Book] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Book'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[StudentSubjectSelectionHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[StudentSubjectSelectionHeader](
	[StudentSubjectSelectionID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[NoOfSubjects]  AS ([dbo].[GetSubjectsTakenByStudent]([StudentSubjectSelectionID])),
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StudentSubjectSelectionHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StudentSubjectSelectionHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StudentSubjectSelectionHeader] PRIMARY KEY CLUSTERED 
(
	[StudentSubjectSelectionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[StockTakingDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[StockTakingDetail](
	[StockTakingDetailID] [int] NOT NULL CONSTRAINT [DF_StockTakingDetail_StockTakingDetailID]  DEFAULT ([dbo].[Link_GetNewID]('Sales.StockTakingDetail')),
	[StockTakingID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[AvailableQuantity] [float] NOT NULL,
	[Expected]  AS ([dbo].[GetCurrentQuantity]([ItemID])),
	[VarianceQty]  AS ([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity]),
	[VariancePc]  AS ((([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity])*(100))/[dbo].[GetCurrentQuantity]([ItemID])),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_StockTakingDetail_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_StockTakingDetail_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_StockTakingDetail] PRIMARY KEY CLUSTERED 
(
	[StockTakingDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Institution].[Student]') AND type in (N'U'))
BEGIN
CREATE TABLE [Institution].[Student](
	[StudentID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[MiddleName] [varchar](50) NOT NULL,
	[NameOfStudent]  AS (((([FirstName]+' ')+[MiddleName])+' ')+[LastName]),
	[Gender] [varchar](50) NOT NULL,
	[ClassID]  AS ([dbo].[GetCurrentClass]([StudentID])),
	[DateOfBirth] [varchar](50) NOT NULL,
	[DateOfAdmission] [varchar](50) NOT NULL,
	[NameOfGuardian] [varchar](50) NOT NULL,
	[GuardianPhoneNo] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[PostalCode] [varchar](50) NOT NULL,
	[IsActive]  AS ([dbo].[GetStudentIsActive]([StudentID])),
	[PreviousBalance] [varchar](50) NOT NULL CONSTRAINT [DF_Student_PreviousBalance]  DEFAULT ('0'),
	[PreviousInstitution] [varchar](50) NULL,
	[KCPEScore] [int] NULL,
	[DormitoryID] [int] NULL,
	[BedNo] [varchar](50) NULL,
	[SPhoto] [varbinary](max) NULL,
	[IsBoarder] [bit] NULL CONSTRAINT [DF_Student_IsBoarder]  DEFAULT ((1)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Student_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Student_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[SaleHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[SaleHeader](
	[SaleID] [int] NOT NULL,
	[CustomerID] [varchar](50) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[PaymentID] [int] NULL CONSTRAINT [DF_SaleHeader_PaymentID]  DEFAULT ((0)),
	[IsCancelled] [varchar](50) NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmt]  AS ([dbo].[GetSaleTotal]([SaleID])),
	[IsDiscount] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_SalesOrderHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SalesOrderHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_SalesOrderHeader] PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sales].[ItemReceiptHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [Sales].[ItemReceiptHeader](
	[ItemReceiptID] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
	[RefNo] [varchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmt]  AS ([dbo].[GetPurchaseTotal]([ItemReceiptID])),
	[IsCancelled] [bit] NOT NULL CONSTRAINT [DF_ItemReceiptHeader_IsCancelled]  DEFAULT ((0)),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_PurchaseOrderHeader_ModifiedDate]  DEFAULT (sysdatetime()),
	[rowguid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PurchaseOrderHeader_rowguid]  DEFAULT (newid()),
 CONSTRAINT [PK_PurchaseOrderHeader] PRIMARY KEY CLUSTERED 
(
	[ItemReceiptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentSubjectSelectionHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionHeader_UpdateID]
 ON [Institution].[StudentSubjectSelectionHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.StudentSubjectSelectionHeader'')
    END
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_StudentSubjectSelectionHeader_SetActive_EndDate]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_StudentSubjectSelectionHeader_SetActive_EndDate]
 ON [Institution].[StudentSubjectSelectionHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     UPDATE [Institution].[StudentSubjectSelectionHeader] SET IsActive = 0 WHERE StudentSubjectSelectionID NOT IN 
  (SELECT StudentSubjectSelectionID FROM inserted) AND StudentID IN (SELECT StudentID FROM inserted) 
   END
END


'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Institution].[TR_Student_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Institution].[TR_Student_UpdateID]
 ON [Institution].[Student] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Institution.Student'')
    END
   END
END
'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_StockTakingDetail_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_StockTakingDetail_UpdateID]
 ON [Sales].[StockTakingDetail] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.StockTakingDetail'')
    END
   END
END
'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_SaleHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_SaleHeader_UpdateID]
 ON [Sales].[SaleHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.SaleHeader'')
    END
   END
END




'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[Sales].[TR_ItemReceiptHeader_UpdateID]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [Sales].[TR_ItemReceiptHeader_UpdateID]
 ON [Sales].[ItemReceiptHeader] AFTER INSERT 
 NOT FOR REPLICATION
AS
BEGIN
 IF @@ROWCOUNT = 0 RETURN
 SET NOCOUNT ON;

 IF exists (SELECT * FROM inserted)
   BEGIN
     DECLARE @inserted_count int = (SELECT COUNT(*) FROM inserted)
  WHILE(@inserted_count>0)
    BEGIN
   SET @inserted_count = @inserted_count-1
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER(''Sales.ItemReceiptHeader'')
    END
   END
END



'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTermOverAllPosition]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTermOverAllPosition](@studentID int,@form varchar(2),@startDateTime datetime,@endDateTime datetime)
    RETURNS varchar(10)
    AS
    BEGIN
    DECLARE @pos varchar(50);

    set @pos = (SELECT CONVERT(varchar(50),row_no)+''/''+CONVERT(varchar(50),studs) FROM(SELECT ROW_NUMBER() OVER(ORDER BY ISNULL(SUM(ISNULL(CONVERT(decimal,erd.Score),0)),0) DESC)
	 row_no, res.StudentID, (SELECT COUNT(*) FROM [Institution].[Student] st LEFT OUTER JOIN [Institution].[Class] cl 
	 ON(st.ClassID=cl.ClassID) WHERE cl.NameOfClass LIKE ''%''+@form+''%'') studs FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN 
	 (SELECT s.StudentID,erh.ExamResultID FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[ExamResultHeader] erh 
	 ON (s.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[ExamHeader] e 
	 ON(e.ExamID=erh.ExamID) LEFT OUTER JOIN [Institution].[ExamClassDetail] ecd 
	 ON(ecd.ExamID = e.ExamID) LEFT OUTER JOIN (SELECT NameOfClass,ClassID FROM [Institution].[Class] WHERE NameOfClass LIKE ''%''+@form+''%'') fc
	 ON(ecd.ClassID=fc.ClassID)
	 WHERE s.IsActive=1 AND erh.IsActive=1 AND fc.NameOfClass LIKE ''%''+@form+''%'' AND 
	 e.ExamDatetime>=@startDateTime AND e.ExamDatetime<=@endDateTime ) res 
	 ON (erd.ExamResultID=res.ExamResultID) GROUP BY res.StudentID)x WHERE x.StudentID=@studentID)

    RETURN @pos
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTermClassPosition]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetTermClassPosition](@studentID int,@classID int,@startDateTime varchar(50),@endDateTime varchar(50))
    RETURNS varchar(10)
    AS
    BEGIN
    DECLARE @pos varchar(50);

    set @pos = (select CONVERT(varchar(50),row_no)+''/''+CONVERT(varchar(50),no_of_students) FROM (SELECT ROW_NUMBER() 
	OVER(ORDER BY ISNULL(SUM(ISNULL(erd.Score,0)),0) DESC) row_no, 
	res.StudentID,(SELECT COUNT(*) FROM [Institution].[Student] WHERE ClassID =@classID AND IsActive=1)no_of_students 
	FROM [Institution].[ExamResultDetail] erd RIGHT OUTER JOIN (SELECT s.StudentID,ExamResultID 
	FROM [Institution].[Student] s LEFT OUTER JOIN [Institution].[ExamResultHeader] erh 
	ON(s.StudentID=erh.StudentID) LEFT OUTER JOIN [Institution].[Class] c 
	ON (s.ClassID=c.ClassID) LEFT OUTER JOIN [Institution].[ExamHeader] e 
	ON (erh.ExamID=e.ExamID) LEFT OUTER JOIN [Institution].[ExamClassDetail] ecd 
	ON (ecd.ExamID=e.ExamID) WHERE s.IsActive=1 AND erh.IsActive=1 AND  s.ClassID=@classID AND ecd.ClassID=@classID AND
	 e.ExamDatetime>=@startDateTime AND e.ExamDatetime<=@endDateTime
	 GROUP BY s.StudentID,erh.ExamResultID )res 
	 ON (res.ExamResultID=erd.ExamResultID) 
	 GROUP BY res.StudentID )x WHERE x.StudentID=@studentID)

    RETURN @pos
    END
    ;


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSubjectSelection]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetSubjectSelection](@subjectID int,@studentID int)
    RETURNS bit
    AS
    BEGIN
 DECLARE @isSelected bit;
 IF EXISTS (SELECT * FROM [Institution].[StudentSubjectSelectionDetail] sssd LEFT OUTER JOIN
 [Institution].[StudentSubjectSelectionHeader] sssh ON(sssd.StudentSubjectSelectionID=sssh.StudentSubjectSelectionID) WHERE 
 sssd.SubjectID=@subjectID AND sssh.StudentID=@studentID)
SET @isSelected=(SELECT 1);
ELSE
SET @isSelected=(SELECT 0);

 RETURN @isSelected
    END


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCurrentBalance]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[GetCurrentBalance](@studentID int)
    RETURNS decimal
    AS
    BEGIN
 DECLARE @currentBl decimal;


 DECLARE  @sal decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,TotalAmt),0)) FROM  [Sales].[SaleHeader] WHERE CustomerID =@studentID);
DECLARE  @pur decimal=(SELECT SUM(ISNULL(CONVERT(DECIMAL,AmountPaid),0)) FROM  [Institution].[FeesPayment] WHERE StudentID =@studentID);
DECLARE  @prev decimal=(SELECT CONVERT(DECIMAL,PreviousBalance) FROM  [Institution].[Student] WHERE StudentID=@studentID)
SET @currentBl=(select (ISNULL(@sal,0)+ISNULL(@prev,0))-ISNULL(@pur,0));

 RETURN @currentBl
    END


' 
END
GO
USE [UmanyiSMS]
GO
GRANT ALTER TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Accounts] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Accounts] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Deputy] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Deputy] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[None] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[None] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Teacher] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Teacher] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[User] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[User] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT DELETE ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT INSERT ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT SELECT ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT UPDATE ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT VIEW DEFINITION ON SCHEMA::[Users] TO [SystemAdmin] AS [dbo]
GO
GRANT CONTROL ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT DELETE ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT INSERT ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT SELECT ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT UPDATE ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT VIEW DEFINITION ON SCHEMA::[Sales] TO [SystemAdmin] AS [dbo]
GO
GRANT CONTROL ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT DELETE ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT INSERT ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT SELECT ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT UPDATE ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT VIEW DEFINITION ON SCHEMA::[Institution] TO [SystemAdmin] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[AddValuesIgnoringNull] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[AddValuesIgnoringNull] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[AddValuesIgnoringNull] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[AddValuesIgnoringNull] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetGrade] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetGrade] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetGrade] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetGrade] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [dbo].[sysIDs] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [dbo].[sysIDs] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [dbo].[sysIDs] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [dbo].[sysIDs] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [dbo].[sysIDs] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [dbo].[sysIDs] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [dbo].[sysIDs] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT SELECT ON [dbo].[sysIDs] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT UPDATE ON [dbo].[sysIDs] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON [dbo].[sysIDs] TO [SystemAdmin] AS [dbo]
GO
GRANT INSERT ON [dbo].[sysIDs] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [dbo].[sysIDs] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [dbo].[sysIDs] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Users].[UserRole] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Users].[UserRole] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Users].[UserRole] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Users].[UserRole] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[UserRole] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Users].[UserRole] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[UserRole] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Users].[UserRole] TO [User] AS [dbo]
GO
GRANT SELECT ON [Users].[User] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Users].[User] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Users].[User] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Users].[User] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[User] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Users].[User] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[User] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Users].[User] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableSettings] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableSettings] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableSettings] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableSettings] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableSettings] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableSettings] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableSettings] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableSettings] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableSettings] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableHeader] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamHeader] TO [User] AS [dbo]
GO
GRANT DELETE ON [Sales].[Item] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[Item] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[Item] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Item] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[Item] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[Item] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[Item] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Item] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[Class] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[Class] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Class] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Class] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Class] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Class] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Class] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Class] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Class] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Class] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassGroupHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassGroupHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassGroupHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassGroupHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassGroupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassGroupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassSetupHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassSetupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassSetupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupHeader] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesPayment] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesPayment] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesPayment] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesPayment] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesPayment] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesPayment] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesPayment] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesPayment] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultHeader] TO [User] AS [dbo]
GO
GRANT DELETE ON [Sales].[StockTakingHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[StockTakingHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[StockTakingHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[StockTakingHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[StockTakingHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[StockTakingHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[StockTakingHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[StockTakingHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTranscriptHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTranscriptHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTranscriptHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Subject] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[Subject] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Subject] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Subject] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Subject] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Subject] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Subject] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Subject] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Subject] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Subject] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Subject] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[SubjectSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[SubjectSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[SubjectSetupHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[SubjectSetupHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[SubjectSetupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[SubjectSetupHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupHeader] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[Staff] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[Staff] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Staff] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Staff] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Staff] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Staff] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Staff] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ResetUniqueIDs] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[ResetUniqueIDs] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[ResetUniqueIDs] TO [SystemAdmin] AS [dbo]
GO
GRANT DELETE ON [Institution].[QBSync] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[QBSync] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[QBSync] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[QBSync] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[QBSync] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[QBSync] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[QBSync] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[QBSync] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[QBSync] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[QBSync] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[QBSync] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[QBSync] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[PayoutHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[PayoutHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[PayoutHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[PayoutHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[PayoutHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[PayoutHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[PayoutHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[PayoutHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemIssueHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemIssueHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemIssueHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemIssueHeader] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetNewID] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetNewID] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetNewID] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[GetNewID] TO [SystemAdmin] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetNewID] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Sales].[Vat] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[Vat] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[Vat] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Vat] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[Vat] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[Vat] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[Vat] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Vat] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[UserDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Users].[UserDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Users].[UserDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Users].[UserDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[UserDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Users].[UserDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Users].[UserDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Users].[UserDetail] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[TimeTableDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[TimeTableDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[TimeTableDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[TimeTableDetail] TO [User] AS [dbo]
GO
GRANT DELETE ON [Sales].[SupplierPayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[SupplierPayment] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[SupplierPayment] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SupplierPayment] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[SupplierPayment] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[SupplierPayment] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[SupplierPayment] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SupplierPayment] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[SupplierDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[SupplierDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[SupplierDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SupplierDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[SupplierDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[SupplierDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[SupplierDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SupplierDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[Supplier] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[Supplier] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[Supplier] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Supplier] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[Supplier] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[Supplier] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[Supplier] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[Supplier] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[LeavingCertificate] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[LeavingCertificate] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[LeavingCertificate] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[LeavingCertificate] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[LeavingCertificate] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[LeavingCertificate] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[LeavingCertificate] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[LeavingCertificate] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[LeavingCertificate] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[LeavingCertificate] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[LeavingCertificate] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[LeavingCertificate] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[LeavingCertificate] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemReceiptDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemReceiptDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemReceiptDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemReceiptDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemReceiptDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemReceiptDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemReceiptDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemReceiptDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemIssueDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemIssueDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemIssueDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemIssueDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemCategory] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemCategory] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemCategory] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemCategory] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemCategory] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemCategory] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemCategory] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemCategory] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[PayoutDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[PayoutDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[PayoutDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[PayoutDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[PayoutDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[PayoutDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[PayoutDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[PayoutDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[SaleDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[SaleDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[SaleDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SaleDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[SaleDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Sales].[SaleDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[SaleDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[SaleDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SaleDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentClearance] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentClearance] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentClearance] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentClearance] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentClearance] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentClearance] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentClearance] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentClearance] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentClearance] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentClearance] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentClearance] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentClearance] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[SubjectSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[SubjectSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[SubjectSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[SubjectSetupDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[SubjectSetupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[SubjectSetupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[SubjectSetupDetail] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTransfer] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTransfer] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTransfer] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTransfer] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTransfer] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTransfer] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTransfer] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTransfer] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTransfer] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamResultDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamResultDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamResultDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamResultDetail] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[Dormitory] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Dormitory] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Dormitory] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Dormitory] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Dormitory] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Dormitory] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Dormitory] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Dormitory] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Dormitory] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Institution].[Discipline] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Discipline] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Discipline] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Discipline] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Discipline] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Discipline] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Discipline] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Discipline] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Discipline] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Discipline] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[CurrentClass] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[CurrentClass] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[CurrentClass] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassSetupDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassSetupDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassSetupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassSetupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassSetupDetail] TO [User] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassGroupDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassGroupDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassGroupDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ClassGroupDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ClassGroupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ClassGroupDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ClassGroupDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Institution].[Book] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[Book] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[Book] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Book] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[Book] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Book] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Book] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Book] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Book] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Book] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Book] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Book] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[Book] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[Book] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Book] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Book] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Gallery] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[Gallery] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Gallery] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Gallery] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Gallery] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Gallery] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Gallery] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Gallery] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Gallery] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[Gallery] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[Gallery] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Gallery] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Gallery] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Gallery] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[FeesStructureDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamDetail] TO [User] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamClassDetail] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamClassDetail] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamClassDetail] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamClassDetail] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamClassDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamClassDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamClassDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamClassDetail] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[ExamClassDetail] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[ExamClassDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[ExamClassDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[ExamClassDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[Event] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Event] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Event] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Event] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Event] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Event] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [User] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamTotalScore] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamTotalScore] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamTotalScore] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamTotalScore] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamSubjectScore] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamSubjectScore] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamSubjectScore] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetExamSubjectScore] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[GetCurrentQuantity] TO [SystemAdmin] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[GetCurrentClass] TO [SystemAdmin] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetStudentIsActive] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetStudentIsActive] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetStudentIsActive] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetStudentIsActive] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSaleTotal] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSaleTotal] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSaleTotal] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetPurchaseTotal] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetPurchaseTotal] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetPurchaseTotal] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectsTakenByStudent] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectsTakenByStudent] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectsTakenByStudent] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectsTakenByStudent] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamTotalScore] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamTotalScore] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamTotalScore] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamTotalScore] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamSubjectScore] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamSubjectScore] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamSubjectScore] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetWeightedExamSubjectScore] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetUnreturnedCopies] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetUnreturnedCopies] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetUnreturnedCopies] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetUnreturnedCopies] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionHeader] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionHeader] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionHeader] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionHeader] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Institution].[StudentSubjectSelectionHeader] TO [Teacher] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentSubjectSelectionHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentSubjectSelectionHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentSubjectSelectionHeader] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Sales].[StockTakingDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[StockTakingDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[StockTakingDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[StockTakingDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[StockTakingDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[StockTakingDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[StockTakingDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[StockTakingDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Student] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[Student] TO [Deputy] AS [dbo]
GO
GRANT INSERT ON [Institution].[Student] TO [Deputy] AS [dbo]
GO
GRANT SELECT ON [Institution].[Student] TO [Deputy] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Student] TO [Deputy] AS [dbo]
GO
GRANT DELETE ON [Institution].[Student] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Student] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[Student] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Student] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[Student] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Student] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Student] TO [Teacher] AS [dbo]
GO
GRANT DELETE ON [Sales].[SaleHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[SaleHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[SaleHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SaleHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[SaleHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[SaleHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[SaleHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[SaleHeader] TO [Principal] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemReceiptHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemReceiptHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemReceiptHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemReceiptHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Sales].[ItemReceiptHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Sales].[ItemReceiptHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Sales].[ItemReceiptHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Sales].[ItemReceiptHeader] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermOverAllPosition] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermOverAllPosition] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermOverAllPosition] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermOverAllPosition] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermClassPosition] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermClassPosition] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermClassPosition] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetTermClassPosition] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectSelection] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectSelection] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectSelection] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetSubjectSelection] TO [Teacher] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentBalance] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentBalance] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentBalance] TO [Principal] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentBalance] TO [Teacher] AS [dbo]
GO
USE [UmanyiSMS]
EXEC [dbo].[ResetUniqueIds]