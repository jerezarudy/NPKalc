
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_CalculationInsert')
	DROP PROCEDURE usp_CalculationInsert
GO
IF EXISTS (SELECT * FROM sys.types WHERE name = 'type_dtFertilizers') DROP TYPE type_dtFertilizers
GO
CREATE TYPE type_dtFertilizers AS TABLE
(
	FertilizerID		INT,
	NoOfBags			Decimal(11,2),
	FertilizerName		NVARCHAR(500),
	Fertilizer_N		Decimal(11,2),
	Fertilizer_P		Decimal(11,2),
	Fertilizer_K		Decimal(11,2),
	cboDisplay			NVARCHAR(500),
	npkRatio			NVARCHAR(500),
	isActive			BIT,
	FertilizerCategory	INT
)
GO


IF EXISTS (SELECT * FROM sys.types WHERE name = 'type_dtFor100Yield') DROP TYPE type_dtFor100Yield
GO
CREATE TYPE type_dtFor100Yield AS TABLE
(
	NoOfBags			Decimal(11,2),
	FertilizerName		NVARCHAR(500),
	N					Decimal(11,2),
	P					Decimal(11,2),
	K					Decimal(11,2),
	N_Output			Decimal(11,2),
	P_Output			Decimal(11,2),
	K_Output			Decimal(11,2)
)
GO

IF EXISTS (SELECT * FROM sys.types WHERE name = 'type_dtForProjectedYield') DROP TYPE type_dtForProjectedYield
GO
CREATE TYPE type_dtForProjectedYield AS TABLE
(
	NoOfBags			Decimal(11,2),
	FertilizerName		NVARCHAR(500),
	N_Output			Decimal(11,2),
	P_Output			Decimal(11,2),
	K_Output			Decimal(11,2),
	NoOfKgs				Decimal(11,2),
	NoTotalOfKgs		Decimal(11,2),
	N					Decimal(11,2),
	P					Decimal(11,2),
	K					Decimal(11,2),
	N_Percentage		Decimal(11,2),
	P_Percentage		Decimal(11,2),
	K_Percentage		Decimal(11,2),
	TEST1				Decimal(11,2),
	TEST2				Decimal(11,2),
	TEST3				Decimal(11,2),
	Test1Reference		INT,
	Test2Reference		INT,
	Test3Reference		INT,
	ProjectedAmount		Decimal(11,2),
	ProjectedPercentage	Decimal(11,2)
)
GO

CREATE PROCEDURE usp_CalculationInsert
(
	@TownCity					NVARCHAR(500) = NULL,
	@Barangay					NVARCHAR(500) = NULL,
	@NameOfFarmer				NVARCHAR(500) = NULL,
	@LandArea					INT = NULL,
	@SoilType					NVARCHAR(500) = NULL,
	@Season						NVARCHAR(500) = NULL,
	
	@Nitrogen 					INT = NULL,
	@Phosphorous				INT = NULL,
	@Potassium					INT = NULL,

	@N							INT = NULL, -- recommended
	@P							INT = NULL, -- recommended
	@K							INT = NULL, -- recommended

	@dtFertilizers				type_dtFertilizers READONLY,
	@dtFor100Yield				type_dtFor100Yield READONLY,
	@dtForProjectedYield		type_dtForProjectedYield READONLY,
	@Total						NVARCHAR(30) = NULL
)
--WITH ENCRYPTION 
AS

SET NOCOUNT OFF
SET XACT_ABORT ON --FORCE ROLLBACK IF RUNTIME ERROR OCCURS
	
	BEGIN TRY
		BEGIN TRANSACTION 

		DECLARE @CalculationID INT

			INSERT INTO Calculations	
				(
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
					isActive,
					TotalPercentage
				)
				VALUES
				(
					@TownCity,
					@Barangay,
					@NameOfFarmer,
					@LandArea,
					@SoilType,
					@Season,
					@Nitrogen,
					@Phosphorous,
					@Potassium,
					@N,
					@P,
					@K,
					1,
					@Total
				)

		SET @CalculationID = (SELECT SCOPE_IDENTITY())

		-- Fertilizerlines
				INSERT INTO FertilizerLines
				(          
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
				)
				SELECT 
					@CalculationID,
					FertilizerID,          
					NoOfBags,          
					FertilizerName,          
					Fertilizer_N,          
					Fertilizer_P,          
					Fertilizer_K,          
					cboDisplay,          
					npkRatio,          
					1,          
					FertilizerCategory
				FROM @dtFertilizers

		-- 100 yield
				INSERT INTO CalculateFor100Yield
				(          
					CalculationID,          
					NoOfBags,
					FertilizerName,
					N,
					P,
					K,
					N_Output,
					P_Output,
					K_Output      
				)
				SELECT 
					@CalculationID,
					NoOfBags		,   
					FertilizerName	,
					N				,     
					P				,   
					K				,   
					N_Output		,   
					P_Output		, 
					K_Output		
				FROM @dtFor100Yield

		-- Projected Yield
				INSERT INTO CalculateForProjectedYield
				(
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
				)
				SELECT
					@CalculationID,
					NoOfBags,
					FertilizerName,
					N,
					P,
					K,
					N_Output,
					P_Output,
					K_Output,
					ProjectedAmount,
					ProjectedPercentage

				FROM @dtForProjectedYield
				
	
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		
	        DECLARE @ErrorNum INT = ERROR_NUMBER();  
	        DECLARE @ErrorLine INT = ERROR_LINE();  
	        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();  
	        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();  
	        DECLARE @ErrorState INT = ERROR_STATE();  
			THROW 51000,@ErrorMsg,1;
			--RAISERROR(@ErrorMsg, @ErrorSeverity, @ErrorState);  --RAISERROR NOT SUPPORTED BY XACT_ABORT
	END CATCH
GO