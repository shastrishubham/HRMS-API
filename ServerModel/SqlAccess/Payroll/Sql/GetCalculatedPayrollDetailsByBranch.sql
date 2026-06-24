
---- CalculatePayrollByMonth.Sql

/*
DECLARE @Year INT = 2025;  -- Change the year as needed
DECLARE @Month INT = 1;    -- Change the month number (1-12)
DECLARE @BranchId INT = 2002;
*/

DECLARE @monthStartDate DATE = DATEFROMPARTS(@Year, @Month, 1);
DECLARE @monthEndDate DATE = EOMONTH(@monthStartDate);

-- Employee Punch In/Out days
WITH PunchDays AS (
    SELECT DISTINCT
        EMP_Info_Id,
        CAST(PunchDate AS DATE) AS WorkDate
    FROM EMP_Punch
    WHERE Status IN ('In', 'Out')
      AND PunchDate BETWEEN @monthStartDate AND @monthEndDate
),

-- Approved Paid Leaves
PaidLeaves AS (
    SELECT 
        EMP_Info_Id,
        CAST(FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq
		INNER JOIN MS_Leave AS lv ON lv.Id = EMP_LeaveReq.MS_Leave_Id
    WHERE LeaveStatus = 2  -- Approved
      AND lv.LeaveType != 2 -- Use your enum or ID for unpaid leave
      AND FromDate BETWEEN @monthStartDate AND @monthEndDate
),

-- Approved Unpaid Leaves (LOP)
UnpaidLeaves AS (
     SELECT 
        EMP_Info_Id,
        CAST(FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq
		INNER JOIN MS_Leave AS lv ON lv.Id = EMP_LeaveReq.MS_Leave_Id
    WHERE LeaveStatus = 2  -- Approved
      AND lv.LeaveType = 2 -- Use your enum or ID for unpaid leave
      AND FromDate BETWEEN @monthStartDate AND @monthEndDate
),

-- Company-wide Holidays
CompanyHolidays AS (
    SELECT DISTINCT HolidayOn AS HolidayDate
    FROM MS_Holiday
    WHERE HolidayOn BETWEEN @monthStartDate AND @monthEndDate
),

-- Pre-aggregate Salary
SalarySummary AS (
    SELECT 
        es.EMP_Info_Id,
        SUM(CASE WHEN slhead.IsEarningComponent = 1 AND slhead.SalaryHeadName <> 'CTC' THEN es.Amount ELSE 0 END)/12 AS TotalEarnings,
        SUM(CASE WHEN slhead.IsEarningComponent = 0 AND slhead.SalaryHeadName <> 'CTC' THEN es.Amount ELSE 0 END)/12 AS TotalDeductions
    FROM EMP_SLHeads es
	WHERE es.IsAmmend = 0
    INNER JOIN MS_SLHeads slhead ON slhead.Id = es.MS_SLHeads_Id
    GROUP BY es.EMP_Info_Id
),

-- Main Payroll Calculation
PayrollCalculation AS (
    SELECT 
        e.Id AS EMP_Info_Id,
        e.FullName,

        -- Fixed month day count
        DAY(EOMONTH(@monthStartDate)) AS TotalMonthDays,

        -- Dynamic day counts
        ISNULL(punchCount.PunchDays, 0) AS PunchDays,
        ISNULL(paidCount.PaidLeaveDays, 0) AS PaidLeaveDays,
        ISNULL(unpaidCount.UnpaidLeaveDays, 0) AS UnpaidLeaveDays,
        (SELECT COUNT(*) FROM CompanyHolidays) AS HolidayDays,

        -- Effective Paid Days = All month days - unpaid
        DAY(EOMONTH(@monthStartDate)) - ISNULL(unpaidCount.UnpaidLeaveDays, 0) AS PaidDays,

        s.TotalEarnings,
        s.TotalDeductions,

        -- Final Salary
        (
            (s.TotalEarnings / DAY(EOMONTH(@monthStartDate))) * 
            (DAY(EOMONTH(@monthStartDate)) - ISNULL(unpaidCount.UnpaidLeaveDays, 0))
            - s.TotalDeductions
        ) AS NetPay

    FROM EMP_Info e
    LEFT JOIN SalarySummary s ON e.Id = s.EMP_Info_Id

    -- Punch Days count per employee
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT WorkDate) AS PunchDays
        FROM PunchDays
        GROUP BY EMP_Info_Id
    ) punchCount ON e.Id = punchCount.EMP_Info_Id

    -- Paid Leave Days
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS PaidLeaveDays
        FROM PaidLeaves
        GROUP BY EMP_Info_Id
    ) paidCount ON e.Id = paidCount.EMP_Info_Id

    -- Unpaid Leave Days
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS UnpaidLeaveDays
        FROM UnpaidLeaves
        GROUP BY EMP_Info_Id
    ) unpaidCount ON e.Id = unpaidCount.EMP_Info_Id
)

-- Final Output with Branch and Shift Info
SELECT
    empShift.CompId,
    e.EMP_Info_Id, e.FullName,
    branch.Id AS BranchId, branch.BranchName,
    e.TotalMonthDays,
    e.PunchDays,
    e.PaidLeaveDays,
    e.UnpaidLeaveDays,
    e.HolidayDays,
    e.PaidDays,
    e.TotalEarnings,
    e.TotalDeductions,
    e.NetPay,
    ISNULL(payrollCrt.PayrollStatus, 'NotProcessed') AS PayrollStatus,
    payrollCrt.Comment
FROM PayrollCalculation e
    INNER JOIN EMP_Shift empShift ON empShift.EMP_Info_Id = e.EMP_Info_Id AND empShift.IsAmmend = 0
    INNER JOIN MS_Shift shift ON shift.Id = empShift.MS_Shift_Id
    INNER JOIN MS_Branch branch ON branch.Id = shift.MS_Branch_Id
    LEFT JOIN PR_CRT payrollCrt ON payrollCrt.EMP_Info_Id = e.EMP_Info_Id
        AND MONTH(payrollCrt.PayrollCreationDt) = @Month
WHERE branch.Id = @BranchId;
