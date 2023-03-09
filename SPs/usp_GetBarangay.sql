
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetBarangay')
	DROP PROCEDURE usp_GetBarangay
GO
CREATE PROCEDURE usp_GetBarangay
(
	@TownCityID		INT,
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT		BarangayID,
				TownCityID,
				BarangayName,
				isActive
	from		Barangay
	WHERE		TownCityID = @TownCityID
	order by	BarangayName ASC
GO

