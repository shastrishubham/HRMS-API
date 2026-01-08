
--- GetEmployeeSalaryHeadsDetails.Sql

--DECLARE @Year INT = 2025;  -- Change the year as needed
--DECLARE @Month INT = 1;    -- Change the month number (1-12)
--DECLARE @EmpId UNIQUEIDENTIFIER = '566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE'

DECLARE @monthStartDate DATE = DATEFROMPARTS(@Year, @Month, 1);

-- Base Salary Heads
SELECT 
    es.Id,
    es.FormDate,
    es.CompId,
    es.CreatedBy,
    es.EMP_Info_Id,
    es.MS_SLHeads_Id,
    slHead.IsEarningComponent,
    slHead.IsShowInSalarySlip,
    slHead.SalaryHeadOrder,
    es.Amount / 12 AS Amount,
    slHead.SalaryHeadName,
    empMain.EmpId,
    bank.BankName,
    dept.DepartmentName,
    desg.DesignationName,
    empMain.FullName,
    empAcct.AccountNo,
	empMain.PANNo,
	empAcct.UAN,
	empAcct.ESINo,
	brnch.BranchName,
	bnkbrnch.IFSC,
	empMain.DateOfJoining,
    empAcct.PFNo
FROM EMP_SLHeads AS es
INNER JOIN EMP_Info AS empMain ON empMain.Id = es.EMP_Info_Id
INNER JOIN EMP_AcctInfo AS empAcct ON empAcct.EMP_Info_Id = empMain.Id
INNER JOIN MS_Bank AS bank ON empAcct.MS_Bank_Id = bank.Id
LEFT JOIN MS_Dept AS dept ON dept.Id = empMain.MS_Dept_Id
INNER JOIN MS_SLHeads AS slHead ON slHead.Id = es.MS_SLHeads_Id
INNER JOIN MS_Designation AS desg ON desg.Id = empMain.MS_Designation_Id
INNER JOIN MS_Bank_Branch AS bnkbrnch ON bnkbrnch.Id = empAcct.MS_Bank_Branch_Id
INNER JOIN MS_Branch AS brnch ON empMain.MS_Branch_Id = brnch.Id
	AND empAcct.IsAmend = 0
WHERE es.EMP_Info_Id = @EmpId
  AND empMain.IsActive = 1
  AND empAcct.IsAmend = 0
  AND es.Amount > 0
  AND UPPER(slHead.SalaryHeadName) <> 'CTC'
  AND es.IsAmmend = 0

UNION ALL

-- Loan EMI as Deduction Head
SELECT 
    CAST('00000000-0000-0000-0000-000000000000'AS UNIQUEIDENTIFIER) AS Id,
    GETDATE() AS FormDate,
    CAST(empLN.CompId AS UNIQUEIDENTIFIER) AS CompId,  -- ✅ ensure type safety
    CAST('00000000-0000-0000-0000-000000000000'AS UNIQUEIDENTIFIER) AS CreatedBy,
    CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER) AS EMP_Info_Id, -- ✅
    NULL AS MS_SLHeads_Id,
    0 AS IsEarningComponent,
    1 AS IsShowInSalarySlip,
    999 AS SalaryHeadOrder,
    rps.EMIAmt AS Amount,
    'Loan EMI' AS SalaryHeadName,
    empMain.EmpId,
    bank.BankName,
    dept.DepartmentName,
    desg.DesignationName,
    empMain.FullName,
    empAcct.AccountNo,
	'' AS 'PANNo',
	'' AS 'UAN',
	'' AS 'ESINo',
	'' AS 'BranchName',
	'' AS 'IFSC',
	'' AS 'DateOfJoining',
    empAcct.PFNo
FROM EMP_LNReq AS empLN
INNER JOIN LN_RepPaySch AS rps 
    ON empLN.Id = rps.EMP_LNReq_Id
INNER JOIN EMP_Info AS empMain 
    ON empMain.Id = CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER)
INNER JOIN EMP_AcctInfo AS empAcct 
    ON empAcct.EMP_Info_Id = empMain.Id AND empAcct.IsAmend = 0
INNER JOIN MS_Bank AS bank 
    ON empAcct.MS_Bank_Id = bank.Id
LEFT JOIN MS_Dept AS dept 
    ON dept.Id = empMain.MS_Dept_Id
INNER JOIN MS_Designation AS desg 
    ON desg.Id = empMain.MS_Designation_Id
WHERE CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER) = @EmpId
  AND empLN.Status = 'Disbursed'
  AND rps.Status = 'Pending'
  -- Compare month and year instead of exact date
 AND YEAR(rps.DueDate) = @Year
  AND MONTH(rps.DueDate) = @Month
--GROUP BY 
--    empLN.CompId,
--    empLN.EMP_Info_Id,
--    empMain.EmpId,
--    bank.BankName,
--    dept.DepartmentName,
--    desg.DesignationName,
--    empMain.FullName,
--    empAcct.AccountNo,
--    empAcct.PFNo,
--	rps.EMIAmt
--	--rps.DueDate
--HAVING SUM(rps.EMIAmt) > 0 -- ✅ prevent 0 amount EMI row

