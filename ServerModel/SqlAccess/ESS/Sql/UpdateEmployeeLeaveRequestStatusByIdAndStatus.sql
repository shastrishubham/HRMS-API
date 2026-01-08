

---Sql.UpdateEmployeeLeaveRequestStatusByIdAndStatus

/*
DECLARE @approverId uniqueidentifier = 'AB159B4D-B47A-4545-88F3-4D763F23A4B6';
DECLARE @leaveStatus INT = 1;
DECLARE @id uniqueidentifier ='AB159B4D-B47A-4545-88F3-4D763F23A4B6';

*/

IF(@leaveStatus = 2)
BEGIN
	UPDATE empLv SET empLv.AvailableLeaves = CASE 
											   WHEN empLv.AvailableLeaves >= empLvReq.TotalDays 
											     THEN empLv.AvailableLeaves - empLvReq.TotalDays
											   ELSE empLv.AvailableLeaves  -- OR 0 if you want to cap
											 END
		FROM EMP_LeaveReq as empLvReq
			INNER JOIN EMP_Leaves as empLv ON empLv.EMP_Info_Id = empLvReq.EMP_Info_Id
				AND empLv.MS_Leave_Id = empLvReq.MS_Leave_Id
		WHERE empLvReq.Id = @id
END


IF(@leaveStatus = 3)
BEGIN
	DECLARE @totalleaveDay DECIMAL(18, 4) = 0;
	
	SELECT @totalleaveDay = TotalDays FROM EMP_LeaveReq WHERE Id = @id AND LeaveStatus = 2
	
	IF EXISTS(SELECT 1 FROM EMP_LeaveReq WHERE Id = @id AND LeaveStatus = 2)
	BEGIN
		UPDATE empLv SET empLv.AvailableLeaves = CASE 
												   WHEN empLv.TotalLeaves >= (empLv.AvailableLeaves + @totalleaveDay) --18 > 10 + 8
												     THEN (empLv.AvailableLeaves + @totalleaveDay)
												   ELSE empLv.TotalLeaves  -- OR 0 if you want to cap
												 END 
			FROM EMP_LeaveReq as empLvReq
				INNER JOIN EMP_Leaves as empLv ON empLv.EMP_Info_Id = empLvReq.EMP_Info_Id
					AND empLv.MS_Leave_Id = empLvReq.MS_Leave_Id
			WHERE empLvReq.Id = @id
	END
END

 UPDATE empLvReq 
	SET empLvReq.IsApprovedBy = @approverId, empLvReq.LeaveStatus = @leaveStatus
		FROM EMP_LeaveReq as empLvReq
			INNER JOIN EMP_Leaves as empLv ON empLv.EMP_Info_Id = empLvReq.EMP_Info_Id
				AND empLv.MS_Leave_Id = empLvReq.MS_Leave_Id
		WHERE empLvReq.Id = @id

-- Return the value of @id
SELECT @id AS Id;

