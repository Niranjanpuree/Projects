
--start of primary table creation
/****** Object:  Table [dbo].[Attributes]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF( NOT EXISTS (SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'Attributes'))
BEGIN
	CREATE TABLE [dbo].[Attributes](
		[AttributeMetaGuid] [uniqueidentifier] NOT NULL,
		[AttributeName] [nvarchar](255) NULL,
		[Resource] [nvarchar](255) NULL,
		[DataType] [nvarchar](255) NULL,
		[Max] [nvarchar](255) NULL,
		[Min] [nvarchar](50) NULL,
		[IsSystem] [bit] NULL,
		[IsAvailableForSearch] [int] NULL,
		[IsReadOnly] [nvarchar](50) NULL,
		[ValidationPayload] [nchar](10) NULL,
		CONSTRAINT [PK_Attributes_AttributeMetaGuid] PRIMARY KEY CLUSTERED
		(
			[AttributeMetaGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[Attributes] ADD  DEFAULT (newid()) FOR [AttributeMetaGuid]
END
GO



IF( NOT EXISTS (SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_NAME = 'EventLog'))
BEGIN
	CREATE TABLE [dbo].[EventLog](
		[EventGuid] [uniqueidentifier] NOT NULL,
		[Application] [nvarchar](255) NULL,
		[Resource] [nvarchar](255) NULL,
		[EventDate] DateTime NULL,
		[Action] [nvarchar](255) NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		[Message] varchar(max) NULL,
		[StackTrace] varchar(max) NULL,
		[InnerException] varchar(max) NULL,
		
		CONSTRAINT [PK_EventLog_EventGuid] PRIMARY KEY CLUSTERED
		(
			[EventGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[EventLog] ADD  DEFAULT (newid()) FOR [EventGuid]
END
GO

/****** Object:  Table [dbo].[ContractType] ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF(NOT EXISTS(SELECT *
--	FROM INFORMATION_SCHEMA.TABLES
--	WHERE TABLE_SCHEMA = 'dbo'
--	AND TABLE_NAME = 'ContractType'))
--BEGIN
--	CREATE TABLE [dbo].[ContractType](
--		[ContractTypeGuid] [uniqueidentifier] NOT NULL,
--		[ID] [nvarchar](50) NOT NULL,
--		[ContractType] [nvarchar](50) NOT NULL,
--		CONSTRAINT [PK_ContractType_ContractTypeGuid] PRIMARY KEY CLUSTERED
--		(
--			[ContractTypeGuid] ASC
--		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]

--	ALTER TABLE [dbo].[ContractType] ADD  DEFAULT (newid()) FOR [ContractTypeGuid]
--END
--GO


/****** Object:  Table [dbo].[FilePath]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'BaseFilePath'))
BEGIN
	CREATE TABLE [dbo].[BaseFilePath](
		FilePathId INT IDENTITY(1,1) NOT NULL,
		BasePath NVARCHAR(500),
		Code NVARCHAR(25),
		FileType NVARCHAR(50),
		Description NVARCHAR(MAX),
		CONSTRAINT [PK_BaseFilePath_FilePathId] PRIMARY KEY ([FilePathId])
	)
END

/****** Object:  Table [dbo].[Country]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Country'))
BEGIN
	CREATE TABLE [dbo].[Country](
		[CountryId] [uniqueidentifier] NOT NULL,
		[CountryName] [nvarchar](255) NOT NULL,
		[Alpha2Code] [nvarchar](50) NOT NULL,
		[Alpha3Code] [nvarchar](50) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[NumericCode] [int] NULL,
		[TLD] [nvarchar](50) NULL,
		[CallingCode] [nvarchar](50) NULL,
	 CONSTRAINT [PK_Country_CountryId] PRIMARY KEY CLUSTERED 
	(
		[CountryId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[Country] ADD  DEFAULT (newid()) FOR [CountryId]
END
GO


/****** Object:  Table [dbo].[QueryOperator]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'QueryOperator'))
BEGIN
	CREATE TABLE [dbo].[QueryOperator](
		[QueryOperatorGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[Type] [nvarchar](255) NULL,
	CONSTRAINT [PK_QueryOperator_QueryOperatorGuid] PRIMARY KEY CLUSTERED
	(
		[QueryOperatorGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[QueryOperator] ADD  DEFAULT (newid()) FOR [QueryOperatorGuid]
END
GO

/****** Object:  Table [dbo].[CustomerType] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'CustomerType'))
BEGIN
	CREATE TABLE [dbo].[CustomerType](
		[CustomerTypeGuid] [uniqueidentifier] NOT NULL,
		[CustomerTypeName] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_CustomerType_CustomerTypeGuid] PRIMARY KEY CLUSTERED 
	(
		[CustomerTypeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[CustomerType] ADD  DEFAULT (newid()) FOR [CustomerTypeGuid]
END
GO

/****** Object:  Table [dbo].[CustomerContactType]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'CustomerContactType'))
BEGIN
	CREATE TABLE [dbo].[CustomerContactType](
		[ContactTypeGuid] [uniqueidentifier] NOT NULL,
		[ContactTypeName] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NULL,
		[IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[ContactType] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_CustomerContactType_ContactTypeGuid] PRIMARY KEY CLUSTERED 
	(
		[ContactTypeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[CustomerContactType] ADD  DEFAULT (newid()) FOR [ContactTypeGuid]
END
GO

/****** Object:  Table [dbo].[NotificationType]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'NotificationType'))
BEGIN
	CREATE TABLE [dbo].[NotificationType](
		[NotificationTypeGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[NotificationTypeName] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_NotificationTypeGuid] PRIMARY KEY CLUSTERED 
	(
		[NotificationTypeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[NotificationType] ADD DEFAULT (newid()) FOR [NotificationTypeGuid]
END
GO

/****** Object:  Table [dbo].[EmailTemplate] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'EmailTemplate'))
BEGIN
	CREATE TABLE [dbo].[EmailTemplate](
		[EmailTemplateGuid] [uniqueidentifier] NOT NULL,
		[Keys] [nvarchar](255) NULL,
		[Subjects] [nvarchar](max) NULL,
		[Message] [nvarchar](max) NULL,
		[Status] [nvarchar](255) NULL,
		[IsActive] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
	CONSTRAINT [PK_EmailTemplate_EmailTemplateGuid] PRIMARY KEY CLUSTERED 
	(
		[EmailTemplateGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[EmailTemplate] ADD  DEFAULT (newid()) FOR [EmailTemplateGuid]
END
GO


/****** Object:  Table [dbo].[Namespace]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Namespace'))
BEGIN
	CREATE TABLE [dbo].[Namespace](
		[NamespaceGuid] [uniqueidentifier] NOT NULL,
		[Namespace] [nvarchar](255) NULL,
		[NamespaceType] [nvarchar](255) NULL,
		CONSTRAINT [PK_Namespace_NamespaceGuid] PRIMARY KEY CLUSTERED
		(
			[NamespaceGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[Namespace] ADD  DEFAULT (newid()) FOR [NamespaceGuid]
END
GO


/****** Object:  Table [dbo].[OrgID]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'OrgID'))
BEGIN
	CREATE TABLE [dbo].[OrgID](
		[OrgIDGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](MAX) NULL,
	 CONSTRAINT [PK_OrgID_OrgID] PRIMARY KEY CLUSTERED 
	(
		[OrgIDGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[OrgID] ADD  DEFAULT (newid()) FOR [OrgIDGuid]
END
GO

/****** Object:  Table [dbo].[PSC] *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'PSC'))
BEGIN
	CREATE TABLE [dbo].[PSC](
		[PSCGuid] [uniqueidentifier] NOT NULL,
		[CodeDescription] [nvarchar](max) NOT NULL,
		[Code] [nvarchar](50) NOT NULL,
		[Level1] [nvarchar](100) NULL,
		[Level1Category] [nvarchar](100) NULL,
		[Level2] [nvarchar](100) NULL,
		[Level2Category] [nvarchar](100) NULL,
	 CONSTRAINT [PK_PSC_PSCGuid] PRIMARY KEY CLUSTERED 
	(
		[PSCGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[PSC] ADD  DEFAULT (newid()) FOR [PSCGuid]
END
GO


/****** Object:  Table [dbo].[Policy] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Policy'))
BEGIN
	CREATE TABLE [dbo].[Policy](
		[PolicyGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[Description] [nvarchar](MAX) NULL,
		[PolicyJson] [nvarchar](max) NULL,
		CONSTRAINT [PK_Policy_PolicyGuid] PRIMARY KEY CLUSTERED
		(
			[PolicyGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[Policy] ADD  DEFAULT (newid()) FOR [PolicyGuid]
END
GO

/****** Object:  Table [dbo].[SyncBatch]    Script Date: 5/10/2019 10:00:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'SyncBatch'))
BEGIN
	CREATE TABLE [dbo].[SyncBatch](
		[SyncBatchGuid] [uniqueidentifier] NOT NULL,
		[BatchStart] [datetime] NOT NULL,
		[BatchEnd] [datetime] NULL,
	 CONSTRAINT [PK_SyncBatch_SyncBatchGuid] PRIMARY KEY CLUSTERED 
	(
		[SyncBatchGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[SyncBatch] ADD  DEFAULT (newid()) FOR [SyncBatchGuid]
END
GO


/****** Object:  Table [dbo].[UserInterfaceMenu]    Script Date: 5/10/2019 10:01:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UserInterfaceMenu'))
BEGIN
	CREATE TABLE [dbo].[UserInterfaceMenu](
		[MenuGuid] [uniqueidentifier] NOT NULL,
		[MenuNamespace] [nvarchar](50) NULL,
		[MenuText] [nvarchar](255) NULL,
		[MenuUrl] [nvarchar](255) NULL,
		[MenuDescription] [nvarchar](255) NULL,
		[ParentMenuNamespace] [nvarchar](50) NULL,
		[MenuClass] [nvarchar](255) NULL,
		[IsDefault] [BIT] NULL,
		[ImageURL] [NVARCHAR](500) NULL,
		[Resource] [NVARCHAR](50) NULL,
		[ResourceAction] [NVARCHAR](50) NULL,
		CONSTRAINT [PK_UserInterfaceMenu_MenuGuid] PRIMARY KEY CLUSTERED
		(
			[MenuGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[UserInterfaceMenu] ADD  DEFAULT (newid()) FOR [MenuGuid]
END
GO


/****** Object:  Table [dbo].[DistributionList] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'DistributionList'))
BEGIN
	CREATE TABLE [dbo].[DistributionList](
		[DistributionListGuid] [uniqueidentifier] NOT NULL,
	    [Name] [nvarchar](255) NOT NULL,
		[Title] [nvarchar](255) NOT NULL,
        [IsPublic] [bit] NULL,
		[IsActive] [bit] NULL,
        [IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
        [UpdatedOn] [datetime] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,

	CONSTRAINT [PK_DistributionList_DistributionListGuid] PRIMARY KEY CLUSTERED 
	(
		[DistributionListGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[DistributionList] ADD  DEFAULT (newid()) FOR [DistributionListGuid]
END
GO

/****** Object:  Table [dbo].[Customer] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Resources] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Resources'))
BEGIN
	CREATE TABLE [dbo].[Resources](
		[ResourceGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[Description] [nvarchar](MAX) NULL,
	CONSTRAINT [PK_Resources_ResourceGuid] PRIMARY KEY CLUSTERED
	(
		[ResourceGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Resources] ADD  DEFAULT (newid()) FOR [ResourceGuid]
END
GO

/****** Object:  Table [dbo].[NAICS] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'NAICS'))
BEGIN
	CREATE TABLE [dbo].[NAICS](
		[NAICSGuid] [uniqueidentifier] NOT NULL,
		[Code] [nvarchar](50) NOT NULL,
		[Title] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_NAICS_NAICSGuid] PRIMARY KEY CLUSTERED 
	(
		[NAICSGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[NAICS] ADD  DEFAULT (newid()) FOR [NAICSGuid]
END
GO

/****** Object:  Table [dbo].[SetAsideCode] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'SetAsideCode'))
BEGIN
	CREATE TABLE [dbo].[SetAsideCode](
		[SetAsideGuid] [uniqueidentifier] NOT NULL,
		[Code] [nvarchar](50) NOT NULL,
		[Abbreviation] [nvarchar](50) NOT NULL,
		CONSTRAINT [PK_SetAsideCode_SetAsideGuid] PRIMARY KEY CLUSTERED
		(
			[SetAsideGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[SetAsideCode] ADD  DEFAULT (newid()) FOR [SetAsideGuid]
END 
GO

/****** Object:  Table [dbo].[Group] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Group'))
BEGIN
	CREATE TABLE [dbo].[Group](
		[GroupGuid] [uniqueidentifier] NOT NULL,
		[ParentGuid] [uniqueidentifier] NULL,
		[GroupName] [nvarchar](255) NOT NULL,
		[CN] [nvarchar](255) NOT NULL,
		[Description] [nvarchar](max) NULL,
	 CONSTRAINT [PK_Group_GroupGuid] PRIMARY KEY CLUSTERED 
	(
		[GroupGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[Group] ADD  DEFAULT (newid()) FOR [GroupGuid]
END
GO

/****** Object:  Table [dbo].[GroupPermission] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'GroupPermission'))
BEGIN
	CREATE TABLE [dbo].[GroupPermission](
		[GroupPermissionGuid] [uniqueidentifier] NOT NULL,
		[GroupGuid] [uniqueidentifier] NULL,
		[ResourceGuid] [uniqueidentifier] NULL,
		[ResourceActionGuid] [uniqueidentifier] NULL
	 CONSTRAINT [PK_GroupPermission_GroupPermissionGuid] PRIMARY KEY CLUSTERED 
	(
		[GroupPermissionGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[GroupPermission] ADD  DEFAULT (newid()) FOR [GroupPermissionGuid]
END
GO

--IF(NOT EXISTS(SELECT *
--	FROM INFORMATION_SCHEMA.TABLES
--	WHERE TABLE_SCHEMA = 'dbo'
--	AND TABLE_NAME = 'ContractUserRole'))
--BEGIN
--	CREATE TABLE [dbo].ContractUserRole(
--	[ContractUserRoleGuid] [uniqueidentifier] NOT NULL,
--	[Name] NVARCHAR(255),
--	[Code] NVARCHAR(255) UNIQUE,
--	[IsActive] BIT DEFAULT 1,
--	CONSTRAINT [PK_ContractUserRole_ContractUserRoleGuid] PRIMARY KEY CLUSTERED
--	(
--		[ContractUserRoleGuid] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	)
--END


--End OR CREATING PRIMARY TABLE

/****** Object:  Table [dbo].[SyncStatus] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'SyncStatus'))
BEGIN
	CREATE TABLE [dbo].[SyncStatus](
		[SyncGuid] [uniqueidentifier] NOT NULL,
		[SyncBatchGuid] [uniqueidentifier] NOT NULL,
		[SyncStatusText] [nvarchar](150) NOT NULL,
		[ErrorMessage] [nvarchar](max) NULL,
		[ObjectType] [nvarchar](250) NOT NULL,
		[ObjectName] [nvarchar](250) NOT NULL,
		[ObjectGuid] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_SyncStatus_SyncGuid] PRIMARY KEY CLUSTERED 
	(
		[SyncGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[SyncStatus]  WITH CHECK ADD  CONSTRAINT [FK_SyncStatus_SyncBatchGuid] FOREIGN KEY([SyncBatchGuid])
	REFERENCES [dbo].[SyncBatch] ([SyncBatchGuid])

	ALTER TABLE [dbo].[SyncStatus] ADD  DEFAULT (newid()) FOR [SyncGuid]
END
GO

/****** Object:  Table [dbo].[ResourceActions] ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--IF(NOT EXISTS (SELECT *
--	FROM INFORMATION_SCHEMA.TABLES
--	WHERE TABLE_SCHEMA = 'dbo'
--	AND TABLE_NAME = 'ResourceActions'))
--BEGIN
--	CREATE TABLE [dbo].[ResourceActions](
--		[ActionGuid] [uniqueidentifier] NOT NULL,
--		[ResourceGuid] [uniqueidentifier] NULL,
--		[Name] [nvarchar](255) NULL,
--		[Title] [nvarchar](255) NULL,
--		[Description] [nvarchar](255) NULL,
--		[ActionType] [nvarchar](25) NULL,
--		CONSTRAINT [PK_ResourceActions_ActionGuid] PRIMARY KEY CLUSTERED
--		(
--			[ActionGuid] ASC
--		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY]

--	ALTER TABLE [dbo].[ResourceActions] ADD  DEFAULT (newid()) FOR [ActionGuid]

--	ALTER TABLE [dbo].[ResourceActions]  WITH CHECK ADD  CONSTRAINT [FK_ResourceActions_ResourceGuid] FOREIGN KEY([ResourceGuid])
--	REFERENCES [dbo].[Resources] ([ResourceGuid])

--	ALTER TABLE [dbo].[ResourceActions] CHECK CONSTRAINT [FK_ResourceActions_ResourceGuid]
--END
--GO


/****** Object:  Table [dbo].[ResourceAttribute] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ResourceAttribute'))
BEGIN
	CREATE TABLE [dbo].[ResourceAttribute](
		[ResourceAttributeGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[ResourceType] [nvarchar](255) NULL,
		[AttributeType] [nvarchar](255) NULL,
		[VisibleToGrid] [bit] NULL,
		[Exportable] [bit] NULL,
		[DefaultSortField] [bit] NULL,
		[GridFieldOrder] [int] NULL,
		[GridColumnCss] [nvarchar](50) NULL,
		[GridColumnFormat] [nvarchar](255) NULL,
		[Searchable] [bit] NULL,
		[IsEntityLookup] [bit] NULL,
		[Entity] [nvarchar](255) Null,
		[ColumnWidth] [int] NULL,
		[ColumnMinimumWidth] [int] NULL,
		[HelpText] [nvarchar](500) NULL,
	 CONSTRAINT [PK_ResourceAttribute_ResourceAttributeGuid] PRIMARY KEY CLUSTERED 
	(
		[ResourceAttributeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ResourceAttribute] ADD  DEFAULT (newid()) FOR [ResourceAttributeGuid]

	ALTER TABLE [dbo].[ResourceAttribute] ADD   DEFAULT ((0)) FOR [VisibleToGrid]

	ALTER TABLE [dbo].[ResourceAttribute] ADD  DEFAULT ((0)) FOR [Exportable]
END


SET QUOTED_IDENTIFIER ON
GO

IF( NOT EXISTS (SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'RecentActivity'))
BEGIN
	CREATE TABLE RecentActivity(
		RecentActivityGuid [uniqueidentifier] NOT NULL,
		Entity NVARCHAR(255) NULL,
		EntityGuid [uniqueidentifier] NOT NULL,
		UserGuid [uniqueidentifier],
		UserAction nvarchar(50),
		CreatedBy [uniqueidentifier] NOT NULL,
		CreatedOn Datetime NOT NULL,
		UpdatedBy [uniqueidentifier],
		UpdatedOn Datetime,
		IsDeleted BIT DEFAULT 0,
		CONSTRAINT [PK_RecentActivity_RecentActivityGuid] PRIMARY KEY CLUSTERED
		(
			[RecentActivityGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		ALTER TABLE [dbo].[RecentActivity] ADD  DEFAULT (newid()) FOR [RecentActivityGuid]
END

/****** Object:  Table [dbo].[Users]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Users'))
BEGIN
	CREATE TABLE [dbo].[Users](
		[UserGuid] [uniqueidentifier] NOT NULL,
		[Username] [nvarchar](255) NULL,
		[Firstname] [nvarchar](255) NULL,
		[Lastname] [nvarchar](255) NULL,
		[Givenname] [nvarchar](255) NULL,
		[Displayname] [nvarchar](255) NULL,
		[UserStatus] [nvarchar](50) NULL,
		[WorkEmail] [nvarchar](255) NULL,
		[PersonalEmail] [nvarchar](255) NULL,
		[WorkPhone] [nvarchar](255) NULL,
		[HomePhone] [nvarchar](255) NULL,
		[MobilePhone] [nvarchar](255) NULL,
		[JobStatus] [nvarchar](255) NULL,
		[JobTitle] [nvarchar](255) NULL,
		[Company] [nvarchar](255) NULL,
		[Department] [nvarchar](255) NULL,
		[Extension] [nvarchar](255) NULL,
	 CONSTRAINT [PK_Users_UserGuid] PRIMARY KEY CLUSTERED 
	(
		[UserGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [UserGuid]
END
GO

/****** Object:  Table [dbo].[NotificationTemplate] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'NotificationTemplate'))
BEGIN
	CREATE TABLE [dbo].[NotificationTemplate](
		[NotificationTemplateGuid] [uniqueidentifier] NOT NULL,
		[Keys] [nvarchar](255) NULL,
		[NotificationTypeGuid] [uniqueidentifier] NOT NULL,
		[Subject] [nvarchar](max) NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[IsActive] [bit] NULL,
		[Priority] [bit] NULL,
		[IsRecurring] [bit] NULL,
		[RecurringInterval] [int] NULL,
		[UserInteraction] [bit] NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_NotificationTemplateGuid] PRIMARY KEY CLUSTERED 
	(
		[NotificationTemplateGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[NotificationTemplate] ADD  DEFAULT (newid()) FOR [NotificationTemplateGuid]

	ALTER TABLE [dbo].[NotificationTemplate] WITH CHECK ADD CONSTRAINT [FK_NotificationTemplate_NotificationTypeGuid] FOREIGN KEY ([NotificationTypeGuid])
	REFERENCES [dbo].[NotificationType]([NotificationTypeGuid])
END
GO

/****** Object:  Table [dbo].[NotificationBatch] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * FROM 
	INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'NotificationBatch'))
BEGIN
	CREATE TABLE [dbo].[NotificationBatch](
		[NotificationBatchGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[NotificationTemplateGuid] [uniqueidentifier] NOT NULL,
		[ResourceId] [uniqueidentifier] NULL,
		[ResourceType] [nvarchar](255) NULL,
		[ResourceAction] [nvarchar](255) NOT NULL,
		[StartDate] [datetime] NOT NULL,
		[AdditionalMessage] [nvarchar](max) NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_NotificationBatchGuid] PRIMARY KEY CLUSTERED 
	(
		[NotificationBatchGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[NotificationBatch] ADD  DEFAULT (newid()) FOR [NotificationBatchGuid]

	ALTER TABLE [dbo].[NotificationBatch]  WITH CHECK ADD  CONSTRAINT [FK_NotificationBatch_NotificationTemplateGuid] FOREIGN KEY([NotificationTemplateGuid])
	REFERENCES [dbo].[NotificationTemplate] ([NotificationTemplateGuid])

	ALTER TABLE [dbo].[NotificationBatch] CHECK CONSTRAINT [FK_NotificationBatch_NotificationTemplateGuid]
END
GO

/****** Object:  Table [dbo].[NotificationMessage]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * FROM
	INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'NotificationMessage'))
BEGIN
	CREATE TABLE [dbo].[NotificationMessage](
		[NotificationMessageGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[NotificationBatchGuid] [uniqueidentifier] NOT NULL,
		[DistributionListGuid] [uniqueidentifier] NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		[Subject] [nvarchar](max) NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[AdditionalMessage] [nvarchar](max) NULL,
		[Status] [bit] NULL,
		[UserResponse] [bit] NULL,
		[NextAction] [datetime] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK__UserNoti__7271E2B6C8E7B7D7] PRIMARY KEY CLUSTERED 
	(
		[NotificationMessageGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[NotificationMessage] ADD DEFAULT (newid()) FOR [NotificationMessageGuid]

	ALTER TABLE [dbo].[NotificationMessage]  WITH CHECK ADD  CONSTRAINT [FK_NotificationMessage_UserGuid] FOREIGN KEY([UserGuid])
	REFERENCES [dbo].[Users] ([UserGuid])

	ALTER TABLE [dbo].[NotificationMessage] CHECK CONSTRAINT [FK_NotificationMessage_UserGuid]
	
	ALTER TABLE [dbo].[NotificationMessage]  WITH CHECK ADD  CONSTRAINT [FK_NotificationMessage_NotificationBatchGuid] FOREIGN KEY([NotificationBatchGuid])
	REFERENCES [dbo].[NotificationBatch] ([NotificationBatchGuid])
	ALTER TABLE [dbo].[NotificationMessage] CHECK CONSTRAINT [FK_NotificationMessage_NotificationBatchGuid]
END
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Customer'))
BEGIN
	CREATE TABLE [dbo].[Customer](
	[CustomerGuid] [uniqueidentifier] NOT NULL,
	[CustomerName] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[UpdatedBy] [uniqueidentifier] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[Address] [nvarchar](255) NULL,
	[AddressLine1] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[StateId] [uniqueidentifier] NULL,
	[ZipCode] [nvarchar](255) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[CustomerTypeGuid] [uniqueidentifier] NULL,
	[CustomerDescription] [nvarchar](max) NULL,
	[PrimaryPhone] [nvarchar](50) NULL,
	[PrimaryEmail] [nvarchar](255) NULL,
	[Abbreviations] [nvarchar](50) NULL,
	[Tags] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[CustomerCode] [nvarchar](50) NULL,
	[Department] [nvarchar](255) NULL,
	[Agency] [nvarchar](255) NULL,
	CONSTRAINT [PK_Customer_CustomerGuid] PRIMARY KEY CLUSTERED
	(
		[CustomerGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Customer] ADD DEFAULT (newid()) FOR [CustomerGuid]
END
GO


/****** Object:  Table [dbo].[CustomerContact]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS (SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'CustomerContact'))
BEGIN
	CREATE TABLE [dbo].[CustomerContact](
		[ContactGuid] [uniqueidentifier] NOT NULL,
		[FirstName] [nvarchar](255) NOT NULL,
		[MiddleName] [nvarchar](255) NULL,
		[LastName] [nvarchar](255) NOT NULL,
		[Gender] [nvarchar](50) NULL,
		[ContactTypeGuid] [uniqueidentifier] NOT NULL,
		[JobTitle] [nvarchar](255) NULL,
		[PhoneNumber] [nvarchar](255) NOT NULL,
		[AltPhoneNumber] [nvarchar](255) NULL,
		[EmailAddress] [nvarchar](255) NULL,
		[AltEmailAddress] [nvarchar](255) NULL,
		[Notes] [nvarchar](255) NULL,
		[IsActive] [bit] NULL,
		[IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[CustomerGuid] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_CustomerContact_ContactGuid] PRIMARY KEY CLUSTERED 
	(
		[ContactGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[CustomerContact] ADD DEFAULT (newid()) FOR [ContactGuid]
END
GO

/****** Object:  Table [dbo].[Company]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Company'))
BEGIN
	CREATE TABLE [dbo].[Company](
		[CompanyGuid] [uniqueidentifier] NOT NULL,
		[CompanyName] [nvarchar](255) NOT NULL,
		[CompanyCode] [nvarchar](50) NOT NULL,
		[President] [uniqueidentifier] NOT NULL,
		[Abbreviation] [nvarchar](50) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Company_CompanyGuid] PRIMARY KEY CLUSTERED 
	(
		[CompanyGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Company] ADD DEFAULT (newid()) FOR [CompanyGuid]

	ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_President] FOREIGN KEY([President])
	REFERENCES [dbo].[Users] ([UserGuid])
END
GO

/****** Object:  Table [dbo].[DistributionUser] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'DistributionUser'))
BEGIN
	CREATE TABLE [dbo].[DistributionUser](
		[DistributionUserGuid] [uniqueidentifier] NOT NULL,
		[DistributionListGuid] [uniqueidentifier] NOT NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
	CONSTRAINT [PK_DistributionUser_DistributionUserGuid] PRIMARY KEY CLUSTERED 
	(
		[DistributionUserGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[DistributionUser] ADD  DEFAULT (newid()) FOR [DistributionUserGuid]

	ALTER TABLE [dbo].[DistributionUser]  WITH CHECK ADD CONSTRAINT [FK_DistributionUser_DistributionListGuid] FOREIGN KEY([DistributionListGuid])
	REFERENCES [dbo].[DistributionList] ([DistributionListGuid])
	
	ALTER TABLE [dbo].[DistributionUser]  WITH CHECK ADD CONSTRAINT [FK_DistributionUser_UserGuid] FOREIGN KEY([UserGuid])
	REFERENCES [dbo].[Users] ([UserGuid])
END
GO

/****** Object:  Table [dbo].[GroupUser] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'GroupUser'))
BEGIN
	CREATE TABLE [dbo].[GroupUser](
		[GroupUserGuid] [uniqueidentifier] NOT NULL,
		[GroupGuid] [uniqueidentifier] NOT NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		CONSTRAINT [PK_GroupUser_GroupUserGuid] PRIMARY KEY CLUSTERED
		(
			[GroupUserGuid]
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[GroupUser] ADD  DEFAULT (newid()) FOR [GroupUserGuid]

	ALTER TABLE [dbo].[GroupUser] WITH CHECK ADD CONSTRAINT [FK_GroupUser_GroupGuid] FOREIGN KEY ([GroupGuid])
	REFERENCES [dbo].[Group]([GroupGuid])
END
GO

/****** Object:  Table [dbo].[GroupPolicy] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'GroupPolicy'))
BEGIN
	CREATE TABLE [dbo].[GroupPolicy](
		[GroupPolicyGuid] [uniqueidentifier] NOT NULL,
		[GroupGuid] [uniqueidentifier] NOT NULL,
		[PolicyGuid] [uniqueidentifier] NOT NULL,
		CONSTRAINT [PK_GroupPolicy_GroupPolicyGuid] PRIMARY KEY CLUSTERED
		(
			[GroupPolicyGuid]
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[GroupPolicy] ADD  DEFAULT (newid()) FOR [GroupPolicyGuid]
END
GO

/****** Object:  Table [dbo].[ManagerUser] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ManagerUser'))
BEGIN
	CREATE TABLE [dbo].[ManagerUser](
		[ManagerUserGuid] [uniqueidentifier] NOT NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		[ManagerGuid] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_ManagerUsers_ManagerUserGuid] PRIMARY KEY CLUSTERED 
	(
		[ManagerUserGUID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ManagerUser] ADD  DEFAULT (newid()) FOR [ManagerUserGuid]

	ALTER TABLE [dbo].[ManagerUser] WITH CHECK ADD CONSTRAINT [FK_ManagerUser_UserGuid] FOREIGN KEY ([UserGuid])
	REFERENCES [dbo].[Users]([UserGuid])

	ALTER TABLE [dbo].[ManagerUser] WITH CHECK ADD CONSTRAINT [FK_ManagerUser_ManagerGuid] FOREIGN KEY ([ManagerGuid])
	REFERENCES [dbo].[Users]([UserGuid])

END
GO

/****** Object:  Table [dbo].[Office] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Office'))
BEGIN
	CREATE TABLE [dbo].[Office](
		[OfficeGuid] [uniqueidentifier] NOT NULL,
		[OfficeCode] [nvarchar](50) NOT NULL,
		[OfficeName] [nvarchar](255) NOT NULL,
		[PhysicalAddress] [nvarchar](255) NULL,
		[MailingAddress] [nvarchar](255) NULL,
		[Phone] [nvarchar](50) NULL,
		[Fax] [nvarchar](50) NULL,
		[IsActive] [bit] NULL,
		[IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		[MailingZipCode] [nvarchar](255) NULL,
		[MailingStateId] [uniqueidentifier] NULL,
		[MailingCountryId] [uniqueidentifier] NULL,
		[MailingCity] [nvarchar](255) NULL,
		[PhysicalZipCode] [nvarchar](255) NULL,
		[PhysicalStateId] [uniqueidentifier] NULL,
		[PhysicalCountryId] [uniqueidentifier] NULL,
		[PhysicalCity] [nvarchar](255) NULL,
		[PhysicalAddressLine1] [nvarchar](255) NULL,
		[MailingAddressLine1] [nvarchar](255) NULL,
		[OperationManagerGuid] [uniqueidentifier] NULL
	 CONSTRAINT [PK_Odffice_OfficeGuid] PRIMARY KEY CLUSTERED 
	(
		[OfficeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Office] ADD  DEFAULT (newid()) FOR [OfficeGuid]
END
GO


/****** Object:  Table [dbo].[ResourceAttribute] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ResourceAttribute'))
BEGIN
	CREATE TABLE [dbo].[ResourceAttribute](
		[ResourceAttributeGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[ResourceType] [nvarchar](255) NULL,
		[AttributeType] [nvarchar](255) NULL,
		[VisibleToGrid] [bit] NULL,
		[Exportable] [bit] NULL,
		[DefaultSortField] [bit] NULL DEFAULT 0,
		[GridFieldOrder] [int] NULL DEFAULT 0,
		[GridColumnCss] [NVARCHAR](50) NULL,
		[GridColumnFormat] [NVARCHAR](255) NULL,
		[Searchable] [bit] null,
		[IsEntityLookup] [bit] null,
		[Entity] [NVARCHAR] (255) null
	 CONSTRAINT [PK_ResourceAttributeGuid] PRIMARY KEY CLUSTERED 
	(
		[ResourceAttributeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ResourceAttribute] ADD  DEFAULT (newid()) FOR [ResourceAttributeGuid]
END
GO

/****** Object:  Table [dbo].[ResourceAttributeValue] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ResourceAttributeValue'))
BEGIN
	CREATE TABLE [dbo].[ResourceAttributeValue](
		[ResourceAttributeValueGuid] [uniqueidentifier] NOT NULL,
		[ResourceAttributeGuid] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](255) NOT NULL,
		[Value] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_ResourceAttributeValueGuid] PRIMARY KEY CLUSTERED 
	(
		[ResourceAttributeValueGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ResourceAttributeValue] ADD  DEFAULT (newid()) FOR [ResourceAttributeValueGuid]
	
	ALTER TABLE [dbo].[ResourceAttributeValue]  WITH CHECK ADD  CONSTRAINT [FK_ResourceAttributeValue_ResourceAttributeGuid] FOREIGN KEY([ResourceAttributeGuid])
	REFERENCES [dbo].[ResourceAttribute] ([ResourceAttributeGuid])

	ALTER TABLE [dbo].[ResourceAttributeValue] CHECK CONSTRAINT [FK_ResourceAttributeValue_ResourceAttributeGuid]
END
GO

/****** Object:  Table [dbo].[ResourceActions] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ResourceActions'))
BEGIN
	CREATE TABLE [dbo].[ResourceActions](
		[ActionGuid] [uniqueidentifier] NOT NULL,
		--[ResourceActionGuid] [uniqueidentifier] NOT NULL,
		[ResourceGuid] [uniqueidentifier] NULL,
		[Name] [nvarchar](255) NULL,
		[Title] [nvarchar](255) NULL,
		[Description] [nvarchar](MAX) NULL,
		[ActionType] [nvarchar](25)  NULL,
		[IsAuditableAction] [bit] NULL,
		[ResourceAction] [nvarchar](255) NULL,
		CONSTRAINT [PK_ResourceAction_ActionGuid] PRIMARY KEY 
		(
			[ActionGuid] ASC
		)
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ResourceActions] ADD  DEFAULT (newid()) FOR [ActionGuid]

	ALTER TABLE [dbo].[ResourceActions]  WITH CHECK ADD  CONSTRAINT [FK_ResourceActions_ResourceGuid] FOREIGN KEY([ResourceGuid])
	REFERENCES [dbo].[Resources] ([ResourceGuid])

	ALTER TABLE [dbo].[ResourceActions] CHECK CONSTRAINT [FK_ResourceActions_ResourceGuid]
END
GO

/****** Object:  Table [dbo].[OfficeContact]  ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO
--IF(NOT EXISTS(SELECT *
--	FROM INFORMATION_SCHEMA.TABLES
--	WHERE TABLE_SCHEMA = 'dbo'
--	AND TABLE_NAME = 'OfficeContact'))
--BEGIN
--	CREATE TABLE [dbo].[OfficeContact](
--		[ContactGuid] [uniqueidentifier] NOT NULL,
--		[FirstName] [nvarchar](255) NOT NULL,
--		[MiddleName] [nvarchar](255) NULL,
--		[LastName] [nvarchar](255) NOT NULL,
--		[ContactType] [uniqueidentifier] NULL,
--		[PhoneNumber] [nvarchar](255) NOT NULL,
--		[AltPhoneNumber] [nvarchar](255) NULL,
--		[EmailAddress] [nvarchar](255) NULL,
--		[AltEmailAddress] [nvarchar](255) NULL,
--		[Description] [nvarchar](255) NULL,
--		[IsActive] [bit] NULL,
--		[IsDeleted] [bit] NULL,
--		[CreatedOn] [datetime] NOT NULL,
--		[CreatedBy] [uniqueidentifier] NOT NULL,
--		[UpdatedOn] [datetime] NOT NULL,
--		[UpdatedBy] [uniqueidentifier] NOT NULL,
--		[OfficeGuid] [uniqueidentifier] NOT NULL,
--		[Address] [nvarchar](max) NULL,
--	 CONSTRAINT [PK_Office_ContactGuid] PRIMARY KEY CLUSTERED 
--	(
--		[ContactGuid] ASC
--	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
--END
--GO


/****** Object:  Table [dbo].[Region]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Region'))
BEGIN
	CREATE TABLE [dbo].[Region](
		[RegionGuid] [uniqueidentifier] NOT NULL,
		[RegionName] [nvarchar](255) NOT NULL,
		[RegionCode] [nvarchar](50) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_Region_RegionGuid] PRIMARY KEY CLUSTERED 
	(
		[RegionGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Region] ADD  DEFAULT (newid()) FOR [RegionGuid]
END
GO

/****** Object:  Table [dbo].[State] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'State'))
BEGIN
	CREATE TABLE [dbo].[State](
		[StateId] [uniqueidentifier] NOT NULL,
		[CountryId] [uniqueidentifier] NOT NULL,
		[StateName] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[Abbreviation] [nvarchar](10) NOT NULL,
		[GRT] [bit] NOT NULL,
	 CONSTRAINT [PK_State_StateId] PRIMARY KEY CLUSTERED 
	(
		[StateId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[State] ADD  DEFAULT (newid()) FOR [StateId]

	ALTER TABLE [dbo].[State]  WITH CHECK ADD  CONSTRAINT [FK_State_CountryId] FOREIGN KEY([CountryId])
	REFERENCES [dbo].[Country] ([CountryId])
	ALTER TABLE [dbo].[State] CHECK CONSTRAINT [FK_State_CountryId]
END
GO

/****** Object:  Table [dbo].[UsCustomerOfficeList] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UsCustomerOfficeList'))
BEGIN
	CREATE TABLE [dbo].[UsCustomerOfficeList](
		[Id] [uniqueidentifier] NOT NULL,
		[DepartmentId] [nvarchar](max) NULL,
		[DepartmentName] [nvarchar](max) NULL,
		[CustomerCode] [nvarchar](max) NULL,
		[CustomerName] [nvarchar](max) NULL,
		[ContractingOfficeCode] [nvarchar](max) NULL,
		[ContractingOfficeName] [nvarchar](max) NULL,
		[StartDate] [datetime] NULL,
		[EndDate] [datetime] NULL,
		[AddressCity] [nvarchar](max) NULL,
		[AddressState] [nvarchar](max) NULL,
		[ZipCode] [nvarchar](max) NULL,
		[CountryCode] [nvarchar](max) NULL,
	 CONSTRAINT [PK_UsCustomerOfficeList_Id] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[UsCustomerOfficeList] ADD  DEFAULT (newid()) FOR [Id]
END
GO

/****** Object:  Table [dbo].[UserNotification]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UserNotification'))
BEGIN
	CREATE TABLE [dbo].[UserNotification](
		[NotificationGuid] [uniqueidentifier] NOT NULL,
		[ResourceType] [nvarchar](255) NOT NULL,
		[ResourceAction] [nvarchar](255) NOT NULL,
		[ModuleGuid] [uniqueidentifier] NOT NULL,
		[StartDate] [datetime] NULL,
		[Subject] [nvarchar](max) NULL,
		[Message] [nvarchar](max) NULL,
		[AdditionalMessage] [nvarchar](max) NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
	CONSTRAINT [PK_UserNotification_NotificationGuid] PRIMARY KEY CLUSTERED 
	(
		[NotificationGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[UserNotification] ADD  DEFAULT (newid()) FOR [NotificationGuid]
END
GO


/****** Object:  Table [dbo].[UserNotificationMessage] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UserNotificationMessage'))
BEGIN
	CREATE TABLE [dbo].[UserNotificationMessage](
		[NotificationMessageGuid] [uniqueidentifier] NOT NULL,
		[UserNotificationGuid] [uniqueidentifier] NULL,
		[DistributionUserGuid] [uniqueidentifier] NULL,
		[UserGuid] [uniqueidentifier] NOT NULL,
		[Status] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_UserNotificationMessage_NotificationMessageGuid] PRIMARY KEY CLUSTERED 
	(
		[NotificationMessageGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[UserNotificationMessage] ADD DEFAULT (newid()) FOR [NotificationMessageGuid]
	
	ALTER TABLE [dbo].[UserNotificationMessage] WITH CHECK ADD CONSTRAINT [FK_UserNotificationMessage_UserNotificationGuid] FOREIGN KEY ([UserNotificationGuid])
	REFERENCES [dbo].[UserNotification] ([NotificationGuid])
	ALTER TABLE [dbo].[UserNotificationMessage] CHECK CONSTRAINT [FK_UserNotificationMessage_UserNotificationGuid]

	ALTER TABLE [dbo].[UserNotificationMessage] WITH CHECK ADD CONSTRAINT [FK_UserNotificationMessage_DistributionUserGuid] FOREIGN KEY ([DistributionUserGuid])
	REFERENCES [dbo].[DistributionUser] ([DistributionUserGuid])
	ALTER TABLE [dbo].[UserNotificationMessage] CHECK CONSTRAINT [FK_UserNotificationMessage_DistributionUserGuid]

	ALTER TABLE [dbo].[UserNotificationMessage] WITH CHECK ADD CONSTRAINT [FK_UserNotificationMessage_UserGuid] FOREIGN KEY ([UserGuid])
	REFERENCES [dbo].[Users] ([UserGuid])
	ALTER TABLE [dbo].[UserNotificationMessage] CHECK CONSTRAINT [FK_UserNotificationMessage_UserGuid]
END
GO

----START of Contract and related entity

/****** Object:  Table [dbo].[Contract]******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Contract'))
BEGIN
	CREATE TABLE [dbo].[Contract](
		[ContractGuid] [uniqueidentifier] NOT NULL,
		[IsIDIQContract] [bit] NOT NULL,
		[IsPrimeContract] [bit] NOT NULL,
		[ContractNumber] [nvarchar](255) NULL,
		[SubContractNumber] [nvarchar](255) NULL,
		[ORGID] [uniqueidentifier] NULL,
		[ProjectNumber] [nvarchar](255) NULL,
		[ContractTitle] [nvarchar](255) NULL,
		--[CompanyPresident] [uniqueidentifier] NULL,
		--[RegionalManager] [uniqueidentifier] NULL,
		--[ProjectManager] [uniqueidentifier] NULL,
		--[ProjectControls] [uniqueidentifier] NULL,
		--[AccountingRepresentative] [uniqueidentifier] NULL,
		--[ContractRepresentative] [uniqueidentifier] NULL,
		[CountryOfPerformance] [uniqueidentifier] NULL,
		[PlaceOfPerformance] [varchar](max) NULL,
		[POPStart] [datetime] NULL,
		[POPEnd] [datetime] NULL,
		[NaicsCode] [uniqueidentifier] NULL,
		[PSCCode] [uniqueidentifier] NULL,
		[CPAREligible] [bit] NULL,
		[QualityLevelRequirements] [bit] NULL,
		[QualityLevel] [nvarchar](255) NULL,
		[AwardingAgencyOffice] [uniqueidentifier] NULL,
		[OfficeContractRepresentative] [uniqueidentifier] NULL,
		[OfficeContractTechnicalRepresent] [uniqueidentifier] NULL,
		[FundingAgencyOffice] [uniqueidentifier] NULL,
		[SetAside] [nvarchar](255) NULL,
		[SelfPerformancePercent] [decimal](18, 3) NULL,
		[SBA] [bit] NULL,
		[Competition] [nvarchar](255) NULL,
		[ContractType] [nvarchar](255) NULL,
		[OverHead] [decimal](18, 3) NULL,
		[GAPercent] [decimal](18, 3) NULL,
		[FeePercent] [decimal](18, 3) NULL,
		[Currency] [nvarchar](255) NULL,
		[BlueSkyAwardAmount] [decimal](18, 3) NULL,
		[AwardAmount] [decimal](18, 3) NULL,
		[FundingAmount] [decimal](18, 3) NULL,
		[BillingAddress] [nvarchar](max) NULL,
		[BillingFrequency] [nvarchar](255) NULL,
		[InvoiceSubmissionMethod] [nvarchar](255) NULL,
		[PaymentTerms] [nvarchar](255) NULL,
		[AppWageDetermineDavisBaconAct] [nvarchar](255) NULL,
		[BillingFormula] [nvarchar](255) NULL,
		[RevenueFormula] [nvarchar](255) NULL,
		[RevenueRecognitionEACPercent] [decimal](18, 3) NULL,
		[OHonsite] [nvarchar](255) NULL,
		[OHoffsite] [nvarchar](255) NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[Description] [nvarchar](MAX) NULL,
		[AppWageDetermineServiceContractAct] NVARCHAR(255)NULL,
		[FundingOfficeContractRepresentative] [uniqueidentifier] NULL,
		[FundingOfficeContractTechnicalRepresent] [uniqueidentifier] NULL,
		[ParentContractGuid] [uniqueidentifier] NULL,
		[ProjectCounter] [int] NULL,
		[ApplicableWageDetermination] [nvarchar](255),
		RevenueRecognitionGuid uniqueidentifier,
	    Status nvarchar(255),
		AddressLine1 nvarchar(255),
		AddressLine2 nvarchar(255),
		AddressLine3 nvarchar(255),
		City nvarchar(255),
		Province nvarchar(255),
		County nvarchar(255),
		PostalCode nvarchar(255),
		FarContractTypeGuid uniqueidentifier NULL,
		IsImported BIT,
		TaskNodeID BIGINT NULL,
		MasterTaskNodeID BIGINT NULL
		
	 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
	(
		[ContractGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[Contract] ADD DEFAULT (newid()) FOR [ContractGuid]
END
GO


/****** Object:  Table [dbo].[ContractUserRole]******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractUserRole'))
BEGIN
	CREATE TABLE [dbo].[ContractUserRole](
	[ContractUserRoleGuid] [uniqueidentifier] NOT NULL,
	[ContractGuid] [uniqueidentifier] NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	[UserRole] NVARCHAR(50),
	CONSTRAINT [PK_ContractUserRoleGuid] PRIMARY KEY 
	(
		[ContractUserRoleGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	)

	ALTER TABLE [dbo].[ContractUserRole] ADD DEFAULT (newid()) FOR [ContractUserRoleGuid]

	ALTER TABLE [dbo].[ContractUserRole] WITH CHECK ADD CONSTRAINT [FK_ContractUserRole_ContractGuid] FOREIGN KEY ([ContractGuid])
	REFERENCES [dbo].[Contract]([ContractGuid])

	ALTER TABLE [dbo].[ContractUserRole] CHECK CONSTRAINT [FK_ContractUserRole_ContractGuid]

	ALTER TABLE [dbo].[ContractUserRole] WITH CHECK ADD CONSTRAINT [FK_ContractUserRole_UserGuid] FOREIGN KEY ([UserGuid])
	REFERENCES [dbo].[Users]([UserGuid])

	ALTER TABLE [dbo].[ContractUserRole] CHECK CONSTRAINT [FK_ContractUserRole_UserGuid]
END
GO
----END of Contract and related entity

/****** Object:  Table [dbo].[ContractResourceFile]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractResourceFile'))
BEGIN
	CREATE TABLE [dbo].[ContractResourceFile](
		[ContractResourceFileGuid] [uniqueidentifier] NOT NULL,
		[MasterStructureGuid] [uniqueidentifier] NULL,
		[MasterFolderGuid] [uniqueidentifier] NULL,
		[ParentId] [uniqueidentifier] NULL,
		[ResourceGuid] [uniqueidentifier] NOT NULL,
		[UploadFileName] [nvarchar](max) NOT NULL,
		[UploadUniqueFileName] [nvarchar](max),
		[IsFile] [BIT] NULL,
		[Keys] [nvarchar](50) NOT NULL,
		[MimeType] [nvarchar](MAX) NULL,
		[FilePath] [NVARCHAR](MAX) NULL,
		[FileSize] [NVARCHAR](50) NULL,
		[Description] [NVARCHAR](MAX) NULL,
		[ResourceType] [NVARCHAR](100) NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[IsCsv] [bit] NULL,
		[IsReadOnly] [BIT] NULL,
		[ContentResourceGuid] [uniqueidentifier] NULL
	 CONSTRAINT [PK_ContractResourceFileGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractResourceFileGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[ContractResourceFile] ADD  DEFAULT (newid()) FOR [ContractResourceFileGuid]
END
GO

/****** Object:  Table [dbo].[ContractModification]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractModification'))
BEGIN
	CREATE TABLE [dbo].[ContractModification](
		[ContractModificationGuid] [uniqueidentifier] NOT NULL,
		[ContractGuid] [uniqueidentifier] NULL,
		[ModificationNumber] [nvarchar](255) NULL,
		[ModificationType] [nvarchar](255) NULL,
		[IsActive] [bit] NULL,
		[EffectiveDate] [datetime] NULL,
		[IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		[EnteredDate] [datetime] NULL,
		[AwardAmount] [decimal](18, 3) NULL,
		[POPStart] [datetime] NULL,
		[POPEnd] [datetime] NULL,
		[UploadedFileName] [nvarchar](max) NULL,
		[Description] [nvarchar](MAX) NULL,
		[ModificationTitle] [nvarchar](255) NULL,
		[IsAwardAmount] [bit] NULL,
		[IsFundingAmount] [bit] NULL,
		[IsPop] [bit] NULL,
		[FundingAmount] [decimal](18, 3) NULL,
		[IsTaskModification] Bit DEFAULT 0 NULL,
		RevenueRecognitionGuid uniqueidentifier,
	 CONSTRAINT [PK_ContractModificationGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractModificationGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	
	ALTER TABLE [dbo].[ContractModification] ADD DEFAULT (newid()) FOR [ContractModificationGuid]

	ALTER TABLE [dbo].[ContractModification]  WITH CHECK ADD  CONSTRAINT [FK_ContractModification_ContractGuid] FOREIGN KEY([ContractGuid])
	REFERENCES [dbo].[Contract] ([ContractGuid])
	ALTER TABLE [dbo].[ContractModification] CHECK CONSTRAINT [FK_ContractModification_ContractGuid]
END
GO

/****** Object:  Table [dbo].[ContractQuestionaire] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractQuestionaire'))
BEGIN
	CREATE TABLE [dbo].[ContractQuestionaire](
		[ContractQuestionaireGuid] [uniqueidentifier] NOT NULL,
		[ContractGuid] [uniqueidentifier] NOT NULL,
		[IsFARclause] [bit] NOT NULL,
		[IsReportExecCompensation] [bit] NOT NULL,
		[ReportLastReportDate] [datetime] NULL,
		[ReportNextReportDate] [datetime] NULL,
		[IsGSAschedulesale] [bit] NOT NULL,
		[GSALastReportDate] [datetime] NULL,
		[GSANextReportDate] [datetime] NULL,
		[IsSBsubcontract] [bit] NOT NULL,
		[SBLastReportDate] [datetime] NULL,
		[SBNextReportDate] [datetime] NULL,
		[IsGovtFurnishedProperty] [bit] NOT NULL,
		[IsServiceContractReport] [bit] NOT NULL,
		[IsGQAC] [bit] NOT NULL,
		[GQACLastReportDate] [datetime] NULL,
		[GQACNextReportDate] [datetime] NULL,
		[IsCPARS] [bit] NOT NULL,
		[CPARSLastReportDate] [datetime] NULL,
		[CPARSNextReportDate] [datetime] NULL,
		[IsWarranties] [bit] NOT NULL,
		[IsStandardIndustryProvision] [bit] NOT NULL,
		[WarrantyProvisionDescription] [nvarchar](max) NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[IsActive] [bit] NOT NULL,
	 CONSTRAINT [PK_ContractQuestionaireGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractQuestionaireGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[ContractQuestionaire] ADD DEFAULT (newid()) FOR [ContractQuestionaireGuid]

	ALTER TABLE [dbo].[ContractQuestionaire]  WITH CHECK ADD  CONSTRAINT [FK_ContractQuestionaire_ContractGuid] FOREIGN KEY([ContractGuid])
	REFERENCES [dbo].[Contract] ([ContractGuid])
	ALTER TABLE [dbo].[ContractQuestionaire] CHECK CONSTRAINT [FK_ContractQuestionaire_ContractGuid]
END
GO

/****** Object:  Table [dbo].[JobRequest]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'JobRequest'))
BEGIN
	CREATE TABLE [dbo].[JobRequest](
		[JobRequestGuid] [uniqueidentifier] NOT NULL,
		[ContractGuid] [uniqueidentifier] NULL,
		[Status] [nvarchar](255) NULL,
		[IsIntercompanyWorkOrder] [bit] NULL,
		[Companies] [nvarchar](max) NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[Notes] [nvarchar] (MAX),
	 CONSTRAINT [PK_JobRequestGuid] PRIMARY KEY CLUSTERED 
	(
		[JobRequestGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[JobRequest] ADD DEFAULT (newid()) FOR [JobRequestGuid]

	ALTER TABLE [dbo].[JobRequest]  WITH CHECK ADD  CONSTRAINT [FK_JobRequest_ContractGuid] FOREIGN KEY([ContractGuid])
	REFERENCES [dbo].[Contract] ([ContractGuid])
	ALTER TABLE [dbo].[JobRequest] CHECK CONSTRAINT [FK_JobRequest_ContractGuid]
END
GO

/****** Object:  Table [dbo].[RevenueRecognization]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT * 
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'RevenueRecognization'))
BEGIN
	CREATE TABLE [dbo].[RevenueRecognization](
		[RevenueRecognizationGuid] [uniqueidentifier] NOT NULL,
		[ContractGuid] [uniqueidentifier] NULL,
		[ProjectReference] [nvarchar](255) NULL,
		[ModificationNumber] [nvarchar](255) NULL,
		[IsModAdministrative] [nvarchar](255) NULL,
		[DoesScopeContractChange] [bit] NULL,
		[IsASC606] [bit] NULL,
		[IsCurrentFiscalYearOfNorthWind] [bit] NULL,
		[OverviewNotes] [nvarchar](max) NULL,
		[IdentityContract] [nvarchar](255) NULL,
		[IsTerminationClauseGovernmentStandard] [nvarchar](255) NULL,
		[IdentifyTerminationClause] [nvarchar](max) NULL,
		[IsContractTermExpansion] [bit] NULL,
		[Approach] [nvarchar](255) NULL,
		[Step1Note] [nvarchar](255) NULL,
		[IdentityPerformanceObligation] [nvarchar](255) NULL,
		[IsMultiRevenueStream] [bit] NULL,
		[IsRepetativeService] [bit] NULL,
		[HasOptionToPurchageAdditionalGoods] [bit] NULL,
		[IsDiscountPurchase] [bit] NULL,
		[IsNonRefundableAdvancePayment] [bit] NULL,
		[HasDiscountProvision] [bit] NULL,
		[HasWarrenty] [bit] NULL,
		[WarrantyTerms] [nvarchar](max) NULL,
		[EstimateWarrantyExposure] [nvarchar](max) NULL,
		[Step2Note] [nvarchar](255) NULL,
		[ContractType] [nvarchar](255) NULL,
		[IsPricingVariation] [bit] NULL,
		[PricingExplanation] [nvarchar](max) NULL,
		[BaseContractPrice] [decimal](18, 3) NULL,
		[AdditionalPeriodOption] [nvarchar](max) NULL,
		[Step3Note] [nvarchar](255) NULL,
		[HasMultipleContractObligations] [nvarchar](50) NULL,
		[EachMultipleObligation] [nvarchar](max) NULL,
		[Step4Note] [nvarchar](255) NULL,
		[HasLicensingOrIntellectualProperty] [bit] NULL,
		[Step5Note] [nvarchar](255) NULL,
		[IsCompleted] bit,
		[ResourceGuid]  uniqueidentifier,
		[IsRevenueCreated] bit,
		[IsFile] [bit] NULL,
		[FilePath] [nvarchar](max) NULL,
		[IsActive] [bit] NULL,
		[IsDeleted] [bit] NULL,
		[CreatedOn] [datetime] NULL,
		[UpdatedOn] [datetime] NULL,
		[CreatedBy] [uniqueidentifier] NULL,
		[UpdatedBy] [uniqueidentifier] NULL,
		[IsNotify] [bit] NULL,

	 CONSTRAINT [PK_RevenueRecognizationGuid] PRIMARY KEY CLUSTERED 
	(
		[RevenueRecognizationGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[RevenueRecognization] ADD DEFAULT (newid()) FOR [RevenueRecognizationGuid]

	ALTER TABLE [dbo].[RevenueRecognization]  WITH CHECK ADD  CONSTRAINT [FK_RevenueRecognization_ContractGuid] FOREIGN KEY([ContractGuid])
	REFERENCES [dbo].[Contract] ([ContractGuid])

	ALTER TABLE [dbo].[RevenueRecognization] CHECK CONSTRAINT [FK_RevenueRecognization_ContractGuid]
END
GO

/****** Object:  Table [dbo].[RevenuePerformanceObligation] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'RevenuePerformanceObligation'))
BEGIN
	CREATE TABLE [dbo].[RevenuePerformanceObligation](
		[PerformanceObligationGuid] [uniqueidentifier] NOT NULL,
		[RevenueGuid] [uniqueidentifier] NOT NULL,
		[RevenueStreamIdentifier] [nvarchar](max) NOT NULL,
		[RightToPayment] [bit] NOT NULL,
		[RoutineService] [bit] NOT NULL,
		[RevenueOverTimePointInTime] [nvarchar](255) NOT NULL,
		[SatisfiedOverTime] [nvarchar](255) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
	 CONSTRAINT [PK_PerformanceObligationGuid] PRIMARY KEY CLUSTERED 
	(
		[PerformanceObligationGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[RevenuePerformanceObligation] ADD DEFAULT (newid()) FOR [PerformanceObligationGuid]

	ALTER TABLE [dbo].[RevenuePerformanceObligation]  WITH CHECK ADD  CONSTRAINT [FK_RevenuePerformanceObligation_RevenueGuid] FOREIGN KEY([RevenueGuid])
	REFERENCES [dbo].[RevenueRecognization] ([RevenueRecognizationGuid])
	ALTER TABLE [dbo].[RevenuePerformanceObligation] CHECK CONSTRAINT [FK_RevenuePerformanceObligation_RevenueGuid]
END
GO

/****** Object:  Table [dbo].[RevenueContractExtension]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'RevenueContractExtension'))
BEGIN
	CREATE TABLE [dbo].[RevenueContractExtension](
		[ContractExtensionGuid] [uniqueidentifier] NOT NULL,
		[RevenueGuid] [uniqueidentifier] NOT NULL,
		[ExtensionDate] [datetime] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_ContractExtensionGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractExtensionGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RevenueContractExtension] ADD  DEFAULT (newid()) FOR [ContractExtensionGuid]

	ALTER TABLE [dbo].[RevenueContractExtension]  WITH CHECK ADD CONSTRAINT [FK_RevenueContractExtension_RevenueGuid] FOREIGN KEY([RevenueGuid])
	REFERENCES [dbo].[RevenueRecognization] ([RevenueRecognizationGuid])
	ALTER TABLE [dbo].[RevenueContractExtension] CHECK CONSTRAINT [FK_RevenueContractExtension_RevenueGuid]
END
GO

/****** Object:  Table [dbo].[WbsDictionaries]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
		IF(NOT EXISTS(SELECT *
			FROM INFORMATION_SCHEMA.TABLES
			WHERE TABLE_SCHEMA = 'dbo'
			AND TABLE_NAME = 'WbsDictionaries'))
		BEGIN
			CREATE TABLE [dbo].[WbsDictionaries](
			[WbsDictionaryGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
			[WbsNumber] [NVARCHAR](255) NULL,
			[ProjectNumber] [NVARCHAR](255) NULL,
			[WbsDictionaryTitle] [NVARCHAR](max) NULL,
			[CreatedBy] [uniqueidentifier] NULL,
			[CreatedOn] [datetime] NULL,
		CONSTRAINT [PK_WbsDictionaries] PRIMARY KEY CLUSTERED 
		(
			[WbsDictionaryGuid] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

		ALTER TABLE [dbo].[WbsDictionaries] ADD  CONSTRAINT [DF_WbsDictionaries_WbsDictionaryGuid]  DEFAULT (newid()) FOR [WbsDictionaryGuid]
END
GO

/****** Object:  Table [dbo].[AuditLog]  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'AuditLog'))
BEGIN
		CREATE TABLE [dbo].[AuditLog](
		[AuditLogGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
		[RawData] [nvarchar](max) NULL,
		[TimeStamp] [datetime]  NULL,
		[Resource] [nvarchar](MAX)  NULL,
		[ResourceId] [uniqueidentifier] NULL,
		[Actor] [nvarchar](255)  NULL,
		[ActorId] [uniqueidentifier]  NULL,
		[IPAddress] [nvarchar](50)  NULL,
		[Action] [nvarchar](255)  NULL,
                [ActionId] [uniqueidentifier]  NULL,
		[ActionResult] [nvarchar](255)  NULL,
		[ActionResultReason] [nvarchar](max)  NULL,
		[AdditionalInformation] [nvarchar](max)  NULL,
		[AdditionalInformationURl] [nvarchar](255) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[AuditLogGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[AuditLog] ADD  DEFAULT (newsequentialid()) FOR [AuditLogGuid]
END
GO

/****** Object:  Table [dbo].[FarClause]  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FarClause'))
BEGIN
	CREATE TABLE [dbo].[FarClause](
	[FarClauseGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Number] [nvarchar](50) NULL,
	[Title] [nvarchar](255) NULL,
	[Paragraph] [nvarchar](255) NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [IsDeleted] bit NOT NULL
		CONSTRAINT [PK_FarClause] PRIMARY KEY CLUSTERED 
	(
		[FarClauseGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[FarClause] ADD  CONSTRAINT [DF_FarClause_FarClauseGuid]  DEFAULT (newid()) FOR [FarClauseGuid]
    ALTER TABLE [dbo].[FarClause] ADD  CONSTRAINT [DF_FarClause_IsDeleted]  DEFAULT (0) FOR [IsDeleted]
END
GO

/****** Object:  Table [dbo].[FarContractType]  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FarContractType'))
BEGIN
	CREATE TABLE [dbo].[FarContractType](
	[FarContractTypeGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Title] [nvarchar](255) NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [IsDeleted] bit NOT NULL
	) ON [PRIMARY]

    ALTER TABLE [dbo].[FarContractType] ADD  CONSTRAINT [DF_FarContractType_IsDeleted]  DEFAULT (0) FOR [IsDeleted]
	END
GO


/****** Object:  Table [dbo].[FarContractTypeClause]  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FarContractTypeClause'))
BEGIN
	CREATE TABLE [dbo].[FarContractTypeClause](
	[FarContractTypeClauseGuid] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[FarContractTypeGuid] [uniqueidentifier] NULL,
	[FarClauseGuid] [uniqueidentifier] NULL,
	[FarContractTypeCode] [nvarchar](50) NULL,
	[IsRequired] [bit] NULL,
	[IsApplicable] [bit] NULL,
	[IsOptional] [bit] NULL,
    [UpdatedBy] [uniqueidentifier] NULL,
    [IsDeleted] bit NOT NULL
	CONSTRAINT [PK_FarContractTypeClause_FarContractTypeClauseGuid] PRIMARY KEY CLUSTERED 
	(
		[FarContractTypeClauseGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
    ALTER TABLE [dbo].[FarContractTypeClause] ADD  CONSTRAINT [DF_FarContractTypeClause_FarContractTypeClauseGuid]  DEFAULT (newid()) FOR [FarContractTypeClauseGuid]
    ALTER TABLE [dbo].[FarContractTypeClause] ADD  CONSTRAINT [DF_FarContractTypeClause_IsDeleted]  DEFAULT (0) FOR [IsDeleted]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FarContract'))
BEGIN
	CREATE TABLE [dbo].[FarContract](
		[FarContractGuid] [uniqueidentifier] NOT NULL,
		[ContractGuid] [uniqueidentifier] NOT NULL,
		[FarContractTypeClauseGuid] [uniqueidentifier] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
		[CreatedBy] [uniqueidentifier] NOT NULL,
		[UpdatedBy] [uniqueidentifier] NOT NULL,
		[CreatedOn] [datetime] NOT NULL,
		[UpdatedOn] [datetime] NOT NULL,
	 CONSTRAINT [PK_FarContract_FarContractGuid] PRIMARY KEY CLUSTERED 
	(
		[FarContractGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[FarContract] ADD DEFAULT (newid()) FOR [FarContractGuid]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'QuestionaireManagerType'))
BEGIN
	CREATE TABLE [dbo].[QuestionaireManagerType](
	[QuestionaireManagerTypeGuid] [uniqueidentifier] NOT NULL,
	[ManagerType] [nvarchar](50) NULL,
	[ManagerTypeNormalize] [nvarchar](50) NULL,
	CONSTRAINT [PK_QuestionaireManagerType_QuestionaireManagerTypeGuid] PRIMARY KEY CLUSTERED 
	(
		[QuestionaireManagerTypeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[QuestionaireManagerType] ADD  DEFAULT (newid()) FOR [QuestionaireManagerTypeGuid]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'QuestionaireMaster'))
BEGIN
	CREATE TABLE [dbo].[QuestionaireMaster](
	[QuestionaireMasterGuid] [uniqueidentifier] NOT NULL,
	[ParentMasterGuid] [uniqueidentifier] NULL,
	[Questions] [nvarchar](max) NOT NULL,
	[OrderNumber] [int] NULL,
	[QuestionaireManagerTypeGuid] [uniqueidentifier] NULL,
	[IsActive] [bit] NULL,
	[ResourceType] [nvarchar](200) NULL,
	[QuestionType] [nvarchar](200) NULL,
	[MultiSelectOption] [nvarchar](max) NULL,
	[Id] [nvarchar](200) NULL,
	[Class] [nvarchar](100) NULL,
	[ChildClass] [nvarchar](200) NULL,
	[ChildId] [nvarchar](200) NULL,
 CONSTRAINT [PK_QuestionaireMaster_QuestionaireMasterGuid] PRIMARY KEY CLUSTERED 
(
	[QuestionaireMasterGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[QuestionaireMaster] Add DEFAULT (newid()) FOR [QuestionaireMasterGuid]
End
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'QuestionaireUserAnswer'))
BEGIN
	CREATE TABLE [dbo].[QuestionaireUserAnswer](
	[QuestionaireUserAnswerGuid] [uniqueidentifier] NOT NULL,
	[ContractGuid] [uniqueidentifier] NOT NULL,
	[QuestionaireMasterGuid] [uniqueidentifier] NOT NULL,
	[Questions] [nvarchar](max) NOT NULL,
	[Answer] [nvarchar](50) NULL,
	[ManagerUserGuid] [uniqueidentifier] NULL,
	[ContractCloseApprovalGuid] [uniqueidentifier] NULL,
	[Status] [nvarchar](50) NULL,
	[RepresentativeType] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedOn] [datetime] NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[TextAnswer] [nvarchar](max) NULL,
	[DateOfLastReport] [datetime] NULL,
	[DateOfNextReport] [datetime] NULL,
	[CheckBoxAnswer] [nvarchar](max) NULL,
	[ChildYesNoAnswer] [nvarchar](200) NULL,
 CONSTRAINT [PK_QuestionaireUserAnswer_QuestionaireUserAnswerGuid] PRIMARY KEY CLUSTERED 
(
	[QuestionaireUserAnswerGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[QuestionaireUserAnswer] ADD   DEFAULT (newid()) FOR [QuestionaireUserAnswerGuid]
End
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FolderStructureFolder'))
BEGIN
	CREATE TABLE [dbo].[FolderStructureFolder](
	[FolderStructureFolderGuid] [uniqueidentifier] NOT NULL,
	[FolderStructureMasterGuid] [uniqueidentifier] NOT NULL,
	[ParentGuid] [uniqueidentifier] NULL,
	[Name] [NVARCHAR](255) NULL,
	[Description] [NVARCHAR](MAX) NULL,
	[Keys] [NVARCHAR](255) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[ResourceType] [NVARCHAR] (255) NULL
	CONSTRAINT [PK_FolderStructureFolder_FolderStructureFolderGuid] PRIMARY KEY CLUSTERED 
	(
		[FolderStructureFolderGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	ALTER TABLE [dbo].[FolderStructureFolder] ADD  DEFAULT (newid()) FOR [FolderStructureFolderGuid]
End
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'FolderStructureMaster'))
BEGIN
	CREATE TABLE [dbo].[FolderStructureMaster]
	(
	[FolderStructureMasterGuid] [uniqueidentifier] NOT NULL,
	[Module] [NVARCHAR](50) NOT NULL,
	[ResourceType] [NVARCHAR](50) NULL,
	[Status] [BIT] NOT NULL,
	[Key] [NVARCHAR](255) NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL
	CONSTRAINT [PK_FolderStructureMaster_FolderStructureMasterGuid] PRIMARY KEY CLUSTERED 
	(
		[FolderStructureMasterGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[FolderStructureMaster] ADD  DEFAULT (newid()) FOR [FolderStructureMasterGuid]
End
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractNotice'))
BEGIN
	CREATE TABLE [dbo].[ContractNotice]
	(
	[ContractNoticeGuid] [uniqueidentifier] NOT NULL,
	[NoticeType] [NVARCHAR](255) NULL,
	[IssuedDate] [DateTime] NULL,
	[LastUpdatedDate] [DateTime] NULL,
	[ResourceGuid] [uniqueidentifier] NULL,
	[Resolution] [NVARCHAR](255) NULL,
	[NoticeDescription] [NVARCHAR](MAX) NULL,
	[UpdatedBy] [uniqueidentifier] NULL
	CONSTRAINT [PK_ContractNotice_ContractNoticeGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractNoticeGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[ContractNotice] ADD  DEFAULT (newid()) FOR [ContractNoticeGuid]
End
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ContractCloseApproval'))
BEGIN
CREATE TABLE ContractCloseApproval
(
[ContractCloseApprovalGuid] [uniqueidentifier] NOT NULL,
[Title]  [NVARCHAR] (50) NOT NULL,
[NormalizedValue] [NVARCHAR] (50)  NULL,
[IsActive] [BIT] NULL,
CONSTRAINT [PK_ContractCloseApproval_ContractCloseApprovalGuid] PRIMARY KEY CLUSTERED 
	(
		[ContractCloseApprovalGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[ContractCloseApproval] ADD  DEFAULT (newid()) FOR [ContractCloseApprovalGuid]
End
GO


/****** Object:  Table [dbo].[RegionUserRoleMapping]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'RegionUserRoleMapping'))
BEGIN
CREATE TABLE [dbo].[RegionUserRoleMapping](
	[RegionUserRoleMappingGuid] [uniqueidentifier] NOT NULL,
	[RegionGuid] [uniqueidentifier] NULL,
	[UserGuid] [uniqueidentifier] NULL,
	[RoleType] [nvarchar](200) NULL,
	[Keys] [nvarchar](100) NULL,
	CONSTRAINT [PK_RegionUserRoleMapping_RegionUserRoleMappingGuid] PRIMARY KEY CLUSTERED
	(
		[RegionUserRoleMappingGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[RegionUserRoleMapping] ADD  DEFAULT (newid()) FOR [RegionUserRoleMappingGuid]

ALTER TABLE [dbo].[RegionUserRoleMapping]  WITH CHECK ADD  CONSTRAINT [FK_RegionUserRoleMapping_UserGuid] FOREIGN KEY([UserGuid])
REFERENCES [dbo].[Users] ([UserGuid])

ALTER TABLE [dbo].[RegionUserRoleMapping] CHECK CONSTRAINT [FK_RegionUserRoleMapping_UserGuid]

END
GO

/****** Object:  Table [dbo].[ApplicationCategory]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ApplicationCategory'))
BEGIN
CREATE TABLE [dbo].[ApplicationCategory](
	[ApplicationCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[OrderIndex] [int] NOT NULL,
	CONSTRAINT [PK_ApplicationCategory_ApplicationCategoryId] PRIMARY KEY CLUSTERED
	(
		[ApplicationCategoryId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Application]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Application'))
BEGIN
CREATE TABLE [dbo].[Application](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationCategoryId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[IconPath] [nvarchar](255) NULL,
	[Url] [nvarchar](255) NULL,
	[IsInternalApplication] [bit] NOT NULL,
	[ShowAppForAllUsers] [bit] NOT NULL,
	[UpdateBy] [uniqueidentifier] NULL,
	[UpdatedOn] [datetime] NULL,
	[Resource] [nvarchar](50) NULL,
	[Action] [nvarchar](50) NULL,
	CONSTRAINT [PK_Application_ApplicationId] PRIMARY KEY CLUSTERED
	(
		[ApplicationId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Application]  WITH CHECK ADD  CONSTRAINT [FK_Application_ApplicationCategoryId] FOREIGN KEY([ApplicationCategoryId])
REFERENCES [dbo].[ApplicationCategory] ([ApplicationCategoryId])

ALTER TABLE [dbo].[Application] CHECK CONSTRAINT [FK_Application_ApplicationCategoryId]
END
GO


/****** Object:  Table [dbo].[ArticleType]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'ArticleType'))
BEGIN
CREATE TABLE [dbo].[ArticleType](
	[ArticleTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	CONSTRAINT [PK_ArticleType_ArticleTypeGuid] PRIMARY KEY CLUSTERED
	(
		[ArticleTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


/****** Object:  Table [dbo].[Article]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'Article'))
BEGIN
CREATE TABLE [dbo].[Article](
	[ArticleId] [int] IDENTITY(1,1) NOT NULL,
	[ArticleTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Excerpt] [nvarchar](255) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[IsLocalMedia] [bit] NOT NULL,
	[PrimaryMediaPath] [nvarchar](255) NULL,
	[PrimaryMediaUrl] [nvarchar](255) NULL,
	[MediaCaption] [nvarchar](255) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsFeatured] [bit] NOT NULL,
	[ShowInArchive] [bit] NOT NULL,
	[CreatedBy] [uniqueidentifier] NULL,
	[CreatedOn] [datetime] NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedOn] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	CONSTRAINT [PK_Article_ArticleId] PRIMARY KEY CLUSTERED
	(
		[ArticleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[Article]  WITH CHECK ADD  CONSTRAINT [FK_Article_ArticleTypeId] FOREIGN KEY([ArticleTypeId])
REFERENCES [dbo].[ArticleType] ([ArticleTypeId])

ALTER TABLE [dbo].[Article] CHECK CONSTRAINT [FK_Article_ArticleTypeId]
END
GO


/****** Object:  Table [dbo].[UserFavoriteApplication]    Script Date: 11/28/2019 4:00:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF(NOT EXISTS(SELECT *
	FROM INFORMATION_SCHEMA.TABLES
	WHERE TABLE_SCHEMA = 'dbo'
	AND TABLE_NAME = 'UserFavoriteApplication'))
BEGIN
CREATE TABLE [dbo].[UserFavoriteApplication](
	[UserFavoriteApplicationGuid] [uniqueidentifier] NOT NULL,
	[ApplicationId] int NOT NULL,
	[UserGuid] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_UserFavoriteApplication_UserFavoriteApplicationGuid] PRIMARY KEY CLUSTERED
	(
		[UserFavoriteApplicationGuid] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	ALTER TABLE [dbo].[UserFavoriteApplication] ADD  DEFAULT (newid()) FOR [UserFavoriteApplicationGuid]

	ALTER TABLE [dbo].[UserFavoriteApplication]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteApplication_ApplicationId] FOREIGN KEY([ApplicationId])
	REFERENCES [dbo].[Application] ([ApplicationId])
	ALTER TABLE [dbo].[UserFavoriteApplication] CHECK CONSTRAINT [FK_UserFavoriteApplication_ApplicationId]

	ALTER TABLE [dbo].[UserFavoriteApplication]  WITH CHECK ADD  CONSTRAINT [FK_UserFavoriteApplication_UserGuid] FOREIGN KEY([UserGuid])
	REFERENCES [dbo].[Users] ([UserGuid])
	ALTER TABLE [dbo].[UserFavoriteApplication] CHECK CONSTRAINT [FK_UserFavoriteApplication_UserGuid]

END
GO









