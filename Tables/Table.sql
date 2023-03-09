

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
