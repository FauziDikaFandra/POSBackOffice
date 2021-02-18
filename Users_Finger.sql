USE [Pos_Server]
GO

/****** Object:  Table [dbo].[Users_Finger]    Script Date: 10/19/2018 16:52:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users_Finger](
	[User_ID] [char](4) NOT NULL,
	[Branch_ID] [char](4) NOT NULL,
	[RegTmp] [varbinary](max) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_Users_Finger] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC,
	[Branch_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

