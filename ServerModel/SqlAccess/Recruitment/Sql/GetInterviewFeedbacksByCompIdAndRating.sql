
----Sql.GetInterviewFeedbacksByCompIdAndRating
/*
DECLARE @interviewRateId INT = 5;
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/

select 
	intFeedbck.Id,
	intFeedbck.CompId,
	intFeedbck.Req_InterviewSch_Id,
	intFeedbck.EMP_Info_Id AS 'FeedBackGivenEmpId',
	empFeedBackGiven.FullName AS 'FeedBackGivenEmpName',
	intSch.InterviewDateTime,
	intSch.Comments AS 'InterviewerComment',
	intSch.Method,
	intSch.InterviewStatus,
	intSch.EMP_Info_Id AS 'InterviewerId',
	intTakenEmp.FullName AS 'InterviewerName',
	jobForm.FullName,
	jobForm.ContactNo,
	jobForm.Email,
	jobForm.YearOfExp,
	jobVacancy.HiringManager_Id AS 'HiringManagerId',
	empHiring.FullName AS 'HiringManager',
	jobVacancy.MS_Designation_Id,
	desg.DesignationName,
	intFeedbck.MS_InterviewRate_Id,
	intRate.InterviewRate,
	intFeedbck.Feedback
	FROM Req_InterviewFedbck AS intFeedbck
		INNER JOIN Req_InterviewSch AS intSch ON intSch.Id = intFeedbck.Req_InterviewSch_Id
		INNER JOIN EMP_Info AS intTakenEmp ON intTakenEmp.Id = intFeedbck.EMP_Info_Id
		INNER JOIN MS_InterviewRate AS intRate ON intRate.Id = intFeedbck.MS_InterviewRate_Id
		INNER JOIN Req_JbForm AS jobForm ON jobForm.Id = intSch.Req_JbForm_Id
		INNER JOIN Req_JbVacancy AS jobVacancy ON jobVacancy.Id = intSch.Req_JbVacancy_Id
		INNER JOIN MS_Designation AS desg ON desg.Id = jobVacancy.MS_Designation_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = jobVacancy.MS_Branch_Id
		INNER JOIN EMP_Info AS empHiring ON empHiring.Id = jobVacancy.HiringManager_Id
		INNER JOIN EMP_Info AS empFeedBackGiven ON empFeedBackGiven.Id = intFeedbck.EMP_Info_Id
	WHERE intSch.Active = 1
		AND intFeedbck.CompId = @compId
		AND (@interviewRateId IS NULL OR intFeedbck.MS_InterviewRate_Id = @interviewRateId);