

/*
--CreateDepartment.Sql

--select * from MS_Dept

DECLARE @Id int = 0;
DECLARE @MachineId nvarchar(50) = '';
DECLARE @MachineIp nvarchar(50) = '';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @DepartmentName nvarchar(255) = 'name2';
DECLARE @DepartmentCode nvarchar(50) = 'name2';
DECLARE @DepartmentShortName nvarchar(50) = 'name2';
DECLARE @MailAlias nvarchar(255) = '';
DECLARE @DepartmentLead_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @ParentDepartment_Id int = 0;
DECLARE @CreatedOn datetime = '';
DECLARE @CreatedBy uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @ModifiedOn datetime = '';
DECLARE @ModifiedBy uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @Active bit = 0;

*/

MERGE INTO MS_Dept as Target
USING (select @Id as Id, @DepartmentName as DepartmentName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 [DepartmentName] = @DepartmentName
		,[DepartmentCode] = @DepartmentCode
		,[DepartmentShortName] = @DepartmentShortName
		,[MailAlias] = @MailAlias
		,[DepartmentLead_Id] = @DepartmentLead_Id
		,[ParentDepartment_Id] = @ParentDepartment_Id
		,[ModifiedOn] = @ModifiedOn
		,[ModifiedBy] = @ModifiedBy
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
            [MachineId]
           ,[MachineIp]
           ,[CompId]
		   ,[DepartmentName]
		   ,[DepartmentCode]
		   ,[DepartmentShortName]
		   ,[MailAlias]
		   ,[DepartmentLead_Id]
		   ,[ParentDepartment_Id]
		   ,[CreatedOn]
		   ,[CreatedBy]
		   ,[ModifiedOn]
		   ,[ModifiedBy]
		   ,[Active])
        VALUES
           (
            @MachineId
           ,@MachineIp
           ,@CompId
		   ,@DepartmentName
		   ,@DepartmentCode
		   ,@DepartmentShortName
		   ,@MailAlias
		   ,@DepartmentLead_Id
		   ,@ParentDepartment_Id
		   ,@CreatedOn
		   ,@CreatedBy
		   ,@ModifiedOn
		   ,@ModifiedBy
		   ,@Active)

OUTPUT INSERTED.Id;


