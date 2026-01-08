
---Sql.UpdateGeneratedDocStatusById

/*
DECLARE @status NVARCHAR(50) = 'Confirmed';
DECLARE @docId INT = 77;
*/

DECLARE @Success BIT = 0;

Update EMP_GenDocs
	SET Status = @status
		WHERE Id = @docId
			AND Ammend = 0
			AND Status IN ('NotConfirmed')

IF @@ROWCOUNT > 0
    SET @Success = 1;  -- TRUE
ELSE
    SET @Success = 0;  -- FALSE

SELECT CAST(@Success AS BIT) AS IsSuccessful;