UNION ALL

-- =====================================================
-- Loan Outstanding Amount Head
-- =====================================================
SELECT 
    CAST('00000000-0000-0000-0000-000000000000'AS UNIQUEIDENTIFIER) AS Id,
    GETDATE() AS FormDate,
    CAST(empLN.CompId AS UNIQUEIDENTIFIER) AS CompId,
    CAST('00000000-0000-0000-0000-000000000000'AS UNIQUEIDENTIFIER) AS CreatedBy,
    CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER) AS EMP_Info_Id,
    NULL AS MS_SLHeads_Id,
    0 AS IsEarningComponent,            -- treated as info/deduction
    1 AS IsShowInSalarySlip,
    1000 AS SalaryHeadOrder,            -- after EMI
    (empLN.ApprAmt - ISNULL(SUM(rps.PaidAmount), 0)) AS Amount,
    'Outstanding Loan Amount' AS SalaryHeadName,
    empMain.EmpId,
    bank.BankName,
    dept.DepartmentName,
    desg.DesignationName,
    empMain.FullName,
    empAcct.AccountNo,
	'' AS 'PANNo',
	'' AS 'UAN',
	'' AS 'ESINo',
	'' AS 'BranchName',
	'' AS 'IFSC',
	'' AS 'DateOfJoining',
    empAcct.PFNo
FROM EMP_LNReq AS empLN
INNER JOIN LN_RepPaySch AS rps 
    ON empLN.Id = rps.EMP_LNReq_Id
    --AND rps.Status = 'Processed'        -- only processed repayments
INNER JOIN EMP_Info AS empMain 
    ON empMain.Id = CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER)
INNER JOIN EMP_AcctInfo AS empAcct 
    ON empAcct.EMP_Info_Id = empMain.Id AND empAcct.IsAmend = 0
INNER JOIN MS_Bank AS bank 
    ON empAcct.MS_Bank_Id = bank.Id
LEFT JOIN MS_Dept AS dept 
    ON dept.Id = empMain.MS_Dept_Id
INNER JOIN MS_Designation AS desg 
    ON desg.Id = empMain.MS_Designation_Id
WHERE CAST(empLN.EMP_Info_Id AS UNIQUEIDENTIFIER) = @EmpId
  AND empLN.Status = 'Disbursed'
     AND YEAR(rps.DueDate) = @Year
  AND MONTH(rps.DueDate) = @Month
GROUP BY 
    empLN.CompId,
    empLN.EMP_Info_Id,
    empLN.ApprAmt,
    empMain.EmpId,
    bank.BankName,
    dept.DepartmentName,
    desg.DesignationName,
    empMain.FullName,
    empAcct.AccountNo,
    empAcct.PFNo
HAVING (empLN.ApprAmt - ISNULL(SUM(rps.PaidAmount), 0)) > 0

UNION ALL

-- Salary Adjustment Head
SELECT 
    sladj.Id,
    sladj.FormDate,
    sladj.CompId,
    sladj.CreatedBy,
    sladj.EMP_Info_Id,
    NULL AS MS_SLHeads_Id,
    adj.IsEarningHead AS IsEarningComponent,
    1 AS IsShowInSalarySlip,
    CASE WHEN adj.IsEarningHead = 0 THEN 1100 ELSE 0000 END AS SalaryHeadOrder,
	--1100 AS SalaryHeadOrder,
    sladj.Amount AS Amount,
    adj.AdjustmentType AS SalaryHeadName,
    empMain.EmpId,
    bank.BankName,
    dept.DepartmentName,
    desg.DesignationName,
    empMain.FullName,
    empAcct.AccountNo,
    empMain.PANNo,
    empAcct.UAN,
    empAcct.ESINo,
    brnch.BranchName,
    bnkbrnch.IFSC,
    empMain.DateOfJoining,
    empAcct.PFNo
FROM PR_EMP_SL_Adjustment sladj
INNER JOIN MS_Payroll_AdjustType adj ON adj.Id = sladj.MS_Payroll_AdjstType_Id
INNER JOIN EMP_Info AS empMain ON empMain.Id = sladj.EMP_Info_Id
INNER JOIN EMP_AcctInfo AS empAcct ON empAcct.EMP_Info_Id = empMain.Id AND empAcct.IsAmend = 0
INNER JOIN MS_Bank AS bank ON empAcct.MS_Bank_Id = bank.Id
INNER JOIN MS_Bank_Branch AS bnkbrnch ON bnkbrnch.Id = empAcct.MS_Bank_Branch_Id
LEFT JOIN MS_Dept AS dept ON dept.Id = empMain.MS_Dept_Id
INNER JOIN MS_Designation AS desg ON desg.Id = empMain.MS_Designation_Id
INNER JOIN MS_Branch AS brnch ON brnch.Id = empMain.MS_Branch_Id
WHERE sladj.EMP_Info_Id = @EmpId
  AND YEAR(sladj.PayrollMonthYear) = @Year
  AND MONTH(sladj.PayrollMonthYear) = @Month


