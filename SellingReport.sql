USE [Pos_Server_History]
GO

/****** Object:  Table [dbo].[SellingReport]    Script Date: 05/12/2017 13:40:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SellingReport](
	[Tgl] [int] NULL,
	[GV10] [int] NULL,
	[Sepuluh] [decimal](25, 0) NULL,
	[GV25] [int] NULL,
	[DuaLima] [decimal](25, 0) NULL,
	[GV30] [int] NULL,
	[TigaPuluh] [decimal](25, 0) NULL,
	[GV50] [int] NULL,
	[LimaPuluh] [decimal](25, 0) NULL,
	[GV60] [int] NULL,
	[EnamPuluh] [decimal](25, 0) NULL,
	[GV75] [int] NULL,
	[TujuhLima] [decimal](25, 0) NULL,
	[GV100] [int] NULL,
	[Seratus] [decimal](25, 0) NULL,
	[Total] [decimal](25, 0) NULL
) ON [PRIMARY]

GO

