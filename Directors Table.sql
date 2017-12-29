CREATE TABLE [dbo].[Directors] (
    [ID]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50)  NULL,
    [Sex]          VARCHAR (20)  NULL,
    [DOB]          DATE          NULL,
    [Bio]          VARCHAR (500) NULL,
    [DateCreated]  DATETIME      NULL,
    [DateModified] DATETIME      NULL,
    CONSTRAINT [PK_Directors] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Directors]
    ON [dbo].[Directors]([Name] ASC);

