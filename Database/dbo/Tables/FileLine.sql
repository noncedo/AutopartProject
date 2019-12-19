CREATE TABLE [dbo].[FileLine] (
    [FileLineId]        INT           IDENTITY (1, 1) NOT NULL,
    [FileSourceId]      INT           NOT NULL,
    [FileDestinationId] INT           NULL,
    [Line]              VARCHAR (250) NOT NULL,
    [CreateDate]        DATETIME      CONSTRAINT [DF_FileLine_CreateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_FileLine] PRIMARY KEY CLUSTERED ([FileLineId] ASC),
    CONSTRAINT [FK_FileLine_FileDestination] FOREIGN KEY ([FileDestinationId]) REFERENCES [dbo].[FileDestination] ([FileId]),
    CONSTRAINT [FK_FileLine_FileSource] FOREIGN KEY ([FileSourceId]) REFERENCES [dbo].[FileSource] ([FileId])
);

