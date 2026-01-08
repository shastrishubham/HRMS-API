

----- Sql.GetEmployeePunchesById

--DECLARE @EmpId UNIQUEIDENTIFIER = '90DAA615-055D-44B1-84F7-905C9E904791'; -- Set desired employee ID
--DECLARE @Month INT = 5;   
--DECLARE @Year INT = 2025;  

DECLARE @MonthStart DATE = DATEFROMPARTS(@Year, @Month, 1);
DECLARE @MonthEnd DATE = EOMONTH(@MonthStart);

-- Generate calendar dates for the month
WITH Calendar AS (
    SELECT @MonthStart AS WorkDate
    UNION ALL
    SELECT DATEADD(DAY, 1, WorkDate)
    FROM Calendar
    WHERE WorkDate < @MonthEnd
),
PunchData AS (
    SELECT 
        EMP_Info_Id,
        PunchDate AS WorkDate,
        MIN(PunchTime) AS PunchIn,
        MAX(PunchTime) AS PunchOut
    FROM EMP_Punch
    WHERE EMP_Info_Id = @EmpId
    GROUP BY EMP_Info_Id, PunchDate
),
-- Step 1: Filter leave requests for the given employee and month
LeaveExpanded AS (
    SELECT 
        EMP_Info_Id,
        MS_Leave_Id,
        FromDate,
        ToDate
    FROM EMP_LeaveReq
    WHERE EMP_Info_Id = @EmpId
      AND ToDate >= @MonthStart
      AND FromDate <= @MonthEnd
),

-- Step 2: Recursively generate one row per day within each leave range
LeaveDates AS (
    SELECT 
        EMP_Info_Id,
        MS_Leave_Id,
        FromDate,
        ToDate,
        FromDate AS WorkDate
    FROM LeaveExpanded
    UNION ALL
    SELECT 
        le.EMP_Info_Id,
        le.MS_Leave_Id,
        le.FromDate,
        le.ToDate,
        DATEADD(DAY, 1, ld.WorkDate)
    FROM LeaveDates ld
    INNER JOIN LeaveExpanded le
        ON ld.EMP_Info_Id = le.EMP_Info_Id
        AND ld.FromDate = le.FromDate
        AND ld.ToDate = le.ToDate
    WHERE ld.WorkDate < le.ToDate
),

HolidayData AS (
    SELECT HolidayOn AS WorkDate, HolidayName
    FROM MS_Holiday
)

-- Final Result: Show PunchIn, PunchOut, Leave Type, and Holiday Name
SELECT 
    C.WorkDate,
    ISNULL(CONVERT(VARCHAR(5), PD.PunchIn, 108), '') AS PunchIn,
    ISNULL(CONVERT(VARCHAR(5), PD.PunchOut, 108), '') AS PunchOut,
    ISNULL(LeaveType.LeaveName, '') AS LeaveType,
    ISNULL(HD.HolidayName, '') AS PublicHoliday
FROM Calendar C
LEFT JOIN PunchData PD ON C.WorkDate = PD.WorkDate
LEFT JOIN LeaveDates LD ON C.WorkDate = LD.WorkDate
	LEFT JOIN MS_Leave AS LeaveType ON LeaveType.Id = LD.MS_Leave_Id
LEFT JOIN HolidayData HD ON C.WorkDate = HD.WorkDate
ORDER BY C.WorkDate
OPTION (MAXRECURSION 1000);
