CREATE TABLE [dbo].[MusicDirectors] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [DOB]          DATE          NULL,
    [Sex]          VARCHAR (20)  NULL,
    [Bio]          VARCHAR (500) NULL,
    [DateCreated]  DATETIME      NULL,
    [DateModified] DATETIME      NULL,
    CONSTRAINT [PK_MusicDirectors] PRIMARY KEY CLUSTERED ([ID] ASC)
);

