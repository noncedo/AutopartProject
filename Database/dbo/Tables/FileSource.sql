CREATE TABLE [dbo].[FileSource] (
    [FileId]       INT           IDENTITY (1, 1) NOT NULL,
    [ProcessRunId] INT           NOT NULL,
    [Path]         VARCHAR (250) NOT NULL,
    [Name]         VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED ([FileId] ASC),
    CONSTRAINT [FK_File_ProcessRun] FOREIGN KEY ([ProcessRunId]) REFERENCES [dbo].[ProcessRun] ([ProcessRunId])
);

