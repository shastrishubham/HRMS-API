
--select * from [dbo].[EMP_WorkExp]


------ UpsertEmployeeWorkExperience
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
--		"PreviousEmployer": "PreviousEmployer",
--		"EmployerAddress": "EmployerAddress",
--		"FromDate": "2010-03-28",
--		"ToDate": "2012-01-17",
--		"BasicSalary": 1000,
--		"NetSalary": 8700,
--		"Designation": "Designation",
--		"ReportingSupervisorName": "ReportingSupervisorName",
--		"SupervisorNo": "SupervisorNo",
--		"LeavingReason": "LeavingReason"
--	},
--	{
--		"Id":"00000000-0000-0000-0000-000000000000",
--		"FormDate":"2023-01-25 23:33:32.017",
--		"MachineIp":"",
--		"MachineId":"",
--		"CompId":"00000000-0000-0000-0000-000000000000",
--		"CreatedBy":"00000000-0000-0000-0000-000000000000",
--		"EMP_Info_Id":"F4E804F9-6F67-46B0-BB51-69C1E9DCD82D",
--		"PreviousEmployer": "PreviousEmployer",
--		"EmployerAddress": "EmployerAddress",
--		"FromDate": "2007-03-28",
--		"ToDate": "2009-01-17",
--		"BasicSalary":15000,
--		"NetSalary": 13500,
--		"Designation": "Designation",
--		"ReportingSupervisorName": "ReportingSupervisorName",
--		"SupervisorNo": "SupervisorNo",
--		"LeavingReason": "LeavingReason"
--	}
--]'

BEGIN TRY
    BEGIN TRANSACTION

	DELETE FROM EMP_WorkExp WHERE EMP_Info_Id = @empId

	MERGE EMP_WorkExp source
	USING (SELECT 
				 Id AS Id
				,FormDate AS FormDate
				,MachineIp AS MachineIp
				,MachineId AS MachineId
				,CompId AS CompId
				,CreatedBy AS CreatedBy
				,EMP_Info_Id AS EMP_Info_Id
				,PreviousEmployer AS PreviousEmployer
				,EmployerAddress AS EmployerAddress
				,FromDate AS FromDate
				,ToDate AS ToDate
				,BasicSalary AS BasicSalary
				,NetSalary AS NetSalary
				,Designation AS Designation
				,ReportingSupervisorName AS ReportingSupervisorName
				,SupervisorNo AS SupervisorNo
				,LeavingReason AS LeavingReason
		FROM   OPENJSON(@json)
       WITH   (Id UNIQUEIDENTIFIER, FormDate DATE, MachineIp nvarchar(50), MachineId nvarchar(50), CompId UNIQUEIDENTIFIER, CreatedBy UNIQUEIDENTIFIER, EMP_Info_Id UNIQUEIDENTIFIER, PreviousEmployer nvarchar(max), EmployerAddress nvarchar(max), FromDate DATE, ToDate DATE, BasicSalary DECIMAL(18,0), NetSalary DECIMAL(18,0), Designation nvarchar(max),
	   ReportingSupervisorName nvarchar(max), SupervisorNo nvarchar(50), LeavingReason nvarchar(max))) target
	ON (source.EMP_Info_Id = target.EMP_Info_Id)
	WHEN MATCHED 
		AND target.EMP_Info_Id = source.EMP_Info_Id
	THEN UPDATE SET
		 source.PreviousEmployer = target.PreviousEmployer
		,source.EmployerAddress = target.EmployerAddress
		,source.FromDate = target.FromDate 
		,source.ToDate = target.ToDate
		,source.BasicSalary = target.BasicSalary
		,source.NetSalary = target.NetSalary
		,source.Designation = target.Designation
		,source.ReportingSupervisorName = target.ReportingSupervisorName
		,source.SupervisorNo = target.SupervisorNo
		,source.LeavingReason = target.LeavingReason
	WHEN NOT MATCHED BY TARGET
	THEN INSERT (Id, FormDate, MachineIp, MachineId, CompId, CreatedBy, EMP_Info_Id, PreviousEmployer, EmployerAddress, FromDate, ToDate, BasicSalary, NetSalary, Designation, ReportingSupervisorName, SupervisorNo, LeavingReason)
    VALUES (NEWID(), GETDATE(), target.MachineIp, target.MachineId, target.CompId, target.CreatedBy, target.EMP_Info_Id, target.PreviousEmployer,target.EmployerAddress, target.FromDate, target.ToDate, target.BasicSalary, target.NetSalary, target.Designation, target.ReportingSupervisorName, target.SupervisorNo, target.LeavingReason);


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