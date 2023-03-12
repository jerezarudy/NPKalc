
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetAllCalculations')
	DROP PROCEDURE usp_GetAllCalculations
GO
CREATE PROCEDURE usp_GetAllCalculations
(
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	CalculationID,          
			TownCity,          
			Barangay,          
			NameOfFarmer,          
			LandArea,          
			SoilType,          
			Season,          
			Nitrogen,          
			Phosphorous,          
			Potassium,          
			Recommended_Nitrogen,          
			Recommended_Phosphorous,          
			Recommended_Potassium,          
			FertilizerLineID,          
			CalculateFor100YieldID,          
			CalculateForProjectedYieldID,          
			isActive,          
			CreatedDateTime,          
			TotalPercentage,
			CONCAT('(', Recommended_Nitrogen, ' - ', Recommended_Phosphorous, ' - ', Recommended_Potassium, ')') as RecommendedNPK
	from	Calculations
	where	isActive = 1
GO

