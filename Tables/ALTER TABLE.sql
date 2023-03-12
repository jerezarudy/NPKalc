ALTER TABLE CalculateForProjectedYield
ADD SuggestedAmount Decimal(11,2) null,
FertilizerPercentage Decimal(11,2) null

ALTER TABLE Calculations
ADD TotalPercentage Decimal(11,2) null,
	CreatedDateTime DateTime Not Null DEFAULT GETDATE()

select * from Calculations
select * from CalculateForProjectedYield