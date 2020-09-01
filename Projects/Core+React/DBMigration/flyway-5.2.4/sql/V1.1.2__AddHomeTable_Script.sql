/****** Object:  Table [dbo].[[HomeContentPage]]    Script Date: 5/10/2019 10:01:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'HomeContentPage'))
BEGIN
	CREATE TABLE [dbo].[HomeContentPage](
		[HomeContentPageGuid] [uniqueidentifier] NOT NULL,
		[Section] [NVARCHAR](50) NULL,
		[Name] [NVARCHAR](255) NULL,
		[Title] [NVARCHAR](255) NULL,
		[BannerURL] [NVARCHAR](255) NULL,
		[ShortMessage] [NVARCHAR](500) NULL,
		[Description] [NVARCHAR](MAX) NULL,
		[Status] [NVARCHAR](50) NULL,
		[IsFeatured] [BIT] NULL,
		[ShowOnArchieve] [BIT] NULL,
		[Order] [INT],
		[IsDeleted] [BIT] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[CreatedOn] [DATETIME] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		[UpdatedOn] [DATETIME] NULL,
		CONSTRAINT [PK_HomeContentPage_HomeContentPageGuid] PRIMARY KEY CLUSTERED
		(
			[HomeContentPageGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[HomeContentPage] ADD  DEFAULT (newid()) FOR [HomeContentPageGuid]
END
GO


/****** Object:  Table [dbo].[UserApp]    Script Date: 5/10/2019 10:01:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UserApp'))
BEGIN
	CREATE TABLE [dbo].[UserApp](
		[UserAppGuid] [uniqueidentifier] NOT NULL,
		[UserGuid] [uniqueidentifier] NULL,
		[MenuGuid] [uniqueidentifier] NULL,
		[Order] [INT] NULL,
		CONSTRAINT [PK_UserApp_UserAppGuid] PRIMARY KEY CLUSTERED
		(
			[UserAppGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[UserApp] ADD  DEFAULT (newid()) FOR [UserAppGuid]
END
GO

/****** Object:  Table [dbo].[MoreToKnow]    Script Date: 5/10/2019 10:01:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'MoreToKnow'))
BEGIN
	CREATE TABLE [dbo].[MoreToKnow](
		[MoreToKnowGuid] [uniqueidentifier] NOT NULL,
		[Name] [NVARCHAR](255) NULL,
		[Title] [NVARCHAR](500) NULL,
		[Description] [NVARCHAR](MAX) NULL,
		[Status] [NVARCHAR](50) NULL,
		[IsDeleted] [BIT] NULL,
		CONSTRAINT [PK_MoreToKnow_MoreToKnowGuid] PRIMARY KEY CLUSTERED
		(
			[MoreToKnowGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[MoreToKnow] ADD  DEFAULT (newid()) FOR [MoreToKnowGuid]
END
GO
