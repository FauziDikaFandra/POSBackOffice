USE [Pos_Server_History]
GO

/****** Object:  Table [dbo].[Voucher_Desc]    Script Date: 12/28/2016 13:15:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Voucher_Desc](
	[No] [int] NOT NULL,
	[Description] [varchar](100) NULL,
 CONSTRAINT [PK_Voucher_Desc] PRIMARY KEY CLUSTERED 
(
	[No] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

