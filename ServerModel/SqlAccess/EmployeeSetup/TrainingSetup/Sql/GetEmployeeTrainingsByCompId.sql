

/*
--GetEmployeeTrainingsByCompId.Sql

--select * from EMP_Training

DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';

*/

SELECT 
	 empTraining.Id
	,empTraining.FormDate
	,empTraining.CompId
	,empTraining.EMP_Info_Id
	,emp.FullName AS 'EmployeeName'
	,empTraining.TrainingHead_EMP_Id
	,empTrainingHead.FullName AS 'TrainingHeadEmployeeName'
	,empTraining.MS_Training_Id
	,training.TrainingName
	,empTraining.StartOn
	,empTraining.EndOn
	,empTraining.Status
	,emp.MS_Designation_Id
	,desg.DesignationName
	FROM EMP_Training AS empTraining
		LEFT JOIN EMP_Info AS emp ON emp.Id = empTraining.EMP_Info_Id AND empTraining.Active = 1
		LEFT JOIN EMP_Info AS empTrainingHead ON empTrainingHead.Id = empTraining.TrainingHead_EMP_Id
		LEFT JOIN MS_Training as training ON training.Id = empTraining.MS_Training_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = emp.MS_Designation_Id
		INNER JOIN MS_Training AS desgTraining ON emp.MS_Designation_Id = desgTraining.MS_Designation_Id
	WHERE emp.CompId = @CompId
		AND emp.IsActive = 1