CREATE TABLE [dbo].[Dealer] (
    [DealerId]      INT          IDENTITY (1, 1) NOT NULL,
    [DealerGroupId] INT          NOT NULL,
    [Code]          VARCHAR (5)  NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [IsActive]      BIT          NOT NULL,
    CONSTRAINT [PK_Dealer_1] PRIMARY KEY CLUSTERED ([DealerId] ASC),
    CONSTRAINT [FK_Dealer_DealerGroup] FOREIGN KEY ([DealerGroupId]) REFERENCES [dbo].[DealerGroup] ([DealerGroupId])
);





