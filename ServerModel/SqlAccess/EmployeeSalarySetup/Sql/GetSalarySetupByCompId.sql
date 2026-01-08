
--GetEmployeeSalarySetup

--DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000'

SELECT 
		 ess.Id
		,ess.EMP_Info_Id
		,emp.FullName
		,desg.DesignationName
		,ess.TotalEarningAmt
		,ess.TotalDeductionAmt
		,ess.MS_PayMode_Id
	FROM EMP_SLSetup AS ess
		INNER JOIN EMP_Info AS emp ON ess.EMP_Info_Id = emp.Id
		INNER JOIN MS_Designation AS desg ON emp.MS_Designation_Id = desg.Id
	WHERE ess.Active = 1
		AND ess.CompId = @compId
		AND ess.IsAmmend = 0