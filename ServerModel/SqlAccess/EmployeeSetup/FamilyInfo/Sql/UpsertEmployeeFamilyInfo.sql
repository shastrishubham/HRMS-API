
----select * from [dbo].[EMP_FamilyInfo]
----truncate table [EMP_FamilyInfo]


------ UpsertEmployeeFamilyInfo
--DECLARE @empId uniqueidentifier = '3D360BA8-2B12-4DB7-BAC9-9ADFD3553E1E'

--DECLARE @json nvarchar(max) = 
--'[
--{
--"Id" :"5D0EBD3A-79F4-49B6-8F3F-41433E001AF0",
--"formdate": "2023-04-15",
--"machineIp":"1.1.1.1",
--"machineId":"Machine",
--"compId" :"00000000-0000-0000-0000-000000000000",
--"createdby" :"00000000-0000-0000-0000-000000000000",
--"EMP_Info_Id" :"3D360BA8-2B12-4DB7-BAC9-9ADFD3553E1E",
--"MemberName":"MemberName",
--"Profession":"profession",
--"DOB":"2001-01-22",
--"BloodGroup":"O+",
--"Gender":"Male",
--"Nationality":"Nationality",
--"RelationshipId":1,
--"IsEarningMember":1,
--"Remark":"Remark",
--"Address":"Address",
--"Pincode":"478221",
--"Mobile":"1235567877",
--"Phone":"1235567890",
--"Email":"email@gmail.com",
--"City":"City",
--"MS_State_Id":1,
--"MS_Country_Id":1
--},
--{
--"Id" :"5D0EBD3A-79F4-49B6-8F3F-41433E001AF0",
--"formdate": "2023-04-15",
--"machineIp":"1.1.1.1",
--"machineId":"Machine",
--"compId" :"00000000-0000-0000-0000-000000000000",
--"createdby" :"00000000-0000-0000-0000-000000000000",
--"EMP_Info_Id" :"3D360BA8-2B12-4DB7-BAC9-9ADFD3553E1E",
--"MemberName":"MemberName1",
--"Profession":"profession1",
--"DOB":"2001-01-22",
--"BloodGroup":"O+",
--"Gender":"Male",
--"Nationality":"Nationality",
--"RelationshipId":1,
--"IsEarningMember":1,
--"Remark":"Remark",
--"Address":"Address",
--"Pincode":"478221",
--"Mobile":"1235567877",
--"Phone":"1235567890",
--"Email":"email@gmail.com",
--"City":"City",
--"MS_State_Id":1,
--"MS_Country_Id":1
--}
--]'


BEGIN TRY
	BEGIN TRANSACTION
	
	DELETE FROM EMP_FamilyInfo WHERE EMP_Info_Id = @empId


	MERGE EMP_FamilyInfo source
	USING (SELECT 
				 Id, FormDate, MachineId, MachineIp, CompId, CreatedBy, EMP_Info_Id, [MemberName],[Profession],[DOB],[BloodGroup],[Gender],[Nationality],[RelationshipId],[IsEarningMember],[Remark],[Address],[Pincode],[Mobile],[Phone],[Email],[City], [MS_State_Id] ,[MS_Country_Id]
		FROM   OPENJSON(@json)
       WITH   (Id UNIQUEIDENTIFIER, FormDate DATE, MachineIp nvarchar(50), MachineId nvarchar(50), CompId UNIQUEIDENTIFIER, CreatedBy UNIQUEIDENTIFIER, EMP_Info_Id UNIQUEIDENTIFIER, 
	   MemberName nvarchar(max), Profession nvarchar(max), DOB DATE, BloodGroup nvarchar(10), Gender nvarchar(10), Nationality nvarchar(100), RelationshipId INT, IsEarningMember BIT,
	   Remark nvarchar(max), Address nvarchar(max),  Pincode nvarchar(50), Mobile nvarchar(50), Phone nvarchar(50), Email nvarchar(max), City nvarchar(max), MS_State_Id INT, MS_Country_Id INT
	   )) target
	ON (source.EMP_Info_Id = target.EMP_Info_Id)
	WHEN MATCHED 
		AND target.EMP_Info_Id = source.EMP_Info_Id
	THEN UPDATE SET
			source.[MemberName] = target.[MemberName],
			source.[Profession] = target.[Profession],
			source.[DOB] = target.[DOB],
			source.[BloodGroup] = target.[BloodGroup],
			source.[Gender] = target.[Gender],
			source.[Nationality] = target.[Nationality],
			source.[RelationshipId] = target.[RelationshipId],
			source.[IsEarningMember] = target.[IsEarningMember],
			source.[Remark] = target.[Remark],
			source.[Address] = target.[Address],
			source.[Pincode] = target.[Pincode],
			source.[Mobile] = target.[Mobile],
			source.[Phone] = target.[Phone],
			source.[Email] = target.[Email],
			source.[City] = target.[City],
			source.[MS_State_Id] = target.[MS_State_Id],
			source.[MS_Country_Id] = target.[MS_Country_Id]
	WHEN NOT MATCHED THEN
		INSERT 
		(
			 Id
			,[FormDate]
			,[MachineId]
			,[MachineIp]
			,[CompId]
			,[CreatedBy]
			,[EMP_Info_Id]
			,[MemberName]
			,[Profession]
			,[DOB]
			,[BloodGroup]
			,[Gender]
			,[Nationality]
			,[RelationshipId]
			,[IsEarningMember]
			,[Remark]
			,[Address]
			,[Pincode]
			,[Mobile]
			,[Phone]
			,[Email]
			,[City]
			,[MS_State_Id]
			,[MS_Country_Id]
		)
		VALUES
		(
			 NEWID()
			,GETDATE()
			,target.MachineId
			,target.MachineIp
			,target.CompId
			,target.CreatedBy
			,target.EMP_Info_Id
			,target.[MemberName]
			,target.[Profession]
			,target.[DOB]
			,target.[BloodGroup]
			,target.[Gender]
			,target.[Nationality]
			,target.[RelationshipId]
			,target.[IsEarningMember]
			,target.[Remark]
			,target.[Address]
			,target.[Pincode]
			,target.[Mobile]
			,target.[Phone]
			,target.[Email]
			,target.[City]
			,target.[MS_State_Id]
			,target.[MS_Country_Id]
		);
	--OUTPUT INSERTED.Id;

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

