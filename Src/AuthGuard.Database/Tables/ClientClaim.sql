﻿CREATE TABLE [dbo].[ClientClaim]
(
	[Id]		[UNIQUEIDENTIFIER] NOT NULL,
	[ClientId]	[UNIQUEIDENTIFIER] NOT NULL,
	[Type]		[NVARCHAR](250) NOT NULL DEFAULT(''),
	[Value]		[NVARCHAR](250) NOT NULL DEFAULT(''),
	CONSTRAINT [PK_ClientClaim_Id] PRIMARY KEY CLUSTERED([Id] ASC),
	CONSTRAINT [FK_ClientClaim_ClientId_Client_Id] FOREIGN KEY([ClientId]) REFERENCES [dbo].[Client] ([Id])
)