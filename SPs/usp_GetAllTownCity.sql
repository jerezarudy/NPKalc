
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetAllTownCity')
	DROP PROCEDURE usp_GetAllTownCity
GO
CREATE PROCEDURE usp_GetAllTownCity
(
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	TownCityID,
			ProvinceID,
			TownCityName,
			ZIP,
			isActive
	from	TownCity
GO

