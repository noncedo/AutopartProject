CREATE TABLE [dbo].[Log] (
    [LogID]        INT           IDENTITY (1, 1) NOT NULL,
    [LogTypeID]    INT           NOT NULL,
    [ProcessRunID] INT           NOT NULL,
    [Source]       VARCHAR (100) NULL,
    [Message]      VARCHAR (200) NOT NULL,
    [StackTrace]   VARCHAR (500) NULL,
    [LogDate]      DATETIME      NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([LogID] ASC),
    CONSTRAINT [FK_Log_LogType] FOREIGN KEY ([LogTypeID]) REFERENCES [dbo].[LogType] ([LogTypeID]),
    CONSTRAINT [FK_Log_ProcessRun] FOREIGN KEY ([ProcessRunID]) REFERENCES [dbo].[ProcessRun] ([ProcessRunId])
);

