CREATE TABLE [dbo].[ILHAudit] (
    [ILHAuditId]   INT           IDENTITY (1, 1) NOT NULL,
    [ProcessRunId] INT           NOT NULL,
    [DealerId]     INT           NOT NULL,
    [Source]       VARCHAR (500) NOT NULL,
    [Destination]  VARCHAR (500) NOT NULL,
    [Timestamp]    DATETIME      NOT NULL,
    CONSTRAINT [PK_ILHAudit] PRIMARY KEY CLUSTERED ([ILHAuditId] ASC),
    CONSTRAINT [FK_ILHAudit_Dealer] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealer] ([DealerId]),
    CONSTRAINT [FK_ILHAudit_ProcessRun] FOREIGN KEY ([ProcessRunId]) REFERENCES [dbo].[ProcessRun] ([ProcessRunId])
);

