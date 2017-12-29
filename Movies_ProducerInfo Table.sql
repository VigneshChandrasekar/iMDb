CREATE TABLE [dbo].[Movies_ProducerInfo] (
    [ID]           INT      IDENTITY (1, 1) NOT NULL,
    [MovieID]      INT      NULL,
    [ProducerID]   INT      NULL,
    [DateCreated]  DATETIME NULL,
    [DateModified] DATETIME NULL,
    CONSTRAINT [PK_Movies_ProducerInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Movies_ProducerInfo_Producers] FOREIGN KEY ([MovieID]) REFERENCES [dbo].[Movies] ([ID]),
    CONSTRAINT [FK_Movies_ProducerInfo_Producers1] FOREIGN KEY ([ProducerID]) REFERENCES [dbo].[Producers] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Movies_ProducerInfo]
    ON [dbo].[Movies_ProducerInfo]([MovieID] ASC);

