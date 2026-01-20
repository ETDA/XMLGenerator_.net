USE [etaxdb]
GO

/****** Object:  Table [dbo].[TradeLineItem]    Script Date: 1/19/2026 11:10:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TradeLineItem](
	[DocID] [nvarchar](35) NOT NULL,
	[ItemID] [nvarchar](50) NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL
) ON [PRIMARY]
GO


