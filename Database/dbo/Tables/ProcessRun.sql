CREATE TABLE [dbo].[ProcessRun] (
    [ProcessRunId] INT      IDENTITY (1, 1) NOT NULL,
    [ProcessId]    INT      NOT NULL,
    [Start]        DATETIME NOT NULL,
    [End]          DATETIME NULL,
    [RunStatus]    BIT      NULL,
    CONSTRAINT [PK_ProcessRun] PRIMARY KEY CLUSTERED ([ProcessRunId] ASC),
    CONSTRAINT [FK_ProcessRun_Process] FOREIGN KEY ([ProcessId]) REFERENCES [dbo].[Process] ([ProcessId]),
    CONSTRAINT [FK_ProcessRun_ProcessRun] FOREIGN KEY ([ProcessRunId]) REFERENCES [dbo].[ProcessRun] ([ProcessRunId])
);



