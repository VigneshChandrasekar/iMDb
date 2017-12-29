-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--[dbo].[SP_iMDBAddMovie] 
CREATE PROCEDURE [dbo].[SP_iMDBAddMovie]
	-- Add the parameters for the stored procedure here
	@MovieID int,		
	@ActorID VARCHAR(MAX),
	@DirectorID VARCHAR(MAX),
	@MusicDirectorID VARCHAR(MAX),
	@ProducerID int,
	@DateCreated Datetime,
	@DateModified DateTime 
AS
BEGIN
	
	SET NOCOUNT ON;	
	DECLARE @max INT = 0
	
	-- Add Actor Info
	WHILE LEN( @ActorID ) > 0
	BEGIN
		DECLARE @SplitText VARCHAR(100)
		IF CHARINDEX( ',',@ActorID ) > 0
		BEGIN
			SET @SplitText = SUBSTRING(@ActorID,0,CHARINDEX(',',@ActorID))
		END		
		ELSE
		BEGIN
			SET @SplitText = @ActorID
			SET @ActorID = ''
		END	
		INSERT INTO [ICDB].[dbo].Movies_ActorInfo VALUES(@MovieID,CAST( @SplitText AS INT ),@DateCreated,@DateModified)	
		SET @ActorID = REPLACE(@ActorID,@SplitText + ',','')
		SET @max = @max + @@ROWCOUNT
	END

	---- Add MusicDirector Info
	SET @max = 0	
	WHILE LEN( @MusicDirectorID ) > 0
	BEGIN
		DECLARE @SplitText2 VARCHAR(100)
		IF CHARINDEX( ',',@MusicDirectorID ) > 0
		BEGIN
			SET @SplitText2 = SUBSTRING(@MusicDirectorID,0,CHARINDEX(',',@MusicDirectorID))
		END		
		ELSE
		BEGIN
			SET @SplitText2 = @MusicDirectorID
			SET @MusicDirectorID = ''
		END	
		INSERT INTO [ICDB].[dbo].Movies_MusicDirectorInfo VALUES(@MovieID,CAST( @SplitText2 AS INT ),@DateCreated,@DateModified)		
		SET @MusicDirectorID = REPLACE(@MusicDirectorID,@SplitText2 + ',','')
		SET @max = @max + @@ROWCOUNT
	END

	

	---- Add Director Info

	SET @max = 0	
	WHILE LEN( @DirectorID ) > 0
	BEGIN
		DECLARE @SplitText3 VARCHAR(100)
		IF CHARINDEX( ',',@DirectorID ) > 0
		BEGIN
			SET @SplitText3 = SUBSTRING(@DirectorID,0,CHARINDEX(',',@DirectorID))
		END		
		ELSE
		BEGIN
			SET @SplitText3 = @DirectorID
			SET @DirectorID = ''
		END	
		INSERT INTO [ICDB].[dbo].Movies_DirectorInfo VALUES(@MovieID,CAST( @SplitText3 AS INT ),@DateCreated,@DateModified)		
		SET @DirectorID = REPLACE(@DirectorID,@SplitText3 + ',','')
		SET @max = @max + @@ROWCOUNT
	END


	---- Add Director Info

	INSERT INTO [ICDB].[dbo].Movies_ProducerInfo VALUES(@MovieID,@ProducerID,@DateCreated,@DateModified)

END