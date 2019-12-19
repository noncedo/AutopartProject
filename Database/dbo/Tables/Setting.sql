CREATE TABLE [dbo].[Setting] (
    [SettingId] INT           IDENTITY (1, 1) NOT NULL,
    [ProcessId] INT           NOT NULL,
    [Key]       VARCHAR (50)  NOT NULL,
    [Value]     VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([SettingId] ASC),
    CONSTRAINT [FK_Setting_Setting] FOREIGN KEY ([ProcessId]) REFERENCES [dbo].[Process] ([ProcessId])
);



