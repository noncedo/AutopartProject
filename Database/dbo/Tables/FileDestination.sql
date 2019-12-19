CREATE TABLE [dbo].[FileDestination] (
    [FileId]       INT           IDENTITY (1, 1) NOT NULL,
    [ProcessRunId] INT           NOT NULL,
    [Path]         VARCHAR (250) NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_FileDestination] PRIMARY KEY CLUSTERED ([FileId] ASC),
    CONSTRAINT [FK_FileDestination_FileDestination] FOREIGN KEY ([FileId]) REFERENCES [dbo].[FileDestination] ([FileId]),
    CONSTRAINT [FK_FileDestination_ProcessRun] FOREIGN KEY ([ProcessRunId]) REFERENCES [dbo].[ProcessRun] ([ProcessRunId])
);

