CREATE TABLE [dbo].[DmsProvider] (
    [DmsProviderId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DmsProvider] PRIMARY KEY CLUSTERED ([DmsProviderId] ASC)
);

