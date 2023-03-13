
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetCalculateFor100Yield')
	DROP PROCEDURE usp_GetCalculateFor100Yield
GO
CREATE PROCEDURE usp_GetCalculateFor100Yield
(
	@CalculationID		int
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	
			CalculateFor100YieldID,          
			CalculationID,          
			NoOfBags,          
			FertilizerName,          
			N,          
			P,          
			K,          
			N_Output,          
			P_Output,          
			K_Output                          
	from	CalculateFor100Yield
	where	CalculationID = @CalculationID
GO

