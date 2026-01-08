
---Sql.GetScheduleInterviewsByCompId

/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @InterviewStatus INT;

*/

SELECT 
	intSch.Id,
	intSch.FormDate,
	intSch.Req_JbVacancy_Id,
	jobVacancy.HiringManager_Id,
	hiringMngr.FullName AS 'HiringManagerName',
	jobVacancy.MS_Designation_Id,
	desg.DesignationName,
	intSch.Req_JbForm_Id,
	jobForm.FullName,
	jobForm.Email,
	jobForm.ContactNo,
	jobForm.YearOfExp,
	jobForm.Resume,
	intSch.InterviewDateTime,
	intSch.Method,
	intSch.EMP_Info_Id,
	emp.FullName AS 'InterviewTakenEmp',
	intSch.InterviewStatus,
	intSch.Comments
	FROM Req_InterviewSch AS intSch
		INNER JOIN Req_JbVacancy AS jobVacancy ON jobVacancy.Id = intSch.Req_JbVacancy_Id
		INNER JOIN Req_JbForm AS jobForm ON jobForm.Id = intSch.Req_JbForm_Id
		INNER JOIN EMP_Info AS emp ON emp.Id = intSch.EMP_Info_Id
		INNER JOIN EMP_Info AS hiringMngr ON hiringMngr.Id = jobVacancy.HiringManager_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = jobVacancy.MS_Designation_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = jobVacancy.MS_Branch_Id
	WHERE intSch.CompId = @compId
		AND intSch.Active = 1
		AND (@InterviewStatus IS NULL OR intSch.InterviewStatus = @InterviewStatus);