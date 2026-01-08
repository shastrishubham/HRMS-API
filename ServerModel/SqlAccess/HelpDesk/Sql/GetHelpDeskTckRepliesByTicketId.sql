

---Sql.GetHelpDeskTckRepliesByTicketId
/*
DECLARE @ticketId INT = 1;

*/

SELECT tkreply.Id,
		tkreply.FormDate,
		tkreply.CompId,
		tkreply.HR_HelpDesk_Id,
		tkreply.RepliedByEmpId,
		emp.FullName AS 'RepliesEmployeeName',
		tkreply.Message,
		tkreply.ReplyDate
	FROM HR_HelpDeskReplies  AS tkreply
		INNER JOIN EMP_Info AS emp ON emp.Id = tkreply.RepliedByEmpId
		INNER JOIN HR_HelpDesk AS tk ON tk.Id = tkreply.HR_HelpDesk_Id
		WHERE tkreply.HR_HelpDesk_Id = @ticketId
			AND tk.IsAmmend = 0
			AND tk.Active = 1
			AND emp.IsActive = 1
		ORDER BY tkreply.ReplyDate DESC