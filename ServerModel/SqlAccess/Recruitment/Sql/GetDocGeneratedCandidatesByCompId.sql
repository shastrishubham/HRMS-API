
---Sql.GetDocGeneratedCandidatesByCompId.sql.sql

/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/

SELECT 
		 empGenDoc.Id
		,empGenDoc.Req_JbForm_Id
		,cand.FullName
		,empGenDoc.Status
		,empGenDoc.DocCreatedOn
		,empGenDoc.DateOfJoining
		,empGenDoc.DocExperiesOn
		,empGenDoc.MS_Doc_Id
	FROM EMP_GenDocs AS empGenDoc
		 INNER JOIN Req_JbForm AS cand ON cand.Id = empGenDoc.Req_JbForm_Id
				AND cand.Active = 1
		WHERE empGenDoc.Status NOT IN ('Confirmed', 'NotConfirmed')
			AND Ammend = 0
			AND empGenDoc.CompId = @compId