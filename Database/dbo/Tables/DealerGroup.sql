CREATE TABLE [dbo].[DealerGroup] (
    [DealerGroupId] INT          IDENTITY (1, 1) NOT NULL,
    [DmsProviderId] INT          NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [Code]          VARCHAR (10) NULL,
    CONSTRAINT [PK_DealerGroup] PRIMARY KEY CLUSTERED ([DealerGroupId] ASC),
    CONSTRAINT [FK_DealerGroup_DmsProvider] FOREIGN KEY ([DmsProviderId]) REFERENCES [dbo].[DmsProvider] ([DmsProviderId])
);





