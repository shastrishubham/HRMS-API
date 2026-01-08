

--UpsertReimbursementClaims.Sql

--select * from HR_HelpDesk

--DECLARE @Id INT =2;
--DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
--DECLARE @EMP_Info_Id uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @MS_TicketCat_Id INT = 1;
--DECLARE @TicketName NVARCHAR(MAX) = 'tick 1-2';
--DECLARE @Subject NVARCHAR(MAX) = '';
--DECLARE @Description NVARCHAR(MAX) = '';
--DECLARE @Severity INT = 0;
--DECLARE @AttachmentPath NVARCHAR(MAX) = '';
--DECLARE @AssignEmpId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
--DECLARE @TicketStatus NVARCHAR(50) = 'Open';
--DECLARE @TicketResolvedDt DATETIME = NULL;
--DECLARE @Active BIT = 1;

DECLARE @NewId INT;
DECLARE @FinalAmmendId INT = NULL;

BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. If record exists, mark as amended
    IF EXISTS (SELECT 1 FROM HR_HelpDesk WHERE Id = @Id)
    BEGIN
        UPDATE HR_HelpDesk
        SET IsAmmend = 1
        WHERE Id = @Id
			AND EMP_Info_Id = @EMP_Info_Id;

		SET @FinalAmmendId = @Id;
    END

    -- 2. Insert new record
    INSERT INTO HR_HelpDesk (
         FormDate,
         CompId,
         EMP_Info_Id,
         MS_TicketCat_Id,
         TicketName,
         [Subject],
         [Description],
         Severity,
         AttachmentPath,
         AssignEmpId,
         TicketStatus,
         TicketResolvedDt,
         IsAmmend,
		 [AmmendId],
         Active
    )
    VALUES (
         GETDATE(),
         @CompId,
         @EMP_Info_Id,
         @MS_TicketCat_Id,
         @TicketName,
         @Subject,
         @Description,
         @Severity,
         @AttachmentPath,
         @AssignEmpId,
         @TicketStatus,
         @TicketResolvedDt,
         0,
		 @FinalAmmendId,
         1
    );
	
	SET @NewId = SCOPE_IDENTITY();
    
	COMMIT;

    SELECT @NewId AS InsertedId;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;

    DECLARE @Error NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@Error, 16, 1);
END CATCH;

