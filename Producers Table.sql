CREATE TABLE [dbo].[Producers] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [Sex]          VARCHAR (20)  NULL,
    [DOB]          DATE          NULL,
    [Bio]          VARCHAR (500) NULL,
    [DateCreated]  DATETIME      NULL,
    [DateModified] DATETIME      NULL,
    CONSTRAINT [PK_Producers] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Producers]
    ON [dbo].[Producers]([Name] ASC);

