USE [weapsy.dev]
GO
/****** Object:  Table [dbo].[TextLocalisation]    Script Date: 11/06/2016 18:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TextLocalisation](
	[TextVersionId] [uniqueidentifier] NOT NULL,
	[LanguageId] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NULL,
 CONSTRAINT [PK_TextLocalisation] PRIMARY KEY CLUSTERED 
(
	[TextVersionId] ASC,
	[LanguageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TextModule]    Script Date: 11/06/2016 18:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TextModule](
	[Id] [uniqueidentifier] NOT NULL,
	[ModuleId] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_Text] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TextVersion]    Script Date: 11/06/2016 18:17:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TextVersion](
	[Id] [uniqueidentifier] NOT NULL,
	[TextModuleId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Content] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_TextVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[TextLocalisation]  WITH CHECK ADD  CONSTRAINT [FK_TextLocalisation_TextVersion] FOREIGN KEY([TextVersionId])
REFERENCES [dbo].[TextVersion] ([Id])
GO
ALTER TABLE [dbo].[TextLocalisation] CHECK CONSTRAINT [FK_TextLocalisation_TextVersion]
GO
ALTER TABLE [dbo].[TextVersion]  WITH CHECK ADD  CONSTRAINT [FK_TextVersion_TextModule] FOREIGN KEY([TextModuleId])
REFERENCES [dbo].[TextModule] ([Id])
GO
ALTER TABLE [dbo].[TextVersion] CHECK CONSTRAINT [FK_TextVersion_TextModule]
GO
