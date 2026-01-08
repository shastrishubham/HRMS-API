
--select * from EMP_Qualification


------ UpsertEmployeeQualification
--DECLARE @empId uniqueidentifier = 'F4E804F9-6F67-46B0-BB51-69C1E9DCD82D'

--DECLARE @json nvarchar(max) = 
--'[
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EMP_Info_Id":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"HighestQualification": "HighestQualification",
--		"UniversityName": "UniversityName",
--		"CollegeName":"CollegeName",
--		"MainSubjects":"MainSubjects",
--		"Division":"Division",
--		"PassingYear":789,
--		"Percentage":98.89
--	},
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EMP_Info_Id":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"HighestQualification": "HighestQualification",
--		"UniversityName": "UniversityName",
--		"CollegeName":"CollegeName",
--		"MainSubjects":"MainSubjects",
--		"Division":"Division",
--		"PassingYear":101112,
--		"Percentage":98.89
--	}
--]'


--SELECT * from EMP_SLSetup
--SELECT * from EMP_SLHeads ORDER BY MS_SLHeads_Id ASC

BEGIN TRY
    BEGIN TRANSACTION

	DELETE FROM EMP_Qualification WHERE EMP_Info_Id = @empId

	MERGE EMP_Qualification source
	USING (SELECT 
				 Id AS Id
				,FormDate AS FormDate
				,MachineIp AS MachineIp
				,MachineId AS MachineId
				,CompId AS CompId
				,CreatedBy AS CreatedBy
				,EMP_Info_Id AS EMP_Info_Id
				,HighestQualification AS HighestQualification
				,UniversityName AS UniversityName
				,CollegeName AS CollegeName
				,MainSubjects AS MainSubjects
				,Division AS Division
				,PassingYear AS PassingYear
				,Percentage AS Percentage
       FROM   OPENJSON(@json)
       WITH   (Id UNIQUEIDENTIFIER, FormDate DATE, MachineIp nvarchar(50), MachineId nvarchar(50), CompId UNIQUEIDENTIFIER, CreatedBy UNIQUEIDENTIFIER, EMP_Info_Id UNIQUEIDENTIFIER, HighestQualification nvarchar(max), UniversityName nvarchar(max), CollegeName nvarchar(max), MainSubjects nvarchar(max), Division nvarchar(max), PassingYear int, [Percentage] DECIMAL(18,4))) target
	ON (source.EMP_Info_Id = target.EMP_Info_Id)
	WHEN MATCHED 
		AND target.EMP_Info_Id = source.EMP_Info_Id
	THEN UPDATE SET
		 source.HighestQualification = target.HighestQualification
		,source.UniversityName = target.UniversityName
		,source.CollegeName = target.CollegeName 
		,source.MainSubjects = target.MainSubjects
		,source.Division = target.Division
		,source.PassingYear = target.PassingYear
		,source.[Percentage] = target.[Percentage]
	WHEN NOT MATCHED BY TARGET
	THEN INSERT (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_Info_Id, HighestQualification, UniversityName, CollegeName, MainSubjects, Division, PassingYear, [Percentage])
    VALUES (NEWID(), GETDATE(), target.MachineIp, target.MachineId, target.CompId, target.CreatedBy, target.EMP_Info_Id, target.HighestQualification,target.UniversityName, target.CollegeName, target.MainSubjects, target.Division, target.PassingYear, target.[Percentage]);


COMMIT TRAN -- Transaction Success!
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN --RollBack in case of Error

    -- <EDIT>: From SQL2008 on, you must raise error messages as follows:
    DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  

    SELECT   
       @ErrorMessage = ERROR_MESSAGE(),  
       @ErrorSeverity = ERROR_SEVERITY(),  
       @ErrorState = ERROR_STATE();  

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);  
    -- </EDIT>
END CATCH