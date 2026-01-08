
--- GetEmployeeSalarySlipByMonthAndId.Sql

--DECLARE @Year INT = 2025;  -- Change the year as needed
--DECLARE @Month INT = 1;    -- Change the month number (1-12)
--DECLARE @EmpId UNIQUEIDENTIFIER = '566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE'

SELECT payrollEmpSL.Id
      ,payrollEmpSL.FormDate
      ,payrollEmpSL.CompId
      ,payrollEmpSL.CreatedBy
      ,payrollEmpSL.PR_CRT_Id
      ,payrollEmpSL.EMP_Info_Id
      ,payrollEmpSL.PayrollCreationDt
      ,payrollEmpSL.MS_SLHeads_Id
	  ,slHead.IsEarningComponent
	  ,slHead.IsShowInSalarySlip
	  ,slHead.SalaryHeadOrder
      ,payrollEmpSL.Amount

	  ,slHead.SalaryHeadName
	  ,empMain.EmpId
	  ,bank.BankName
	  ,dept.DepartmentName
	  ,empMain.FullName
	  ,empAcct.AccountNo
	  ,empAcct.PFNo
	  ,YEAR(payrollEmpSL.PayrollCreationDt) AS 'Year'
	  ,MONTH(payrollEmpSL.PayrollCreationDt) AS 'Month'
  FROM PR_EMP_SL AS payrollEmpSL
	INNER JOIN EMP_Info AS empMain ON empMain.Id = payrollEmpSL.EMP_Info_Id
	INNER JOIN EMP_AcctInfo AS empAcct ON empAcct.EMP_Info_Id = empMain.Id
	INNER JOIN MS_Bank AS bank ON empAcct.MS_Bank_Id = bank.Id
	LEFT JOIN MS_Dept AS dept ON dept.Id = empMain.MS_Dept_Id
	INNER JOIN MS_SLHeads AS slHead ON slHead.Id = payrollEmpSL.MS_SLHeads_Id
  WHERE payrollEmpSL.EMP_Info_Id = @EmpId
	AND empMain.IsActive = 1
	AND empAcct.IsAmend = 0
	AND YEAR(payrollEmpSL.PayrollCreationDt) = @Year
	AND MONTH(payrollEmpSL.PayrollCreationDt) = @Month

	