
---Sql.GetConfirmedCandidatesByCompId

/*
DECLARE @status NVARCHAR(50) = 'Confirmed'
DECLARE @compId UNIQUEIDENTIFIER =  '00000000-0000-0000-0000-000000000000'
*/

SELECT 
		 cand.Id
		,cand.CompId
		,cand.CTC
		,cand.Status
		,cand.DateOfJoining
		,cand.DocExperiesOn
		,candDetail.FullName
		,cand.Req_JbForm_Id
		,candDetail.Email
		,candDetail.ContactNo
		,brn.BranchName
		,desg.DesignationName
		,hiremng.FullName AS 'ReportingManager'
		,intSch.Id AS 'InterviewScheduledId'
		,intSch.InterviewStatus AS 'InterviewStatus'
	FROM EMP_GenDocs AS cand
		INNER JOIN Req_JbForm AS candDetail on candDetail.Id = cand.Req_JbForm_Id
			AND cand.Ammend = 0
		INNER JOIN Req_JbVacancy AS jobVacn on jobVacn.Id = candDetail.Req_JbVacancy_Id
		INNER JOIN MS_Designation AS desg on desg.Id = jobVacn.MS_Designation_Id
		INNER JOIN MS_Branch AS brn on brn.Id = candDetail.MS_Branch_Id
		LEFT JOIN EMP_Info As hiremng on hiremng.Id = jobVacn.HiringManager_Id
		INNER JOIN Req_InterviewSch AS intSch ON intSch.Req_JbForm_Id = cand.Req_JbForm_Id
			AND intSch.Req_JbVacancy_Id = candDetail.Req_JbVacancy_Id
	WHERE cand.Status = @status
		and cand.Ammend = 0
		and cand.CompId = @compId