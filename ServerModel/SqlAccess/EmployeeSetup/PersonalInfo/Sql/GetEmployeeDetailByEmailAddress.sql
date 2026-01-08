
---SQL.GetEmployeeDetailByEmailAddress
/*
DEClARE @emailAddress NVARCHAR(MAX) = 'abc@gmail.com';

*/
SELECT 
		 emp.Id
		,emp.CompId
		,emp.FullName
		,empPersonal.Email
		,compPlan.ActiveFrom
		,compPlan.ActiveTo
			FROM EMP_PersonalInfo AS empPersonal
				INNER JOIN EMP_Info AS emp ON emp.Id = empPersonal.EMP_Info_Id
				INNER JOIN MS_CompReg AS comp ON comp.Id = empPersonal.CompId
				LEFT JOIN MS_CompPlan AS compPlan ON compPlan.CompId = comp.Id
					AND compPlan.Active = CAST(1 AS BIT)
			WHERE empPersonal.Email = @emailAddress
				AND emp.IsActive = 1
