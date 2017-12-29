CREATE TABLE [dbo].[Movies] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NULL,
    [Plot]         VARCHAR (MAX)  NULL,
    [Poster]       IMAGE          NULL,
    [PosterPath]   NVARCHAR (256) NULL,
    [Release Date] INT            NULL,
    [DateCreated]  DATETIME       NULL,
    [DateModified] DATETIME       NULL,
    CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Movies]
    ON [dbo].[Movies]([Name] ASC);

