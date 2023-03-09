
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetAllSoilTypes')
	DROP PROCEDURE usp_GetAllSoilTypes
GO
CREATE PROCEDURE usp_GetAllSoilTypes
(
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	SoilTypeID,
			Description,
			isActive
	from	SoilTypes
GO

