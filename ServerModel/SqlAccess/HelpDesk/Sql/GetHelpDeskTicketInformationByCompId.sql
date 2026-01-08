

---Sql.GetHelpDeskTicketInformationByCompId
/*
DECLARE @CompId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'

*/


SELECT hdTck.Id,
		hdTck.FormDate,
		hdTck.CompId,
		hdTck.EMP_Info_Id,
		emp.FullName AS 'EmployeeName',
		hdTck.MS_TicketCat_Id,
		tk.TicketCategory,
		hdTck.TicketName,
		hdTck.Subject,
		hdTck.Description,
		hdTck.Severity,
		hdTck.AttachmentPath,
		hdTck.AssignEmpId,
		assgEmp.FullName AS 'AssignEmployeeName',
		hdTck.TicketStatus,
		hdTck.TicketResolvedDt
	FROM HR_HelpDesk AS hdTck
		INNER JOIN EMP_Info AS emp ON emp.Id = hdTck.EMP_Info_Id
		INNER JOIN MS_TicketCat AS tk ON tk.Id = hdTck.MS_TicketCat_Id
		LEFT JOIN EMP_Info AS assgEmp ON assgEmp.Id = hdTck.AssignEmpId
		WHERE hdTck.CompId = @CompId
			AND hdTck.IsAmmend = 0
			AND hdTck.Active = 1
			AND emp.IsActive = 1