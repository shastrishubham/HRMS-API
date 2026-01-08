

/*

--UpsertTraining.Sql

--select * from MS_Training

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @TrainingName nvarchar(MAX) = 'name23';
DECLARE @TrainingShortName nvarchar(50) = 'name23';
DECLARE @Description nvarchar(MAX) = 'name23';
DECLARE @IsTrainingMandatory BIT = 1;
DECLARE @MS_Designation_Id INT = 1;


*/


MERGE INTO MS_Training as Target
USING (select @Id as Id, @TrainingName as TrainingName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 TrainingName = @TrainingName 
		,TrainingShortName = @TrainingShortName
		,Description = @Description
		,IsTrainingMandatory = @IsTrainingMandatory
		,MS_Designation_Id = @MS_Designation_Id

WHEN NOT MATCHED THEN
        INSERT (
			 FormDate
			,MachineId
			,MachineIp
            ,CompId
			,TrainingName
			,TrainingShortName
			,Description
			,IsTrainingMandatory
			,MS_Designation_Id)
        VALUES
           (
            GETDATE()
		   ,'AJINKYA-PC'
		   ,'1.1.1.1'
		   ,@CompId
		   ,@TrainingName
		   ,@TrainingShortName
		   ,@Description
		   ,@IsTrainingMandatory
		   ,@MS_Designation_Id)

OUTPUT INSERTED.Id;


