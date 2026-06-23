
/**
--UpsertPMSGoalCategory.Sql

--select * from MS_GoalCategories

DECLARE @CategoryId int = 1;
DECLARE @MachineId NVARCHAR(255) = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @MachineIp NVARCHAR(255) = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @CategoryName nvarchar(MAX) = 'name23';
DECLARE @CreatedBy uniqueidentifier = NEWID();
DECLARE @ModifiedBy uniqueidentifier = NEWID();
DECLARE @Active bit = 1;

*/

MERGE INTO MS_GoalCategories as Target
USING (select @CategoryId as CategoryId, @CategoryName as CategoryName) AS source
    ON (Target.CategoryId = source.CategoryId)

WHEN MATCHED THEN
	UPDATE SET
		 CategoryName = @CategoryName
		,ModifiedBy = @ModifiedBy
		,ModifiedOn = GETDATE()
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,MachineId
			,MachineIp
			,CategoryName
			,CreatedBy
			,CreatedOn
			,ModifiedBy
			,ModifiedOn
			,Active)
        VALUES
           (
             @CompId
			,@MachineId
			,@MachineIp
			,@CategoryName
			,@CreatedBy
			,GETDATE()
			,@ModifiedBy
			,GETDATE()
			,CAST(1 AS BIT))

OUTPUT INSERTED.CategoryId;