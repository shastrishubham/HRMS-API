
/*

--select * from EMP_Info
--select * from EMP_LNReq

DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @status NVARCHAR(50) = 'Pending'

*/


SELECT hrLnreq.Id
      ,empLnReq.FormDate
      ,hrLnreq.MachineId
      ,hrLnreq.MachineIp
      ,empLnReq.CompId
      ,empLnReq.Id AS 'EMP_LNReq_Id'
	   ,empLnReq.TenureMonths
	  ,Ln.LNTypeName
	  ,Ln.InterestRate
	  ,Ln.MaxAmount
	  ,Ln.IsMaxAmtManual
	  ,msSl.SalaryHeadName
	  ,Ln.MS_SLHead_Id
	  ,Ln.Percentage
	  ,empLnReq.ReqAmt
	  ,empLnReq.Status AS 'LoanApplicantStatus'
	  ,empLnReq.Remark AS 'LoanApplicantRemark'
	  ,empLn.FullName AS 'LoanApplicant'
	  ,empLnReq.EMP_Info_Id AS 'LoanApplicantId'
      ,hrLnreq.ApproverId
	  ,empAppr.FullName AS 'Approver'
      ,hrLnreq.Status
      ,hrLnreq.Remark
  FROM EMP_LNReq AS empLnReq 
	INNER JOIN EMP_Info AS empLn ON empLn.Id = empLnReq.EMP_Info_Id
	LEFT JOIN HR_LNReq AS hrLnreq ON empLnReq.Id = hrLnreq.EMP_LNReq_Id
	LEFT JOIN EMP_Info AS empAppr ON empAppr.Id = hrLnreq.ApproverId
	INNER JOIN MS_LN AS Ln ON Ln.Id = empLnReq.MS_LN_Id
	LEFT JOIN MS_SLHeads AS msSl ON msSl.Id = Ln.MS_SLHead_Id
  WHERE empLnReq.CompId = @compId
	AND empLnReq.Ammend = 0
	AND empLnReq.Status = @status