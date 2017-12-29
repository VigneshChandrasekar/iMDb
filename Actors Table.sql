CREATE TABLE [dbo].[Actors] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [Sex]          VARCHAR (20)  NULL,
    [DOB]          DATE          NULL,
    [Bio]          VARCHAR (500) NULL,
    [DateCreated]  DATETIME      NULL,
    [DateModified] DATETIME      NULL,
    CONSTRAINT [PK_Actors] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Actors]
    ON [dbo].[Actors]([Name] ASC);

