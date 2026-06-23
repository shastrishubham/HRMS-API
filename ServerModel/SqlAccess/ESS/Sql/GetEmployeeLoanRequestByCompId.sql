
/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
*/

SELECT empLn.Id
      ,empLn.FormDate
      ,empLn.MachineId
      ,empLn.MachineIp
      ,empLn.CompId
      ,empLn.EMP_Info_Id
	  ,emp.FullName
      ,empLn.MS_LN_Id
	  ,ln.LNTypeName
	  ,ln.InterestRate
      ,empLn.ReqAmt
      ,empLn.ApprAmt
      ,empLn.TenureMonths
      ,empLn.Status
      ,empLn.Remark
      ,hrln.Remark AS 'HRRemark'
      ,empLn.Ammend
      ,empLn.Active
  FROM EMP_LNReq AS empLn
		INNER JOIN EMP_Info AS emp ON empLn.EMP_Info_Id = emp.Id
		INNER JOIN MS_LN AS ln ON ln.Id = empLn.MS_LN_Id
        LEFT JOIN HR_LNReq AS hrln ON hrln.EMP_LNReq_Id = empLn.Id
  WHERE empLn.CompId = @compId
	AND Ammend = 0