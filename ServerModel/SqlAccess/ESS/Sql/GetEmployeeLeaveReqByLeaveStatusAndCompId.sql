
---Sql.GetEmployeeLeaveReqByLeaveStatusAndCompId

/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @leaveStatus INT = 1

*/

SELECT 
		 empLvReq.Id
		,empLvReq.EMP_Info_Id
		,emp.FullName
		,empLvReq.MS_Leave_Id
		,lv.LeaveName
		,empLvReq.FromDate
		,empLvReq.ToDate
		,empLvReq.TotalDays AS 'Applied Days'
		,empLv.AvailableLeaves
		,empLv.Id AS 'EMP_Leaves_Id'
		,empLvReq.LeaveStatus
		,empLvReq.IsApprovedBy
		,CASE 
			WHEN empApprover.FullName IS NULL 
			THEN 'HR Dept' 
			ELSE empApprover.FullName
			END AS 'Approver'
	FROM EMP_LeaveReq AS empLvReq
		INNER JOIN EMP_Info AS emp ON emp.Id = empLvReq.EMP_Info_Id
		INNER JOIN MS_Leave AS lv ON empLvReq.MS_Leave_Id = lv.Id
		LEFT JOIN EMP_Info AS empApprover ON empLvReq.IsApprovedBy = empApprover.Id
		INNER JOIN EMP_Leaves AS empLv ON empLv.EMP_Info_Id = empLvReq.EMP_Info_Id
			AND empLv.MS_Leave_Id = empLvReq.MS_Leave_Id
	WHERE empLvReq.CompId = @compId
		AND empLvReq.LeaveStatus = @leaveStatus
		AND emp.IsActive= 1
		AND empLv.Active = 1