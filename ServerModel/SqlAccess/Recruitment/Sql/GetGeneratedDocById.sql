
---Sql.GetGeneratedDocumentsByCompId.sql

/*
DECLARE @docId INT = 1
*/

;WITH LatestFeedback AS (
    SELECT 
        fed.*,
        ROW_NUMBER() OVER (PARTITION BY fed.Req_InterviewSch_Id ORDER BY fed.FormDate DESC) AS rn
    FROM Req_InterviewFedbck AS fed
)
SELECT 
     empGenDoc.Id
	,empGenDoc.CompId
    ,empGenDoc.Status
    ,empGenDoc.CTC
    ,empGenDoc.DateOfJoining
    ,empGenDoc.MS_Doc_Id
    ,doc.DocName
    ,empGenDoc.DocPath
    ,cand.FullName
    ,cand.Email
    ,cand.ContactNo
    ,candBranch.BranchName
    ,candDesg.DesignationName
    ,hiringMngEmp.FullName AS HiringManager
    ,interviewer.FullName AS InterviewerName
	,comp.CompanyName
	,empGenDoc.DocExperiesOn
	,comp.ReportingCnt AS 'CompanyContact'
	,comp.Website
	,empGenDoc.Req_JbForm_Id
	,empGenDoc.ProbationPeriod
	,empGenDoc.NoticePeriod
FROM EMP_GenDocs AS empGenDoc
    INNER JOIN MS_DocList AS doc 
        ON doc.Id = empGenDoc.MS_Doc_Id
    INNER JOIN Req_JbForm AS cand 
        ON cand.Id = empGenDoc.Req_JbForm_Id
       AND cand.Active = 1
    INNER JOIN MS_Branch AS candBranch 
        ON candBranch.Id = cand.MS_Branch_Id
    INNER JOIN Req_JbVacancy AS vacancy 
        ON vacancy.Id = cand.Req_JbVacancy_Id
       AND vacancy.Active = 1
    INNER JOIN MS_Designation AS candDesg 
        ON candDesg.Id = vacancy.MS_Designation_Id
    LEFT JOIN EMP_Info AS hiringMngEmp 
        ON hiringMngEmp.Id = vacancy.HiringManager_Id
       AND hiringMngEmp.IsActive = 1
    INNER JOIN Req_InterviewSch AS candIntSch 
        ON candIntSch.Req_JbForm_Id = empGenDoc.Req_JbForm_Id
       AND candIntSch.Req_JbVacancy_Id = cand.Req_JbVacancy_Id
       AND candIntSch.Active = 1
    INNER JOIN LatestFeedback AS candIntFedbck 
        ON candIntFedbck.Req_InterviewSch_Id = candIntSch.Id
       AND candIntFedbck.rn = 1
    INNER JOIN EMP_Info AS interviewer 
        ON interviewer.Id = candIntFedbck.EMP_Info_Id
	INNER JOIN MS_CompReg AS comp ON comp.Id = cand.CompId
WHERE empGenDoc.Id = @docId
  AND empGenDoc.Ammend = 0;
