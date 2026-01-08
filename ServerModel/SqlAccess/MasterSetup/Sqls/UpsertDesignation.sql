
/*
--CreateDesignation.Sql

--select * from MS_Designation

DECLARE @Id int = 0;
DECLARE @MachineId nvarchar(50) = '';
DECLARE @MachineIp nvarchar(50) = '';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @DesignationName nvarchar(255) = 'name2';
DECLARE @DesignationCode nvarchar(50) = 'name2';
DECLARE @DesignationShortName nvarchar(50) = 'name2';
DECLARE @Active bit = 1;


*/


MERGE INTO MS_Designation as Target
USING (select @Id as Id, @DesignationName as DesignationName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 DesignationName = @DesignationName
		,DesignationCode = @DesignationCode
		,DesignationShortName = @DesignationShortName
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             MachineId
			,MachineIp
			,CompId
			,DesignationName
			,DesignationCode
			,DesignationShortName
			,Active)
        VALUES
           (
            @MachineId
           ,@MachineIp
           ,@CompId
		   ,@DesignationName
		   ,@DesignationCode
		   ,@DesignationShortName
		   ,@Active)

OUTPUT INSERTED.Id;


