




CREATE VIEW [dbo].[GetMovieInfo_vw]
AS
WITH Results
AS(
SELECT 
	a.ID MovieID,
	a.Name MovieName,
	a.[Release Date],
	a.Plot,
	a.Poster,
	b.Name ProducerName,
	c.Name DirectorName,
	d.Name MusicDirectorName,
	e.Name ActorName
FROM [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies] a
LEFT JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies_ProducerInfo] MIP ON a.ID = MIP.MovieID
INNER JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Producers] b ON MIP.ProducerID = b.ID
LEFT JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies_DirectorInfo] DI ON a.ID = DI.MovieID
INNER JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Directors] c ON DI.DirectorID = c.ID
LEFT JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies_MusicDirectorInfo] MDI ON a.ID = MDI.MovieID
INNER JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[MusicDirectors] d ON MDI.MusicDirectorID = d.ID
LEFT JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies_ActorInfo] AI ON a.ID = AI.MovieID
INNER JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Actors] e ON AI.ActorID = e.ID
),
tempMovies
AS
(
SELECT MovieID,
 		ActorName = 
    STUFF((SELECT DISTINCT ', ' + ActorName
           FROM Results b 
           WHERE b.MovieID = a.MovieID 
          FOR XML PATH('')), 1, 2, ''),
		  MusicDirectorName = 
    STUFF((SELECT DISTINCT ', ' + MusicDirectorName
           FROM Results b 
           WHERE b.MovieID = a.MovieID 
          FOR XML PATH('')), 1, 2, ''),
		  DirectorName = 
    STUFF((SELECT DISTINCT ', ' + DirectorName
           FROM Results b 
           WHERE b.MovieID = a.MovieID 
          FOR XML PATH('')), 1, 2, ''),
		  	  ProducerName			   = 
    STUFF((SELECT DISTINCT ', ' + ProducerName
           FROM Results b 
           WHERE b.MovieID = a.MovieID 
          FOR XML PATH('')), 1, 2, '')
FROM Results a
GROUP BY MovieID
)
SELECT a.MovieID,b.Name MovieName,b.[Release Date],b.Plot,b.Poster,a.ActorName,a.DirectorName,a.MusicDirectorName,a.ProducerName FROM tempMovies a
INNER JOIN [dbb0dc1f95675f4c699625a85601440769].[dbo].[Movies] b ON a.MovieID = b.ID
GO


