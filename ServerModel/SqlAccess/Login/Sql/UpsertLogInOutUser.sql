
/*

--UpsertLogInOutUser.Sql

--select * from MS_LogInOut

DECLARE @Id int = 1;
DECLARE @OAuthUserId NVARCHAR(MAX) = '12345566';
DECLARE @UserId UNIQUEIDENTIFIER = NEWID();

*/



MERGE INTO MS_LogInOut as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 LogoutDateTime = GETDATE()

WHEN NOT MATCHED THEN
        INSERT (
             LoginDateTime
			,OAuthUserId
			,UserId)
        VALUES
           (
            GETDATE()
		   ,@OAuthUserId
		   ,@UserId
		   )

OUTPUT INSERTED.Id;


