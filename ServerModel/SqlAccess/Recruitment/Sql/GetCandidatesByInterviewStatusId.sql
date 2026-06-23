

--GETCandidatesByInterviewStatusId.SQL
--DECLARE @compId UNIQUEIDENTIFIER = '5F7BDA13-00DB-4AE3-9C7A-0A3A78BDBF59'
--DECLARE @InterviewStatusIds VARCHAR(100) = '7,8'

SELECT 
		 cand.Id
		,cand.CompId
		,cand.FullName
			FROM Req_InterviewFedbck AS intFedbck
				INNER JOIN Req_InterviewSch AS intSch ON intFedbck.Req_InterviewSch_Id = intSch.Id
				INNER JOIN Req_JbForm AS cand ON cand.Id =intSch.Req_JbForm_Id
				LEFT JOIN EMP_GenDocs AS draftletter ON draftletter.Req_JbForm_Id = cand.Id
			WHERE intSch.InterviewStatus IN ( SELECT CAST(value AS INT)
												FROM STRING_SPLIT(@InterviewStatusIds, ','))
				AND draftletter.Req_JbForm_Id IS NULL   -- 🔥 This hides existing rows
				AND intSch.CompId = @compId