
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_CalculateNPK_ProjectedYieldV2')
	DROP PROCEDURE usp_CalculateNPK_ProjectedYieldV2
GO

IF EXISTS (SELECT * FROM sys.types WHERE name = 'type_CalculateNPK_ProjectedYieldV2')
	DROP TYPE type_CalculateNPK_ProjectedYieldV2
GO

CREATE TYPE type_CalculateNPK_ProjectedYieldV2 as TABLE
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

CREATE PROCEDURE usp_CalculateNPK_ProjectedYieldV2
(
	@dtFertilizers		type_CalculateNPK_ProjectedYield READONLY,
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
	
	DECLARE @dtResult as Table(NoOfBags Decimal(11,2), FertilizerName NVARCHAR(500), N INT, P INT, K INT)  (exec usp_CalculateNPK_100YieldV2 @dtFertilizers, @LandArea, @N, @P, @K, @calculationType, @N_Counter, @P_Counter, @K_Counter)
	






	--if(@N_Output < 0 OR @P_Output < 0 OR @K_Output < 0)
	--BEGIN
	--	RAISERROR('Invalid combination of NPK Ratio. Please conform to the Recommended Fertilizer Rate', 16, 1);
	--END
	--ELSE
	--BEGIN
	--	select		*,
	--				@N_Output as N_Output, 
	--				@P_Output as P_Output ,
	--				@K_Output as K_Output,
	--				dtf.NoOfBags*50 as NoOfKgs,
	--				dtr.NoOfBags - (dtf.NoOfBags*50) as NoTotalOfKgs
	--	from		@dtResult dtr
	--	LEFT JOIN	@dtFertilizers dtf
	--	on			dtr.FertilizerName = dtf.FertilizerName
	END
GO

