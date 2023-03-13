
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetCalculation')
	DROP PROCEDURE usp_GetCalculation
GO
CREATE PROCEDURE usp_GetCalculation
(
	@CalculationID		int
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	
		CalculationID,          
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
			TotalPercentage          
	from	Calculations
	where	CalculationID = @CalculationID
	AND		isActive = 1
GO

