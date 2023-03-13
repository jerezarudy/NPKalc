
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetCalculateForProjectedYield')
	DROP PROCEDURE usp_GetCalculateForProjectedYield
GO
CREATE PROCEDURE usp_GetCalculateForProjectedYield
(
	@CalculationID		int
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	
			CalculateForProjectedYieldID,          
			CalculationID,          
			NoOfBags,          
			FertilizerName,          
			N,          
			P,          
			K,          
			N_Output,          
			P_Output,          
			K_Output,          
			SuggestedAmount,          
			FertilizerPercentage                          
	from	CalculateForProjectedYield
	where	CalculationID = @CalculationID
GO

