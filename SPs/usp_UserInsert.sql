
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_UserInsert')
	DROP PROCEDURE usp_UserInsert
GO

CREATE PROCEDURE usp_UserInsert
(
	@FullName					NVARCHAR(500) = NULL,
	@Username					NVARCHAR(500) = NULL,
	@UserPassword				NVARCHAR(Max) = NULL
)
--WITH ENCRYPTION 
AS

SET NOCOUNT OFF
SET XACT_ABORT ON --FORCE ROLLBACK IF RUNTIME ERROR OCCURS
	
	BEGIN TRY
		BEGIN TRANSACTION 
			INSERT INTO Users
				(
					FullName, 
					Username, 
					UserPassword, 
					isActive
				) 
				values
				(
					@FullName, 
					@Username, 
					@UserPassword, 
					1
				)

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