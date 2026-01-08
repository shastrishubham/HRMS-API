
----select * from EMP_SLSetup
----select * from EMP_SLHeads

----truncate table EMP_SLSetup
----truncate table EMP_SLHeads


------ AddUpdateEmployeeSalarySetup

----DECLARE @machineIp nvarchar(50) = '1.1.1.1'
----DECLARE @machineId nvarchar(50) = 'Machine'
----DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
----DECLARE @empId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
----DECLARE @MS_SLHeads_Id int = 0
----DECLARE @amount DECIMAL(18,10) = 0
----DECLARE @userId uniqueidentifier = 0



--DECLARE @json nvarchar(max) = 
--'[
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EmpId":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"TotalEarningAmt":1234,
--		"TotalDeductionAmt":45678,
--		"MS_PayMode_Id":1,
--		"Active":1
--	}
--]'



--DECLARE @json1 nvarchar(max) = 
--'[
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EmpId":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"MS_SLHeads_Id": 2,
--		"Amount":0.0,
--		"Active":1
--	},
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EmpId":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"MS_SLHeads_Id": 1,
--		"Amount":7000,
--		"Active":1
--	}

--]'

--SELECT * from EMP_SLSetup
--SELECT * from EMP_SLHeads ORDER BY MS_SLHeads_Id ASC



-- Table to store Id mapping
DECLARE @InsertedIds TABLE (InsertedId UNIQUEIDENTIFIER, EmpId UNIQUEIDENTIFIER);

