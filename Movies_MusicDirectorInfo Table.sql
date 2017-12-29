CREATE TABLE [dbo].[Movies_MusicDirectorInfo] (
    [ID]              INT      IDENTITY (1, 1) NOT NULL,
    [MovieID]         INT      NULL,
    [MusicDirectorID] INT      NULL,
    [DateCreated]     DATETIME NULL,
    [DateModified]    DATETIME NULL,
    CONSTRAINT [PK_Movies_MusicDirectorInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Movies_MusicDirectorInfo_MusicDirectors] FOREIGN KEY ([MusicDirectorID]) REFERENCES [dbo].[MusicDirectors] ([ID]),
    CONSTRAINT [FK_Movies_MusicDirectorInfo_Movies] FOREIGN KEY ([MovieID]) REFERENCES [dbo].[Movies] ([ID])
);

