

/*
--UpsertEmployeeTraining.Sql

--select * from EMP_Training

DECLARE @Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @EMP_Info_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @TrainingHead_EMP_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @MS_Training_Id INT = 0;
DECLARE @StartOn datetime2 = GETDATE();
DECLARE @EndOn datetime2;
DECLARE @Status INT = 1;
DECLARE @Active bit = 1;

*/

MERGE INTO EMP_Training as Target
USING (select @Id as Id) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET 
		 EMP_Info_Id = @EMP_Info_Id
		,TrainingHead_EMP_Id = @TrainingHead_EMP_Id
		,MS_Training_Id = @MS_Training_Id
		,StartOn = @StartOn
		,EndOn = @EndOn
		,[Status] = @Status
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             Id
			,FormDate
			,CompId
			,EMP_Info_Id
			,TrainingHead_EMP_Id
			,MS_Training_Id
			,StartOn
			,EndOn
			,Status
			,Active)
        VALUES
           (
            NEWID()
           ,GETDATE()
           ,@CompId
		   ,@EMP_Info_Id
		   ,@TrainingHead_EMP_Id
		   ,@MS_Training_Id
		   ,@StartOn
		   ,@EndOn
		   ,@Status
		   ,1)

OUTPUT INSERTED.Id;