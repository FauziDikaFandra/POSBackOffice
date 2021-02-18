USE [pos_server]
GO

/****** Object:  Table [dbo].[Cust_Point_Trans]    Script Date: 09/10/2015 09:12:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Cust_Point_Trans](
	[Transaction_Number] [char](21) NULL,
	[Trans_Nr] [char](20) NOT NULL,
	[Card_Nr] [varchar](11) NULL,
	[Current_Point] [int] NULL,
	[Claim_Point] [int] NULL,
	[Claim_Rp] [numeric](18, 0) NULL,
	[Date_Trans] [datetime] NULL,
	[User_Id] [char](20) NULL,
	[Data_Status] [varchar](2) NULL,
 CONSTRAINT [PK_Trans_Payment_By_Point] PRIMARY KEY CLUSTERED 
(
	[Trans_Nr] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


