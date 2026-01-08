
/*
DECLARE @Year INT = 2025;
DECLARE @Month INT = 7;
DECLARE @BranchId INT = 5004;
DECLARE @CompId UNIQUEIDENTIFIER = 'c225a53f-9ecb-4b9a-88dc-eaef6115c2a4'

*/

DECLARE @monthStartDate DATE = DATEFROMPARTS(@Year, @Month, 1);
DECLARE @monthEndDate DATE = EOMONTH(@monthStartDate);


-- Get first punch per day (used to simulate 'In' punch)
WITH FirstPunchPerDay AS (
    SELECT 
        EMP_Info_Id,
        CAST(PunchDate AS DATE) AS WorkDate,
        MIN(CAST(PunchTime AS TIME)) AS FirstPunchTime
    FROM EMP_PUNCH
    WHERE PunchDate BETWEEN @monthStartDate AND @monthEndDate
    GROUP BY EMP_Info_Id, CAST(PunchDate AS DATE)
),

-- Paid leave days
PaidLeaves AS (
    SELECT 
        elr.EMP_Info_Id,
        CAST(elr.FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq elr
    INNER JOIN MS_Leave lv ON lv.Id = elr.MS_Leave_Id
    WHERE elr.LeaveStatus = 2
      AND lv.LeaveType != 2  -- Exclude unpaid leave
      AND elr.FromDate BETWEEN @monthStartDate AND @monthEndDate
	  AND elr.CompId = @CompId
),

-- Unpaid leave days
UnpaidLeaves AS (
    SELECT 
        elr.EMP_Info_Id,
        CAST(elr.FromDate AS DATE) AS LeaveDate
    FROM EMP_LeaveReq elr
    INNER JOIN MS_Leave lv ON lv.Id = elr.MS_Leave_Id
    WHERE elr.LeaveStatus = 2
      AND lv.LeaveType = 2  -- Only unpaid leave
      AND elr.FromDate BETWEEN @monthStartDate AND @monthEndDate
	  AND elr.CompId = @CompId
),

-- Holidays
CompanyHolidays AS (
    SELECT DISTINCT HolidayOn AS HolidayDate
    FROM MS_Holiday
    WHERE HolidayOn BETWEEN @monthStartDate AND @monthEndDate
		AND CompId = @CompId
),

-- Monthly earnings & deductions
SalarySummary AS (
    SELECT 
        es.EMP_Info_Id,
        SUM(CASE WHEN slhead.IsEarningComponent = 1 AND slhead.SalaryHeadName <> 'CTC' THEN es.Amount ELSE 0 END)/12 AS TotalEarnings,
        SUM(CASE WHEN slhead.IsEarningComponent = 0 AND slhead.SalaryHeadName <> 'CTC' THEN es.Amount ELSE 0 END)/12 AS TotalDeductions
    FROM EMP_SLHeads es
		INNER JOIN MS_SLHeads slhead ON slhead.Id = es.MS_SLHeads_Id
	WHERE es.CompId = @CompId
		AND es.IsAmmend = 0
    GROUP BY es.EMP_Info_Id
),

-- Shift Allowance Calculation
ShiftAllowanceCalc AS (
     SELECT 
        es.EMP_Info_Id,
        es.MS_Shift_Id,
        COUNT(DISTINCT fp.WorkDate) AS ShiftAllowanceDays,
        COUNT(DISTINCT fp.WorkDate) * ISNULL(s.ShiftAllowanceAmtPerDay, 0) AS ShiftAllowanceAmount
    FROM EMP_Shift es
    INNER JOIN MS_Shift s ON s.Id = es.MS_Shift_Id
		AND s.CompId = @CompId
    INNER JOIN FirstPunchPerDay fp ON fp.EMP_Info_Id = es.EMP_Info_Id
        AND fp.WorkDate BETWEEN 
            CASE WHEN es.StartFrom < @monthStartDate THEN @monthStartDate ELSE es.StartFrom END AND
            CASE WHEN es.EndTo IS NULL OR es.EndTo > @monthEndDate THEN @monthEndDate ELSE es.EndTo END
    WHERE es.IsAmmend = 0
	  AND es.CompId = @CompId
      AND ISNULL(s.ShiftAllowanceAmtPerDay, 0) > 0 -- ✅ Only include shifts with allowance
      AND (
            (s.StartTime < s.EndTime AND 
             fp.FirstPunchTime BETWEEN s.StartTime AND s.EndTime)
         OR
            (s.StartTime > s.EndTime AND 
             (fp.FirstPunchTime >= s.StartTime OR fp.FirstPunchTime <= s.EndTime))
        )
    GROUP BY es.EMP_Info_Id, es.MS_Shift_Id, s.ShiftAllowanceAmtPerDay
),

ShiftAllowancePerEmp AS (
    SELECT 
        EMP_Info_Id,
        SUM(ShiftAllowanceDays) AS ShiftAllowanceDays,
        SUM(ShiftAllowanceAmount) AS TotalShiftAllowance
    FROM ShiftAllowanceCalc
    GROUP BY EMP_Info_Id
),

-- Loan EMI due in the month
LoanDeductions AS (
    SELECT 
        empLN.EMP_Info_Id,
        SUM(rps.EMIAmt) AS TotalLoanEMI
    FROM LN_RepPaySch rps
		INNER JOIN  EMP_LNReq AS empLN ON empLN.Id = rps.EMP_LNReq_Id
		INNER JOIN EMP_Info AS emp ON emp.Id = empLN.EMP_Info_Id
    WHERE rps.DueDate BETWEEN @monthStartDate AND @monthEndDate
      AND rps.Status IN ('Pending')  -- only unpaid EMIs
    GROUP BY empLN.EMP_Info_Id
),

-- Salary Adjustments
SalaryAdjustments AS (
    SELECT 
        sladj.EMP_Info_Id,
        sladj.Amount,
        adj.IsEarningHead
    FROM PR_EMP_SL_Adjustment sladj
    INNER JOIN MS_Payroll_AdjustType adj ON adj.Id = sladj.MS_Payroll_AdjstType_Id
    WHERE MONTH(sladj.PayrollMonthYear) = @Month
      AND YEAR(sladj.PayrollMonthYear) = @Year
      AND sladj.CompId = @CompId
),

-- Adjustment totals
AdjEarning AS (
    SELECT EMP_Info_Id, SUM(Amount) AS AdjAmount
    FROM SalaryAdjustments
    WHERE IsEarningHead = 1
    GROUP BY EMP_Info_Id
),
AdjDeduction AS (
    SELECT EMP_Info_Id, SUM(Amount) AS AdjAmount
    FROM SalaryAdjustments
    WHERE IsEarningHead = 0
    GROUP BY EMP_Info_Id
),

-- Final Payroll Calculation
PayrollCalculation AS (
    SELECT 
        e.Id AS EMP_Info_Id,
        e.FullName,

        -- Month days
        DAY(EOMONTH(@monthStartDate)) AS TotalMonthDays,

        -- Derived counts
        ISNULL(punchDays.PunchDays, 0) AS PunchDays,
        ISNULL(paidDays.PaidLeaveDays, 0) AS PaidLeaveDays,
        ISNULL(unpaidDays.UnpaidLeaveDays, 0) AS UnpaidLeaveDays,
        (SELECT COUNT(*) FROM CompanyHolidays) AS HolidayDays,

        -- Paid = Full month days - unpaid
        DAY(EOMONTH(@monthStartDate)) - ISNULL(unpaidDays.UnpaidLeaveDays, 0) AS PaidDays,

        -- Salary
        ISNULL(s.TotalEarnings,0) + ISNULL(ae.AdjAmount,0) AS TotalEarnings,
        ISNULL(s.TotalDeductions,0) + ISNULL(ld.TotalLoanEMI,0) + ISNULL(ad.AdjAmount,0) AS TotalDeductions,  -- ✅ Added Loan EMI to Deductions

        -- Shift allowance
        ISNULL(sa.ShiftAllowanceDays, 0) AS ShiftAllowanceDays,
        ISNULL(sa.TotalShiftAllowance, 0) AS TotalShiftAllowance,

		 -- Loan deductions
        ISNULL(ld.TotalLoanEMI, 0) AS LoanDeductions,

        -- Final pay (salary - deductions - loan EMI + shift allowance)
       (
            ((ISNULL(s.TotalEarnings,0) + ISNULL(ae.AdjAmount,0)) / DAY(EOMONTH(@monthStartDate)))
            * (DAY(EOMONTH(@monthStartDate)) - ISNULL(unpaidDays.UnpaidLeaveDays,0))
            - (ISNULL(s.TotalDeductions,0) + ISNULL(ad.AdjAmount,0))
            - ISNULL(ld.TotalLoanEMI,0)
        ) + ISNULL(sa.TotalShiftAllowance,0) AS FinalPay

    FROM EMP_Info e
    LEFT JOIN SalarySummary s ON e.Id = s.EMP_Info_Id

    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT WorkDate) AS PunchDays
        FROM FirstPunchPerDay
        GROUP BY EMP_Info_Id
    ) punchDays ON e.Id = punchDays.EMP_Info_Id

    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS PaidLeaveDays
        FROM PaidLeaves
        GROUP BY EMP_Info_Id
    ) paidDays ON e.Id = paidDays.EMP_Info_Id

    LEFT JOIN (
        SELECT EMP_Info_Id, COUNT(DISTINCT LeaveDate) AS UnpaidLeaveDays
        FROM UnpaidLeaves
        GROUP BY EMP_Info_Id
    ) unpaidDays ON e.Id = unpaidDays.EMP_Info_Id

    LEFT JOIN ShiftAllowancePerEmp sa ON sa.EMP_Info_Id = e.Id
	LEFT JOIN LoanDeductions ld ON ld.EMP_Info_Id = e.Id
	LEFT JOIN AdjEarning ae ON ae.EMP_Info_Id = e.Id
    LEFT JOIN AdjDeduction ad ON ad.EMP_Info_Id = e.Id
	WHERE MONTH(e.DateOfJoining) <= @Month
		AND YEAR(e.DateOfJoining) <= @Year
		AND (e.ConfirmationDate IS NULL 
			OR MONTH(e.ConfirmationDate) <= @Month OR YEAR(e.ConfirmationDate) <= @Year)
)

