CREATE TABLE [dbo].[Movies_DirectorInfo] (
    [ID]           INT      IDENTITY (1, 1) NOT NULL,
    [MovieID]      INT      NULL,
    [DirectorID]   INT      NULL,
    [DateCreated]  DATETIME NULL,
    [DateModified] DATETIME NULL,
    CONSTRAINT [PK_Movies_DirectorInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Movies_DirectorInfo_Directors] FOREIGN KEY ([DirectorID]) REFERENCES [dbo].[Directors] ([ID]),
    CONSTRAINT [FK_Movies_DirectorInfo_Movies] FOREIGN KEY ([MovieID]) REFERENCES [dbo].[Movies] ([ID])
);

