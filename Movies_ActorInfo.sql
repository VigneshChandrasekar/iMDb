CREATE TABLE [dbo].[Movies_ActorInfo] (
    [ID]           INT      IDENTITY (1, 1) NOT NULL,
    [MovieID]      INT      NULL,
    [ActorID]      INT      NULL,
    [DateCreated]  DATETIME NULL,
    [DateModified] DATETIME NULL,
    CONSTRAINT [PK_Movies_ActorInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Movies_ActorInfo_Movies] FOREIGN KEY ([MovieID]) REFERENCES [dbo].[Movies] ([ID]),
    CONSTRAINT [FK_Movies_ActorInfo_Actors] FOREIGN KEY ([ActorID]) REFERENCES [dbo].[Actors] ([ID])
);

