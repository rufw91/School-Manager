
USE [Starehe]
GO
CREATE ROLE [User]
GO
CREATE ROLE [Teacher]
GO
CREATE ROLE [SystemAdmin]
GO
CREATE ROLE [Principal]
GO
CREATE ROLE [None]
GO
CREATE ROLE [Deputy]
GO
CREATE ROLE [Accounts]
GO
CREATE SCHEMA [Institution]
GO
CREATE SCHEMA [Sales]
GO
CREATE SCHEMA [Users]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetUniqueIDs]
AS
BEGIN
 SET NOCOUNT ON;
 if EXISTS(
select s.name +'.'+t.name, 0 from [Starehe].[sys].[tables] t inner JOIN Starehe.sys.schemas s on (t.schema_id=s.schema_id)
and NOT EXISTS (SELECT * from dbo.sysIDs WHERE table_name=(s.name +'.'+t.name)))

INSERT  dbo.sysIDs (table_name,last_id)
select s.name +'.'+t.name, 0 from [Starehe].[sys].[tables] t inner JOIN Starehe.sys.schemas s on (t.schema_id=s.schema_id)
and NOT EXISTS (SELECT * from dbo.sysIDs WHERE table_name=(s.name +'.'+t.name))
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetCurrentQuantity](@itemID bigint)
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
 set @totalSold = ISNULL((SELECT SUM(Quantity) FROM [Sales].[SaleDetail] where ItemID = @itemID),0)
 set @totalBought = ISNULL((SELECT SUM(Quantity) FROM [Sales].[ItemReceiptDetail] where ItemID = @itemID),0)

    set @currentQty=@startQuantity+@totalBought-@totalSold;
    END
 RETURN @currentQty
    END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sysIDs](
	[table_name] [varchar](50) NOT NULL,
	[last_id] [int] NOT NULL,
 CONSTRAINT [PK_sysIDs] PRIMARY KEY CLUSTERED 
(
	[table_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Book](
	[BookID] [int] NOT NULL,
	[ISBN] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Author] [varchar](50) NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[BookID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookIssueDetail](
	[BookIssueDetailID] [int] NOT NULL,
	[BookIssueID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookIssueDetail] PRIMARY KEY CLUSTERED 
(
	[BookIssueDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookIssueHeader](
	[BookIssueID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookIssue] PRIMARY KEY CLUSTERED 
(
	[BookIssueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookReturnDetail](
	[BookReturnDetailID] [int] NOT NULL,
	[BookReturnID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookReturnDetail] PRIMARY KEY CLUSTERED 
(
	[BookReturnDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookReturnHeader](
	[BookReturnID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookReturnHeader] PRIMARY KEY CLUSTERED 
(
	[BookReturnID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Class](
	[ClassID] [int] NOT NULL,
	[NameOfClass] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Class] PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ClassGroupDetail](
	[ClassGroupDetailID] [int] NOT NULL,
	[ClassGroupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ClassGroupHeader](
	[ClassGroupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ClassGroup] PRIMARY KEY CLUSTERED 
(
	[ClassGroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ClassSetupDetail](
	[ClassSetupDetailID] [int] NOT NULL,
	[ClassSetupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ClassSetupDetail] PRIMARY KEY CLUSTERED 
(
	[ClassSetupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ClassSetupHeader](
	[ClassSetupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SubjectSetup_1] PRIMARY KEY CLUSTERED 
(
	[ClassSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Dormitory](
	[DormitoryID] [int] NOT NULL,
	[NameOfDormitory] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Dormitory] PRIMARY KEY CLUSTERED 
(
	[DormitoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[EmployeePayment](
	[EmployeePaymentID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[AmountPaid] [decimal](18, 0) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[Notes] [varchar](max) NULL,
	[ModifiedDate] [nchar](10) NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EmployeePayment] PRIMARY KEY CLUSTERED 
(
	[EmployeePaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Event](
	[EventID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[StartDateTime] [varchar](50) NULL,
	[EndDateTime] [varchar](50) NULL,
	[Location] [varchar](50) NULL,
	[Subject] [varchar](50) NULL,
	[Message] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ExamDetail](
	[ExamDetailID] [int] NOT NULL,
	[ExamID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ExamDateTime] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ExamDetail] PRIMARY KEY CLUSTERED 
(
	[ExamDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[ExamHeader](
	[ExamID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[NameOfExam] [varchar](50) NOT NULL,
	[ExamDatetime] [datetime] NOT NULL,
	[Modifieddate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ExamHeader] PRIMARY KEY CLUSTERED 
(
	[ExamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[ExamResultDetail](
	[ExamResultDetail] [int] NOT NULL,
	[ExamResultID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[Score] [varchar](50) NOT NULL,
	[Remarks] [varchar](50) NULL,
	[Tutor] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ExamResultDetail] PRIMARY KEY CLUSTERED 
(
	[ExamResultDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[ExamResultHeader](
	[ExamResultID] [int] NOT NULL,
	[ExamID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ExamResultHeader] PRIMARY KEY CLUSTERED 
(
	[ExamResultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[FeesPayment](
	[FeesPaymentID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[AmountPaid] [varchar](50) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FeesPayment] PRIMARY KEY CLUSTERED 
(
	[FeesPaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[FeesStructureDetail](
	[FeesStructureDetailID] [int] NOT NULL,
	[FeesStructureID] [int] NULL,
	[Name] [varchar](50) NULL,
	[Amount] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FeesStructureDetail] PRIMARY KEY CLUSTERED 
(
	[FeesStructureDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[FeesStructureHeader](
	[FeesStructureID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FeesStructureHeader] PRIMARY KEY CLUSTERED 
(
	[FeesStructureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Gallery](
	[GalleryID] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Gallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[LeavingCertificate](
	[LeavingCertificateID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[DateOfIssue] [datetime] NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[DateOfAdmission] [datetime] NOT NULL,
	[DateOfLeaving] [datetime] NOT NULL,
	[Nationality] [varchar](50) NOT NULL,
	[ClassEntered] [varchar](50) NOT NULL,
	[ClassLeft] [varchar](50) NOT NULL,
	[Remarks] [varchar](1000) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_LeavingCertificate] PRIMARY KEY CLUSTERED 
(
	[LeavingCertificateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[PayoutDetail](
	[PayoutDetailID] [int] NOT NULL,
	[PayoutID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[Amount] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PayoutDetail] PRIMARY KEY CLUSTERED 
(
	[PayoutDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[PayoutHeader](
	[PayoutID] [int] NOT NULL,
	[Payee] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[TotalPaid] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PayoutHeader] PRIMARY KEY CLUSTERED 
(
	[PayoutID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Student](
	[StudentID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[MiddleName] [varchar](50) NOT NULL,
	[ClassID] [int] NOT NULL,
	[DateOfBirth] [varchar](50) NOT NULL,
	[DateOfAdmission] [varchar](50) NOT NULL,
	[NameOfGuardian] [varchar](50) NOT NULL,
	[GuardianPhoneNo] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[PostalCode] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[PreviousBalance] [varchar](50) NOT NULL,
	[PreviousInstitution] [varchar](50) NULL,
	[DormitoryID] [varchar](50) NULL,
	[BedNo] [varchar](50) NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[StudentClearance](
	[StudentClearanceID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[DateCleared] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StudentClearance] PRIMARY KEY CLUSTERED 
(
	[StudentClearanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[StudentTranscriptHeader](
	[StudentTranscriptID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
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
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StudentTranscriptHeader] PRIMARY KEY CLUSTERED 
(
	[StudentTranscriptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[StudentTransfer](
	[StudentTransferID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[DateTransferred] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[riwguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StudentTransfer] PRIMARY KEY CLUSTERED 
(
	[StudentTransferID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[Subject](
	[SubjectID] [int] NOT NULL,
	[NameOfSubject] [varchar](50) NOT NULL,
	[MaximumScore] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED 
(
	[SubjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[SubjectSetupDetail](
	[SubjectSetupDetailID] [int] NOT NULL,
	[SubjectSetupID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SubjectSetupDetail] PRIMARY KEY CLUSTERED 
(
	[SubjectSetupDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[SubjectSetupHeader](
	[SubjectSetupID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SubjectSetup] PRIMARY KEY CLUSTERED 
(
	[SubjectSetupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Institution].[TimeTableDetail](
	[TimeTableDetailID] [int] NOT NULL,
	[TimeTableID] [int] NOT NULL,
	[SubjectID] [int] NOT NULL,
	[Tutor] [varchar](50) NOT NULL,
	[Day] [varchar](50) NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TimeTableDetail] PRIMARY KEY CLUSTERED 
(
	[TimeTableDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[TimeTableHeader](
	[TimeTableID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TimeTableHeader] PRIMARY KEY CLUSTERED 
(
	[TimeTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[Item](
	[ItemID] [bigint] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[ItemCategoryID] [int] NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Cost] [decimal](18, 0) NOT NULL,
	[StartQuantity] [decimal](18, 0) NOT NULL,
	[VatID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[ItemCategory](
	[ItemCategoryID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[ItemCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales].[ItemReceiptDetail](
	[ItemReceiptDetailID] [int] NOT NULL,
	[ItemReceiptID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Quantity] [decimal](18, 0) NOT NULL,
	[UnitCost] [decimal](18, 0) NOT NULL,
	[LineTotal] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PurchaseOrderDetail] PRIMARY KEY CLUSTERED 
(
	[ItemReceiptDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[ItemReceiptHeader](
	[ItemReceiptID] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
	[RefNo] [varchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmt] [decimal](18, 0) NOT NULL,
	[IsCancelled] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PurchaseOrderHeader] PRIMARY KEY CLUSTERED 
(
	[ItemReceiptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[SaleDetail](
	[SalesOrderDetailID] [int] NOT NULL,
	[SaleID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SalesOrderDetail] PRIMARY KEY CLUSTERED 
(
	[SalesOrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[SaleHeader](
	[SaleID] [int] NOT NULL,
	[CustomerID] [varchar](50) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[PaymentID] [int] NULL,
	[IsCancelled] [varchar](50) NULL,
	[OrderDate] [datetime] NOT NULL,
	[TotalAmt] [decimal](18, 0) NOT NULL,
	[IsDiscount] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SalesOrderHeader] PRIMARY KEY CLUSTERED 
(
	[SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales].[StockTakingDetail](
	[StockTakingDetailID] [int] NOT NULL,
	[StockTakingID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[AvailableQuantity] [decimal](18, 0) NOT NULL,
	[Expected]  AS ([dbo].[GetCurrentQuantity]([ItemID])),
	[VarianceQty]  AS ([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity]),
	[VariancePc]  AS ((([dbo].[GetCurrentQuantity]([ItemID])-[AvailableQuantity])*(100))/[dbo].[GetCurrentQuantity]([ItemID])),
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StockTakingDetail] PRIMARY KEY CLUSTERED 
(
	[StockTakingDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales].[StockTakingHeader](
	[StockTakingID] [int] NOT NULL,
	[DateTaken] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_StockTakingHeader] PRIMARY KEY CLUSTERED 
(
	[StockTakingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[Supplier](
	[SupplierID] [int] NOT NULL,
	[NameOfSupplier] [varchar](50) NOT NULL,
	[PhoneNo] [varchar](50) NOT NULL,
	[AltPhoneNo] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Address] [varchar](50) NOT NULL,
	[PostalCode] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[PINNo] [varchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Sales].[SupplierDetail](
	[SupplierDetailID] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SupplierDetail] PRIMARY KEY CLUSTERED 
(
	[SupplierDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[SupplierPayment](
	[SupplierPaymentID] [int] NOT NULL,
	[SupplierID] [int] NOT NULL,
	[DatePaid] [datetime] NOT NULL,
	[AmountPaid] [decimal](18, 0) NOT NULL,
	[Notes] [varchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_SupplierPayment] PRIMARY KEY CLUSTERED 
(
	[SupplierPaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Sales].[Vat](
	[VatID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Rate] [decimal](18, 0) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Vat] PRIMARY KEY CLUSTERED 
(
	[VatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Users].[User](
	[UserID] [varchar](50) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Users].[UserDetail](
	[UserDetailID] [int] NOT NULL,
	[UserID] [varchar](50) NOT NULL,
	[UserRoleID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[UserDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Users].[UserRole](
	[UserRoleID] [int] NOT NULL,
	[Description] [varchar](20) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'dbo.sysIDs', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Book', 1)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.BookIssueDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.BookIssueHeader', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.BookReturnDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.BookReturnHeader', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Class', 32)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ClassSetupDetail', 32)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ClassSetupHeader', 35)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Dormitory', 1)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.EmployeePayment', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Event', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ExamDetail', 44)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ExamHeader', 7)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ExamResultDetail', 62)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.ExamResultHeader', 12)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.FeesPayment', 62)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.FeesStructureDetail', 6)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.FeesStructureHeader', 5)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Gallery', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.LeavingCertificate', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.PayoutDetail', 4)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.PayoutHeader', 3)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Staff', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Student', 861)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.StudentClearance', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.StudentTranscriptDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.StudentTranscriptHeader', 11)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.StudentTransfer', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.Subject', 163)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.SubjectSetupDetail', 163)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.SubjectSetupHeader', 32)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.TimeTableDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Institution.TimeTableHeader', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.Item', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.ItemCategory', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.ItemReceiptDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.ItemReceiptHeader', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.SaleDetail', 226)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.SaleHeader', 84)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.StockTakingDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.StockTakingHeader', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.Supplier', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.SupplierDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.SupplierPayment', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Sales.Vat', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Users.User', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Users.UserDetail', 0)
GO
INSERT [dbo].[sysIDs] ([table_name], [last_id]) VALUES (N'Users.UserRole', 0)
GO
INSERT [Institution].[Book] ([BookID], [ISBN], [Name], [Author], [SPhoto], [ModifiedDate], [rowguid]) VALUES (1, N'x', N'x', N'x', 0x89504E470D0A1A0A0000000D494844520000000A0000000A0802000000025058EA000000017352474200AECE1CE90000000467414D410000B18F0BFC6105000000097048597300000EC300000EC301C76FA8640000001A74455874536F667477617265005061696E742E4E45542076332E352E313147F342370000001249444154285363F88F178C4A6301FFFF0300FA1E2AE46C7C29CD0000000049454E44AE426082, CAST(0x0000A44D0021706C AS DateTime), N'22746150-aa05-48ef-931b-01514cea7d76')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (27, N'FORM 4 WEST', CAST(0x0000A44C0002B27D AS DateTime), N'29324483-9279-41bc-b97c-4f0d44d01824')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (28, N'FORM 4 EAST', CAST(0x0000A44C0002B287 AS DateTime), N'ff53fad0-61d6-4aa2-b904-0d8a2cc94795')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (29, N'FORM 3 WEST', CAST(0x0000A44C0002B292 AS DateTime), N'4458e075-23ef-4dc3-93ba-18d3cf032372')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (30, N'FORM 3 EAST', CAST(0x0000A44C0002B29D AS DateTime), N'b234c83a-8950-47f9-b07e-c987a7f5ce94')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (31, N'FORM 2 EAST', CAST(0x0000A44C0002B2AD AS DateTime), N'49f74747-5fe8-4a35-a6a6-ea60643d7240')
GO
INSERT [Institution].[Class] ([ClassID], [NameOfClass], [ModifiedDate], [rowguid]) VALUES (32, N'FORM 2 WEST', CAST(0x0000A44C0002B2BC AS DateTime), N'bb901bb7-db07-45c9-b568-8d5d9473d273')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (27, 35, 27, CAST(0x0000A44C0002B27C AS DateTime), N'187fbc5f-b876-4274-b174-004d749e8917')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (28, 35, 28, CAST(0x0000A44C0002B286 AS DateTime), N'3efb03df-e633-43f8-b1d7-c4a921e48386')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (29, 35, 29, CAST(0x0000A44C0002B291 AS DateTime), N'de61d2a5-6d95-4178-a230-0227c40f0194')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (30, 35, 30, CAST(0x0000A44C0002B29C AS DateTime), N'f6aeafbe-ee17-46bd-b651-f4f8abccbef1')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (31, 35, 31, CAST(0x0000A44C0002B2AC AS DateTime), N'16e3df4e-89d2-4939-ae1c-64e29909d595')
GO
INSERT [Institution].[ClassSetupDetail] ([ClassSetupDetailID], [ClassSetupID], [ClassID], [ModifiedDate], [rowguid]) VALUES (32, 35, 32, CAST(0x0000A44C0002B2BC AS DateTime), N'8d0c4087-27b7-484a-8c75-ccfa27a6d760')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (30, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0002B284 AS DateTime), CAST(0x0000A44C0002B278 AS DateTime), N'7e2543de-11a2-48c9-a479-78728a3c9e39')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (31, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0002B28F AS DateTime), CAST(0x0000A44C0002B284 AS DateTime), N'dabd7dfc-0114-4fca-9c16-6481f72b61bb')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (32, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0002B299 AS DateTime), CAST(0x0000A44C0002B28E AS DateTime), N'd3eb95a0-f3c6-410a-9a59-09cb71b3a0dd')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (33, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0002B2A5 AS DateTime), CAST(0x0000A44C0002B299 AS DateTime), N'00a98d2e-e3b0-427a-b956-97973ccb053f')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (34, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0002B2B7 AS DateTime), CAST(0x0000A44C0002B2A5 AS DateTime), N'8596bbcb-3ab5-48aa-b191-834b4e592f28')
GO
INSERT [Institution].[ClassSetupHeader] ([ClassSetupID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (35, 1, CAST(0x0000A44C00000000 AS DateTime), NULL, CAST(0x0000A44C0002B2B7 AS DateTime), N'21838cd4-7db9-4caf-87a6-a339a5c1abec')
GO
INSERT [Institution].[Dormitory] ([DormitoryID], [NameOfDormitory], [ModifiedDate], [rowguid]) VALUES (1, N'Kanzalu', CAST(0x0000A44D0107C0AF AS DateTime), N'6a9be3fe-3d08-4f09-ad6f-a707ca54ce20')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (23, 6, 87, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AA AS DateTime), N'02e5ed7e-d419-4a49-ac9b-89d786de8bb1')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (24, 6, 88, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AC AS DateTime), N'a188a112-67e8-4879-83cb-67f1f1749cc0')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (25, 6, 89, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AC AS DateTime), N'a542bb97-2e04-4428-bad2-005fc3fae1fd')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (26, 6, 90, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AD AS DateTime), N'5586c440-1e33-4912-94d8-941ea96a37ab')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (27, 6, 91, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AE AS DateTime), N'25611a9e-3bbb-4905-943b-efcdd3dc79e4')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (28, 6, 92, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AE AS DateTime), N'75a866e9-e028-4f90-a15d-bfeee2247203')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (29, 6, 93, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659AF AS DateTime), N'ee5dff4d-52a2-498b-8809-28d2296f5e0c')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (30, 6, 94, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659B0 AS DateTime), N'9b36df72-5847-457c-8aa8-9b00a98ccdba')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (31, 6, 95, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659B1 AS DateTime), N'de662325-fda3-4620-b84f-2116d4f1af78')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (32, 6, 96, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659B1 AS DateTime), N'bbc3ebf7-9916-4744-8233-2b70fe7a77ad')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (33, 6, 97, CAST(0x0000A44C00060AE0 AS DateTime), CAST(0x0000A44C000659B2 AS DateTime), N'9f2270c4-5eff-4049-a6bc-6b14e9c068d7')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (34, 7, 153, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392502 AS DateTime), N'fd5348bc-8680-43ba-a238-02ca393bffba')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (35, 7, 154, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392502 AS DateTime), N'c71e4a68-0535-4bba-aa72-fd0b81491cb3')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (36, 7, 155, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392503 AS DateTime), N'50e59b9d-26bb-41a7-9d11-c3ab7601ea07')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (37, 7, 156, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392504 AS DateTime), N'fab72a98-5a0a-4b35-8d35-dd4a81de3559')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (38, 7, 157, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392505 AS DateTime), N'4ce36a66-b5e1-42f6-8b20-03b36347a161')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (39, 7, 158, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392505 AS DateTime), N'3dcf1fb3-bf5d-4d65-97b1-e3fc9a37d070')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (40, 7, 159, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392506 AS DateTime), N'58bddff3-7b05-4bec-9478-e02107708911')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (41, 7, 160, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392506 AS DateTime), N'd7bb117a-ac6a-4575-9942-a79e5fe9c55e')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (42, 7, 161, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392507 AS DateTime), N'67bedb8c-d74a-427c-aed3-46269ba345aa')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (43, 7, 162, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392508 AS DateTime), N'6272354f-a5d7-4901-bf41-6b287564fa31')
GO
INSERT [Institution].[ExamDetail] ([ExamDetailID], [ExamID], [SubjectID], [ExamDateTime], [ModifiedDate], [rowguid]) VALUES (44, 7, 163, CAST(0x0000A44C0138D5F0 AS DateTime), CAST(0x0000A44C01392509 AS DateTime), N'c93c84bf-fee3-4aab-ac81-a3f41e631422')
GO
INSERT [Institution].[ExamHeader] ([ExamID], [ClassID], [NameOfExam], [ExamDatetime], [Modifieddate], [rowguid]) VALUES (6, 27, N'CAT 1', CAST(0x0000A44C00065130 AS DateTime), CAST(0x0000A44C000659A8 AS DateTime), N'2f5dd304-df3e-4d6e-bc63-90a2b64282d9')
GO
INSERT [Institution].[ExamHeader] ([ExamID], [ClassID], [NameOfExam], [ExamDatetime], [Modifieddate], [rowguid]) VALUES (7, 28, N'CAT 1', CAST(0x0000A44C01391C40 AS DateTime), CAST(0x0000A44C01392501 AS DateTime), N'0f677fe2-9e7f-49cd-80b8-33f5f2111637')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (9, 6, 87, N'75', N'GOOD', N'AD', CAST(0x0000A44C00083A6D AS DateTime), N'cf2935d5-189b-4492-bf4d-1996e4bce39a')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (10, 6, 88, N'67', N'JARIBIO ZURI', N'HU', CAST(0x0000A44C00083A6F AS DateTime), N'1a91269f-c491-4e82-bdff-bf97608da76e')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (11, 6, 89, N'85', N'GOOD', N'FT', CAST(0x0000A44C00083A70 AS DateTime), N'a89334c9-caaa-4de6-b7d2-2547ba0a545f')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (12, 6, 90, N'90', N'EXCELLENT', N'JZ', CAST(0x0000A44C00083A70 AS DateTime), N'3b9de9ee-1ff0-45e4-a297-5211aacb039b')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (13, 6, 91, N'89', N'GOOD', N'JB', CAST(0x0000A44C00083A71 AS DateTime), N'ce0112d8-c512-44be-9958-cb59253d9e53')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (14, 6, 92, N'47', N'TRY HARDER', N'TG', CAST(0x0000A44C00083A71 AS DateTime), N'88e454a0-3cc6-4066-9f69-b142c0d4b7a9')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (15, 6, 93, N'78', N'FAIR', N'GU', CAST(0x0000A44C00083A72 AS DateTime), N'8fb13743-b936-4ebb-afc4-9d8fa339674e')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (16, 6, 96, N'98', N'THANK YOU', N'FT', CAST(0x0000A44C00083A73 AS DateTime), N'4d8513ca-3e0b-4d39-b45f-ec78a4d59f90')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (17, 7, 87, N'90', N'X', N'X', CAST(0x0000A44C000A61AD AS DateTime), N'c5aba632-b86e-4b03-89a7-c80590cbc68d')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (18, 7, 88, N'90', N'X', N'X', CAST(0x0000A44C000A61AE AS DateTime), N'262d66d9-5415-4941-9845-82c9f06db9c7')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (19, 7, 89, N'90', N'X', N'X', CAST(0x0000A44C000A61AE AS DateTime), N'4141cccf-6bf0-4504-9778-55d6a3446da8')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (20, 7, 90, N'90', N'X', N'X', CAST(0x0000A44C000A61AF AS DateTime), N'bd3ad857-6d52-4abd-9322-a0200b873c4d')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (21, 7, 91, N'90', N'X', N'X', CAST(0x0000A44C000A61AF AS DateTime), N'2c98a40d-137b-44f6-86c7-18f11be953ca')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (22, 7, 94, N'90', N'X', N'X', CAST(0x0000A44C000A61B0 AS DateTime), N'136fbb3d-07b3-40fa-ba21-60de2121c4af')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (23, 7, 96, N'90', N'X', N'X', CAST(0x0000A44C000A61B1 AS DateTime), N'880e034a-0e6f-4062-8340-56011495f37b')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (24, 8, 87, N'95', N'X', N'X', CAST(0x0000A44C000C59C4 AS DateTime), N'30165dcf-482e-4219-bf3b-7c92b24935fb')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (25, 8, 88, N'95', N'X', N'XX', CAST(0x0000A44C000C59C5 AS DateTime), N'daf9b16a-d4e1-41c9-8ea7-18fc1346da90')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (26, 8, 89, N'95', N'X', N'X', CAST(0x0000A44C000C59C6 AS DateTime), N'268cdafd-aed4-4b50-adc7-dcdda1b8756e')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (27, 8, 95, N'95', N'X', N'X', CAST(0x0000A44C000C59C6 AS DateTime), N'0d84b713-d798-4aad-bbe5-d70dc49968de')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (28, 8, 91, N'95', N'X', N'X', CAST(0x0000A44C000C59C7 AS DateTime), N'2824a1c8-d870-4fe8-b089-8d913084886f')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (29, 8, 92, N'95', N'X', N'X', CAST(0x0000A44C000C59C9 AS DateTime), N'8a7beaaa-360f-4b5d-ab76-57b3b7ccbdde')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (30, 8, 97, N'95', N'X', N'X', CAST(0x0000A44C000C59CA AS DateTime), N'c336a7ce-241d-451a-ad9e-dc1999e746ab')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (31, 9, 87, N'96', N'X', N'X', CAST(0x0000A44C000F17E9 AS DateTime), N'69bdc0d3-b92b-41bf-97ab-a548cbb9a7c3')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (32, 9, 88, N'96', N'X', N'X', CAST(0x0000A44C000F17E9 AS DateTime), N'1dbdd021-e4ec-4028-9c68-0ae00af1e2a6')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (33, 9, 89, N'96', N'X', N'X', CAST(0x0000A44C000F17EA AS DateTime), N'f54ddaa9-b768-43dd-9263-b0f717d23579')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (34, 9, 90, N'96', N'X', N'X', CAST(0x0000A44C000F17EB AS DateTime), N'3e3d2fa5-7514-4f9d-950f-5b9d7cf791dd')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (35, 9, 91, N'96', N'X', N'X', CAST(0x0000A44C000F17EB AS DateTime), N'7dc20a70-0b86-4393-9e40-2a4005c90377')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (36, 9, 92, N'96', N'X', N'X', CAST(0x0000A44C000F17EC AS DateTime), N'ce702643-1a5c-4e62-838f-5408baa08c4e')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (37, 9, 93, N'96', N'X', N'X', CAST(0x0000A44C000F17ED AS DateTime), N'9b21fed9-2b86-48cd-b618-431dc8262512')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (38, 9, 96, N'96', N'X', N'X', CAST(0x0000A44C000F17ED AS DateTime), N'4cf35c85-e8d1-41e5-a320-545d1535ccc1')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (39, 10, 87, N'90', N'X', N'X', CAST(0x0000A44C000F6939 AS DateTime), N'2a956b50-59fa-42c2-a258-783024f8caf4')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (40, 10, 88, N'90', N'X', N'X', CAST(0x0000A44C000F693A AS DateTime), N'1f76ec2e-9afb-4ffb-9c81-e0ae7d4448dd')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (41, 10, 89, N'90', N'X', N'X', CAST(0x0000A44C000F693A AS DateTime), N'7f812e32-747b-4115-9862-7f112a74db65')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (42, 10, 90, N'90', N'X', N'X', CAST(0x0000A44C000F693B AS DateTime), N'2938e9b7-155f-40f1-9df6-cf5595cbbccf')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (43, 10, 91, N'90', N'X', N'X', CAST(0x0000A44C000F693C AS DateTime), N'11cf7a2c-add5-4f80-b797-f4b8e5f76113')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (44, 10, 94, N'90', N'X', N'X', CAST(0x0000A44C000F693C AS DateTime), N'd67db0d3-37ea-4813-96ed-39151c55d943')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (45, 10, 96, N'90', N'X', N'X', CAST(0x0000A44C000F693D AS DateTime), N'30a9d078-e926-4200-8117-521c0c13cf20')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (46, 10, 92, N'90', N'X', N'X', CAST(0x0000A44C000F693E AS DateTime), N'f189c32b-7a25-4bc1-8570-41f69ea57eca')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (47, 11, 87, N'95', N'X', N'X', CAST(0x0000A44C000F82D6 AS DateTime), N'206c31b6-06c0-468e-8329-42c6912e0dcf')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (48, 11, 88, N'95', N'X', N'XX', CAST(0x0000A44C000F82D7 AS DateTime), N'e62273ee-0345-447b-bcfd-83df2b4d11eb')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (49, 11, 89, N'95', N'X', N'X', CAST(0x0000A44C000F82D8 AS DateTime), N'f744c4b1-2cdf-42fe-a3b0-8b8f5c215e9a')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (50, 11, 95, N'95', N'X', N'X', CAST(0x0000A44C000F82D8 AS DateTime), N'5df47091-4d17-4579-80c9-5112d8a55ba1')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (51, 11, 91, N'95', N'X', N'X', CAST(0x0000A44C000F82D9 AS DateTime), N'd3d5e7cb-b3ec-4a45-9069-449328ea098d')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (52, 11, 92, N'95', N'X', N'X', CAST(0x0000A44C000F82D9 AS DateTime), N'513e8b81-bbfb-431a-9d6e-92ec50a55a12')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (53, 11, 97, N'95', N'X', N'X', CAST(0x0000A44C000F82DA AS DateTime), N'91a7a315-c7e3-4839-98e2-a72e1f4709df')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (54, 11, 96, N'95', N'X', N'X', CAST(0x0000A44C000F82DB AS DateTime), N'c04e7bb7-4c32-4883-a068-3301990b5183')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (55, 12, 153, N'100', N'X', N'X', CAST(0x0000A44C0139CA07 AS DateTime), N'89cb79ab-649c-4497-b260-af31e6e5f836')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (56, 12, 154, N'100', N'X', N'X', CAST(0x0000A44C0139CA08 AS DateTime), N'd8e5a603-43a7-434b-a166-782c83b12cdd')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (57, 12, 155, N'100', N'X', N'X', CAST(0x0000A44C0139CA09 AS DateTime), N'b875d700-b6cd-424e-bdfc-0bcf2becf7bc')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (58, 12, 156, N'100', N'X', N'X', CAST(0x0000A44C0139CA0A AS DateTime), N'cd4ae12f-459c-4cbf-bcd4-efb276da7b30')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (59, 12, 157, N'100', N'X', N'X', CAST(0x0000A44C0139CA0B AS DateTime), N'b72bb918-ec8b-4f8d-b321-9b3a07a4b6af')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (60, 12, 158, N'100', N'X', N'X', CAST(0x0000A44C0139CA0C AS DateTime), N'91f6e04c-de29-4dd2-a8fd-9a42148b8d96')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (61, 12, 159, N'100', N'X', N'X', CAST(0x0000A44C0139CA0C AS DateTime), N'17ac7caa-1fb0-4dda-b494-ab0a75db8c98')
GO
INSERT [Institution].[ExamResultDetail] ([ExamResultDetail], [ExamResultID], [SubjectID], [Score], [Remarks], [Tutor], [ModifiedDate], [rowguid]) VALUES (62, 12, 162, N'100', N'X', N'X', CAST(0x0000A44C0139CA0D AS DateTime), N'd91c8659-8b20-4f97-bb9b-d14b4aede7c3')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (6, 6, 434, 1, CAST(0x0000A44C00083A5C AS DateTime), N'd76ee1e6-d7d9-4695-b881-0e61eb6df6a6')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (7, 6, 497, 0, CAST(0x0000A44C000A61AC AS DateTime), N'af3f2f9b-8a19-4152-89c7-d35f6abdd12c')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (8, 6, 520, 0, CAST(0x0000A44C000C59C3 AS DateTime), N'5f04603d-41eb-420e-a088-ea2cf576f4ef')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (9, 6, 521, 1, CAST(0x0000A44C000F17E8 AS DateTime), N'8ef0fee3-1270-4716-a87f-eec74c927185')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (10, 6, 497, 1, CAST(0x0000A44C000F6938 AS DateTime), N'50f3e9ff-ec21-426f-9f8e-97a6e79ff072')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (11, 6, 520, 1, CAST(0x0000A44C000F82D5 AS DateTime), N'30f8ec34-2cc8-41cc-9b67-83e698f366c4')
GO
INSERT [Institution].[ExamResultHeader] ([ExamResultID], [ExamID], [StudentID], [IsActive], [ModifiedDate], [rowguid]) VALUES (12, 7, 495, 1, CAST(0x0000A44C0139CA06 AS DateTime), N'dc02fcc2-42be-4de3-b3a3-6f353c2f6a9f')
GO
INSERT [Institution].[PayoutDetail] ([PayoutDetailID], [PayoutID], [Description], [DatePaid], [Amount], [ModifiedDate], [rowguid]) VALUES (1, 1, N'TEST', CAST(0x0000A4AB00000000 AS DateTime), N'50000', CAST(0x0000A44A016848F3 AS DateTime), N'2359d637-cb9a-4bcb-82c1-939e32b79c31')
GO
INSERT [Institution].[PayoutDetail] ([PayoutDetailID], [PayoutID], [Description], [DatePaid], [Amount], [ModifiedDate], [rowguid]) VALUES (2, 1, N'WHAT', CAST(0x0000A4AB00000000 AS DateTime), N'700', CAST(0x0000A44A016848F8 AS DateTime), N'7aaa6723-374f-4641-825e-ef3ba5f56912')
GO
INSERT [Institution].[PayoutDetail] ([PayoutDetailID], [PayoutID], [Description], [DatePaid], [Amount], [ModifiedDate], [rowguid]) VALUES (3, 2, N'jghjgj', CAST(0x0000A41400000000 AS DateTime), N'6767', CAST(0x0000A44A01699DB1 AS DateTime), N'17dd3f7c-10e4-4f62-a556-fa1213c95e4f')
GO
INSERT [Institution].[PayoutDetail] ([PayoutDetailID], [PayoutID], [Description], [DatePaid], [Amount], [ModifiedDate], [rowguid]) VALUES (4, 3, N'dfgfdg', CAST(0x0000A41400000000 AS DateTime), N'3434', CAST(0x0000A44A016DA490 AS DateTime), N'46166445-2ad3-4ca6-80fa-e0310028210d')
GO
INSERT [Institution].[PayoutHeader] ([PayoutID], [Payee], [Address], [TotalPaid], [ModifiedDate], [rowguid]) VALUES (1, N'TEST', N'TEST', N'0', CAST(0x0000A44A016848EE AS DateTime), N'ddeb609f-9200-4116-b550-2a0407a7592b')
GO
INSERT [Institution].[PayoutHeader] ([PayoutID], [Payee], [Address], [TotalPaid], [ModifiedDate], [rowguid]) VALUES (2, N'TEST2', N'TEST2', N'0', CAST(0x0000A44A01699DB1 AS DateTime), N'e7ff7772-c884-4b40-82b0-3bf334602644')
GO
INSERT [Institution].[PayoutHeader] ([PayoutID], [Payee], [Address], [TotalPaid], [ModifiedDate], [rowguid]) VALUES (3, N'dfgf', N'dfgdfg', N'0', CAST(0x0000A44A016DA48B AS DateTime), N'c43b686f-6089-4902-a4c9-0a4671f66311')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (1, N'Eric', N'Wambua', N'Kioko', 27, N'01/01/1980 00:00:00', N'28/02/2015 15:32:32', N'Wambua', N'0724853350', N'test@example.com', N'26676', N'Nairobi', N'254', 1, N'10000', N'Kangundo high school', N'', N'1', 0x, CAST(0x0000A44D0101EDDE AS DateTime), N'aca755cb-9f47-4eaa-b955-a29c8ed0c690')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (2, N'Davies', N'Wambua', N'Mutua', 27, N'01/01/1986 00:00:00', N'28/02/2015 15:39:05', N'Wambua', N'0722868173', N'test@example.com', N'266800', N'Nairobi', N'00100', 1, N'2000', N'Mangu', N'', N'2', 0x, CAST(0x0000A44D0102A065 AS DateTime), N'a69c2116-fa85-4a06-8c5a-da90a2d2064a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (434, N'-', N'NTHENYA', N'CHRISTINE', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0723 794 158', N'test@example.com', N'X', N'', N'X', 1, N'3857', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2C5 AS DateTime), N'448f7bee-7813-4098-a96f-c91c6b1760b1')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (495, N'MUTINDA', N'MBINYA', N'CATHERINE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0710 535 0347', N'test@example.com', N'X', N'', N'X', 1, N'5442', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2CF AS DateTime), N'87b72302-534b-449d-938c-d5a506de6b6c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (496, N'MUTUA', N'MWIKALI', N'MERCY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 113 355', N'test@example.com', N'X', N'', N'X', 1, N'4475', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2D1 AS DateTime), N'b3bb722d-603a-469f-9fdf-b78f6783e802')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (497, N'-', N'MUMBUA', N'FAITH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0710 246 583', N'test@example.com', N'X', N'', N'X', 1, N'3315', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2D3 AS DateTime), N'09551528-09ef-4c75-b851-19730c64fb49')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (498, N'MBITHI', N'MUKII', N'JANET', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0702 788 511', N'test@example.com', N'X', N'', N'X', 1, N'4481', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2D4 AS DateTime), N'c7c5f601-efa2-45d8-9cab-cf5f92b4e11b')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (517, N'NTHENYA', N'WANZILA', N'VERONICA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0701 191 698', N'test@example.com', N'X', N'', N'X', 1, N'6664', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2D6 AS DateTime), N'2ad4c2f5-4b2a-44b9-a9ba-3ff715dcd6f9')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (518, N'-', N'NGINA', N'MARY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 321 042', N'test@example.com', N'X', N'', N'X', 1, N'9514', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2D8 AS DateTime), N'2d2ee461-588a-4c97-b5e7-61e048bbea89')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (520, N'-', N'WANJIKU', N'MARTHA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 938 946/0713 366 705', N'test@example.com', N'X', N'', N'X', 1, N'9384', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2DA AS DateTime), N'fe48b683-88f1-4d1e-a80e-600b75af32fb')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (521, N'-', N'MWENDE', N'ESTHER', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 522 245', N'test@example.com', N'X', N'', N'X', 1, N'9091', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2DB AS DateTime), N'3b3cf2a0-df55-4225-b290-91d4fd76d618')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (526, N'-', N'WAVINYA', N'ABIGAEL', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 317 210', N'test@example.com', N'X', N'', N'X', 1, N'9279', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2DD AS DateTime), N'1fce3925-d116-4477-b1da-e731319f4ceb')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (534, N'MUNGUTI', N'KATUNGE', N'CAROL', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 864 056', N'test@example.com', N'X', N'', N'X', 1, N'2003', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2DF AS DateTime), N'bad7c430-2b33-4e87-b1d1-94936dfef74f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (535, N'-', N'MUTINDI', N'VIRGINIA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 002 515', N'test@example.com', N'X', N'', N'X', 1, N'6163', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2E0 AS DateTime), N'801a3356-5063-4502-8e31-9167de5fa51d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (537, N'MUASYA', N'ALICE', N'MERCY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0716 429 745', N'test@example.com', N'X', N'', N'X', 1, N'3379', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2E2 AS DateTime), N'356a9099-f79f-47f6-a3d3-da435d28a752')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (538, N'-', N'MUMBUA', N'BENEDETTA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 408 145', N'test@example.com', N'X', N'', N'X', 1, N'9910', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2E4 AS DateTime), N'ce8c964a-2957-44c7-b1a4-5b9096ee1215')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (539, N'MUTHUI', N'MWENDE', N'MAUREEN', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 688 777', N'test@example.com', N'X', N'', N'X', 1, N'8877', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2E8 AS DateTime), N'e15ba168-7cfb-4723-b335-3dd41b606db8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (542, N'MUIRURI', N'WANJIRU', N'GILIAN', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0702 258 185', N'test@example.com', N'X', N'', N'X', 1, N'4932', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2EA AS DateTime), N'9764cd70-a57a-4a55-a5a4-17e65196ed13')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (546, N'KILONZI', N'MUMO', N'JACINTA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0714 594 528', N'test@example.com', N'X', N'', N'X', 1, N'5618', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2EC AS DateTime), N'462f5b53-2627-4303-8dfd-de89be07556e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (547, N'MUEMA', N'MUTANU', N'W', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 217 715', N'test@example.com', N'X', N'', N'X', 1, N'7425', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2EE AS DateTime), N'45beabb8-cce2-4707-8c28-ee336e0a4c17')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (550, N'MUTUKU', N'-', N'SERAH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0738 262 709', N'test@example.com', N'X', N'', N'X', 1, N'6546', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2EF AS DateTime), N'f73b4d7f-18d5-4423-9eb9-800c87614455')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (552, N'KYUMA', N'MUNYIVA', N'BETTY', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0710 678 392', N'test@example.com', N'X', N'', N'X', 1, N'6219', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2F2 AS DateTime), N'8ba835ba-710f-4061-9c0d-39d24e98bc44')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (556, N'-', N'NDINDA', N'THERESIA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 095 380', N'test@example.com', N'X', N'', N'X', 1, N'3976', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2F4 AS DateTime), N'2739c8f9-1520-48c3-a8aa-c1e647903351')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (557, N'MAKAI', N'MUTHEU', N'STELLA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 257 129', N'test@example.com', N'X', N'', N'X', 1, N'4548', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2F5 AS DateTime), N'5eedaf5c-1ae2-4489-ad48-f05e4c581c29')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (561, N'WAWIRA', N'-', N'YVONNE', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 221 743', N'test@example.com', N'X', N'', N'X', 1, N'6221', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2F7 AS DateTime), N'ab9765e1-61a5-4306-a9d7-5aa82a05c11f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (564, N'-', N'MUTHEU', N'SHARON', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'7990', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2F9 AS DateTime), N'6010e741-272f-4bf4-b94c-8f714d12c8d0')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (567, N'-', N'INJETE', N'GEORGINA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 215 264', N'test@example.com', N'X', N'', N'X', 1, N'6813', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2FB AS DateTime), N'32479086-a369-46aa-9bb1-eda0dc86a29d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (570, N'MUSEMBI', N'NTHAMBI', N'ANGELINE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 973 051', N'test@example.com', N'X', N'', N'X', 1, N'7479', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2FC AS DateTime), N'4fb90292-8032-4534-98d2-2237d4e2fe43')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (572, N'-', N'MWIKALI', N'JUDITH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'1857', N'', N'', N'X', 0x, CAST(0x0000A44C0002B2FE AS DateTime), N'6d40bd78-d4a1-4045-9665-eb526efe3ff0')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (573, N'ITUTE', N'-', N'HILARY', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'3169', N'', N'', N'X', 0x, CAST(0x0000A44C0002B300 AS DateTime), N'197b98e6-aee5-4089-9f7c-8126e4e65e06')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (575, N'MUTUA', N'WAYUA', N'FRADINA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 429 288', N'test@example.com', N'X', N'', N'X', 1, N'9411', N'', N'', N'X', 0x, CAST(0x0000A44C0002B302 AS DateTime), N'92063843-71d0-4212-af2d-635bc3cf7065')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (576, N'-', N'MBITHE', N'ASSUMPTA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 832 023', N'test@example.com', N'X', N'', N'X', 1, N'1956', N'', N'', N'X', 0x, CAST(0x0000A44C0002B305 AS DateTime), N'85df4215-2e9a-4f70-bc01-bf602c43df2a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (578, N'GACHUNGI', N'MUTHONI', N'MARY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 859 606', N'test@example.com', N'X', N'', N'X', 1, N'6996', N'', N'', N'X', 0x, CAST(0x0000A44C0002B307 AS DateTime), N'69445306-3555-4408-82cc-c172d2ffd2e5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (579, N'KIOKO', N'NDINDI', N'VICTORIA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 129 250', N'test@example.com', N'X', N'', N'X', 1, N'9248', N'', N'', N'X', 0x, CAST(0x0000A44C0002B30A AS DateTime), N'8e374bae-4851-44f9-83c2-92f4f614d4ca')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (582, N'-', N'MWONGELI', N'MARY', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0727 460 063', N'test@example.com', N'X', N'', N'X', 1, N'1963', N'', N'', N'X', 0x, CAST(0x0000A44C0002B30C AS DateTime), N'a476e547-e558-4ee7-82e8-88b9f155ace9')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (584, N'MUTUKU', N'MUENI', N'EVERLYNE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0713 798 417', N'test@example.com', N'X', N'', N'X', 1, N'4926', N'', N'', N'X', 0x, CAST(0x0000A44C0002B30D AS DateTime), N'2a42fceb-0891-4b94-b4d0-0b1ac98a8007')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (585, N'MUTHIANI', N'MUMBUA', N'ESTHER', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0715 165 893', N'test@example.com', N'X', N'', N'X', 1, N'3446', N'', N'', N'X', 0x, CAST(0x0000A44C0002B30F AS DateTime), N'aba28054-1915-4969-8bbe-628003c08070')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (587, N'MUTHIANI', N'NDUKU', N'LEAH', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 236 490', N'test@example.com', N'X', N'', N'X', 1, N'4874', N'', N'', N'X', 0x, CAST(0x0000A44C0002B311 AS DateTime), N'cdc42bd3-0b2c-4c01-8b48-bba9a470799e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (588, N'MWANIA', N'MBEKE', N'PATRICIA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0723 469 624', N'test@example.com', N'X', N'', N'X', 1, N'2125', N'', N'', N'X', 0x, CAST(0x0000A44C0002B312 AS DateTime), N'de3e65d5-bef1-4ac6-8cb5-e8a3b6d1735b')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (589, N'-', N'NDANU', N'CAROLINE', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0719 406 354', N'test@example.com', N'X', N'', N'X', 1, N'3815', N'', N'', N'X', 0x, CAST(0x0000A44C0002B314 AS DateTime), N'fdd44477-23cf-4de5-89a7-b81c9e306e91')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (590, N'KIMANTHI', N'MUTHEU', N'MONICA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0728 135 484', N'test@example.com', N'X', N'', N'X', 1, N'9983', N'', N'', N'X', 0x, CAST(0x0000A44C0002B316 AS DateTime), N'653869bc-1f62-4b44-be4a-1e6ec2881f38')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (591, N'PHILOMON', N'-', N'MARIAM', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'+255754 361 575', N'test@example.com', N'X', N'', N'X', 1, N'3439', N'', N'', N'X', 0x, CAST(0x0000A44C0002B318 AS DateTime), N'f014f51d-eb35-4e98-b5fe-ed88a78b0f07')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (592, N'-', N'NDUNGE', N'FAITH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0723 806 684', N'test@example.com', N'X', N'', N'X', 1, N'5675', N'', N'', N'X', 0x, CAST(0x0000A44C0002B319 AS DateTime), N'09688b75-2863-479f-9116-12a6fd545bfd')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (595, N'MULI', N'NDUNGWA', N'IMMACULATE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0729 644 370', N'test@example.com', N'X', N'', N'X', 1, N'1317', N'', N'', N'X', 0x, CAST(0x0000A44C0002B31B AS DateTime), N'dbe8610b-0a9c-4df7-be54-31ea47db96c5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (596, N'NGOTHO', N'WANJIKU', N'MARY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 665 980', N'test@example.com', N'X', N'', N'X', 1, N'1845', N'', N'', N'X', 0x, CAST(0x0000A44C0002B31D AS DateTime), N'2218c283-11ee-41e2-bfab-777f48c3a80e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (597, N'WAMBUA', N'MBITHE', N'EUNICE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0729 640 383', N'test@example.com', N'X', N'', N'X', 1, N'7369', N'', N'', N'X', 0x, CAST(0x0000A44C0002B31F AS DateTime), N'40043ac2-b01b-45b8-a660-308c96fdd68d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (598, N'MUTISO', N'NZILA', N'LOISE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 628 109', N'test@example.com', N'X', N'', N'X', 1, N'8481', N'', N'', N'X', 0x, CAST(0x0000A44C0002B321 AS DateTime), N'3e5ff452-24e3-4462-bfa1-a9a7431948f7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (601, N'-', N'WAIRIMU', N'SALOME', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 704 965', N'test@example.com', N'X', N'', N'X', 1, N'7050', N'', N'', N'X', 0x, CAST(0x0000A44C0002B329 AS DateTime), N'1f3f9795-b027-4e1f-b567-13f5725a8996')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (608, N'PATRICK', N'-', N'JACINTA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0733 401 666', N'test@example.com', N'X', N'', N'X', 1, N'8700', N'', N'', N'X', 0x, CAST(0x0000A44C0002B32B AS DateTime), N'f27af6e8-2a03-420a-acc1-5bbb8427c40b')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (616, N'-', N'WANGARI', N'SUSAN', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 262 987', N'test@example.com', N'X', N'', N'X', 1, N'7645', N'', N'', N'X', 0x, CAST(0x0000A44C0002B32C AS DateTime), N'6371f51e-c0d3-4b47-8859-603df98183c3')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (617, N'MONGARE', N'ESTHER', N'MAUREEN', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0714 938 414', N'test@example.com', N'X', N'', N'X', 1, N'1652', N'', N'', N'X', 0x, CAST(0x0000A44C0002B32F AS DateTime), N'a21afa7d-3678-4208-9f80-b4b9c98a02b1')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (624, N'-', N'MBINYA', N'DAMARIS', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'7134', N'', N'', N'X', 0x, CAST(0x0000A44C0002B330 AS DateTime), N'8c8859d0-6141-4155-bf22-5172b70e7e7a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (631, N'-', N'KIOKO', N'SELINA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 441 492', N'test@example.com', N'X', N'', N'X', 1, N'3957', N'', N'', N'X', 0x, CAST(0x0000A44C0002B332 AS DateTime), N'2f976d6e-2eb1-4033-98b0-bc2e62794e8e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (637, N'-', N'MUMBI', N'LILIAN', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0711 670 573', N'test@example.com', N'X', N'', N'X', 1, N'4133', N'', N'', N'X', 0x, CAST(0x0000A44C0002B334 AS DateTime), N'8aa60be4-d559-4353-9bc7-2d9e8a0851c3')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (639, N'KAVITI', N'MWENDE', N'FAITH', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0720 421 079', N'test@example.com', N'X', N'', N'X', 1, N'1528', N'', N'', N'X', 0x, CAST(0x0000A44C0002B335 AS DateTime), N'c3f5b44e-947c-4ed5-9248-af65a7abcc40')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (641, N'-', N'NDUKU', N'SCHOLASTICA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0736 903 895', N'test@example.com', N'X', N'', N'X', 1, N'3346', N'', N'', N'X', 0x, CAST(0x0000A44C0002B337 AS DateTime), N'2171c512-a608-4a15-8dc4-84e915e22bb0')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (645, N'KYALO', N'MUTHEU', N'FAITH', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 462 697', N'test@example.com', N'X', N'', N'X', 1, N'1722', N'', N'', N'X', 0x, CAST(0x0000A44C0002B339 AS DateTime), N'2895a28c-f2d5-42c4-b8e3-191f573395f4')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (646, N'-', N'KAVUTHA', N'LUCKY', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'4972', N'', N'', N'X', 0x, CAST(0x0000A44C0002B33A AS DateTime), N'f4afc3de-a62f-4ec2-92a9-5699efa871b3')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (649, N'KIOKO', N'MBULWA', N'MERCY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 476 112', N'test@example.com', N'X', N'', N'X', 1, N'9023', N'', N'', N'X', 0x, CAST(0x0000A44C0002B33C AS DateTime), N'6a671ed8-25ad-47e8-8fa2-0153d131ac32')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (651, N'WAMBUA', N'MWENDE', N'STELLA', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0736 858 866', N'test@example.com', N'X', N'', N'X', 1, N'2253', N'', N'', N'X', 0x, CAST(0x0000A44C0002B33E AS DateTime), N'9ea2923e-6de8-42f7-b33c-82780d1afd86')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (652, N'-', N'MWIKALI', N'AGNES', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0712 063 744', N'test@example.com', N'X', N'', N'X', 1, N'6840', N'', N'', N'X', 0x, CAST(0x0000A44C0002B341 AS DateTime), N'456985d5-d8cb-4a64-8a29-354a95cbcab5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (655, N'NGILA', N'KOKI', N'EUNICE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0729 599 067', N'test@example.com', N'X', N'', N'X', 1, N'6536', N'', N'', N'X', 0x, CAST(0x0000A44C0002B343 AS DateTime), N'f9c83830-07a8-4125-9ca0-55238c857e3e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (656, N'-', N'NDANU', N'ANITA', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'7647', N'', N'', N'X', 0x, CAST(0x0000A44C0002B344 AS DateTime), N'51786506-d756-4fa3-82d6-cc48b180aaf5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (657, N'MUE', N'MUMBUA', N'-', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0718 498 692', N'test@example.com', N'X', N'', N'X', 1, N'3160', N'', N'', N'X', 0x, CAST(0x0000A44C0002B346 AS DateTime), N'af1852b5-3910-4f69-a326-44e7e53db95a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (658, N'-', N'MBUKI', N'TINA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 233 821', N'test@example.com', N'X', N'', N'X', 1, N'8492', N'', N'', N'X', 0x, CAST(0x0000A44C0002B348 AS DateTime), N'9c17d348-b68b-489a-82ed-7953da055107')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (661, N'KIOKO', N'NDULULU', N'LUCY', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0723 536 257', N'test@example.com', N'X', N'', N'X', 1, N'2249', N'', N'', N'X', 0x, CAST(0x0000A44C0002B349 AS DateTime), N'851bdca9-ea8d-4e2b-9a4f-fe7d35467d09')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (662, N'MUTHAMA', N'MUENI', N'CECILIA', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0723 035 941', N'test@example.com', N'X', N'', N'X', 1, N'1703', N'', N'', N'X', 0x, CAST(0x0000A44C0002B34C AS DateTime), N'18a92627-c127-44d4-9405-bf03498eb83c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (663, N'MUTINDA', N'MUTIO', N'AGNES', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0711 894 274', N'test@example.com', N'X', N'', N'X', 1, N'2870', N'', N'', N'X', 0x, CAST(0x0000A44C0002B34D AS DateTime), N'0bb521aa-74eb-474c-ba20-acf1ff5dd64a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (665, N'MAWEU', N'NGII', N'FIONA', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 287 484', N'test@example.com', N'X', N'', N'X', 1, N'2534', N'', N'', N'X', 0x, CAST(0x0000A44C0002B34F AS DateTime), N'f0306d56-f9b9-4f16-91bc-60e0aaccd809')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (666, N'MUTISO', N'MUMO', N'GRACE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0724 570 498', N'test@example.com', N'X', N'', N'X', 1, N'9857', N'', N'', N'X', 0x, CAST(0x0000A44C0002B351 AS DateTime), N'03f01735-6505-47de-8c42-54e52b8dac8f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (667, N'MWANGANGI', N'MWENDE', N'LAUREEN', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0727 305 624', N'test@example.com', N'X', N'', N'X', 1, N'9129', N'', N'', N'X', 0x, CAST(0x0000A44C0002B352 AS DateTime), N'92cfabca-7533-4ce0-857b-30f380302aff')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (668, N'-', N'JEPKOSGEY', N'JUDY', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0725 346 875', N'test@example.com', N'X', N'', N'X', 1, N'1148', N'', N'', N'X', 0x, CAST(0x0000A44C0002B35A AS DateTime), N'3951553d-d635-4df4-b21a-6255f083e0f1')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (670, N'MWEU', N'MUTIO', N'EVERLYNE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0726 956 760', N'test@example.com', N'X', N'', N'X', 1, N'2495', N'', N'', N'X', 0x, CAST(0x0000A44C0002B35C AS DateTime), N'afd78496-aef1-4372-a7b6-225841cc4586')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (672, N'MAWEU', N'MUTHEU', N'JANE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0700 138 558', N'test@example.com', N'X', N'', N'X', 1, N'1695', N'', N'', N'X', 0x, CAST(0x0000A44C0002B360 AS DateTime), N'a278da58-6210-4966-8bcf-67642a386092')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (674, N'LUKO', N'MBITHE', N'ALICE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0727 243 189', N'test@example.com', N'X', N'', N'X', 1, N'9021', N'', N'', N'X', 0x, CAST(0x0000A44C0002B362 AS DateTime), N'7a0db789-0aae-4031-bf86-3730ac591944')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (675, N'MWAMBI', N'KANINI', N'CAROLYNE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0727 807 226', N'test@example.com', N'X', N'', N'X', 1, N'2094', N'', N'', N'X', 0x, CAST(0x0000A44C0002B363 AS DateTime), N'069a67ba-3a73-4ea5-ad79-457fca075e13')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (676, N'-', N'NGOSO', N'MARIATTA', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 577 233', N'test@example.com', N'X', N'', N'X', 1, N'8899', N'', N'', N'X', 0x, CAST(0x0000A44C0002B365 AS DateTime), N'1ee08468-dc3a-4784-ba0c-2d53335b7632')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (678, N'MUTISYA', N'SAULI', N'-', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0722 784 369', N'test@example.com', N'X', N'', N'X', 1, N'6462', N'', N'', N'X', 0x, CAST(0x0000A44C0002B367 AS DateTime), N'3d1602a5-3d92-447c-ae07-c8f94542a4ab')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (679, N'MUEMA', N'-', N'MAUREEN', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:49', N'X', N'0711 111 753', N'test@example.com', N'X', N'', N'X', 1, N'2611', N'', N'', N'X', 0x, CAST(0x0000A44C0002B369 AS DateTime), N'1aa4d9a9-b7d4-4d04-96f8-dff9219799d3')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (680, N'MATUNDA', N'NYANGE', N'ANN', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0700 624 701', N'test@example.com', N'X', N'', N'X', 1, N'1058', N'', N'', N'X', 0x, CAST(0x0000A44C0002B36B AS DateTime), N'1aaaf014-e64e-48ea-9aa8-71d6f54cd6c9')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (682, N'MUSYOKA', N'MWENDE', N'-', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 588 210', N'test@example.com', N'X', N'', N'X', 1, N'8807', N'', N'', N'X', 0x, CAST(0x0000A44C0002B36E AS DateTime), N'5be4982c-d8aa-4e1c-a494-55ce412291a2')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (687, N'-', N'MWIKALI', N'RUTH', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0707 378 582', N'test@example.com', N'X', N'', N'X', 1, N'2661', N'', N'', N'X', 0x, CAST(0x0000A44C0002B370 AS DateTime), N'27654932-7480-402e-8f42-adc25c349621')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (689, N'KITHEKA', N'MUVOYE', N'IMMACULATE', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0710 755 085', N'test@example.com', N'X', N'', N'X', 1, N'2430', N'', N'', N'X', 0x, CAST(0x0000A44C0002B372 AS DateTime), N'1a95ebd1-797b-476f-a4ce-5478d44280d5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (690, N'MUTINDI', N'WANZILA', N'-', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0713 315 719', N'test@example.com', N'X', N'', N'X', 1, N'5344', N'', N'', N'X', 0x, CAST(0x0000A44C0002B374 AS DateTime), N'd08216b8-bdc3-43e2-a4fb-61063874998e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (691, N'MORRIS', N'-', N'MARIETTA', 29, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0724 705 555', N'test@example.com', N'X', N'', N'X', 1, N'5194', N'', N'', N'X', 0x, CAST(0x0000A44C0002B376 AS DateTime), N'94694503-15ce-4e94-b104-c151115a571e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (693, N'MWIKALI', N'NTHENYA', N'CICILIA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0718 769 838', N'test@example.com', N'X', N'', N'X', 1, N'8350', N'', N'', N'X', 0x, CAST(0x0000A44C0002B378 AS DateTime), N'88d23712-904c-42ab-858e-075ed9b68941')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (694, N'MUTUA', N'MUTHEU', N'DIANA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 289 860', N'test@example.com', N'X', N'', N'X', 1, N'6312', N'', N'', N'X', 0x, CAST(0x0000A44C0002B379 AS DateTime), N'f1df9ed2-e1b8-434b-a09c-bb22fd76f899')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (696, N'MUOKA', N'MUMBI', N'CAROLINE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'5964', N'', N'', N'X', 0x, CAST(0x0000A44C0002B37C AS DateTime), N'22180971-ca1a-4ca4-be4f-2b15a2c6f8fa')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (697, N'MUIA', N'MUTHOKI', N'MARY', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0710 177 044', N'test@example.com', N'X', N'', N'X', 1, N'9688', N'', N'', N'X', 0x, CAST(0x0000A44C0002B37F AS DateTime), N'ca642e5c-c2ff-4e82-9fbe-9ed6d95a87be')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (699, N'MOMAMNYI', N'NDANU', N'CHARITY', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'2034', N'', N'', N'X', 0x, CAST(0x0000A44C0002B381 AS DateTime), N'c6798988-6298-4670-9efa-cfb1d269b98e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (701, N'DAVID', N'MUTINDI', N'RAEL', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'1266', N'', N'', N'X', 0x, CAST(0x0000A44C0002B383 AS DateTime), N'f5960668-1f42-490f-b201-0455f99bda74')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (703, N'KIKUVI', N'MWIKALI', N'YVONNE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0703 384 690', N'test@example.com', N'X', N'', N'X', 1, N'6693', N'', N'', N'X', 0x, CAST(0x0000A44C0002B385 AS DateTime), N'8efb2320-9779-4481-a7a8-b3cc066bd492')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (704, N'MUTIE', N'MWONGELI', N'ESTHER', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0717 577 042', N'test@example.com', N'X', N'', N'X', 1, N'3023', N'', N'', N'X', 0x, CAST(0x0000A44C0002B387 AS DateTime), N'498ef6dc-aadb-42ce-a4c7-7b7d107b26a6')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (705, N'KIOKO', N'KATITI', N'FLORENCE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0736 111 415', N'test@example.com', N'X', N'', N'X', 1, N'9471', N'', N'', N'X', 0x, CAST(0x0000A44C0002B389 AS DateTime), N'd18ff611-9de5-41a5-a254-a4d87d278728')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (706, N'MUSYOKI', N'KYEE', N'M', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 903 724', N'test@example.com', N'X', N'', N'X', 1, N'1186', N'', N'', N'X', 0x, CAST(0x0000A44C0002B38A AS DateTime), N'55d632d7-3b85-43f9-b7e8-3f1b18da23f1')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (712, N'DERE', N'NCHUSYUYA', N'REGINA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 325 674', N'test@example.com', N'X', N'', N'X', 1, N'2836', N'', N'', N'X', 0x, CAST(0x0000A44C0002B38C AS DateTime), N'3729be5a-5a14-4d75-bc04-3660f1ed09df')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (714, N'NYAEGA', N'KEMUNTO', N'CAROLINE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 485 298', N'test@example.com', N'X', N'', N'X', 1, N'4464', N'', N'', N'X', 0x, CAST(0x0000A44C0002B38E AS DateTime), N'34807497-35a9-461e-8deb-5370d2ab8409')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (715, N'KIETI', N'MUMO', N'JENNIFER', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0718 578 050', N'test@example.com', N'X', N'', N'X', 1, N'7685', N'', N'', N'X', 0x, CAST(0x0000A44C0002B390 AS DateTime), N'38f2127e-fb08-4a83-9dbd-5c3f41adcfef')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (716, N'-', N'MUASYA', N'DIANA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 675 606', N'test@example.com', N'X', N'', N'X', 1, N'9417', N'', N'', N'X', 0x, CAST(0x0000A44C0002B392 AS DateTime), N'67f69493-8081-4d70-a914-67f0daba1d3f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (718, N'MUTINDA', N'NZIKU', N'FELISTUS', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0729 827 413', N'test@example.com', N'X', N'', N'X', 1, N'8430', N'', N'', N'X', 0x, CAST(0x0000A44C0002B393 AS DateTime), N'166eb35d-e4e0-4f51-91f0-6a2bddb01ace')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (719, N'-', N'KILOKO', N'ANNE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0728 546 012', N'test@example.com', N'X', N'', N'X', 1, N'2676', N'', N'', N'X', 0x, CAST(0x0000A44C0002B395 AS DateTime), N'0ace9ee6-f5c7-4e05-b2df-6e02e021026c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (720, N'EMESE', N'NDUNGE', N'MELODY', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0704 505 520', N'test@example.com', N'X', N'', N'X', 1, N'3989', N'', N'', N'X', 0x, CAST(0x0000A44C0002B397 AS DateTime), N'06c9bd77-fab9-41f5-8bc0-45ab66c6c19e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (724, N'-', N'AKINYI', N'NELLY', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0714 992 204', N'test@example.com', N'X', N'', N'X', 1, N'9204', N'', N'', N'X', 0x, CAST(0x0000A44C0002B39B AS DateTime), N'304c6f6b-a874-4afa-a708-f13952486a68')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (726, N'ISAIAH', N'NDUNGE', N'ANITA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 236 576', N'test@example.com', N'X', N'', N'X', 1, N'6434', N'', N'', N'X', 0x, CAST(0x0000A44C0002B39D AS DateTime), N'f3312a3b-0e39-428a-93d4-1bc2203828fe')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (727, N'MWANGI', N'KANINI', N'FAITH', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 770 615', N'test@example.com', N'X', N'', N'X', 1, N'6090', N'', N'', N'X', 0x, CAST(0x0000A44C0002B39F AS DateTime), N'2883eaac-b758-4124-9107-82a96d502595')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (728, N'MWANZIA', N'MUMBE', N'ELIZABETH', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 474 705', N'test@example.com', N'X', N'', N'X', 1, N'5278', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A1 AS DateTime), N'bb046cfe-1efa-4ff6-b573-bb291acc3bc5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (729, N'NDUNDA', N'MUENI', N'CAROLYNE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0712 374 101', N'test@example.com', N'X', N'', N'X', 1, N'2720', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A2 AS DateTime), N'a70544d0-8828-4a1e-82bf-cab2bdb51363')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (730, N'MWENDA', N'WAENI', N'IRENE', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0705 537 066', N'test@example.com', N'X', N'', N'X', 1, N'1537', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A4 AS DateTime), N'0bf80c92-077a-4eff-ae85-cff9e93cfb8c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (731, N'-', N'MWIKALI', N'CAROLINE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 299 278', N'test@example.com', N'X', N'', N'X', 1, N'8015', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A6 AS DateTime), N'61f52354-f08f-461f-8400-efe36e53a249')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (732, N'-', N'MUTHEU', N'ELIZABETH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0733 903 123', N'test@example.com', N'X', N'', N'X', 1, N'9693', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A8 AS DateTime), N'58c4594e-422a-489a-ad7d-4b68c44fa723')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (733, N'IBRAHIM', N'-', N'FATMA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 879 847', N'test@example.com', N'X', N'', N'X', 1, N'2077', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3A9 AS DateTime), N'17c266fe-49fd-47cd-83e4-5ad392fbe20f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (734, N'WAIYAKI', N'-', N'ROSE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 343 212', N'test@example.com', N'X', N'', N'X', 1, N'7986', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3AB AS DateTime), N'9902ce70-07cf-4975-86ec-018425563542')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (735, N'LATIA', N'WAYUA', N'FELISTAS', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0713 207 453', N'test@example.com', N'X', N'', N'X', 1, N'4968', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3AD AS DateTime), N'9f60af38-f0a3-47bb-b691-3d5e224a66c8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (736, N'-', N'WAMBUA', N'LILIAN', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 524 129', N'test@example.com', N'X', N'', N'X', 1, N'2890', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3AF AS DateTime), N'284ac48f-5b59-49d7-9ba0-461d8d556e43')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (737, N'MUENDO', N'MUNYIVA', N'RUTH', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0713 207 453', N'test@example.com', N'X', N'', N'X', 1, N'4767', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3B1 AS DateTime), N'acbf6cd1-868a-47cf-8bca-46d89ae29785')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (738, N'MULANDI', N'MWONGELI', N'AGNES', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 590 198', N'test@example.com', N'X', N'', N'X', 1, N'5401', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3B2 AS DateTime), N'f86dd2e0-22ab-4f13-b585-0d4f67edd71e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (739, N'-', N'SYOKAU', N'ANGELA', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 968 447', N'test@example.com', N'X', N'', N'X', 1, N'1228', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3B4 AS DateTime), N'd52933a0-a888-4483-9aca-c9873d8358d1')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (740, N'MUTINDA ', N'MUTHEU', N'IRENE ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 033 589', N'test@example.com', N'X', N'', N'X', 1, N'4629', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3B6 AS DateTime), N'81fc9567-a9ec-44c9-9032-0373eae754b5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (742, N'MWENGU', N'MUKAI', N'DORCAS ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0716 680 906', N'test@example.com', N'X', N'', N'X', 1, N'6897', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3B9 AS DateTime), N'6acdedf1-6077-4414-856d-59733024b443')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (743, N'-', N'NJOKI', N'FAITH', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 771 711', N'test@example.com', N'X', N'', N'X', 1, N'9664', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3BB AS DateTime), N'512cbeca-029d-4fd8-9756-a649bd73d8b6')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (744, N'MULUSA ', N'MUENI', N'CATHERINE ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0703 906 610', N'test@example.com', N'X', N'', N'X', 1, N'7880', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3BD AS DateTime), N'a06c97d7-9b3b-4c32-9583-b6aaad7b1eaf')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (745, N'MWANZIA ', N'MUENDI', N'WINFRED ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0723 691 515', N'test@example.com', N'X', N'', N'X', 1, N'6648', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3BF AS DateTime), N'04d58d08-97a1-4d1c-ab15-4e330b6c5aab')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (747, N'SYOKAU', N'MWENDE ', N'M', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0725 352 515', N'test@example.com', N'X', N'', N'X', 1, N'1766', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C0 AS DateTime), N'c16e4e5e-d238-4182-8492-e7ceb91fa531')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (749, N'KAUNDA ', N'MALINDA', N'ESTHER ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 668 870', N'test@example.com', N'X', N'', N'X', 1, N'9928', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C2 AS DateTime), N'2a253274-8da4-4015-a135-5727c0d059b6')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (751, N'NGAYAI ', N'NTHAMBA', N'IRENE ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0705 009 198', N'test@example.com', N'X', N'', N'X', 1, N'5160', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C4 AS DateTime), N'2e0e8947-eb1c-432e-9da5-80310551cf5e')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (752, N'DAVID ', N'MWIKALI', N'ROSALIA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0707 262 218', N'test@example.com', N'X', N'', N'X', 1, N'3695', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C6 AS DateTime), N'e8f4fade-5fd2-4efe-b47d-db4e3b15adde')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (753, N'KIOKO', N'MWENDE', N'ESTHER ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 948 556', N'test@example.com', N'X', N'', N'X', 1, N'1862', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C7 AS DateTime), N'c73b8188-4a05-46fe-9e68-d37df24dceff')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (754, N'MOHAMMED ', N'IBRAHIM', N'AMINAH', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 879 847', N'test@example.com', N'X', N'', N'X', 1, N'1233', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3C9 AS DateTime), N'b160b6d3-d6ce-4743-b30f-4911e068a587')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (759, N'MUINDI ', N'NDUKU', N'FELISTER', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0718 375 178', N'test@example.com', N'X', N'', N'X', 1, N'5395', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3CB AS DateTime), N'b3dffa2e-ea26-46a4-b10c-5cd0e4499cd7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (760, N'MUINDI', N'MUMO', N'FAITH', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0716 719 690', N'test@example.com', N'X', N'', N'X', 1, N'2846', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3CD AS DateTime), N'f0afbb51-7e01-4df6-9e02-f6e5272fc952')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (761, N'WINFRED', N'PETER', N'MUTHEU ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0718 26049', N'test@example.com', N'X', N'', N'X', 1, N'4186', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3CF AS DateTime), N'ae581d5c-d0e1-413d-934a-b3d60d3e96be')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (763, N'MUSAU ', N'MUTHEU', N'LILIAN', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'020 236 0883', N'test@example.com', N'X', N'', N'X', 1, N'7189', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3D0 AS DateTime), N'08e1a86e-9481-468f-b9dc-08d91170ddf6')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (764, N'FAITH', N'MWANZIA', N'MUTHEU ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0725 978 554', N'test@example.com', N'X', N'', N'X', 1, N'4726', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3D2 AS DateTime), N'5b2fe4e8-37db-45ae-b246-96580ebc02c2')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (767, N'SUMAILI', N'ELIZABETH', N'KATETHYA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0723 447 754', N'test@example.com', N'X', N'', N'X', 1, N'7029', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3D4 AS DateTime), N'fb6df764-e23c-4d00-9d64-f57646c1cac7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (768, N'KATUMBI', N'WAMBUI', N'VALENTINE ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 796 597', N'test@example.com', N'X', N'', N'X', 1, N'8311', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3D9 AS DateTime), N'd12ff89b-5f26-44b7-86d2-1379c7a1d7fa')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (769, N'KILUU', N'SYOKAU', N'SILVIA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 587 962', N'test@example.com', N'X', N'', N'X', 1, N'4600', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3DA AS DateTime), N'7b98c4f2-e8c0-4f3f-bcfe-a6aa9498ac9f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (770, N'KIOKO', N'SYOKAU', N'SILVIA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 587 962', N'test@example.com', N'X', N'', N'X', 1, N'8551', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3DC AS DateTime), N'8d7e5b42-eee4-4801-8a0c-6b21157f26a4')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (771, N'JOHN ', N'MWELU', N'FAITH ', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'9687', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3DE AS DateTime), N'9ed5e26b-6e80-4c65-a23c-97757ac763a5')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (773, N'-', N'NDUKU', N'IRENE', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 962 874', N'test@example.com', N'X', N'', N'X', 1, N'3502', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E0 AS DateTime), N'15aa74aa-3d2e-4e36-9eb7-4ceb1207ff4b')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (774, N'KILONZO', N'KHADIJA', N'ESTHER', 28, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0728 809 964', N'test@example.com', N'X', N'', N'X', 1, N'4068', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E2 AS DateTime), N'80c1ffb7-b027-4d16-a00c-0ce78973a96f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (775, N'TRACY', N'MUKONYO', N'MUNYAO', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0724 701 310', N'test@example.com', N'X', N'', N'X', 1, N'4138', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E3 AS DateTime), N'bb84a5f3-2976-47fa-96f5-91b6b397f8ee')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (776, N'MBITHI', N'MWENDE', N'FAITH', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0700 338 197', N'test@example.com', N'X', N'', N'X', 1, N'6768', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E5 AS DateTime), N'e01fc68e-423c-44a5-813f-31ebb8df917c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (777, N'OLIVER', N'AKINYI', N'VIOLET', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0724 112 630', N'test@example.com', N'X', N'', N'X', 1, N'8490', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E7 AS DateTime), N'4f3dba27-8bb7-4bde-9c78-e49d7e651ee7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (781, N'MUTUA ', N'MUMBUA', N'MARY', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0723 001 445', N'test@example.com', N'X', N'', N'X', 1, N'4570', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3E9 AS DateTime), N'e5f74c26-a251-4d49-a718-7a9af55e798c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (783, N'NGATI', N'NTHENYA', N'ROSE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 219 264', N'test@example.com', N'X', N'', N'X', 1, N'4481', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3EB AS DateTime), N'033eb71b-b2a6-42dd-870e-61213e5f47a7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (784, N'MUSAKWA ', N'CYNTHIA', N'-', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 272 668', N'test@example.com', N'X', N'', N'X', 1, N'1319', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3EC AS DateTime), N'55e8fefa-be82-48b3-a854-11aacd660fc4')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (785, N'KUNGU', N'MBATHA', N'ANNA', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 314 857', N'test@example.com', N'X', N'', N'X', 1, N'5622', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3EE AS DateTime), N'fe5dc7ad-ad90-420c-a671-cd6fab3c4429')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (786, N'KYALO', N'NDUKU', N'MERCYLENE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 111 202', N'test@example.com', N'X', N'', N'X', 1, N'6737', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3F0 AS DateTime), N'e09e8216-cccd-4bf0-94df-ed8957c29f44')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (787, N'KILUNGYA ', N'JOY', N'NGWEMBE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 181 169', N'test@example.com', N'X', N'', N'X', 1, N'4633', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3F1 AS DateTime), N'62247125-d6ce-49e8-9f08-45944d6e66cf')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (788, N'MUEMA', N'MINOO', N'CAROLINE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0715 754 540', N'test@example.com', N'X', N'', N'X', 1, N'1128', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3F4 AS DateTime), N'd90610e1-5c52-43af-b92b-8b1142ec8bba')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (790, N'MULI', N'NDANU', N'PHOEBE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0713 888 176', N'test@example.com', N'X', N'', N'X', 1, N'1638', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3F7 AS DateTime), N'c8a56dac-fff4-4e70-b359-333a23040df4')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (791, N'MUITHYA', N'MUTHEU', N'CATHERINE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0710 539 495', N'test@example.com', N'X', N'', N'X', 1, N'8838', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3F8 AS DateTime), N'8330f256-6893-4c73-b952-8973e2d23558')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (792, N'KARIUKI', N'WAMBUI', N'MARGARET', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0723 386 514', N'test@example.com', N'X', N'', N'X', 1, N'6555', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3FA AS DateTime), N'7b880bd1-dff5-443e-9ece-93cd5aebdebe')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (793, N'MBAI', N'KINANDA', N'JANET', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 660 567', N'test@example.com', N'X', N'', N'X', 1, N'9855', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3FC AS DateTime), N'5c3fa625-b664-4a0f-845c-12feefbc700c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (794, N'KYALO ', N'MWIKALI', N'MARTHA', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 444 071', N'test@example.com', N'X', N'', N'X', 1, N'4386', N'', N'', N'X', 0x, CAST(0x0000A44C0002B3FE AS DateTime), N'e2b0135a-f0de-4eac-b512-a7017fbee9a6')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (795, N'NGUTI', N'NZISA', N'SHARON', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0733 768 726', N'test@example.com', N'X', N'', N'X', 1, N'7574', N'', N'', N'X', 0x, CAST(0x0000A44C0002B400 AS DateTime), N'9a6ab910-e1cc-492c-9016-ed2ee263fa43')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (798, N'CHRISTINE', N'PETER', N'MWONGELI', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0723 426 714', N'test@example.com', N'X', N'', N'X', 1, N'4701', N'', N'', N'X', 0x, CAST(0x0000A44C0002B401 AS DateTime), N'ed3f7576-cf8f-4a8c-8c06-4985d6f82f7a')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (799, N'WINFRIDAH', N'MURANDU', N'-', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0754 335 632', N'test@example.com', N'X', N'', N'X', 1, N'6717', N'', N'', N'X', 0x, CAST(0x0000A44C0002B403 AS DateTime), N'0598d74a-2fe2-44c3-b51d-64feb47d1ca8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (800, N'MAKAU ', N'MUTHEU', N'JANET', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0717 035 047', N'test@example.com', N'X', N'', N'X', 1, N'7624', N'', N'', N'X', 0x, CAST(0x0000A44C0002B405 AS DateTime), N'465c7297-e55b-4c4b-bc53-3f5d0a7d0d9d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (801, N'-', N'NYOKABI', N'JULYNE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 648 648', N'test@example.com', N'X', N'', N'X', 1, N'8905', N'', N'', N'X', 0x, CAST(0x0000A44C0002B406 AS DateTime), N'c48bc330-e6a2-4dc5-9234-7eba9487dd0d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (803, N'NDETO', N'NTHENYA', N'WINFRED', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0720 022 491', N'test@example.com', N'X', N'', N'X', 1, N'1052', N'', N'', N'X', 0x, CAST(0x0000A44C0002B409 AS DateTime), N'ba7951df-4ca6-4ec9-a994-26e75ab37e4d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (804, N'KALOVYA', N'MBITHE', N'CAROLINE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 983 401', N'test@example.com', N'X', N'', N'X', 1, N'8554', N'', N'', N'X', 0x, CAST(0x0000A44C0002B40A AS DateTime), N'746a7443-aad8-4bd0-904b-21ed396997f8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (806, N'SILA', N'MUKONYO', N'FRANCIA', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0728 202 346', N'test@example.com', N'X', N'', N'X', 1, N'1148', N'', N'', N'X', 0x, CAST(0x0000A44C0002B40C AS DateTime), N'c088f13c-35ea-4f11-a3d6-9c899e3b436c')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (807, N'MATOKE', N'KERUBO', N'NAOMI', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 065 476', N'test@example.com', N'X', N'', N'X', 1, N'7495', N'', N'', N'X', 0x, CAST(0x0000A44C0002B40E AS DateTime), N'fa5f93f4-a1f9-4b56-a74b-3622be7269d2')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (808, N'KISANGAU ', N'MBENGE', N'FAITH', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0729 771 726', N'test@example.com', N'X', N'', N'X', 1, N'3035', N'', N'', N'X', 0x, CAST(0x0000A44C0002B40F AS DateTime), N'87437fd1-f305-46a2-a65c-9b47d3a3cab4')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (809, N'MAKAU ', N'NGUNA', N'IMELDA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0719 890 410', N'test@example.com', N'X', N'', N'X', 1, N'8930', N'', N'', N'X', 0x, CAST(0x0000A44C0002B414 AS DateTime), N'b3158001-6b0c-4c18-b8fe-39a4ea716d93')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (810, N'MUMBUA', N'MARY', N'-', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 773 063', N'test@example.com', N'X', N'', N'X', 1, N'1265', N'', N'', N'X', 0x, CAST(0x0000A44C0002B416 AS DateTime), N'39f83c6b-587e-4aac-a32f-f7c0c62150a2')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (811, N'KIDINGA', N'ADHIAMBO', N'SILVIA', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 543 022', N'test@example.com', N'X', N'', N'X', 1, N'4351', N'', N'', N'X', 0x, CAST(0x0000A44C0002B417 AS DateTime), N'e8a4fa11-973e-4336-9e44-30927ab9b3e8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (812, N'OULA', N'AWUOR', N'JANET', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0724 010 949', N'test@example.com', N'X', N'', N'X', 1, N'5056', N'', N'', N'X', 0x, CAST(0x0000A44C0002B419 AS DateTime), N'1b222ea9-5c21-4485-a24c-c8cd47ce045f')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (813, N'MWALILI', N'NGINA', N'MARY', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0728 527 935', N'test@example.com', N'X', N'', N'X', 1, N'9520', N'', N'', N'X', 0x, CAST(0x0000A44C0002B41B AS DateTime), N'3f2674f6-862a-48b4-87b7-362399910d44')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (814, N'KELO', N'NTHENYA', N'CAROLINE', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0725 311 028', N'test@example.com', N'X', N'', N'X', 1, N'4151', N'', N'', N'X', 0x, CAST(0x0000A44C0002B41D AS DateTime), N'f4845a89-2d95-4b8d-a303-3c9522dccdb9')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (815, N'NZILU', N'NDUKU', N'LINET', 32, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0727 466 238', N'test@example.com', N'X', N'', N'X', 1, N'7151', N'', N'', N'X', 0x, CAST(0x0000A44C0002B41F AS DateTime), N'39576710-9f9c-4e92-97c2-d9e4c6a6e26d')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (820, N'MUUMBI', N'MUMO', N'-', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'8035', N'', N'', N'X', 0x, CAST(0x0000A44C0002B421 AS DateTime), N'299d7ec4-a148-4e36-94ff-fe8372f6c247')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (821, N'MUUMBI', N'KITENGELE', N'-', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'-', N'test@example.com', N'X', N'', N'X', 1, N'7937', N'', N'', N'X', 0x, CAST(0x0000A44C0002B422 AS DateTime), N'e145c810-4883-4913-b23c-2649451850d7')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (823, N'MOSAISE', N'NDUKU', N'NORAH', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0721 956 242', N'test@example.com', N'X', N'', N'X', 1, N'1383', N'', N'', N'X', 0x, CAST(0x0000A44C0002B424 AS DateTime), N'18d6fc35-3f65-4370-8067-9fbff9768671')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (824, N'JOHN ', N'MUKAMI', N'SETH', 31, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 696 815', N'test@example.com', N'X', N'', N'X', 1, N'3004', N'', N'', N'X', 0x, CAST(0x0000A44C0002B426 AS DateTime), N'cf168651-47d1-423a-be39-40df7f8764c8')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (825, N'MUTELE', N'MUTELE', N'BRENDA', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0722 639 765', N'test@example.com', N'X', N'', N'X', 1, N'7399', N'', N'', N'X', 0x, CAST(0x0000A44C0002B428 AS DateTime), N'32d1e8f6-76c9-4349-bf86-e1b4abb4e1d2')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (826, N'MONICAH', N'VERONICAH', N'NZILANI', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0711 668 067', N'test@example.com', N'X', N'', N'X', 1, N'9704', N'', N'', N'X', 0x, CAST(0x0000A44C0002B42A AS DateTime), N'f5843582-cae6-40c2-9434-295d0683668b')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (827, N'MUSEMBI', N'WAENI', N'JOSEPHINE', 30, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0726 164 476', N'test@example.com', N'X', N'', N'X', 1, N'2246', N'', N'', N'X', 0x, CAST(0x0000A44C0002B42B AS DateTime), N'edbb317f-a090-4878-a4e6-6b1f1c699805')
GO
INSERT [Institution].[Student] ([StudentID], [FirstName], [LastName], [MiddleName], [ClassID], [DateOfBirth], [DateOfAdmission], [NameOfGuardian], [GuardianPhoneNo], [Email], [Address], [City], [PostalCode], [IsActive], [PreviousBalance], [PreviousInstitution], [DormitoryID], [BedNo], [SPhoto], [ModifiedDate], [rowguid]) VALUES (828, N'NDILA', N'WANZA', N'REDEMPTER', 27, N'01/01/2000 00:00:00', N'27/02/2015 00:09:50', N'X', N'0724 371 097', N'test@example.com', N'X', N'', N'X', 1, N'7347', N'', N'', N'X', 0x, CAST(0x0000A44C0002B42D AS DateTime), N'9cae9e5c-fcfe-4706-8f68-c5356931ff5c')
GO
INSERT [Institution].[StudentTranscriptHeader] ([StudentTranscriptID], [StudentID], [Responsibilities], [ClubsAndSport], [Boarding], [ClassTeacher], [ClassTeacherComments], [Principal], [PrincipalComments], [OpeningDay], [ClosingDay], [Term1Pos], [Term2Pos], [Term3Pos], [DateSaved], [ModifiedDate], [rowguid]) VALUES (7, 497, N'', N'', N'', N'', N'', N'', N'', CAST(0x0000A445011DA500 AS DateTime), CAST(0x0000A445011DA500 AS DateTime), N'', N'', N'', CAST(0x0000A44C013E5430 AS DateTime), CAST(0x0000A445011DBC1B AS DateTime), N'fe659fea-b8b1-4b6b-97a6-0cb914c1801c')
GO
INSERT [Institution].[StudentTranscriptHeader] ([StudentTranscriptID], [StudentID], [Responsibilities], [ClubsAndSport], [Boarding], [ClassTeacher], [ClassTeacherComments], [Principal], [PrincipalComments], [OpeningDay], [ClosingDay], [Term1Pos], [Term2Pos], [Term3Pos], [DateSaved], [ModifiedDate], [rowguid]) VALUES (8, 434, N'', N'', N'', N'', N'', N'', N'', CAST(0x0000A44B0031FCE0 AS DateTime), CAST(0x0000A44B0031FCE0 AS DateTime), N'', N'', N'', CAST(0x0000A44D00EF8080 AS DateTime), CAST(0x0000A44B00321831 AS DateTime), N'44d13923-bc09-4477-9772-ca2aaff13952')
GO
INSERT [Institution].[StudentTranscriptHeader] ([StudentTranscriptID], [StudentID], [Responsibilities], [ClubsAndSport], [Boarding], [ClassTeacher], [ClassTeacherComments], [Principal], [PrincipalComments], [OpeningDay], [ClosingDay], [Term1Pos], [Term2Pos], [Term3Pos], [DateSaved], [ModifiedDate], [rowguid]) VALUES (9, 520, N'', N'', N'', N'', N'', N'', N'', CAST(0x0000A44C000CA260 AS DateTime), CAST(0x0000A44C000CA260 AS DateTime), N'', N'', N'', CAST(0x0000A44C013E9A80 AS DateTime), CAST(0x0000A44C000CDB31 AS DateTime), N'f66dc506-013c-4498-ac91-4a69630f8c49')
GO
INSERT [Institution].[StudentTranscriptHeader] ([StudentTranscriptID], [StudentID], [Responsibilities], [ClubsAndSport], [Boarding], [ClassTeacher], [ClassTeacherComments], [Principal], [PrincipalComments], [OpeningDay], [ClosingDay], [Term1Pos], [Term2Pos], [Term3Pos], [DateSaved], [ModifiedDate], [rowguid]) VALUES (10, 521, N'', N'', N'', N'', N'', N'', N'', CAST(0x0000A44C000FA7D0 AS DateTime), CAST(0x0000A44C000FA7D0 AS DateTime), N'', N'', N'', CAST(0x0000A44C013E9A80 AS DateTime), CAST(0x0000A44C000FD39A AS DateTime), N'82bffe50-fed2-49cd-9676-df1b9a64a4ce')
GO
INSERT [Institution].[StudentTranscriptHeader] ([StudentTranscriptID], [StudentID], [Responsibilities], [ClubsAndSport], [Boarding], [ClassTeacher], [ClassTeacherComments], [Principal], [PrincipalComments], [OpeningDay], [ClosingDay], [Term1Pos], [Term2Pos], [Term3Pos], [DateSaved], [ModifiedDate], [rowguid]) VALUES (11, 495, N'', N'', N'', N'', N'', N'', N'', CAST(0x0000A44C0139EF30 AS DateTime), CAST(0x0000A44C0139EF30 AS DateTime), N'', N'', N'', CAST(0x0000A44C013E5430 AS DateTime), CAST(0x0000A44C0139F45B AS DateTime), N'b83f53fb-7a88-449b-8e01-4b2cb82ea7f5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (32, N'MATHEMATICS', N'100', CAST(0x0000A44C000587E3 AS DateTime), N'bc78056b-b14e-491c-b738-d46eae52aa97')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (33, N'MATHEMATICS', N'100', CAST(0x0000A44C00059121 AS DateTime), N'a62fb8b3-d211-419a-9b89-2010e1e4edae')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (34, N'KISWAHILI', N'100', CAST(0x0000A44C00059122 AS DateTime), N'e8b9ceb8-2ebb-4a05-ac3d-f0dad1cc7fef')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (35, N'MATHEMATICS', N'100', CAST(0x0000A44C00059BAF AS DateTime), N'da39060a-b3fe-46c9-8394-06068430ff70')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (36, N'KISWAHILI', N'100', CAST(0x0000A44C00059BB1 AS DateTime), N'7b01d15e-2987-4a13-9b04-94abbf71e40d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (37, N'ENGLISH', N'100', CAST(0x0000A44C00059BB2 AS DateTime), N'6e69c3d5-a7e2-4003-a4bd-f3367bff966a')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (38, N'MATHEMATICS', N'100', CAST(0x0000A44C0005B541 AS DateTime), N'6ecc26b1-7285-44de-8dd8-e9cba7365f43')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (39, N'KISWAHILI', N'100', CAST(0x0000A44C0005B542 AS DateTime), N'4b7625ec-cb18-4652-8f49-08360d6aa116')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (40, N'ENGLISH', N'100', CAST(0x0000A44C0005B543 AS DateTime), N'0879adca-fcd7-47bd-abf3-22214fa07418')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (41, N'BIOLOGY', N'100', CAST(0x0000A44C0005B544 AS DateTime), N'3b0b473a-9a5a-40c6-bdc9-f93ddc9099f4')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (42, N'MATHEMATICS', N'100', CAST(0x0000A44C0005C12F AS DateTime), N'809ff6e3-b694-4028-8453-660f475ee919')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (43, N'KISWAHILI', N'100', CAST(0x0000A44C0005C131 AS DateTime), N'e6930013-6dbc-4f51-8035-81e90da56daf')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (44, N'ENGLISH', N'100', CAST(0x0000A44C0005C132 AS DateTime), N'323b5fb6-7645-46b2-bc42-54d955dbb2f1')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (45, N'BIOLOGY', N'100', CAST(0x0000A44C0005C133 AS DateTime), N'faf9395d-593d-4c91-b16f-8f105f578e17')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (46, N'CHEMISTRY', N'100', CAST(0x0000A44C0005C134 AS DateTime), N'efec634f-8166-4c77-bc24-85f9523606ec')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (47, N'MATHEMATICS', N'100', CAST(0x0000A44C0005C7E0 AS DateTime), N'9af7d71c-f8b5-4721-b06f-a58937a5e092')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (48, N'KISWAHILI', N'100', CAST(0x0000A44C0005C7E1 AS DateTime), N'65b2ae13-3590-406c-9d71-325e923437ad')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (49, N'ENGLISH', N'100', CAST(0x0000A44C0005C7E2 AS DateTime), N'd364c4e7-ef08-4ca3-a0f9-d3937c7fc3a6')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (50, N'BIOLOGY', N'100', CAST(0x0000A44C0005C7E3 AS DateTime), N'e31a4343-9a7a-4487-ad7a-d839b5691d49')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (51, N'CHEMISTRY', N'100', CAST(0x0000A44C0005C7E5 AS DateTime), N'ee33ad55-38bb-4c33-91bc-728690d5804d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (52, N'PHYSICS', N'100', CAST(0x0000A44C0005C7E6 AS DateTime), N'ab699114-7748-4053-a366-53b94e7f85dd')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (53, N'MATHEMATICS', N'100', CAST(0x0000A44C0005D228 AS DateTime), N'd957b2c3-36ba-4d3d-a0a1-b383f4f2c54b')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (54, N'KISWAHILI', N'100', CAST(0x0000A44C0005D229 AS DateTime), N'e90f50fb-6a3c-4c0e-bfe2-3a4bc7acddf5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (55, N'ENGLISH', N'100', CAST(0x0000A44C0005D22A AS DateTime), N'697ae0fb-f6e5-4432-b7b1-9e31318162d5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (56, N'BIOLOGY', N'100', CAST(0x0000A44C0005D22C AS DateTime), N'e49822b8-f7e5-4278-8142-783decac55cf')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (57, N'CHEMISTRY', N'100', CAST(0x0000A44C0005D22D AS DateTime), N'77e8dfa6-4c93-425d-85d6-079886f6fc08')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (58, N'PHYSICS', N'100', CAST(0x0000A44C0005D22F AS DateTime), N'6ed0b373-7b15-443d-9ac4-bd82fac4ccf0')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (59, N'GEOGRAPHY', N'100', CAST(0x0000A44C0005D230 AS DateTime), N'1e91a309-81bc-46bc-b528-b446d822e1d3')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (60, N'MATHEMATICS', N'100', CAST(0x0000A44C0005D8BE AS DateTime), N'bb606bcd-2c54-4a38-bb88-7f59aceb8dd9')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (61, N'KISWAHILI', N'100', CAST(0x0000A44C0005D8BF AS DateTime), N'c83ae63b-a178-47c5-a801-2099b5c061ba')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (62, N'ENGLISH', N'100', CAST(0x0000A44C0005D8C0 AS DateTime), N'd709746f-8d01-4898-a7ac-13a171d4fda7')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (63, N'BIOLOGY', N'100', CAST(0x0000A44C0005D8C1 AS DateTime), N'5f11beb1-9372-4bb7-92f3-b33ee3f955a2')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (64, N'CHEMISTRY', N'100', CAST(0x0000A44C0005D8C3 AS DateTime), N'7445fc8a-07a1-469b-9a95-560400f65247')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (65, N'PHYSICS', N'100', CAST(0x0000A44C0005D8C3 AS DateTime), N'7b277158-bc5d-4d8a-969f-781447f551cb')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (66, N'GEOGRAPHY', N'100', CAST(0x0000A44C0005D8C4 AS DateTime), N'36177599-a14f-42c7-8804-bf0d7eab9425')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (67, N'HISTORY', N'100', CAST(0x0000A44C0005D8C5 AS DateTime), N'698f4f12-98df-4d99-ad70-62ad50619963')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (68, N'MATHEMATICS', N'100', CAST(0x0000A44C0005DEE4 AS DateTime), N'47789add-7224-4c2f-8407-8b95b45863c5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (69, N'KISWAHILI', N'100', CAST(0x0000A44C0005DEE6 AS DateTime), N'd9a7c4c3-a575-424b-8dfa-b8784f3f2c70')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (70, N'ENGLISH', N'100', CAST(0x0000A44C0005DEE8 AS DateTime), N'e4f1b47f-003d-48cd-96e2-3171c4cbee1f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (71, N'BIOLOGY', N'100', CAST(0x0000A44C0005DEEA AS DateTime), N'456b046b-2c36-4d93-b6d3-ed0ffde1b816')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (72, N'CHEMISTRY', N'100', CAST(0x0000A44C0005DEEE AS DateTime), N'5c1bb952-341f-468f-9f77-2a9a11e5e269')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (73, N'PHYSICS', N'100', CAST(0x0000A44C0005DEF0 AS DateTime), N'a463d166-1296-4284-bbd9-b133e72d4058')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (74, N'GEOGRAPHY', N'100', CAST(0x0000A44C0005DEF2 AS DateTime), N'1062d832-2821-48e5-9ea0-629a4630079d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (75, N'HISTORY', N'100', CAST(0x0000A44C0005DEF4 AS DateTime), N'af117d08-4a5d-4374-a7ae-0a8e7a1eb38c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (76, N'CRE', N'100', CAST(0x0000A44C0005DEF6 AS DateTime), N'45e5d535-6bb7-4958-af91-7672a6ffd6b3')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (77, N'MATHEMATICS', N'100', CAST(0x0000A44C0005EDE5 AS DateTime), N'827788f7-bedb-4224-bdd2-7b40e654261b')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (78, N'KISWAHILI', N'100', CAST(0x0000A44C0005EDE7 AS DateTime), N'8c7162dd-3066-4c62-aa75-b18a125f168f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (79, N'ENGLISH', N'100', CAST(0x0000A44C0005EDE7 AS DateTime), N'9cf73dbf-f7f5-4dd0-ba95-09622889b88e')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (80, N'BIOLOGY', N'100', CAST(0x0000A44C0005EDE8 AS DateTime), N'43e79bc2-513d-472f-8939-df1040887791')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (81, N'CHEMISTRY', N'100', CAST(0x0000A44C0005EDE9 AS DateTime), N'00112cd6-7f78-4388-bb84-ed4740144e4d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (82, N'PHYSICS', N'100', CAST(0x0000A44C0005EDEA AS DateTime), N'5fb0fde2-75be-4de0-af25-fbd4a9f55517')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (83, N'GEOGRAPHY', N'100', CAST(0x0000A44C0005EDEC AS DateTime), N'7e5806f2-0a66-470d-9a2d-2840e3eedf24')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (84, N'HISTORY', N'100', CAST(0x0000A44C0005EDED AS DateTime), N'c2b21fce-b207-4a9e-b925-216f790f7122')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (85, N'CRE', N'100', CAST(0x0000A44C0005EDEF AS DateTime), N'35310e33-80e6-446b-8ee3-1b5b136732d8')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (86, N'BUSINESS STUDIES', N'100', CAST(0x0000A44C0005EDF0 AS DateTime), N'07a1755a-3b25-4ff5-add4-d4e50431aa99')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (87, N'MATHEMATICS', N'100', CAST(0x0000A44C0005FA83 AS DateTime), N'9774470b-3f61-4667-a6ea-947f2982bbed')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (88, N'KISWAHILI', N'100', CAST(0x0000A44C0005FA84 AS DateTime), N'a34a5c14-73a3-4b72-a803-affac9a2086c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (89, N'ENGLISH', N'100', CAST(0x0000A44C0005FA85 AS DateTime), N'0527db2a-f611-470a-bef6-7b68cf99e631')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (90, N'BIOLOGY', N'100', CAST(0x0000A44C0005FA86 AS DateTime), N'565374af-0bcc-45eb-85b5-3a8eec822f5e')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (91, N'CHEMISTRY', N'100', CAST(0x0000A44C0005FA87 AS DateTime), N'ab720f59-65dd-4f3a-8bb1-4bd12c9ba60b')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (92, N'PHYSICS', N'100', CAST(0x0000A44C0005FA88 AS DateTime), N'21d7f1ec-0bbb-4c25-99c4-d87e782eb8dd')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (93, N'GEOGRAPHY', N'100', CAST(0x0000A44C0005FA89 AS DateTime), N'bdcf0b1f-e6d6-47b6-b138-b231bcd728e7')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (94, N'HISTORY', N'100', CAST(0x0000A44C0005FA8A AS DateTime), N'efee659d-388f-4b63-9724-f3e90ba211ff')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (95, N'CRE', N'100', CAST(0x0000A44C0005FA8B AS DateTime), N'd5f67087-f319-494f-83b4-b82e1e893c4d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (96, N'BUSINESS STUDIES', N'100', CAST(0x0000A44C0005FA8C AS DateTime), N'4d81ee83-39ba-46b9-a7ca-721675016292')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (97, N'AGRICULTURE', N'100', CAST(0x0000A44C0005FA8D AS DateTime), N'de0563eb-3ae4-4136-bb99-af405f276226')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (98, N'MATHEMATICS', N'100', CAST(0x0000A44C0138973E AS DateTime), N'fb54a394-ca97-4512-9c8c-b3d691452eb0')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (99, N'MATHEMATICS', N'100', CAST(0x0000A44C0138A234 AS DateTime), N'37dd674e-69e6-446f-b729-ca8913e8d34b')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (100, N'KISWAHILI', N'100', CAST(0x0000A44C0138A236 AS DateTime), N'ad9e07c4-3630-4f73-804d-74557bbccabf')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (101, N'MATHEMATICS', N'100', CAST(0x0000A44C0138AA82 AS DateTime), N'96b200e0-92a7-4dc2-b91f-c6814293db8d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (102, N'KISWAHILI', N'100', CAST(0x0000A44C0138AA84 AS DateTime), N'1299a597-27cc-42fb-9aa6-489433f1a8d3')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (103, N'ENGLISH', N'100', CAST(0x0000A44C0138AA86 AS DateTime), N'65c872ab-1673-49c4-888b-4312d40d66c0')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (104, N'MATHEMATICS', N'100', CAST(0x0000A44C0138B1DA AS DateTime), N'eedeb73f-74fa-4b05-a3e6-b3db6f944410')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (105, N'KISWAHILI', N'100', CAST(0x0000A44C0138B1DC AS DateTime), N'c0696af8-52bd-4236-9bb2-f6e3da96c116')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (106, N'ENGLISH', N'100', CAST(0x0000A44C0138B1DD AS DateTime), N'83379e96-3176-40fa-8c02-6c85f1359533')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (107, N'BIOLOGY', N'100', CAST(0x0000A44C0138B1DE AS DateTime), N'1efa35e1-7254-4830-af74-892c3a35876c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (108, N'MATHEMATICS', N'100', CAST(0x0000A44C0138C01B AS DateTime), N'afc9ff51-d7cc-445f-bc27-77f44a697cdc')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (109, N'KISWAHILI', N'100', CAST(0x0000A44C0138C01D AS DateTime), N'c5d633d4-aa66-41f5-ac05-e13e65f3cbaf')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (110, N'ENGLISH', N'100', CAST(0x0000A44C0138C01E AS DateTime), N'5d6fbc06-70e4-4a26-86d3-66c2819cab3b')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (111, N'BIOLOGY', N'100', CAST(0x0000A44C0138C020 AS DateTime), N'ebc1daad-a583-433a-a96a-6ea6d82d294c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (112, N'CHEMISTRY', N'100', CAST(0x0000A44C0138C022 AS DateTime), N'2946ee82-f8b1-440c-ae63-d7df0a0fa2ef')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (113, N'MATHEMATICS', N'100', CAST(0x0000A44C0138C6E2 AS DateTime), N'aaba1b8f-2340-4ade-994a-dbb1ff0e021f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (114, N'KISWAHILI', N'100', CAST(0x0000A44C0138C6E7 AS DateTime), N'0c623a83-0ef6-4c34-bc7a-0efcf4751e5f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (115, N'ENGLISH', N'100', CAST(0x0000A44C0138C6E9 AS DateTime), N'87cd551b-f8ab-46cc-9da9-6704949bdf7c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (116, N'BIOLOGY', N'100', CAST(0x0000A44C0138C6E9 AS DateTime), N'90eee31e-8447-4488-8cf3-974f1d162932')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (117, N'CHEMISTRY', N'100', CAST(0x0000A44C0138C6EB AS DateTime), N'7af56c3e-e2eb-4eaf-9ad4-4fe118532c0f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (118, N'PHYSICS', N'100', CAST(0x0000A44C0138C6EC AS DateTime), N'f99e6852-e3b2-495a-a03b-514e7b582bbc')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (119, N'MATHEMATICS', N'100', CAST(0x0000A44C0138D2DC AS DateTime), N'369c00fe-968f-42dc-a3db-e57138bad674')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (120, N'KISWAHILI', N'100', CAST(0x0000A44C0138D2DE AS DateTime), N'db5bf739-a746-4329-a059-7172b2022955')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (121, N'ENGLISH', N'100', CAST(0x0000A44C0138D2DF AS DateTime), N'dd360f6e-e136-4f62-a1bf-7b5af279612a')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (122, N'BIOLOGY', N'100', CAST(0x0000A44C0138D2E1 AS DateTime), N'4f0e4b92-5427-41b1-bce3-1daed7c71775')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (123, N'CHEMISTRY', N'100', CAST(0x0000A44C0138D2E3 AS DateTime), N'22db4fdb-fbed-469f-82ea-f008751a2902')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (124, N'PHYSICS', N'100', CAST(0x0000A44C0138D2E4 AS DateTime), N'42016948-63b7-4fa3-a244-75f5dc1d3ec4')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (125, N'GEOGRAPHY', N'100', CAST(0x0000A44C0138D2E6 AS DateTime), N'c87570c2-d730-4d06-a307-a5ef6a0b189f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (126, N'MATHEMATICS', N'100', CAST(0x0000A44C0138DD5F AS DateTime), N'39ab42c9-9101-4f21-b81a-6de9bae01684')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (127, N'KISWAHILI', N'100', CAST(0x0000A44C0138DD60 AS DateTime), N'e5c2d002-ae81-4919-acf4-f49baec1dc49')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (128, N'ENGLISH', N'100', CAST(0x0000A44C0138DD62 AS DateTime), N'5dcffba5-7e9a-4a87-a527-ee707be1c263')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (129, N'BIOLOGY', N'100', CAST(0x0000A44C0138DD65 AS DateTime), N'a2017bb4-01c5-42fd-8df0-ca7f3d36a454')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (130, N'CHEMISTRY', N'100', CAST(0x0000A44C0138DD67 AS DateTime), N'acefa24f-0773-4bab-80cb-d6bd7fabb66c')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (131, N'PHYSICS', N'100', CAST(0x0000A44C0138DD68 AS DateTime), N'59da64d9-06a0-401a-91ed-57a34ca55072')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (132, N'GEOGRAPHY', N'100', CAST(0x0000A44C0138DD69 AS DateTime), N'76398cd6-870d-4ffc-bc60-fb808946ebfb')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (133, N'HISTORY', N'100', CAST(0x0000A44C0138DD6A AS DateTime), N'4470db47-13b5-401b-800e-41357f72c62d')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (134, N'MATHEMATICS', N'100', CAST(0x0000A44C0138E34D AS DateTime), N'67e8a904-124a-4c59-a55e-c62b07020a70')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (135, N'KISWAHILI', N'100', CAST(0x0000A44C0138E34E AS DateTime), N'8a33d59c-8dd2-4a72-bf84-05c430720611')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (136, N'ENGLISH', N'100', CAST(0x0000A44C0138E34F AS DateTime), N'3aa687ac-ff33-40c5-a87f-b6c3c7025cbd')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (137, N'BIOLOGY', N'100', CAST(0x0000A44C0138E350 AS DateTime), N'ec800a14-4594-4c09-bdd8-c2d709555bbc')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (138, N'CHEMISTRY', N'100', CAST(0x0000A44C0138E351 AS DateTime), N'ab22b97c-33e2-4825-86f7-2201758e1660')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (139, N'PHYSICS', N'100', CAST(0x0000A44C0138E353 AS DateTime), N'574988e2-c155-42c8-9b25-f5ff3d44ddf4')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (140, N'GEOGRAPHY', N'100', CAST(0x0000A44C0138E355 AS DateTime), N'a22dfc21-3a76-47fd-95c4-73da777f8cc1')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (141, N'HISTORY', N'100', CAST(0x0000A44C0138E356 AS DateTime), N'a175237c-4289-4a84-9287-30dcddc3cc35')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (142, N'CRE', N'100', CAST(0x0000A44C0138E357 AS DateTime), N'95f4f686-ff90-4dec-8003-cc90031df12a')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (143, N'MATHEMATICS', N'100', CAST(0x0000A44C0138F28A AS DateTime), N'5f1f7dea-850d-4f19-9bc6-1b2a047649ae')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (144, N'KISWAHILI', N'100', CAST(0x0000A44C0138F28B AS DateTime), N'a857f419-e1f5-4d6e-b6b1-76aaf01e8f83')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (145, N'ENGLISH', N'100', CAST(0x0000A44C0138F28D AS DateTime), N'502e4df5-1842-4178-9f45-2ebfd07041aa')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (146, N'BIOLOGY', N'100', CAST(0x0000A44C0138F28E AS DateTime), N'f50c56f5-74b6-4e5d-a7b5-3dddf9cbabe5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (147, N'CHEMISTRY', N'100', CAST(0x0000A44C0138F28F AS DateTime), N'c28c2853-088a-43b0-9d6f-bd541d4f4455')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (148, N'PHYSICS', N'100', CAST(0x0000A44C0138F290 AS DateTime), N'7eb151ae-b06a-4b15-9d3c-5954b47edee5')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (149, N'GEOGRAPHY', N'100', CAST(0x0000A44C0138F291 AS DateTime), N'8f3891eb-6921-4edc-8dfc-abe68bbe1815')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (150, N'HISTORY', N'100', CAST(0x0000A44C0138F293 AS DateTime), N'285d6276-11c5-4727-8c68-f4d1ddf2b7ee')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (151, N'CRE', N'100', CAST(0x0000A44C0138F294 AS DateTime), N'6ef0d931-ed5a-4b4b-8f94-7424ff0e0f3f')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (152, N'BUSINESS S/T', N'100', CAST(0x0000A44C0138F295 AS DateTime), N'7d49d274-98af-4a1d-9072-68b7dbe78b5a')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (153, N'MATHEMATICS', N'100', CAST(0x0000A44C0138F9B9 AS DateTime), N'233d8c84-587f-4a0d-a7ef-ec6eefedd5da')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (154, N'KISWAHILI', N'100', CAST(0x0000A44C0138F9BA AS DateTime), N'dc2c3d56-89d1-4459-afbf-edb42133a1fa')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (155, N'ENGLISH', N'100', CAST(0x0000A44C0138F9BB AS DateTime), N'61b9ce02-d708-4e99-9460-6e61a03fff93')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (156, N'BIOLOGY', N'100', CAST(0x0000A44C0138F9BC AS DateTime), N'cc4d2c38-8ba9-4297-a87e-5ae5cb7a76cb')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (157, N'CHEMISTRY', N'100', CAST(0x0000A44C0138F9BE AS DateTime), N'e55fa94a-932a-4000-9db1-59d856a3d940')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (158, N'PHYSICS', N'100', CAST(0x0000A44C0138F9BF AS DateTime), N'd6b43f53-2265-43af-833f-129b48472161')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (159, N'GEOGRAPHY', N'100', CAST(0x0000A44C0138F9C2 AS DateTime), N'981ea86c-a87b-46a5-9ebb-6c76b46fd6c2')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (160, N'HISTORY', N'100', CAST(0x0000A44C0138F9C3 AS DateTime), N'e129a689-4822-4005-ae4d-e954a508b5d4')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (161, N'CRE', N'100', CAST(0x0000A44C0138F9C4 AS DateTime), N'679a0851-1da2-4261-8535-879b68d81134')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (162, N'BUSINESS S/T', N'100', CAST(0x0000A44C0138F9C5 AS DateTime), N'6f12c3ef-ffe8-4db5-bc72-c0b8ae703e60')
GO
INSERT [Institution].[Subject] ([SubjectID], [NameOfSubject], [MaximumScore], [ModifiedDate], [rowguid]) VALUES (163, N'AGRICULTURE', N'100', CAST(0x0000A44C0138F9C6 AS DateTime), N'3888aacb-bb3f-4e03-a5a1-4560b5263a36')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (32, 11, 32, CAST(0x0000A44C000587E1 AS DateTime), N'e7981b33-88f5-46dd-be5d-4da68d18b84f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (33, 12, 33, CAST(0x0000A44C00059120 AS DateTime), N'658e518a-d0d6-41db-aad2-bc25500bf328')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (34, 12, 34, CAST(0x0000A44C00059121 AS DateTime), N'863e5d5a-739c-4630-ab7b-0607633d29e9')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (35, 13, 35, CAST(0x0000A44C00059BAF AS DateTime), N'f00e8dde-7512-4ca3-8f7b-4e383cbff9fe')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (36, 13, 36, CAST(0x0000A44C00059BB0 AS DateTime), N'c1f93df8-2f40-4b31-bcfb-3e38952ec02d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (37, 13, 37, CAST(0x0000A44C00059BB2 AS DateTime), N'357c7b2d-d008-44de-8e21-f597345b2e91')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (38, 14, 38, CAST(0x0000A44C0005B541 AS DateTime), N'be499f2c-14cc-4228-8fd8-7162be1a993d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (39, 14, 39, CAST(0x0000A44C0005B541 AS DateTime), N'55b9e72a-4042-4c49-9295-51a2069b1feb')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (40, 14, 40, CAST(0x0000A44C0005B542 AS DateTime), N'218da529-d034-4d3e-a78e-15c749103dcb')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (41, 14, 41, CAST(0x0000A44C0005B544 AS DateTime), N'0dac66be-2682-419e-937f-0240e32d865c')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (42, 15, 42, CAST(0x0000A44C0005C12F AS DateTime), N'd7ff0e3d-c87d-4230-b5ac-150084e354d9')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (43, 15, 43, CAST(0x0000A44C0005C130 AS DateTime), N'c8532b3d-cacc-49af-9be7-08498fa186b0')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (44, 15, 44, CAST(0x0000A44C0005C132 AS DateTime), N'ec80435b-e3aa-4fe3-b38d-b431aa9e8875')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (45, 15, 45, CAST(0x0000A44C0005C132 AS DateTime), N'a7306e7c-a7c8-4c9b-810c-981575516913')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (46, 15, 46, CAST(0x0000A44C0005C134 AS DateTime), N'a7b12231-37d0-4380-8e1e-c5ff01296f50')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (47, 16, 47, CAST(0x0000A44C0005C7E0 AS DateTime), N'5c8807e8-050f-4ffc-a4e0-296cfedb96af')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (48, 16, 48, CAST(0x0000A44C0005C7E1 AS DateTime), N'3c74508e-1679-4973-a84f-31c5d2b74d3f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (49, 16, 49, CAST(0x0000A44C0005C7E2 AS DateTime), N'5bf48b51-6c7a-42ee-beff-8c0f2e6eec3d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (50, 16, 50, CAST(0x0000A44C0005C7E3 AS DateTime), N'a87d7c74-e900-409e-9d45-badacbfdd604')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (51, 16, 51, CAST(0x0000A44C0005C7E4 AS DateTime), N'4f1faf7c-01d6-4d64-b243-cf7d7c1a8611')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (52, 16, 52, CAST(0x0000A44C0005C7E5 AS DateTime), N'f890b74e-26dc-4c42-bed9-6fed3614b416')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (53, 17, 53, CAST(0x0000A44C0005D224 AS DateTime), N'5dcb487e-04a0-4e5f-a82c-7ab0271eea49')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (54, 17, 54, CAST(0x0000A44C0005D229 AS DateTime), N'911ada28-c135-4715-bde9-140d17039f30')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (55, 17, 55, CAST(0x0000A44C0005D22A AS DateTime), N'bdb994d6-93e2-48bf-8074-921e0f318758')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (56, 17, 56, CAST(0x0000A44C0005D22B AS DateTime), N'ef3ec447-e6d1-49b3-9d68-8daa294be834')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (57, 17, 57, CAST(0x0000A44C0005D22D AS DateTime), N'bd46324f-22e6-4321-9e39-8314f0d3bf0a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (58, 17, 58, CAST(0x0000A44C0005D22E AS DateTime), N'df14b322-5a46-49df-8b34-31ef4d38dc46')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (59, 17, 59, CAST(0x0000A44C0005D230 AS DateTime), N'dc92f005-b000-4055-aa2e-640adca4c15d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (60, 18, 60, CAST(0x0000A44C0005D8BD AS DateTime), N'7a17e496-ac21-40da-8289-31bb96850b94')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (61, 18, 61, CAST(0x0000A44C0005D8BE AS DateTime), N'aefd4748-3e3b-4fac-ba07-24a81d1687fc')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (62, 18, 62, CAST(0x0000A44C0005D8C0 AS DateTime), N'adcc19f3-cf6f-49fe-b982-f4346680548d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (63, 18, 63, CAST(0x0000A44C0005D8C1 AS DateTime), N'9be2d7a3-ba24-4642-864a-9b3468b6f4cd')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (64, 18, 64, CAST(0x0000A44C0005D8C2 AS DateTime), N'1d13ed45-8c2f-4425-aebe-fe514bdd8934')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (65, 18, 65, CAST(0x0000A44C0005D8C3 AS DateTime), N'5dea85ed-4778-4cc7-88d1-9c77e878d36b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (66, 18, 66, CAST(0x0000A44C0005D8C4 AS DateTime), N'5d63b934-7d7b-445a-b006-7cad357e5ec8')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (67, 18, 67, CAST(0x0000A44C0005D8C5 AS DateTime), N'048f4ee6-3ccd-47a5-9d3d-789007fdb6b8')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (68, 19, 68, CAST(0x0000A44C0005DEE3 AS DateTime), N'aa7280fc-e888-4f5f-b744-fb62d8a11acd')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (69, 19, 69, CAST(0x0000A44C0005DEE5 AS DateTime), N'7d075289-618e-468e-902e-e2caaa016f2b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (70, 19, 70, CAST(0x0000A44C0005DEE7 AS DateTime), N'5e25451e-3a6f-428c-9688-2e55ca1fb266')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (71, 19, 71, CAST(0x0000A44C0005DEE8 AS DateTime), N'2a4372e7-16f0-47e8-a4a6-35be0b261779')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (72, 19, 72, CAST(0x0000A44C0005DEED AS DateTime), N'7cd32e60-a757-44b0-80e7-5cf9b3212ed3')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (73, 19, 73, CAST(0x0000A44C0005DEF0 AS DateTime), N'5070bd3d-810d-454d-b878-98e747558a60')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (74, 19, 74, CAST(0x0000A44C0005DEF1 AS DateTime), N'4a2be407-265c-4c32-8e5b-e492289b561f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (75, 19, 75, CAST(0x0000A44C0005DEF3 AS DateTime), N'8dda59fa-056b-4bf5-ab2e-1cde168f3688')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (76, 19, 76, CAST(0x0000A44C0005DEF6 AS DateTime), N'323a1958-ab49-48cf-9a88-bac8fe9e7cad')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (77, 20, 77, CAST(0x0000A44C0005EDE5 AS DateTime), N'f1b8398c-cd4c-407b-ac13-7c663ab856ac')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (78, 20, 78, CAST(0x0000A44C0005EDE6 AS DateTime), N'1c6a9bfa-186c-4814-bd03-7ffdec89110e')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (79, 20, 79, CAST(0x0000A44C0005EDE7 AS DateTime), N'04c7bbe2-4da0-424b-b7df-68e198583316')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (80, 20, 80, CAST(0x0000A44C0005EDE8 AS DateTime), N'6673601e-112e-4187-8445-dfe0cebf4243')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (81, 20, 81, CAST(0x0000A44C0005EDE9 AS DateTime), N'3e21cc52-3d4d-4262-994d-bc1d96fb86fc')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (82, 20, 82, CAST(0x0000A44C0005EDEA AS DateTime), N'ba8248e7-f6cb-4122-9b1f-ab1e6366de84')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (83, 20, 83, CAST(0x0000A44C0005EDEB AS DateTime), N'fcb548a7-de30-4de9-aed2-d98cea3c4a7f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (84, 20, 84, CAST(0x0000A44C0005EDED AS DateTime), N'5c27e663-3910-4f89-b890-7b184208012b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (85, 20, 85, CAST(0x0000A44C0005EDEE AS DateTime), N'f4537da5-e166-416d-9f4f-939605d88dd3')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (86, 20, 86, CAST(0x0000A44C0005EDF0 AS DateTime), N'5888ce7e-fc36-420b-8722-486ab2bdbd9f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (87, 21, 87, CAST(0x0000A44C0005FA83 AS DateTime), N'bf1822f1-27c3-4137-ae5c-0983cc4f4822')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (88, 21, 88, CAST(0x0000A44C0005FA83 AS DateTime), N'3ace30c6-27d9-4d12-b026-b3b14049a0ef')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (89, 21, 89, CAST(0x0000A44C0005FA84 AS DateTime), N'26a293db-a766-4c59-8e11-a3b8c86a9c6e')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (90, 21, 90, CAST(0x0000A44C0005FA85 AS DateTime), N'b2e2baf3-bdf6-4f99-bf3b-6a549f8879da')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (91, 21, 91, CAST(0x0000A44C0005FA86 AS DateTime), N'8ad1fc8d-770c-4fce-a9e4-1b4f973cb518')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (92, 21, 92, CAST(0x0000A44C0005FA88 AS DateTime), N'267b7623-b27a-464c-887d-b211c047b78e')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (93, 21, 93, CAST(0x0000A44C0005FA89 AS DateTime), N'2e321583-175b-4e41-beec-b97e7bcb195f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (94, 21, 94, CAST(0x0000A44C0005FA8A AS DateTime), N'4eed4bc1-cad7-4ba1-b322-c6ef422c57ef')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (95, 21, 95, CAST(0x0000A44C0005FA8B AS DateTime), N'628089a5-0c97-4e73-9d7a-a3dacd74ffd6')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (96, 21, 96, CAST(0x0000A44C0005FA8C AS DateTime), N'faff1008-6583-4683-badd-5308e9675a17')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (97, 21, 97, CAST(0x0000A44C0005FA8D AS DateTime), N'17e9caf6-3b27-4b7d-879b-74a8062841fb')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (98, 22, 98, CAST(0x0000A44C0138973D AS DateTime), N'67efc99c-fe5c-4ec9-9a2d-db60ce7eb432')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (99, 23, 99, CAST(0x0000A44C0138A233 AS DateTime), N'428cdcf2-a5fb-4ddd-af47-01018d0f3d72')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (100, 23, 100, CAST(0x0000A44C0138A235 AS DateTime), N'f90f73dc-8b6d-4cce-aad5-4d4f4930c862')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (101, 24, 101, CAST(0x0000A44C0138AA81 AS DateTime), N'd88edd33-aff0-4420-bab7-133ae10dd912')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (102, 24, 102, CAST(0x0000A44C0138AA83 AS DateTime), N'97f84da0-c699-455c-8cbb-e5633dbb064a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (103, 24, 103, CAST(0x0000A44C0138AA85 AS DateTime), N'3b735d94-fd82-4438-9f16-82832cca1fb6')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (104, 25, 104, CAST(0x0000A44C0138B1DA AS DateTime), N'cdc3430e-1fb2-4ef3-b293-13abda0cfa62')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (105, 25, 105, CAST(0x0000A44C0138B1DB AS DateTime), N'37b5491b-36b1-4d12-8a9f-747367726223')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (106, 25, 106, CAST(0x0000A44C0138B1DD AS DateTime), N'7569cdf0-5094-48f2-ad10-45010166e1f3')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (107, 25, 107, CAST(0x0000A44C0138B1DE AS DateTime), N'011919e0-49d1-4e0f-999e-1b1153c30379')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (108, 26, 108, CAST(0x0000A44C0138C01B AS DateTime), N'9ffe7b18-5a78-4d76-9599-8061c5991d62')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (109, 26, 109, CAST(0x0000A44C0138C01C AS DateTime), N'cf0c20d2-521f-41a3-893e-85867e5ca0e8')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (110, 26, 110, CAST(0x0000A44C0138C01D AS DateTime), N'e01e4426-f3d1-4588-90a2-6d8a4bfd2e4b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (111, 26, 111, CAST(0x0000A44C0138C020 AS DateTime), N'1f4125d2-dd08-42f7-9dac-d052717586a8')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (112, 26, 112, CAST(0x0000A44C0138C021 AS DateTime), N'862b1f70-ca48-4934-a6c4-6a9a617cc679')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (113, 27, 113, CAST(0x0000A44C0138C6E2 AS DateTime), N'9d6090ad-9514-4f75-a951-df337ddc61eb')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (114, 27, 114, CAST(0x0000A44C0138C6E5 AS DateTime), N'4ab2639a-ad23-4a12-87dd-5cbfe91e5041')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (115, 27, 115, CAST(0x0000A44C0138C6E8 AS DateTime), N'a14c2457-d44c-4609-bb4c-6aef99bb772a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (116, 27, 116, CAST(0x0000A44C0138C6E9 AS DateTime), N'8ac9a5c2-61ef-4c6e-bacf-b288cb08178a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (117, 27, 117, CAST(0x0000A44C0138C6EB AS DateTime), N'cfbaa09b-36d7-468a-b39e-dc21f6f916a6')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (118, 27, 118, CAST(0x0000A44C0138C6EC AS DateTime), N'5552c41e-2c64-4c01-995d-cb343e81dcfd')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (119, 28, 119, CAST(0x0000A44C0138D2DB AS DateTime), N'e5e2be25-eef3-47e3-a9f9-4b8b65bf7443')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (120, 28, 120, CAST(0x0000A44C0138D2DD AS DateTime), N'0effd8d5-306e-48ab-9a26-96729aad0ec2')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (121, 28, 121, CAST(0x0000A44C0138D2DF AS DateTime), N'da1ea41e-5c87-4f64-bc15-df7f5f811849')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (122, 28, 122, CAST(0x0000A44C0138D2E0 AS DateTime), N'3325b1ef-a53d-4564-a749-b94decdc946d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (123, 28, 123, CAST(0x0000A44C0138D2E2 AS DateTime), N'1baa2c96-1827-4518-9794-0f61e450d110')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (124, 28, 124, CAST(0x0000A44C0138D2E3 AS DateTime), N'28c88bc1-2316-43bb-b255-0bf5e7e8a1fd')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (125, 28, 125, CAST(0x0000A44C0138D2E5 AS DateTime), N'69ff1a7d-12ed-495f-9c88-60dbaccd2279')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (126, 29, 126, CAST(0x0000A44C0138DD5F AS DateTime), N'9ad7670b-bd09-4cc0-b420-f4bb00c4d8f2')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (127, 29, 127, CAST(0x0000A44C0138DD60 AS DateTime), N'e0b55050-5123-48eb-aff1-4602e10636f3')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (128, 29, 128, CAST(0x0000A44C0138DD61 AS DateTime), N'309e3417-f615-4b9e-aa99-ad50935a8f38')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (129, 29, 129, CAST(0x0000A44C0138DD65 AS DateTime), N'333802d9-15c0-4a28-a9e3-836d46ee789d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (130, 29, 130, CAST(0x0000A44C0138DD66 AS DateTime), N'8f15a176-9184-403d-b9ea-2de7a443b099')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (131, 29, 131, CAST(0x0000A44C0138DD67 AS DateTime), N'741b7c66-1b41-4795-8678-fccdfb5cee24')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (132, 29, 132, CAST(0x0000A44C0138DD69 AS DateTime), N'f7eb2dda-45ed-4a82-a203-e36c4bb5c2da')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (133, 29, 133, CAST(0x0000A44C0138DD6A AS DateTime), N'1231ced9-8764-40dc-a569-49955b09a38b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (134, 30, 134, CAST(0x0000A44C0138E34C AS DateTime), N'd42154b4-ee93-4908-bca4-ad0b0365f47a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (135, 30, 135, CAST(0x0000A44C0138E34D AS DateTime), N'31be97fc-a9ba-45ae-aebd-127c40ed871a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (136, 30, 136, CAST(0x0000A44C0138E34E AS DateTime), N'377fe695-35c8-4124-9cba-3cf78b563a00')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (137, 30, 137, CAST(0x0000A44C0138E350 AS DateTime), N'16922dbe-98a2-426e-8bbe-022c7173912c')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (138, 30, 138, CAST(0x0000A44C0138E351 AS DateTime), N'84b51693-9b06-4ff5-b7ca-9eeb763e8f94')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (139, 30, 139, CAST(0x0000A44C0138E352 AS DateTime), N'3f31b7df-0ce4-44d7-80b7-d75e28638637')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (140, 30, 140, CAST(0x0000A44C0138E354 AS DateTime), N'8c977f05-7a45-4209-8284-68bcbe40f4b4')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (141, 30, 141, CAST(0x0000A44C0138E355 AS DateTime), N'53795068-84df-4aae-ba68-310336c6f463')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (142, 30, 142, CAST(0x0000A44C0138E357 AS DateTime), N'ac7c50b3-8d16-4c13-ae64-692ea8b0e509')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (143, 31, 143, CAST(0x0000A44C0138F289 AS DateTime), N'5d18f39d-97d7-43cf-93c9-65df10cff598')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (144, 31, 144, CAST(0x0000A44C0138F28B AS DateTime), N'1f59a7ad-0f17-428e-8889-6ddee4ff3c14')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (145, 31, 145, CAST(0x0000A44C0138F28C AS DateTime), N'f09fbdcc-4cdc-45e8-b878-04a1b553bc27')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (146, 31, 146, CAST(0x0000A44C0138F28D AS DateTime), N'2838251f-3a7c-4d68-ad83-b9820d4e891f')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (147, 31, 147, CAST(0x0000A44C0138F28F AS DateTime), N'10c0bafd-d897-4e36-ae90-ae5d779e4d14')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (148, 31, 148, CAST(0x0000A44C0138F290 AS DateTime), N'2b8002b0-f797-44ba-add7-70d6221afff6')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (149, 31, 149, CAST(0x0000A44C0138F291 AS DateTime), N'28d39533-0365-4fe6-b8c3-47fa9d2c6123')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (150, 31, 150, CAST(0x0000A44C0138F292 AS DateTime), N'72f17f42-abbf-4160-be38-5705e6d3004b')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (151, 31, 151, CAST(0x0000A44C0138F293 AS DateTime), N'fd273cbd-e128-4a3e-ab0f-97982fe6673d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (152, 31, 152, CAST(0x0000A44C0138F295 AS DateTime), N'90e89f43-8dd7-4f68-87e9-3dfea4dbf54d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (153, 32, 153, CAST(0x0000A44C0138F9B8 AS DateTime), N'90241e10-3503-4a4f-bd57-215de193639d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (154, 32, 154, CAST(0x0000A44C0138F9B9 AS DateTime), N'f299e31c-2588-4cfd-848d-5b8a102af0ff')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (155, 32, 155, CAST(0x0000A44C0138F9BA AS DateTime), N'fd6607fe-9fe6-4b01-9865-ac014794f1c7')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (156, 32, 156, CAST(0x0000A44C0138F9BC AS DateTime), N'9c49b787-d08b-4f7a-95eb-c25a9d2e4a2d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (157, 32, 157, CAST(0x0000A44C0138F9BD AS DateTime), N'c6406cd5-bafc-4b94-83db-47719a47b4d8')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (158, 32, 158, CAST(0x0000A44C0138F9BF AS DateTime), N'c85b0976-8dfb-4975-8833-da4f1bd37313')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (159, 32, 159, CAST(0x0000A44C0138F9C1 AS DateTime), N'a6ef5dd1-f676-4661-8d8c-6b13940b8b3a')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (160, 32, 160, CAST(0x0000A44C0138F9C2 AS DateTime), N'63ae6728-df67-4dd8-9ce9-ab3d8e0c5d4d')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (161, 32, 161, CAST(0x0000A44C0138F9C4 AS DateTime), N'3ae2d16a-bbd7-42d6-8886-56f253bd809e')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (162, 32, 162, CAST(0x0000A44C0138F9C5 AS DateTime), N'd9d383d2-e608-46aa-b8e3-3cff786746bb')
GO
INSERT [Institution].[SubjectSetupDetail] ([SubjectSetupDetailID], [SubjectSetupID], [SubjectID], [ModifiedDate], [rowguid]) VALUES (163, 32, 163, CAST(0x0000A44C0138F9C6 AS DateTime), N'806d94b7-b89a-4aaf-adc5-51c3f94f6cc9')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (11, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C00059120 AS DateTime), CAST(0x0000A44C000587CC AS DateTime), N'61c59ce6-e850-496b-9914-a0575ac70a0f')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (12, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C00059BAF AS DateTime), CAST(0x0000A44C00059120 AS DateTime), N'090f7387-2738-4c49-b0b4-76b6f80e5fd3')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (13, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005B540 AS DateTime), CAST(0x0000A44C00059BAF AS DateTime), N'26e4faa6-24f1-4aa7-b0fa-6b0470142b9f')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (14, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005C12E AS DateTime), CAST(0x0000A44C0005B540 AS DateTime), N'946555ac-d601-4475-9d9a-70d204c82f5b')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (15, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005C7DF AS DateTime), CAST(0x0000A44C0005C12E AS DateTime), N'c5aa8f79-d1d1-4872-8eed-37f5f6b3feff')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (16, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005D223 AS DateTime), CAST(0x0000A44C0005C7DF AS DateTime), N'85e64b6c-f43e-4caf-8b4d-5396b9d81195')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (17, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005D8BC AS DateTime), CAST(0x0000A44C0005D223 AS DateTime), N'811eedf0-b152-4a99-bd45-9fd768e4bb73')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (18, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005DEE3 AS DateTime), CAST(0x0000A44C0005D8BC AS DateTime), N'374afaf7-e3ce-4b1b-ba22-bf93f9e2c053')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (19, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005EDE4 AS DateTime), CAST(0x0000A44C0005DEE3 AS DateTime), N'd55aaeb1-2aaf-47f3-a1b1-4e496ed8abb6')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (20, 27, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0005FA82 AS DateTime), CAST(0x0000A44C0005EDE4 AS DateTime), N'2d85952b-b2da-418f-a557-40fd6e264f15')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (21, 27, 1, CAST(0x0000A44C00000000 AS DateTime), NULL, CAST(0x0000A44C0005FA82 AS DateTime), N'47c428ba-ac14-4534-a215-36d22a2609af')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (22, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138A232 AS DateTime), CAST(0x0000A44C0138973D AS DateTime), N'e74d7c6a-bffe-4c90-b608-8f2f47a93998')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (23, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138AA80 AS DateTime), CAST(0x0000A44C0138A232 AS DateTime), N'a7450ea3-f51e-4d34-96a9-0c9c06ceb100')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (24, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138B1D9 AS DateTime), CAST(0x0000A44C0138AA80 AS DateTime), N'05999947-f316-4f91-90cc-0a84dcab357a')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (25, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138C015 AS DateTime), CAST(0x0000A44C0138B1D9 AS DateTime), N'38080ed1-b427-465f-9323-48bb73325ccf')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (26, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138C6E1 AS DateTime), CAST(0x0000A44C0138C015 AS DateTime), N'c453c984-1006-4285-aeca-7c94f07abcb6')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (27, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138D2DA AS DateTime), CAST(0x0000A44C0138C6E1 AS DateTime), N'06ea1c94-5144-439d-938d-789b3caf01d8')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (28, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138DD5E AS DateTime), CAST(0x0000A44C0138D2DA AS DateTime), N'e4f4d635-2c3f-463a-a6a3-89ab0772f0b8')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (29, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138E34B AS DateTime), CAST(0x0000A44C0138DD5E AS DateTime), N'8d3d937a-f9f1-45f5-8795-c2fe5688110b')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (30, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138F288 AS DateTime), CAST(0x0000A44C0138E34B AS DateTime), N'd829e77a-0907-439f-bdad-102fac85b46c')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (31, 28, 0, CAST(0x0000A44C00000000 AS DateTime), CAST(0x0000A44C0138F9B7 AS DateTime), CAST(0x0000A44C0138F288 AS DateTime), N'dc563273-7daf-4817-a819-254d75d8254f')
GO
INSERT [Institution].[SubjectSetupHeader] ([SubjectSetupID], [ClassID], [IsActive], [StartDate], [EndDate], [ModifiedDate], [rowguid]) VALUES (32, 28, 1, CAST(0x0000A44C00000000 AS DateTime), NULL, CAST(0x0000A44C0138F9B7 AS DateTime), N'19edc63c-d7c0-4aae-8b05-578b1cf8ad5f')
GO
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_BookID]  DEFAULT ([dbo].[GetNewID]('Institution.Book')) FOR [BookID]
GO
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[BookIssueDetail] ADD  CONSTRAINT [DF_BookIssueDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[BookIssueDetail] ADD  CONSTRAINT [DF_BookIssueDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[BookIssueHeader] ADD  CONSTRAINT [DF_BookIssueHeader_BookIssueID]  DEFAULT ([dbo].[GetNewID]('BookIssueHeader')) FOR [BookIssueID]
GO
ALTER TABLE [Institution].[BookIssueHeader] ADD  CONSTRAINT [DF_BookIssue_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[BookIssueHeader] ADD  CONSTRAINT [DF_BookIssue_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[BookReturnDetail] ADD  CONSTRAINT [DF_BookReturnDetail_BookReturnDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.BookReturnDetail')) FOR [BookReturnDetailID]
GO
ALTER TABLE [Institution].[BookReturnDetail] ADD  CONSTRAINT [DF_BookReturnDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[BookReturnDetail] ADD  CONSTRAINT [DF_BookReturnDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[BookReturnHeader] ADD  CONSTRAINT [DF_BookReturnHeader_BookReturnID]  DEFAULT ([dbo].[GetNewID]('Institution.BookReturnHeader')) FOR [BookReturnID]
GO
ALTER TABLE [Institution].[BookReturnHeader] ADD  CONSTRAINT [DF_BookReturnHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[BookReturnHeader] ADD  CONSTRAINT [DF_BookReturnHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Class] ADD  CONSTRAINT [DF_Class_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Class] ADD  CONSTRAINT [DF_Class_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ClassGroupDetail] ADD  CONSTRAINT [DF_ClassGroupDetail_ClassGroupDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.ClassGroupDetail')) FOR [ClassGroupDetailID]
GO
ALTER TABLE [Institution].[ClassGroupDetail] ADD  CONSTRAINT [DF_ClassGroupDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ClassGroupDetail] ADD  CONSTRAINT [DF_ClassGroupDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ClassGroupHeader] ADD  CONSTRAINT [DF_ClassGroup_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ClassGroupHeader] ADD  CONSTRAINT [DF_ClassGroup_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ClassSetupDetail] ADD  CONSTRAINT [DF_ClassSetupDetail_ClassSetupDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.ClassSetupDetail')) FOR [ClassSetupDetailID]
GO
ALTER TABLE [Institution].[ClassSetupDetail] ADD  CONSTRAINT [DF_ClassSetupDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ClassSetupDetail] ADD  CONSTRAINT [DF_ClassSetupDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ClassSetupHeader] ADD  CONSTRAINT [DF_ClassSetupHeader_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[ClassSetupHeader] ADD  CONSTRAINT [DF_SubjectSetup_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ClassSetupHeader] ADD  CONSTRAINT [DF_SubjectSetup_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Dormitory] ADD  CONSTRAINT [DF_Dormitory_DormitoryID]  DEFAULT ([dbo].[GetNewID]('Institution.Dormitory')) FOR [DormitoryID]
GO
ALTER TABLE [Institution].[Dormitory] ADD  CONSTRAINT [DF_Dormitory_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Dormitory] ADD  CONSTRAINT [DF_Dormitory_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[EmployeePayment] ADD  CONSTRAINT [DF_EmployeePayment_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[EmployeePayment] ADD  CONSTRAINT [DF_EmployeePayment_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Event] ADD  CONSTRAINT [DF_Event_EventID]  DEFAULT ([dbo].[GetNewID]('Institution.Event')) FOR [EventID]
GO
ALTER TABLE [Institution].[Event] ADD  CONSTRAINT [DF_Event_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Event] ADD  CONSTRAINT [DF_Event_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ExamDetail] ADD  CONSTRAINT [DF_ExamDetail_ExamDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.ExamDetail')) FOR [ExamDetailID]
GO
ALTER TABLE [Institution].[ExamDetail] ADD  CONSTRAINT [DF_ExamDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ExamDetail] ADD  CONSTRAINT [DF_ExamDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ExamHeader] ADD  CONSTRAINT [DF_ExamHeader_ExamDatetime]  DEFAULT (sysdatetime()) FOR [ExamDatetime]
GO
ALTER TABLE [Institution].[ExamHeader] ADD  CONSTRAINT [DF_ExamHeader_Modifieddate]  DEFAULT (sysdatetime()) FOR [Modifieddate]
GO
ALTER TABLE [Institution].[ExamHeader] ADD  CONSTRAINT [DF_ExamHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ExamResultDetail] ADD  CONSTRAINT [DF_ExamResultDetail_ExamResultDetail]  DEFAULT ([dbo].[GetNewID]('Institution.ExamResultDetail')) FOR [ExamResultDetail]
GO
ALTER TABLE [Institution].[ExamResultDetail] ADD  CONSTRAINT [DF_ExamResultDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ExamResultDetail] ADD  CONSTRAINT [DF_ExamResultDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[ExamResultHeader] ADD  CONSTRAINT [DF_ExamResultHeader_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[ExamResultHeader] ADD  CONSTRAINT [DF_ExamResultHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[ExamResultHeader] ADD  CONSTRAINT [DF_ExamResultHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[FeesPayment] ADD  CONSTRAINT [DF_FeesPayment_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[FeesPayment] ADD  CONSTRAINT [DF_FeesPayment_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[FeesStructureDetail] ADD  CONSTRAINT [DF_FeesStructureDetail_FeesStructureDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.FeesStructureDetail')) FOR [FeesStructureDetailID]
GO
ALTER TABLE [Institution].[FeesStructureDetail] ADD  CONSTRAINT [DF_FeesStructureDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[FeesStructureDetail] ADD  CONSTRAINT [DF_FeesStructureDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[FeesStructureHeader] ADD  CONSTRAINT [DF_FeesStructureHeader_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[FeesStructureHeader] ADD  CONSTRAINT [DF_FeesStructureHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[FeesStructureHeader] ADD  CONSTRAINT [DF_FeesStructureHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Gallery] ADD  CONSTRAINT [DF_Gallery_GalleryID]  DEFAULT ([dbo].[GetNewID]('Institution.Gallery')) FOR [GalleryID]
GO
ALTER TABLE [Institution].[Gallery] ADD  CONSTRAINT [DF_Gallery_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Gallery] ADD  CONSTRAINT [DF_Gallery_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[LeavingCertificate] ADD  CONSTRAINT [DF_LeavingCertificate_LeavingCertificateID]  DEFAULT ([dbo].[GetNewID]('Institution.LeavingCertificate')) FOR [LeavingCertificateID]
GO
ALTER TABLE [Institution].[LeavingCertificate] ADD  CONSTRAINT [DF_LeavingCertificate_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[LeavingCertificate] ADD  CONSTRAINT [DF_LeavingCertificate_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[PayoutDetail] ADD  CONSTRAINT [DF_PayoutDetail_PayoutDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.PayoutDetail')) FOR [PayoutDetailID]
GO
ALTER TABLE [Institution].[PayoutDetail] ADD  CONSTRAINT [DF_PayoutDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[PayoutDetail] ADD  CONSTRAINT [DF_PayoutDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[PayoutHeader] ADD  CONSTRAINT [DF_PayoutHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[PayoutHeader] ADD  CONSTRAINT [DF_PayoutHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Staff] ADD  CONSTRAINT [DF_Staff_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Staff] ADD  CONSTRAINT [DF_Staff_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[Student] ADD  CONSTRAINT [DF_Student_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[Student] ADD  CONSTRAINT [DF_Student_PreviousBalance]  DEFAULT ('0') FOR [PreviousBalance]
GO
ALTER TABLE [Institution].[Student] ADD  CONSTRAINT [DF_Student_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Student] ADD  CONSTRAINT [DF_Student_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[StudentClearance] ADD  CONSTRAINT [DF_StudentClearance_StudentClearanceID]  DEFAULT ([dbo].[GetNewID]('Institution.StudentClearance')) FOR [StudentClearanceID]
GO
ALTER TABLE [Institution].[StudentClearance] ADD  CONSTRAINT [DF_StudentClearance_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[StudentClearance] ADD  CONSTRAINT [DF_StudentClearance_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[StudentTranscriptHeader] ADD  CONSTRAINT [DF_StudentTranscriptHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[StudentTranscriptHeader] ADD  CONSTRAINT [DF_StudentTranscriptHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[StudentTransfer] ADD  CONSTRAINT [DF_StudentTransfer_StudentTransferID]  DEFAULT ([dbo].[GetNewID]('Institution.StudentTransfer')) FOR [StudentTransferID]
GO
ALTER TABLE [Institution].[StudentTransfer] ADD  CONSTRAINT [DF_StudentTransfer_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[StudentTransfer] ADD  CONSTRAINT [DF_StudentTransfer_riwguid]  DEFAULT (newid()) FOR [riwguid]
GO
ALTER TABLE [Institution].[Subject] ADD  CONSTRAINT [DF_Subject_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Subject] ADD  CONSTRAINT [DF_Subject_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[SubjectSetupDetail] ADD  CONSTRAINT [DF_SubjectSetupDetail_SubjectSetupDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.SubjectSetupDetail')) FOR [SubjectSetupDetailID]
GO
ALTER TABLE [Institution].[SubjectSetupDetail] ADD  CONSTRAINT [DF_SubjectSetupDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[SubjectSetupDetail] ADD  CONSTRAINT [DF_SubjectSetupDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[SubjectSetupHeader] ADD  CONSTRAINT [DF_SubjectSetupHeader_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[SubjectSetupHeader] ADD  CONSTRAINT [DF_SubjectSetupHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[SubjectSetupHeader] ADD  CONSTRAINT [DF_SubjectSetupHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[TimeTableDetail] ADD  CONSTRAINT [DF_TimeTableDetail_TimeTableDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.TimeTableDetail')) FOR [TimeTableDetailID]
GO
ALTER TABLE [Institution].[TimeTableDetail] ADD  CONSTRAINT [DF_TimeTableDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[TimeTableDetail] ADD  CONSTRAINT [DF_TimeTableDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[TimeTableHeader] ADD  CONSTRAINT [DF_TimeTableHeader_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[TimeTableHeader] ADD  CONSTRAINT [DF_TimeTableHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[TimeTableHeader] ADD  CONSTRAINT [DF_TimeTableHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[Item] ADD  CONSTRAINT [DF_Product_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[Item] ADD  CONSTRAINT [DF_Product_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[ItemCategory] ADD  CONSTRAINT [DF_ItemCategory_ItemCategoryID]  DEFAULT ([dbo].[GetNewID]('Sales.ItemCategory')) FOR [ItemCategoryID]
GO
ALTER TABLE [Sales].[ItemCategory] ADD  CONSTRAINT [DF_ProductCategory_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[ItemCategory] ADD  CONSTRAINT [DF_ProductCategory_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[ItemReceiptDetail] ADD  CONSTRAINT [DF_ItemReceiptDetail_ItemReceiptDetailID]  DEFAULT ([dbo].[GetNewID]('Sales.ItemreceiptDetail')) FOR [ItemReceiptDetailID]
GO
ALTER TABLE [Sales].[ItemReceiptDetail] ADD  CONSTRAINT [DF_PurchaseOrderDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[ItemReceiptDetail] ADD  CONSTRAINT [DF_PurchaseOrderDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[ItemReceiptHeader] ADD  CONSTRAINT [DF_ItemReceiptHeader_IsCancelled]  DEFAULT ((0)) FOR [IsCancelled]
GO
ALTER TABLE [Sales].[ItemReceiptHeader] ADD  CONSTRAINT [DF_PurchaseOrderHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[ItemReceiptHeader] ADD  CONSTRAINT [DF_PurchaseOrderHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[SaleDetail] ADD  CONSTRAINT [DF_SaleDetail_SalesOrderDetailID]  DEFAULT ([dbo].[GetNewID]('Sales.SaleDetail')) FOR [SalesOrderDetailID]
GO
ALTER TABLE [Sales].[SaleDetail] ADD  CONSTRAINT [DF_SalesOrderDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[SaleDetail] ADD  CONSTRAINT [DF_SalesOrderDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[SaleHeader] ADD  CONSTRAINT [DF_SaleHeader_PaymentID]  DEFAULT ((0)) FOR [PaymentID]
GO
ALTER TABLE [Sales].[SaleHeader] ADD  CONSTRAINT [DF_SalesOrderHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[SaleHeader] ADD  CONSTRAINT [DF_SalesOrderHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[StockTakingDetail] ADD  CONSTRAINT [DF_StockTakingDetail_StockTakingDetailID]  DEFAULT ([dbo].[GetNewID]('Sales.StockTakingDetail')) FOR [StockTakingDetailID]
GO
ALTER TABLE [Sales].[StockTakingDetail] ADD  CONSTRAINT [DF_StockTakingDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[StockTakingDetail] ADD  CONSTRAINT [DF_StockTakingDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[StockTakingHeader] ADD  CONSTRAINT [DF_StockTakingHeader_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[StockTakingHeader] ADD  CONSTRAINT [DF_StockTakingHeader_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[Supplier] ADD  CONSTRAINT [DF_Supplier_SupplierID]  DEFAULT ([dbo].[GetNewID]('Sales.Supplier')) FOR [SupplierID]
GO
ALTER TABLE [Sales].[Supplier] ADD  CONSTRAINT [DF_Vendor_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[Supplier] ADD  CONSTRAINT [DF_Vendor_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[SupplierDetail] ADD  CONSTRAINT [DF_SupplierDetail_SupplierDetailID]  DEFAULT ([dbo].[GetNewID]('Sales.SupplierDetail')) FOR [SupplierDetailID]
GO
ALTER TABLE [Sales].[SupplierDetail] ADD  CONSTRAINT [DF_SupplierDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[SupplierDetail] ADD  CONSTRAINT [DF_SupplierDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[SupplierPayment] ADD  CONSTRAINT [DF_SupplierPayment_SupplierPaymentID]  DEFAULT ([dbo].[GetNewID]('Sales.SupplierPayment')) FOR [SupplierPaymentID]
GO
ALTER TABLE [Sales].[SupplierPayment] ADD  CONSTRAINT [DF_SupplierPayment_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[SupplierPayment] ADD  CONSTRAINT [DF_SupplierPayment_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Sales].[Vat] ADD  CONSTRAINT [DF_Vat_VatID]  DEFAULT ([dbo].[GetNewID]('Sales.Vat')) FOR [VatID]
GO
ALTER TABLE [Sales].[Vat] ADD  CONSTRAINT [DF_Vat_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Sales].[Vat] ADD  CONSTRAINT [DF_Vat_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Users].[User] ADD  CONSTRAINT [DF_User_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Users].[User] ADD  CONSTRAINT [DF_User_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Users].[UserDetail] ADD  CONSTRAINT [DF_UserDetail_UserDetail]  DEFAULT ([dbo].[GetNewID]('Users.UserDetail')) FOR [UserDetailID]
GO
ALTER TABLE [Users].[UserDetail] ADD  CONSTRAINT [DF_UserDetail_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Users].[UserDetail] ADD  CONSTRAINT [DF_UserDetail_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Users].[UserRole] ADD  CONSTRAINT [DF_UserRole_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Users].[UserRole] ADD  CONSTRAINT [DF_UserRole_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Book_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Book')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_BookIssueDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookIssueDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_BookIssueHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookIssueHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_BookReturnDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookReturnDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_BookReturnHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.BookReturnHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Class_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Class')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ClassGroupHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ClassSetupDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassSetupDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ClassSetupHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ClassSetupHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ClassSetupHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Dormitory_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Dormitory')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_EmployeePayment_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.EmployeePayment')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Event_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Event')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ExamDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ExamHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamHeader')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ExamResultDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamResultDetail')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ExamResultHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_ExamResultHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.ExamResultHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_FeesPayment_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesPayment')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_FeesStructureDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesStructureDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_FeesStructureHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_FeesStructureHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.FeesStructureHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Gallery_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Gallery')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_LeavingCertificate_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.LeavingCertificate')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_PayoutDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayoutDetail')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_PayoutHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.PayoutHeader')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Staff_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Staff')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Student_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Student')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_StudentTranscriptHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTranscriptHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_Subject_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.Subject')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_SubjectSetupDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.SubjectSetupDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_SubjectSetupHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.SubjectSetupHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_TimeTableDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.TimeTableDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_TimeTableHeader_SetActive_EndDate]
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_TimeTableHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.TimeTableHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_Item_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Item')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_ItemCategory_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemCategory')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_ItemReceiptDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemReceiptDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_ItemReceiptHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.ItemReceiptHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_SaleDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SaleDetail')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_SaleHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SaleHeader')
    END
   END
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_StockTakingDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.StockTakingDetail')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_StockTakingHeader_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.StockTakingHeader')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_Supplier_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Supplier')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_SupplierPayment_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.SupplierPayment')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Sales].[TR_Vat_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Sales.Vat')
    END
   END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Users].[TR_UserDetail_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Users.UserDetail')
    END
   END
END
GO
USE [master]
GO
ALTER DATABASE [Starehe] SET  READ_WRITE 
GO

USE Starehe

exec dbo.ResetUniqueIds