CREATE TABLE [dbo].[LogType] (
    [LogTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [LogType]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([LogTypeID] ASC),
    CONSTRAINT [FK_LogType_LogType] FOREIGN KEY ([LogTypeID]) REFERENCES [dbo].[LogType] ([LogTypeID])
);

