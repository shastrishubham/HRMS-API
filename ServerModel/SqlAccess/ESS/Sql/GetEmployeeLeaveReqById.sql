
/**
----GetEmployeeLeaveReqsByCompId.Sql

DECLARE @Id UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'


**/

SELECT  empLvReq.Id
      ,empLvReq.FormDate
      ,empLvReq.CompId
      ,empLvReq.CreatedBy
      ,empLvReq.EMP_Info_Id
	  ,emp.FullName
      ,empLvReq.MS_Leave_Id
	  ,lv.LeaveName
      ,empLvReq.FromDate
      ,empLvReq.ToDate
      ,empLvReq.TotalDays
      ,empLvReq.LeaveFor
      ,empLvReq.LeaveReason
      ,empLvReq.IsApprovedBy
	  ,empApp.FullName AS 'ApproverName'
      ,empLvReq.LeaveStatus
      ,empLvReq.Docs
  FROM EMP_LeaveReq AS empLvReq
	INNER JOIN EMP_Info AS emp ON emp.Id = empLvReq.EMP_Info_Id
	INNER JOIN MS_Leave AS lv ON lv.Id =empLvReq.MS_Leave_Id
	LEFT JOIN EMP_Info AS empApp ON empApp.Id = empLvReq.IsApprovedBy
WHERE empLvReq.Id = @Id