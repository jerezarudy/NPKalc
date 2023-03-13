
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_GetUser')
	DROP PROCEDURE usp_GetUser
GO
CREATE PROCEDURE usp_GetUser
(
	@Username		nvarchar(500)
)
AS
SET NOCOUNT OFF
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	SELECT	*                         
	from	Users
	where	Username = @Username
GO