BEGIN TRY
    BEGIN TRANSACTION;

    -- Parse @json into temp table
    DECLARE @EmployeeSetup TABLE (
        Id UNIQUEIDENTIFIER,
        MachineIp NVARCHAR(50),
        MachineId NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        CreatedBy UNIQUEIDENTIFIER,
        EmpId UNIQUEIDENTIFIER,
        TotalEarningAmt DECIMAL(18,10),
        TotalDeductionAmt DECIMAL(18,10),
        MS_PayMode_Id INT,
        Active BIT
    );

    INSERT INTO @EmployeeSetup
    SELECT * FROM OPENJSON(@json)
    WITH (
        Id UNIQUEIDENTIFIER,
        MachineIp NVARCHAR(50),
        MachineId NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        CreatedBy UNIQUEIDENTIFIER,
        EmpId UNIQUEIDENTIFIER,
        TotalEarningAmt DECIMAL(18,10),
        TotalDeductionAmt DECIMAL(18,10),
        MS_PayMode_Id INT,
        Active BIT
    );

    -- Loop through employee setup
    DECLARE setup_cursor CURSOR FOR
    SELECT * FROM @EmployeeSetup;

    DECLARE @Id UNIQUEIDENTIFIER, @MachineIp NVARCHAR(50), @MachineId NVARCHAR(50),
            @CompId UNIQUEIDENTIFIER, @CreatedBy UNIQUEIDENTIFIER, @EmpId UNIQUEIDENTIFIER,
            @TotalEarningAmt DECIMAL(18,10), @TotalDeductionAmt DECIMAL(18,10),
            @MS_PayMode_Id INT, @Active BIT;

    OPEN setup_cursor;
    FETCH NEXT FROM setup_cursor INTO @Id, @MachineIp, @MachineId, @CompId, @CreatedBy, @EmpId,
                                      @TotalEarningAmt, @TotalDeductionAmt, @MS_PayMode_Id, @Active;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @isSalaryCreatedPrev BIT = 0;
		IF EXISTS (SELECT TOP 1 Id
           FROM EMP_SLHeads 
           WHERE EMP_Info_Id = @EmpId 
             AND Active = 1)
		BEGIN
			SET @isSalaryCreatedPrev = 1;
		END

        IF (@isSalaryCreatedPrev <> 1) 
        BEGIN
            SET @Id = NEWID();
            INSERT INTO EMP_SLSetup (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_Info_Id, TotalEarningAmt, TotalDeductionAmt, MS_PayMode_Id, IsAmmend, Active)
            VALUES (@Id, GETDATE(), @MachineIp, @MachineId, @CompId, @CreatedBy, @EmpId, @TotalEarningAmt, @TotalDeductionAmt, @MS_PayMode_Id, 0, 1);
        END
        ELSE
        BEGIN
            --UPDATE EMP_SLSetup
            --SET TotalEarningAmt = @TotalEarningAmt,
            --    TotalDeductionAmt = @TotalDeductionAmt,
            --    MachineIp = @MachineIp,
            --    MachineId = @MachineId,
            --    CompId = @CompId,
            --    CreatedBy = @CreatedBy,
            --    MS_PayMode_Id = @MS_PayMode_Id,
            --    Active = @Active
            --WHERE Id = @Id;
			UPDATE EMP_SLSetup SET IsAmmend = 1 WHERE EMP_Info_Id = @EmpId AND IsAmmend = 0;

			SET @Id = NEWID();
            INSERT INTO EMP_SLSetup (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_Info_Id, TotalEarningAmt, TotalDeductionAmt, MS_PayMode_Id, IsAmmend, Active)
            VALUES (@Id, GETDATE(), @MachineIp, @MachineId, @CompId, @CreatedBy, @EmpId, @TotalEarningAmt, @TotalDeductionAmt, @MS_PayMode_Id, 0, 1);
        END

        -- Add to Id mapping
        INSERT INTO @InsertedIds (InsertedId, EmpId) VALUES (@Id, @EmpId);

        FETCH NEXT FROM setup_cursor INTO @Id, @MachineIp, @MachineId, @CompId, @CreatedBy, @EmpId,
                                          @TotalEarningAmt, @TotalDeductionAmt, @MS_PayMode_Id, @Active;
    END
    CLOSE setup_cursor;
    DEALLOCATE setup_cursor;

    -------------------------------
    -- Now Process EMP_SLHeads
    -------------------------------

    DECLARE @SalaryHeads TABLE (
        Id UNIQUEIDENTIFIER,
        MachineIp NVARCHAR(50),
        MachineId NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        CreatedBy UNIQUEIDENTIFIER,
        EmpId UNIQUEIDENTIFIER,
        MS_SLHeads_Id INT,
        Amount DECIMAL(18,10),
        Active BIT
    );

    INSERT INTO @SalaryHeads
    SELECT * FROM OPENJSON(@json1)
    WITH (
        Id UNIQUEIDENTIFIER,
        MachineIp NVARCHAR(50),
        MachineId NVARCHAR(50),
        CompId UNIQUEIDENTIFIER,
        CreatedBy UNIQUEIDENTIFIER,
        EmpId UNIQUEIDENTIFIER,
        MS_SLHeads_Id INT,
        Amount DECIMAL(18,10),
        Active BIT
    );

    DECLARE @headId UNIQUEIDENTIFIER, @headMachineIp NVARCHAR(50), @headMachineId NVARCHAR(50),
            @headCompId UNIQUEIDENTIFIER, @headCreatedBy UNIQUEIDENTIFIER, @headEmpId UNIQUEIDENTIFIER,
            @headMS_SLHeads_Id INT, @headAmount DECIMAL(18,10), @headActive BIT;

    DECLARE head_cursor CURSOR FOR
    SELECT * FROM @SalaryHeads;

    OPEN head_cursor;
    FETCH NEXT FROM head_cursor INTO @headId, @headMachineIp, @headMachineId, @headCompId, @headCreatedBy, @headEmpId,
                                       @headMS_SLHeads_Id, @headAmount, @headActive;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @empSlSetupId UNIQUEIDENTIFIER;
        SELECT @empSlSetupId = InsertedId FROM @InsertedIds WHERE EmpId = @headEmpId;

        DECLARE @isSalaryHeadCreatedPrev BIT = 0;
		IF EXISTS (SELECT TOP 1 Id
           FROM EMP_SLHeads 
           WHERE EMP_Info_Id = @headEmpId 
			 AND MS_SLHeads_Id = @headMS_SLHeads_Id
             AND Active = 1)
		BEGIN
			SET @isSalaryHeadCreatedPrev = 1;
		END

        IF (@isSalaryHeadCreatedPrev <> 1)
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM EMP_SLHeads 
                WHERE EMP_Info_Id = @headEmpId AND MS_SLHeads_Id = @headMS_SLHeads_Id
            )
            BEGIN
                INSERT INTO EMP_SLHeads (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_SLSetup_Id, EMP_Info_Id, MS_SLHeads_Id, Amount, IsAmmend, Active)
                VALUES (NEWID(), GETDATE(), @headMachineIp, @headMachineId, @headCompId, @headCreatedBy, @empSlSetupId, @headEmpId, @headMS_SLHeads_Id, @headAmount, 0, 1);
            END
            ELSE
            BEGIN
                -- If exists, update
                --UPDATE EMP_SLHeads
                --SET Amount = @headAmount,
                --    CreatedBy = @headCreatedBy,
                --    MachineIp = @headMachineIp,
                --    MachineId = @headMachineId,
                --    Active = @headActive
                --WHERE EMP_Info_Id = @headEmpId AND MS_SLHeads_Id = @headMS_SLHeads_Id;
				UPDATE EMP_SLHeads SET IsAmmend = 1 WHERE EMP_Info_Id = @headEmpId AND MS_SLHeads_Id = @headMS_SLHeads_Id AND IsAmmend = 0;

				INSERT INTO EMP_SLHeads (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_SLSetup_Id, EMP_Info_Id, MS_SLHeads_Id, Amount, IsAmmend, Active)
                VALUES (NEWID(), GETDATE(), @headMachineIp, @headMachineId, @headCompId, @headCreatedBy, @empSlSetupId, @headEmpId, @headMS_SLHeads_Id, @headAmount, 0, 1);
            END
        END
        ELSE
        BEGIN
            --UPDATE EMP_SLHeads
            --SET Amount = @headAmount,
            --    CreatedBy = @headCreatedBy,
            --    MachineIp = @headMachineIp,
            --    MachineId = @headMachineId,
            --    Active = @headActive
            --WHERE Id = @headId;
			UPDATE EMP_SLHeads SET IsAmmend = 1 WHERE EMP_Info_Id = @headEmpId AND MS_SLHeads_Id = @headMS_SLHeads_Id AND IsAmmend = 0;

			INSERT INTO EMP_SLHeads (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_SLSetup_Id, EMP_Info_Id, MS_SLHeads_Id, Amount, IsAmmend, Active)
                VALUES (NEWID(), GETDATE(), @headMachineIp, @headMachineId, @headCompId, @headCreatedBy, @empSlSetupId, @headEmpId, @headMS_SLHeads_Id, @headAmount, 0, 1);
        END

        FETCH NEXT FROM head_cursor INTO @headId, @headMachineIp, @headMachineId, @headCompId, @headCreatedBy, @headEmpId,
                                          @headMS_SLHeads_Id, @headAmount, @headActive;
    END
    CLOSE head_cursor;
    DEALLOCATE head_cursor;

    COMMIT TRANSACTION;
    PRINT 'Transaction committed successfully.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(4000),
            @ErrorSeverity INT,
            @ErrorState INT;

    SELECT
        @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