-- Final Output with Branch & Payroll Status
SELECT
    empShift.CompId,
    e.EMP_Info_Id,
    e.FullName,
    branch.Id AS BranchId,
    branch.BranchName,
    e.TotalMonthDays,
    e.PunchDays,
    e.PaidLeaveDays,
    e.UnpaidLeaveDays,
    e.HolidayDays,
    e.PaidDays,
    e.TotalEarnings,
    e.TotalDeductions,
    e.ShiftAllowanceDays,
    e.TotalShiftAllowance,
	e.LoanDeductions,
    e.FinalPay,
    ISNULL(payrollCrt.PayrollStatus, 'NotProcessed') AS PayrollStatus,
    payrollCrt.Comment
FROM PayrollCalculation e
INNER JOIN EMP_Shift empShift ON empShift.EMP_Info_Id = e.EMP_Info_Id AND empShift.IsAmmend = 0
INNER JOIN MS_Shift shift ON shift.Id = empShift.MS_Shift_Id
INNER JOIN MS_Branch branch ON branch.Id = shift.MS_Branch_Id
LEFT JOIN PR_CRT payrollCrt ON payrollCrt.EMP_Info_Id = e.EMP_Info_Id
    AND MONTH(payrollCrt.PayrollCreationDt) = @Month
	AND payrollCrt.IsAmmend = 0
WHERE branch.Id = @BranchId;