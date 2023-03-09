
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_CalculateNPK_100Yield')
	DROP PROCEDURE usp_CalculateNPK_100Yield
GO

IF EXISTS (SELECT * FROM sys.types WHERE name = 'type_CalculateNPK_100Yield')
	DROP TYPE type_CalculateNPK_100Yield
GO

CREATE TYPE type_CalculateNPK_100Yield as TABLE
(
	[FertilizerID]			int,
	[NoOfBags]				int,
	[FertilizerName]		NVARCHAR(500),
	[Fertilizer_N]			int,
	[Fertilizer_P]			int,
	[Fertilizer_K]			int,
	[cboDisplay]			NVARCHAR(500),
	[npkRatio]				NVARCHAR(500),
	[isActive]				BIT,
	[FertilizerCategory]	int
)
GO

CREATE PROCEDURE usp_CalculateNPK_100Yield
(
	@dtFertilizers		type_CalculateNPK_100Yield READONLY,
	@LandArea			 Decimal(30,2) = 0,
	@N					int,
	@P					int,
	@K					int,
	@calculationType	int = 1,
	@N_Counter			int = 0,
	@P_Counter			int = 0,
	@K_Counter			int = 0

)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	DECLARE @dtResult as Table(NoOfBags Decimal(11,2), FertilizerName NVARCHAR(500), N INT, P INT, K INT)
	DECLARE @dtFertilizersToLoop 
	as Table(
				[UniqueID] INT IDENTITY(1,1), 
				[FertilizerID]  int,
				[NoOfBags]  int,
				[FertilizerName]  NVARCHAR(500),
				[Fertilizer_N]  int,
				[Fertilizer_P]  int,
				[Fertilizer_K]  int,
				[cboDisplay]  NVARCHAR(500),
				[npkRatio]  NVARCHAR(500),
				[isActive]  BIT,
				[FertilizerCategory]  int
			)
	DECLARE @N_Output Decimal(30,2) = 0
	DECLARE @P_Output Decimal(30,2) = 0
	DECLARE @K_Output Decimal(30,2) = 0
	DECLARE @SKIP_P BIT = 0
	DECLARE @SKIP_K BIT = 0
	DECLARE @SKIP_SUB_N BIT = 0
	DECLARE @SKIP_SUB_P BIT = 0
	DECLARE @SKIP_SUB_K BIT = 0
	DECLARE @RowCount INT= 1
	DECLARE @LoopCounter INT= 1
	
	DECLARE @N_Numerator Decimal(30,2)
	DECLARE @P_Numerator Decimal(30,2)
	DECLARE @K_Numerator Decimal(30,2)

	DECLARE @N_Denominator Decimal(30,2)
	DECLARE @P_Denominator Decimal(30,2)
	DECLARE @K_Denominator Decimal(30,2)
	
	DECLARE @N_CalculationStatus BIT = 0
	DECLARE @P_CalculationStatus BIT = 0
	DECLARE @K_CalculationStatus BIT = 0

	
	DECLARE @SelectedUniqueID INT= 1
	DECLARE @SelectedFertilizerName NVARCHAR(500)

	INSERT INTO @dtFertilizersToLoop 
		(
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
	select * from @dtFertilizers order by FertilizerCategory desc

	


	-- Calculation for Nitrogen
	if(@calculationType = 3)
		BEGIN
			if(@N_Counter = 1)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of N
							if(@N_CalculationStatus = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @N_Numerator = @N * @LandArea
									SET @N_Output = @N_Numerator/@N_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
									SET @N_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_P = 1;
									SET @SKIP_SUB_K = 1;
									
								END
								
							-- start the calculation of P
							if(@P_CalculationStatus = 0 AND @SKIP_SUB_P = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									IF(@P_Denominator >0)
									BEGIN
										SET @P_Numerator = (@P * @LandArea) - (@N_Output * 0.14)
										SET @P_Output = @P_Numerator/@P_Denominator;
										SET @P_CalculationStatus = 1;

										INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
										SET @SKIP_SUB_N = 1;
										SET @SKIP_SUB_K = 1;

									END
									
									SET @P_CalculationStatus = 1;
								END
								
								
							-- start the calculation of K
							if(@K_CalculationStatus = 0 AND @SKIP_SUB_K = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = (@K * @LandArea) - (@N_Output * 0.14)
									SET @K_Output = @K_Numerator/@K_Denominator;
									SET @K_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					SET @SKIP_P = 1
				END
			
			if(@P_Counter = 1 AND @SKIP_P = 0)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of P
							if(@P_CalculationStatus = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @P_Numerator = @P * @LandArea
									SET @P_Output = @P_Numerator/@P_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
									SET @P_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_K = 1;
									
								END
								
							-- start the calculation of N
							if(@N_CalculationStatus = 0 AND @SKIP_SUB_N = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									IF(@N_Denominator > 0)
									BEGIN
										SET @N_Numerator = (@N * @LandArea) - (@P_Output * 0.14)
										SET @N_Output = @N_Numerator/@N_Denominator;

										INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
										SET @SKIP_SUB_P = 1;
										SET @SKIP_SUB_K = 1;
									END
									SET @N_CalculationStatus = 1;
								END
								
								
							-- start the calculation of K
							if(@K_CalculationStatus = 0 AND @SKIP_SUB_K = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = (@K * @LandArea) - (@P_Output * 0.14)
									SET @K_Output = @K_Numerator/@K_Denominator;
									SET @K_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					SET @SKIP_K = 1
				END
				
			if(@K_Counter = 1 AND @SKIP_K = 0)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of K
							if(@K_CalculationStatus = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = @K * @LandArea
									SET @K_Output = @K_Numerator/@K_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @K_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
									
								END
								
							-- start the calculation of N
							if(@N_CalculationStatus = 0 AND @SKIP_SUB_N = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @N_Numerator = (@N * @LandArea) - (@K_Output * 0.14)
									SET @N_Output = @N_Numerator/@N_Denominator;
									SET @N_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_P = 1;
									SET @SKIP_SUB_K = 1;
								END
								
								
							-- start the calculation of P
							if(@P_CalculationStatus = 0 AND @SKIP_SUB_P = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @P_Numerator = (@P * @LandArea) - (@K_Output * 0.14)
									SET @P_Output = @P_Numerator/@P_Denominator;
									SET @P_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_K = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					
					SET @SKIP_K = 1
				END
		END

	
	if(@calculationType = 2)
		BEGIN
			if(@N_Counter = 1)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of N
							if(@N_CalculationStatus = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @N_Numerator = @N * @LandArea
									SET @N_Output = @N_Numerator/@N_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
									SET @N_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_P = 1;
									SET @SKIP_SUB_K = 1;
									
								END
								
							-- start the calculation of P
							if(@P_CalculationStatus = 0 AND @SKIP_SUB_P = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @P_Numerator = (@P * @LandArea) - (@N_Output * 0.14)
									SET @P_Output = @P_Numerator/@P_Denominator;
									SET @P_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_K = 1;
								END
								
								
							-- start the calculation of K
							if(@K_CalculationStatus = 0 AND @SKIP_SUB_K = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = (@K * @LandArea) - (@N_Output * 0.14)
									SET @K_Output = @K_Numerator/@K_Denominator;
									SET @K_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					SET @SKIP_P = 1
				END
			
			if(@P_Counter = 1 AND @SKIP_P = 0)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of P
							if(@P_CalculationStatus = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @P_Numerator = @P * @LandArea
									SET @P_Output = @P_Numerator/@P_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
									SET @P_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_K = 1;
									
								END
								
							-- start the calculation of N
							if(@N_CalculationStatus = 0 AND @SKIP_SUB_N = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @N_Numerator = (@N * @LandArea) - (@P_Output * 0.14)
									SET @N_Output = @N_Numerator/@N_Denominator;
									SET @N_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_P = 1;
									SET @SKIP_SUB_K = 1;
								END
								
								
							-- start the calculation of K
							if(@K_CalculationStatus = 0 AND @SKIP_SUB_K = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = (@K * @LandArea) - (@P_Output * 0.14)
									SET @K_Output = @K_Numerator/@K_Denominator;
									SET @K_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					SET @SKIP_K = 1
				END
				
			if(@K_Counter = 1 AND @SKIP_K = 0)
				BEGIN
					SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
					while(@RowCount >= @LoopCounter)
						BEGIN
							SET @SKIP_SUB_N = 0
							SET @SKIP_SUB_P = 0
							SET @SKIP_SUB_K = 0
							SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
							-- start the calculation of K
							if(@K_CalculationStatus = 0)
								BEGIN
									SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @K_Numerator = @K * @LandArea
									SET @K_Output = @K_Numerator/@K_Denominator;
									 
									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
									SET @K_CalculationStatus = 1;		-- For Complete Fertilizer
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_P = 1;
									
								END
								
							-- start the calculation of N
							if(@N_CalculationStatus = 0 AND @SKIP_SUB_N = 0)
								BEGIN
									SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @N_Numerator = (@N * @LandArea) - (@K_Output * 0.14)
									SET @N_Output = @N_Numerator/@N_Denominator;
									SET @N_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_P = 1;
									SET @SKIP_SUB_K = 1;
								END
								
								
							-- start the calculation of P
							if(@P_CalculationStatus = 0 AND @SKIP_SUB_P = 0)
								BEGIN
									SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
									SET @P_Numerator = (@P * @LandArea) - (@K_Output * 0.14)
									SET @P_Output = @P_Numerator/@P_Denominator;
									SET @P_CalculationStatus = 1;

									INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
									SET @SKIP_SUB_N = 1;
									SET @SKIP_SUB_K = 1;
								END


							SET @LoopCounter = @LoopCounter + 1;
						END
					
					SET @SKIP_K = 1
				END
		END


	
	if(@calculationType = 1)
		BEGIN
			SET @RowCount = (SELECT top 1 UniqueID from @dtFertilizersToLoop order by 1 desc) -- get row count
			while(@RowCount >= @LoopCounter)
				BEGIN
					SET @SKIP_SUB_N = 0
					SET @SKIP_SUB_P = 0
					SET @SKIP_SUB_K = 0
					SET @SelectedUniqueID = (SELECT UniqueID from @dtFertilizersToLoop where UniqueID = @LoopCounter)
					SET @SelectedFertilizerName = (SELECT FertilizerName from @dtFertilizersToLoop where UniqueID = @LoopCounter)
					-- start the calculation of N
					if(@N_CalculationStatus = 0)
						BEGIN
							SET @N_Denominator = (SELECT top 1 Fertilizer_N from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
							SET @N_Numerator = @N * @LandArea
							SET @N_Output = @N_Numerator/@N_Denominator;
									 
							INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@N_Output, @SelectedFertilizerName)
							SET @N_CalculationStatus = 1;		-- For Complete Fertilizer
							SET @SKIP_SUB_P = 1;
							SET @SKIP_SUB_K = 1;
									
						END
								
					-- start the calculation of N
					if(@P_CalculationStatus = 0 AND @SKIP_SUB_P = 0)
						BEGIN
							SET @P_Denominator = (SELECT top 1 Fertilizer_P from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
							SET @P_Numerator = (@P * @LandArea)
							SET @P_Output = @P_Numerator/@P_Denominator;
							SET @P_CalculationStatus = 1;

							INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@P_Output, @SelectedFertilizerName)
							SET @SKIP_SUB_N = 1;
							SET @SKIP_SUB_K = 1;
						END
								
								
					-- start the calculation of K
					if(@K_CalculationStatus = 0 AND @SKIP_SUB_K = 0)
						BEGIN
							SET @K_Denominator = (SELECT top 1 Fertilizer_K from @dtFertilizersToLoop where UniqueID = @SelectedUniqueID) * 0.01
							SET @K_Numerator = (@K * @LandArea)
							SET @K_Output = @K_Numerator/@K_Denominator;
							SET @K_CalculationStatus = 1;

							INSERT INTO @dtResult(NoOfBags, FertilizerName) VALUES (@K_Output, @SelectedFertilizerName)
							SET @SKIP_SUB_N = 1;
							SET @SKIP_SUB_P = 1;
						END


					SET @LoopCounter = @LoopCounter + 1;
				END
			SET @SKIP_K = 1
		END



	--INSERT into @dtResult VALUES
	--(
	--	1,'100% test', 100,100,100
	--)
	if(@N_Output < 0 OR @P_Output < 0 OR @K_Output < 0)
	BEGIN
		RAISERROR('Invalid combination of NPK Ratio. Please conform to the Recommended Fertilizer Rate', 16, 1);
	END
	ELSE
	BEGIN
		select *,@N_Output as N_Output, @P_Output as P_Output , @K_Output as K_Output from @dtResult
	END

GO

