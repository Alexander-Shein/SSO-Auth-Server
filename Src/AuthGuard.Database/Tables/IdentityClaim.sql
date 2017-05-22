﻿CREATE TABLE [dbo].[IdentityClaim]
(
	[Id]			UNIQUEIDENTIFIER	NOT NULL,
	[Type]			NVARCHAR(200)		NOT NULL DEFAULT(''),
	[Description]	NVARCHAR(1000)		NOT NULL DEFAULT(''),
	[IsReadOnly]	BIT					NOT NULL DEFAULT(0),
	[IsEnabled]		BIT					NOT NULL DEFAULT(1),
	[Ts]			ROWVERSION			NOT NULL,
	CONSTRAINT [PK_IdentityClaim_Id] PRIMARY KEY CLUSTERED([Id] ASC),
	CONSTRAINT [IDX_IdentityClaim_Type_U_N] UNIQUE NONCLUSTERED ([Type] ASC)
)