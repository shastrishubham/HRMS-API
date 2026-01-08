
/*

--UpsertLeave.Sql

--select * from MS_Leave

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
DECLARE @LeaveName nvarchar(255) = 'name23';
DECLARE @LeaveCode nvarchar(50) = 'name23';
DECLARE @LeaveType nvarchar(50) = 'AA';
DECLARE @Unit nvarchar(50) = '22';
DECLARE @EffectiveAfterOnTypes int = 1;
DECLARE @CarryForward int = 2;
DECLARE @ApplicableFor nvarchar(50) = 'M';
DECLARE @MS_Branch_Id int = 2;
DECLARE @DurationAllowed nvarchar(50) = 'TEST';
DECLARE @Active bit = 1;

*/



MERGE INTO MS_Leave as Target
USING (select @Id as Id, @LeaveName as LeaveName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,LeaveName = @LeaveName
		,LeaveCode = @LeaveCode
		,LeaveType = @LeaveType
		,Unit = @Unit
		,EffectiveAfterOnTypes = @EffectiveAfterOnTypes
		,CarryForward = @CarryForward
		,ApplicableFor = @ApplicableFor
		,MS_Branch_Id = @MS_Branch_Id
		,DurationAllowed = @DurationAllowed
		,[Active] = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,LeaveName
			,LeaveCode
			,LeaveType
			,Unit
			,EffectiveAfterOnTypes
			,CarryForward
			,ApplicableFor
			,MS_Branch_Id
			,DurationAllowed
			,Active)
        VALUES
           (
            @CompId
		   ,@LeaveName
		   ,@LeaveCode
		   ,@LeaveType
		   ,@Unit
		   ,@EffectiveAfterOnTypes
		   ,@CarryForward
		   ,@ApplicableFor
		   ,@MS_Branch_Id
		   ,@DurationAllowed
		   ,@Active)

OUTPUT INSERTED.Id;


