

/*

--UpsertShift.Sql

--select * from MS_Shift

DECLARE @Id int = 5006;
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @MS_Branch_Id INT = 1;
DECLARE @ShiftName nvarchar(255) = 'name23';
DECLARE @ShiftCode nvarchar(50) = 'name23';
DECLARE @ShiftShortName nvarchar(50) = 'name23';
DECLARE @StartTime TIME(7) = '09:30:00';
DECLARE @EndTime TIME(7) = '18:30:00';
DECLARE @TotalHrs DECIMAL(18,0) = 9;
DECLARE @WeeklyOffDay NVARCHAR(50) = '7,1';
DECLARE @IsShiftAllowance BIT = 1;
DECLARE @ShiftAllowanceAmtPerDay DECIMAL(18, 4) = 300;
DECLARE @IsOTApplicable BIT = 0;
DECLARE @OTAmtPerHrs DECIMAL(18, 4) = 300;
DECLARE @OTApplicableAfterEndTime TIME(7) = '00:00:00.0000000'
DECLARE @EMP_Info_Id uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @IsLateMarkApplicable BIT = 0
DECLARE @LateMarkAfterOn TIME(7) = '00:00:00.0000000'
DECLARE @Active bit = 1;



*/

MERGE INTO MS_Shift as Target
USING (select @Id as Id, @ShiftName as ShiftName) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		 CompId = @CompId
		,ShiftName = @ShiftName 
		,ShiftCode = @ShiftCode
		,ShiftShortName = @ShiftShortName
		,StartTime = @StartTime
		,EndTime = @EndTime
		,TotalHrs = @TotalHrs
		,WeeklyOffDay = ISNULL(@WeeklyOffDay, NULL)
		,IsShiftAllowance = @IsShiftAllowance
		,ShiftAllowanceAmtPerDay = @ShiftAllowanceAmtPerDay
		,IsOTApplicable = @IsOTApplicable
		,OTAmtPerHrs = @OTAmtPerHrs
		,OTApplicableAfterEndTime = @OTApplicableAfterEndTime
		,EMP_Info_Id = @EMP_Info_Id
		,IsLateMarkApplicable = @IsLateMarkApplicable
		,LateMarkAfterOn = @LateMarkAfterOn
		,Active = @Active


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,MS_Branch_Id
			,ShiftName
			,ShiftCode
			,ShiftShortName
			,StartTime
			,EndTime
			,TotalHrs
			,WeeklyOffDay
			,IsShiftAllowance
			,ShiftAllowanceAmtPerDay
			,IsOTApplicable
			,OTAmtPerHrs
			,OTApplicableAfterEndTime
			,EMP_Info_Id
			,IsLateMarkApplicable
			,LateMarkAfterOn
			,Active)
        VALUES
           (
            @CompId
		    ,@MS_Branch_Id
			,@ShiftName
			,@ShiftCode
			,@ShiftShortName
			,@StartTime
			,@EndTime
			,@TotalHrs
			,ISNULL(@WeeklyOffDay, NULL)
			,@IsShiftAllowance
			,@ShiftAllowanceAmtPerDay
			,ISNULL(@IsOTApplicable, CAST(0 AS BIT))
			,ISNULL(@OTAmtPerHrs, 0)
			,ISNULL(@OTApplicableAfterEndTime, '00:00:00.0000000')
			,ISNULL(@EMP_Info_Id,  '00000000-0000-0000-0000-000000000000')
			,@IsLateMarkApplicable
			,@LateMarkAfterOn
		    ,CAST(1 AS BIT))

OUTPUT INSERTED.Id;



