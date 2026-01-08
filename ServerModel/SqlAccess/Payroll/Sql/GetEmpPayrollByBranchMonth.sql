
---- Sql.GetEmpPayrollByBranchMonth

/*
DECLARE @Year INT = 2025;  
DECLARE @Month INT = 1;    
DECLARE @BranchId INT = 2002;
DECLARE @empId UNIQUEIDENTIFIER = '2916952C-8B28-41F4-8022-6783BACDA7F5'

*/

DECLARE @CompId UNIQUEIDENTIFIER = (SELECT TOP 1 CompId FROM EMP_Info WHERE Id = @empId);
DECLARE @monthStartDate DATE = DATEFROMPARTS(@Year, @Month, 1);
DECLARE @monthEndDate DATE = EOMONTH(@monthStartDate);

-- Get dynamic salary heads
DECLARE @cols NVARCHAR(MAX), @sql NVARCHAR(MAX);

SELECT @cols = STRING_AGG(QUOTENAME(SalaryHeadName), ',')
FROM (
    SELECT sh.SalaryHeadName
    FROM EMP_SLHeads es
    INNER JOIN MS_SLHeads sh ON sh.Id = es.MS_SLHeads_Id
    WHERE es.EMP_Info_Id = @empId 
		AND sh.SalaryHeadName <> 'CTC' 
		AND es.Amount > 0
		AND sh.Active = 1
		AND es.CompId = @CompId
		AND es.IsAmmend = 0
    GROUP BY sh.SalaryHeadName
    HAVING SUM(CASE WHEN es.Amount > 0 THEN es.Amount ELSE 0 END) > 0
) AS ValidHeads;

--select @cols

-- Build dynamic SQL for salary head pivot
SET @sql = N'
WITH SalaryPivot AS (
    SELECT 
        es.EMP_Info_Id,
        sh.SalaryHeadName,
        es.Amount / 12 AS Amount
    FROM EMP_SLHeads es
    INNER JOIN MS_SLHeads sh ON sh.Id = es.MS_SLHeads_Id
    WHERE sh.SalaryHeadName <> ''CTC''
      AND es.Amount > 0
	  AND es.CompId = ''' + CAST(@CompId AS NVARCHAR(36)) + '''
	  AND es.IsAmmend = 0

),
Pivoted AS (
    SELECT * FROM SalaryPivot
    PIVOT (
        SUM(Amount) FOR SalaryHeadName IN (' + @cols + ')
    ) AS pvt
)


