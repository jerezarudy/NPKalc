
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetAllFertilizers')
	DROP PROCEDURE usp_GetAllFertilizers
GO
CREATE PROCEDURE usp_GetAllFertilizers
(
	@isActive		BIT = 0
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	FertilizerID,
			FertilizerName,
			Fertilizer_N,
			Fertilizer_P,
			Fertilizer_K,
			CONCAT(FertilizerName, ' (',Fertilizer_N, ' - ',Fertilizer_P, ' - ',Fertilizer_K, ')') as cboDisplay,
			CONCAT('(',Fertilizer_N, ' - ',Fertilizer_P, ' - ',Fertilizer_K, ')') as npkRatio,
			isActive,
			FertilizerCategory
	from	Fertilizers
	where	isActive = 1
GO

