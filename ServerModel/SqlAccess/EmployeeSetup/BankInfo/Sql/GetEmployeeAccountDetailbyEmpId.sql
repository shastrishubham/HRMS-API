


---Sql.GetEmployeeAccountDetailByEmpId
/*
DECLARE @EMP_Info_Id uniqueidentifier = '566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE'
DECLARE @compId uniqueidentifier = '566E3EC9-41E2-4EB5-A33A-FF92BFBE2CBE'

*/


SELECT 
       ISNULL(empAcct.Id, 0) AS Id
      ,empInfo.FormDate
      --,empInfo.MachineId
      --,empInfo.MachineIp
      ,empInfo.CompId
      --,empAcct.CreatedBy
      ,empInfo.Id AS EMP_Info_Id
	  ,empInfo.FullName
      ,empAcct.MS_Bank_Id
	  ,bank.BankName
      ,empAcct.MS_Bank_Branch_Id
	  ,branch.BranchName
      ,empAcct.AccountNo
      ,branch.IFSC
      ,branch.MICR
      ,empAcct.MS_AccountType_Id
      ,empAcct.MS_PaymentType_Id
      ,empAcct.DDPayableAt
      ,empAcct.NameAsPerBank
      ,empAcct.UAN
      ,empAcct.PFNo
      ,empAcct.IsCoveredESI
      ,empAcct.ESINo
      ,empAcct.IsCoveredLWF
      ,empAcct.AmendId
      ,empAcct.IsAmend
  FROM EMP_Info AS empInfo 
	LEFT JOIN EMP_AcctInfo AS empAcct ON empInfo.Id = empAcct.EMP_Info_Id AND empAcct.IsAmend = 0
	LEFT JOIN MS_Bank AS bank ON bank.Id = empAcct.MS_Bank_Id
	LEFT JOIN MS_Bank_Branch AS Branch ON Branch.Id = empAcct.MS_Bank_Branch_Id
	WHERE empInfo.Id = @EMP_Info_Id
		AND empInfo.CompId = @compId