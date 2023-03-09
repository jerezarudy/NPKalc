
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetAllSeasons')
	DROP PROCEDURE usp_GetAllSeasons
GO
CREATE PROCEDURE usp_GetAllSeasons
(
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	SeasonID,
			Description,
			isActive
	from	Seasons
GO

