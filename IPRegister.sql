USE [Pos_Server_History]
GO

/****** Object:  Table [dbo].[IPRegister]    Script Date: 04/12/2017 11:03:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IPRegister](
	[Branch_ID] [char](4) NOT NULL,
	[Cash_Register_ID] [char](3) NOT NULL,
	[IP_Register] [char](15) NOT NULL,
 CONSTRAINT [PK_IPRegister] PRIMARY KEY CLUSTERED 
(
	[Branch_ID] ASC,
	[Cash_Register_ID] ASC,
	[IP_Register] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