-- Join with other payroll data
, PunchDays AS (
    SELECT DISTINCT
        EMP_Info_Id,
        CAST(PunchDate AS DATE) AS WorkDate
    FROM EMP_Punch
    WHERE Status IN (''In'', ''Out'')
      AND PunchDate BETWEEN ''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''' AND ''' + CONVERT(VARCHAR, @monthEndDate, 23) + '''
),
PaidLeaves AS (
    SELECT 
        EMP_Info_Id,
        CAST(FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq
        INNER JOIN MS_Leave AS lv ON lv.Id = EMP_LeaveReq.MS_Leave_Id
    WHERE LeaveStatus = 2 AND lv.LeaveType != 2 AND EMP_LeaveReq.CompId = ''' + CAST(@CompId AS NVARCHAR(36)) + '''
      AND FromDate BETWEEN ''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''' AND ''' + CONVERT(VARCHAR, @monthEndDate, 23) + '''
),
UnpaidLeaves AS (
     SELECT 
        EMP_Info_Id,
        CAST(FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq
        INNER JOIN MS_Leave AS lv ON lv.Id = EMP_LeaveReq.MS_Leave_Id
    WHERE LeaveStatus = 2 AND lv.LeaveType = 2 AND EMP_LeaveReq.CompId = ''' + CAST(@CompId AS NVARCHAR(36)) + '''
      AND FromDate BETWEEN ''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''' AND ''' + CONVERT(VARCHAR, @monthEndDate, 23) + '''
),
CompanyHolidays AS (
    SELECT DISTINCT HolidayOn AS HolidayDate
    FROM MS_Holiday
    WHERE CompId = ''' + CAST(@CompId AS NVARCHAR(36)) + ''' AND HolidayOn BETWEEN ''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''' AND ''' + CONVERT(VARCHAR, @monthEndDate, 23) + '''
),
SalarySummary AS (
    SELECT 
        es.EMP_Info_Id,
        SUM(CASE WHEN slhead.IsEarningComponent = 1 AND slhead.SalaryHeadName <> ''CTC'' THEN es.Amount ELSE 0 END)/12 AS TotalEarnings,
        SUM(CASE WHEN slhead.IsEarningComponent = 0 AND slhead.SalaryHeadName <> ''CTC'' THEN es.Amount ELSE 0 END)/12 AS TotalDeductions
    FROM EMP_SLHeads es
    INNER JOIN MS_SLHeads slhead ON slhead.Id = es.MS_SLHeads_Id
	WHERE es.CompId = ''' + CAST(@CompId AS NVARCHAR(36)) + '''
		AND es.IsAmmend = 0
    GROUP BY es.EMP_Info_Id
),
FinalPayroll AS (
    SELECT 
        e.Id AS EMP_Info_Id,
        e.FullName,
		e.MS_Designation_Id,
        DAY(EOMONTH(''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''')) AS TotalMonthDays,
        ISNULL(punchCount.PunchDays, 0) AS PunchDays,
        ISNULL(paidCount.PaidLeaveDays, 0) AS PaidLeaveDays,
        ISNULL(unpaidCount.UnpaidLeaveDays, 0) AS UnpaidLeaveDays,
        (SELECT COUNT(*) FROM CompanyHolidays) AS HolidayDays,
        DAY(EOMONTH(''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''')) - ISNULL(unpaidCount.UnpaidLeaveDays, 0) AS PaidDays,
        s.TotalEarnings,
        s.TotalDeductions,
        (
            (s.TotalEarnings / DAY(EOMONTH(''' + CONVERT(VARCHAR, @monthStartDate, 23) + '''))) * 
            (DAY(EOMONTH(''' + CONVERT(VARCHAR, @monthStartDate, 23) + ''')) - ISNULL(unpaidCount.UnpaidLeaveDays, 0))
            - s.TotalDeductions
        ) AS NetPay
    FROM EMP_Info e
    LEFT JOIN SalarySummary s ON e.Id = s.EMP_Info_Id
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT WorkDate) AS PunchDays
        FROM PunchDays
        GROUP BY EMP_Info_Id
    ) punchCount ON e.Id = punchCount.EMP_Info_Id
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS PaidLeaveDays
        FROM PaidLeaves
        GROUP BY EMP_Info_Id
    ) paidCount ON e.Id = paidCount.EMP_Info_Id
    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS UnpaidLeaveDays
        FROM UnpaidLeaves
        GROUP BY EMP_Info_Id
    ) unpaidCount ON e.Id = unpaidCount.EMP_Info_Id
)

SELECT
    empShift.CompId,
    e.EMP_Info_Id, e.FullName,
    branch.Id AS BranchId, branch.BranchName, e.MS_Designation_Id, desg.DesignationName,
    e.TotalMonthDays, e.PunchDays, e.PaidLeaveDays, e.UnpaidLeaveDays, e.HolidayDays, e.PaidDays,
    e.TotalEarnings, e.TotalDeductions, e.NetPay,
    ' + @cols + ',
    ISNULL(payrollCrt.PayrollStatus, ''NotProcessed'') AS PayrollStatus,
    payrollCrt.Comment
FROM FinalPayroll e
INNER JOIN EMP_Shift empShift ON empShift.EMP_Info_Id = e.EMP_Info_Id AND empShift.IsAmmend = 0
INNER JOIN MS_Shift shift ON shift.Id = empShift.MS_Shift_Id
INNER JOIN MS_Branch branch ON branch.Id = shift.MS_Branch_Id
LEFT JOIN PR_CRT payrollCrt ON payrollCrt.EMP_Info_Id = e.EMP_Info_Id AND MONTH(payrollCrt.PayrollCreationDt) = ' + CAST(@Month AS NVARCHAR) + '
LEFT JOIN Pivoted pvt ON pvt.EMP_Info_Id = e.EMP_Info_Id
INNER JOIN MS_Designation AS desg ON desg.Id = e.MS_Designation_Id
WHERE branch.Id = ' + CAST(@BranchId AS NVARCHAR) + ';
';


-- Execute dynamic SQL
EXEC sp_executesql @sql;