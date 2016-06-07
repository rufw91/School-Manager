CREATE TABLE [Institution].[ProjectDetail] (
    [ProjectDetailID] INT              CONSTRAINT [DF_ProjectDetail_ProjectDetailID] DEFAULT ([dbo].[Link_GetNewID]('Institution.ProjectDetail')) NOT NULL,
    [ProjectID]       INT              NOT NULL,
    [Name]            VARCHAR (50)     NOT NULL,
    [Allocation]      DECIMAL (18)     NOT NULL,
    [StartDate]       DATETIME         NOT NULL,
    [EndDate]         DATETIME         NOT NULL,
    [ModifiedDate]    DATETIME         CONSTRAINT [DF_ProjectDetail_ModifiedDate] DEFAULT (sysdatetime()) NOT NULL,
    [rowguid]         UNIQUEIDENTIFIER CONSTRAINT [DF_ProjectDetail_rowguid] DEFAULT (newid()) NOT NULL,
    CONSTRAINT [PK_ProjectDetail] PRIMARY KEY CLUSTERED ([ProjectDetailID] ASC)
);

