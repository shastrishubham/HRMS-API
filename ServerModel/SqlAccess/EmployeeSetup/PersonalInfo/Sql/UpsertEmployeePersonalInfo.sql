/*

-- UpsertEmployeePersonalInfo


DECLARE @id uniqueidentifier = '2E2605ED-A9B7-4600-9BDC-6CF3F656D1FB'
DECLARE @formdate DATE = '2023-04-15'
DECLARE @machineIp nvarchar(50) = '1.1.1.1'
DECLARE @machineId nvarchar(50) = 'Machine'
DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @createdby uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @empId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @email nvarchar(max) = 'email@gmail.com'
DECLARE @mobile1 nvarchar(50) = '1245631155'
DECLARE @mobile2 nvarchar(50) = '4321631155'
DECLARE @maritialstatus nvarchar(50) = 'Single'
DECLARE @marriagedate date = GETDATE()
DECLARE @nosofchild int = 0
DECLARE @birthplace nvarchar(max) = 'Birth Place'
DECLARE @religion nvarchar(100) = 'Religion'
DECLARE @licno nvarchar(50) = 'LIC no'
DECLARE @passportno nvarchar(50) = 'Passport no'
DECLARE @vehicleno nvarchar(50) = 'Vehicle no'
DECLARE @bloodgrp nvarchar(10) = 'O+ve'
DECLARE @hobbies nvarchar(max) = 'hobbies1,hobbies2,hobbies3,hobbies4'

*/

BEGIN TRY
	BEGIN TRANSACTION
	
	MERGE INTO EMP_PersonalInfo AS T
	USING
	(
		VALUES
		(
			@id, @formdate, @machineId, @machineIp, @compId, @createdby, @empId, @email, @mobile1, @mobile2, @maritialstatus, @marriagedate, @nosofchild, @birthplace, @religion, @licno, @passportno, @vehicleno, @bloodgrp, @hobbies
		)
	)AS S
	(
		Id, FormDate, MachineId, MachineIp, CompId, CreatedBy, EMP_Info_Id, Email, Mobile1, Mobile2, MaritialStatus, MarrigeDate, NoOfChildren, BirthPlace, Religion, LICNo, PassportNo, VehicleNo, BloodGroup, Hobbies
	)
	ON T.Id = S.Id AND T.EMP_Info_Id = S.EMP_Info_Id
	WHEN MATCHED THEN
		UPDATE SET
			T.MachineId = S.MachineId,
			T.MachineIp = S.MachineIp,
			T.Email = S.Email,
			T.Mobile1 = S.Mobile1,
			T.Mobile2 = S.Mobile2,
			T.MaritialStatus = S.MaritialStatus,
			T.MarrigeDate = S.MarrigeDate,
			T.NoOfChildren = S.NoOfChildren,
			T.BirthPlace = S.BirthPlace,
			T.Religion = S.Religion,
			T.LICNo = S.LICNo,
			T.PassportNo = S.PassportNo,
			T.VehicleNo = S.VehicleNo,
			T.BloodGroup = S.BloodGroup,
			T.Hobbies = S.Hobbies
	WHEN NOT MATCHED THEN
		INSERT 
		(
			 Id
			,FormDate
			,MachineId
			,MachineIp
			,CompId
			,CreatedBy
			,EMP_Info_Id
			,Email
			,Mobile1
			,Mobile2
			,MaritialStatus
			,MarrigeDate
			,NoOfChildren
			,BirthPlace
			,Religion
			,LICNo
			,PassportNo
			,VehicleNo
			,BloodGroup
			,Hobbies
		)
		VALUES
		(
			 NEWID()
			,S.FormDate
			,S.MachineId
			,S.MachineIp
			,S.CompId
			,S.CreatedBy
			,S.EMP_Info_Id
			,S.Email
			,S.Mobile1
			,S.Mobile2
			,S.MaritialStatus
			,S.MarrigeDate
			,S.NoOfChildren
			,S.BirthPlace
			,S.Religion
			,S.LICNo
			,S.PassportNo
			,S.VehicleNo
			,S.BloodGroup
			,S.Hobbies
		)
	OUTPUT INSERTED.Id;

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

