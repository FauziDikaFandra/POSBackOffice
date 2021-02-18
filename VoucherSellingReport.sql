USE [pos_server_history]
GO

/****** Object:  Table [dbo].[lain]    Script Date: 10/12/2015 10:20:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[VoucherSellingReport](
	[lokasi] [char](1) NOT NULL,
	[L_REF] [nvarchar](8) NOT NULL,
	[L_TGL] [smalldatetime] NOT NULL,
	[L_TYPE] [nvarchar](1) NOT NULL,
	[L_AMT] [float] NOT NULL,
	[L_DISC] [float] NOT NULL,
	[L_KET] [nvarchar](30) NOT NULL,
	[L_LUPDATE] [smalldatetime] NOT NULL,
	[L_USER] [nvarchar](10) NOT NULL,
	[V_NO] [char](100) NOT NULL,
	[V_AMT] [decimal](18, 0) NOT NULL,
	[V_FLAG] [char](1) NULL,
	[V_REC] [smalldatetime] NULL,
	[V_KUNCI] [varchar](13) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


