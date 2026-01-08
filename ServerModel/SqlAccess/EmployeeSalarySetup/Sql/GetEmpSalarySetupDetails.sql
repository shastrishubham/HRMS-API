
--GetEmpSalarySetupDetails

--DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
--DECLARE @empId UNIQUEIDENTIFIER = 'F4E804F9-6F67-46B0-BB51-69C1E9DCD82D'

SELECT
	     ISNULL(empSH.Id, '00000000-0000-0000-0000-000000000000') AS Id
		,ISNULL(empSH.EMP_Info_Id, '00000000-0000-0000-0000-000000000000') AS EMP_Info_Id
		,emp.FullName
		,sl.Id AS MS_SLHeads_Id
		,sl.SalaryHeadName
		,sl.IsEarningComponent
		,ISNULL(empSH.Amount, 0) AS Amount
		,empslsetup.MS_PayMode_Id
		,sl.SalaryHeadOrder
		,empslsetup.Id AS 'EMP_SLSetup_Id'
		,sl.CompId
	FROM MS_SLHeads AS sl 
		LEFT JOIN EMP_SLHeads as empSH ON empSH.MS_SLHeads_Id = sl.Id 
			AND empSH.EMP_Info_Id = @empId and empSH.CompId = @compId
			AND empSH.IsAmmend = 0
		LEFT JOIN EMP_SLSetup AS empslsetup ON empSH.EMP_Info_Id = empslsetup.EMP_Info_Id
			and empslsetup.Active = 1
			AND empslsetup.IsAmmend = 0
		LEFT JOIN EMP_Info AS emp ON empSH.EMP_Info_Id = emp.Id
			AND emp.IsActive = 1
	WHERE sl.CompId = @compId
	ORDER BY sl.SalaryHeadOrder ASC