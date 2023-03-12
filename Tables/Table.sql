

CREATE TABLE [dbo].[SoilTypes]
(
	SoilTypeID int identity(1,1) ,
	Description nvarchar (500) null,
	SoilType_N int null,
	SoilType_P int null,
	SoilType_K int null,
	isActive BIT not NULL Default 1,
	CONSTRAINT [PK_SoilType] PRIMARY KEY CLUSTERED ([SoilTypeID] ASC)
);


CREATE TABLE [dbo].[Seasons]
(
	SeasonID int identity(1,1) ,
	Description nvarchar (500) null,
	isActive BIT not NULL Default 1,
	CONSTRAINT [PK_Seasons] PRIMARY KEY CLUSTERED ([SeasonID] ASC)
);

CREATE TABLE [dbo].[Fertilizers]
(
	FertilizerID int identity(1,1) ,
	FertilizerName nvarchar (500) null,
	Fertilizer_N int null,
	Fertilizer_P int null,
	Fertilizer_K int null,
	isActive BIT not NULL Default 1,
	CONSTRAINT [PK_Fertilizers] PRIMARY KEY CLUSTERED ([FertilizerID] ASC)
);

CREATE TABLE [dbo].[Calculations]
(
	CalculationID int identity(1,1) ,
	TownCity nvarchar (1000) null,
	Barangay nvarchar (1000) null,
	NameOfFarmer nvarchar (1000) null,
	LandArea int null,
	SoilType nvarchar (500) null,
	Season nvarchar (500) null,
	Nitrogen int null,
	Phosphorous int null,
	Potassium int null,
	Recommended_Nitrogen int null,
	Recommended_Phosphorous int null,
	Recommended_Potassium int null,
	FertilizerLineID int null,
	CalculateFor100YieldID int null,
	CalculateForProjectedYieldID int null,
	isActive BIT not NULL Default 1,
	CONSTRAINT [PK_Calculations] PRIMARY KEY CLUSTERED ([CalculationID] ASC)
);

CREATE TABLE [dbo].[FertilizerLines]
(
	FertilizerLineID int identity(1,1) ,
	CalculationID int null,
	FertilizerID int null,
	NoOfBags Decimal(11,2) null,
	FertilizerName nvarchar (500) null,
	Fertilizer_N Decimal(11,2) null,
	Fertilizer_P Decimal(11,2) null,
	Fertilizer_K Decimal(11,2) null,
	cboDisplay nvarchar (500) null,
	npkRatio nvarchar (500) null,
	isActive BIT not NULL Default 1,
	FertilizerCategory int null,
	CONSTRAINT [PK_FertilizerLines] PRIMARY KEY CLUSTERED ([FertilizerLineID] ASC)
);

CREATE TABLE [dbo].[CalculateFor100Yield]
(
	CalculateFor100YieldID int identity(1,1) ,
	CalculationID int null,
	NoOfBags Decimal(11,2) null,
	FertilizerName nvarchar (500) null,
	N Decimal(11,2) null,
	P Decimal(11,2) null,
	K Decimal(11,2) null,
	N_Output Decimal(11,2) null,
	P_Output Decimal(11,2) null,
	K_Output Decimal(11,2) null,
	CONSTRAINT [PK_CalculateFor100Yield] PRIMARY KEY CLUSTERED ([CalculateFor100YieldID] ASC)
);

CREATE TABLE [dbo].[CalculateForProjectedYield]
(
	CalculateForProjectedYieldID int identity(1,1) ,
	CalculationID int null,
	NoOfBags Decimal(11,2) null,
	FertilizerName nvarchar (500) null,
	N Decimal(11,2) null,
	P Decimal(11,2) null,
	K Decimal(11,2) null,
	N_Output Decimal(11,2) null,
	P_Output Decimal(11,2) null,
	K_Output Decimal(11,2) null,
	CONSTRAINT [PK_CalculateForProjectedYield] PRIMARY KEY CLUSTERED ([CalculateForProjectedYieldID] ASC)
);