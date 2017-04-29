﻿CREATE TABLE [dbo].[SecurityCodeAction]
(
	[Id]	[INT]			NOT NULL IDENTITY(1,1),
	[Name]	[NVARCHAR](100)	NOT NULL DEFAULT(''),
	CONSTRAINT [PK_SecurityCodeAction_Id] PRIMARY KEY CLUSTERED([Id] ASC),
	CONSTRAINT [IDX_SecurityCodeAction_Name_U_N] UNIQUE NONCLUSTERED ([Name] ASC)
)
