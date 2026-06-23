
/**
--UpsertPerformanceCycle.Sql

--select * from MS_PerformanceCycles

DECLARE @CycleId int = 1;
DECLARE @MachineId NVARCHAR(255) = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @MachineIp NVARCHAR(255) = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @CycleName nvarchar(MAX) = 'name23';
DECLARE @StartDate DATETIME2 = GETDATE();
DECLARE @EndDate DATETIME2 = GETDATE();
DECLARE @ReviewType NVARCHAR(50) = 'rrr';
DECLARE @Status  NVARCHAR(50) = 'rrr';
DECLARE @CreatedBy uniqueidentifier = NEWID();
DECLARE @ModifiedBy uniqueidentifier = NEWID();
DECLARE @Active bit = 1;

*/

MERGE INTO MS_PerformanceCycles as Target
USING (select @CycleId as CycleId, @CycleName as CycleName) AS source
    ON (Target.CycleId = source.CycleId)

WHEN MATCHED THEN
	UPDATE SET
		 CycleName = @CycleName
		,StartDate = @StartDate
		,EndDate = @EndDate
		,ReviewType = @ReviewType
		,Status = @Status
		,ModifiedBy = @ModifiedBy
		,ModifiedOn = GETDATE()
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,MachineId
			,MachineIp
			,CycleName
			,StartDate
			,EndDate
			,ReviewType
			,Status
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
			,@CycleName
			,@StartDate
			,@EndDate
			,@ReviewType
			,@Status
			,@CreatedBy
			,GETDATE()
			,@ModifiedBy
			,GETDATE()
			,CAST(1 AS BIT))

OUTPUT INSERTED.CycleId;