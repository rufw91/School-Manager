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
GRANT ALTER TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[User] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[User] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Teacher] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Teacher] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[None] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[None] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Deputy] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Deputy] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT ALTER ON ROLE::[Accounts] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON ROLE::[Accounts] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
CREATE SCHEMA [Institution]
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
CREATE SCHEMA [Sales]
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
CREATE SCHEMA [Users]
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
GRANT EXECUTE ON [dbo].[ResetUniqueIDs] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[ResetUniqueIDs] TO [SystemAdmin] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetCurrentClass](@studentID int)
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

GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Principal] AS [dbo]
GO
GRANT CONTROL ON [dbo].[GetCurrentClass] TO [SystemAdmin] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentClass] TO [Teacher] AS [dbo]
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
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Accounts] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Deputy] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[GetCurrentQuantity] TO [Principal] WITH GRANT OPTION  AS [dbo]
GO
GRANT CONTROL ON [dbo].[GetCurrentQuantity] TO [SystemAdmin] AS [dbo]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetPurchaseTotal](@purchaseID int)
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetSaleTotal](@saleID int)
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
	[Author] [varchar](50) NOT NULL,
	[Publisher] [varchar](50) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
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
GRANT SELECT ON [Institution].[Book] TO [Accounts] AS [dbo]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookIssueDetail](
	[BookIssueDetailID] [int] NOT NULL,
	[BookIssueID] [int] NOT NULL,
	[BookID] [int] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookIssueDetail] PRIMARY KEY CLUSTERED 
(
	[BookIssueDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueDetail] TO [Teacher] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookIssueHeader](
	[BookIssueID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[DateIssued] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookIssue] PRIMARY KEY CLUSTERED 
(
	[BookIssueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookIssueHeader] TO [Teacher] AS [dbo]
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
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnDetail] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnDetail] TO [Teacher] AS [dbo]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[BookReturnHeader](
	[BookReturnID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[DateReturned] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BookReturnHeader] PRIMARY KEY CLUSTERED 
(
	[BookReturnID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[BookReturnHeader] TO [Teacher] AS [dbo]
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Institution].[CurrentClass](
	[CurrentClassID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CurrentClass] PRIMARY KEY CLUSTERED 
(
	[CurrentClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Accounts] AS [dbo]
GO
GRANT DELETE ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[CurrentClass] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[CurrentClass] TO [Teacher] AS [dbo]
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
GRANT DELETE ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[EmployeePayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[EmployeePayment] TO [Principal] AS [dbo]
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
GRANT SELECT ON [Institution].[Event] TO [Accounts] AS [dbo]
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
GRANT INSERT ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[Event] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[Event] TO [User] AS [dbo]
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
GRANT DELETE ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesPayment] TO [Accounts] AS [dbo]
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
GRANT DELETE ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureDetail] TO [Accounts] AS [dbo]
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
GRANT DELETE ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT SELECT ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
GO
GRANT UPDATE ON [Institution].[FeesStructureHeader] TO [Accounts] AS [dbo]
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
GRANT SELECT ON [Institution].[Gallery] TO [Accounts] AS [dbo]
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
GRANT SELECT ON [Institution].[LeavingCertificate] TO [Accounts] AS [dbo]
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
GRANT SELECT ON [Institution].[Staff] TO [Accounts] AS [dbo]
GO
GRANT INSERT ON [Institution].[Staff] TO [Deputy] AS [dbo]
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
	[ClassID]  AS ([dbo].[GetCurrentClass]([StudentID])),
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
	[DormitoryID] [int] NULL,
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
GRANT SELECT ON [Institution].[Student] TO [Accounts] AS [dbo]
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
GRANT SELECT ON [Institution].[StudentClearance] TO [Accounts] AS [dbo]
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
GRANT DELETE ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTranscriptHeader] TO [Principal] AS [dbo]
GO
GRANT INSERT ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTranscriptHeader] TO [Teacher] AS [dbo]
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
GRANT SELECT ON [Institution].[StudentTransfer] TO [Accounts] AS [dbo]
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
GRANT INSERT ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT SELECT ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
GO
GRANT UPDATE ON [Institution].[StudentTransfer] TO [Teacher] AS [dbo]
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
	[TotalAmt]  AS ([dbo].[GetPurchaseTotal]([ItemReceiptID])),
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
	[TotalAmt]  AS ([dbo].[GetSaleTotal]([SaleID])),
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
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [Users].[User](
	[UserID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[SPhoto] [varbinary](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
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
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_BookID]  DEFAULT ([dbo].[GetNewID]('Institution.Book')) FOR [BookID]
GO
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[Book] ADD  CONSTRAINT [DF_Book_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO
ALTER TABLE [Institution].[BookIssueDetail] ADD  CONSTRAINT [DF_BookIssueDetail_BookIssueDetailID]  DEFAULT ([dbo].[GetNewID]('Institution.BookIssueDetail')) FOR [BookIssueDetailID]
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
ALTER TABLE [Institution].[CurrentClass] ADD  CONSTRAINT [DF_CurrentClass_CurrentClassID]  DEFAULT ([dbo].[GetNewID]('Institution.CurrentClass')) FOR [CurrentClassID]
GO
ALTER TABLE [Institution].[CurrentClass] ADD  CONSTRAINT [DF_CurrentClass_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [Institution].[CurrentClass] ADD  CONSTRAINT [DF_CurrentClass_ModifiedDate]  DEFAULT (sysdatetime()) FOR [ModifiedDate]
GO
ALTER TABLE [Institution].[CurrentClass] ADD  CONSTRAINT [DF_CurrentClass_rowguid]  DEFAULT (newid()) FOR [rowguid]
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
CREATE TRIGGER [Institution].[TR_CurrentClass_SetActive_EndDate]
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

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [Institution].[TR_CurrentClass_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.CurrentClass')
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
CREATE TRIGGER [Institution].[TR_StudentClearance_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentClearance')
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
CREATE TRIGGER [Institution].[TR_StudentTransfer_UpdateID]
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
   UPDATE dbo.sysIDs SET last_id = last_id+1 where UPPER(table_name)=UPPER('Institution.StudentTransfer')
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
USE [Starehe]
EXEC [dbo].[ResetUniqueIDs]
