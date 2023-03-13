
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetFertilizerLines')
	DROP PROCEDURE usp_GetFertilizerLines
GO
CREATE PROCEDURE usp_GetFertilizerLines
(
	@CalculationID		int
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	
			FertilizerLineID,          
			CalculationID,          
			FertilizerID,          
			NoOfBags,          
			FertilizerName,          
			Fertilizer_N,          
			Fertilizer_P,          
			Fertilizer_K,          
			cboDisplay,          
			npkRatio,          
			isActive,          
			FertilizerCategory                   
	from	FertilizerLines
	where	CalculationID = @CalculationID
	AND		isActive = 1
GO

