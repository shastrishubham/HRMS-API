
/**
----GetReimbursementClaimsByCompIdAndBranchId.sql.Sql

DECLARE @CompId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @BranchId int = 5004;

**/

select 
		 claim.Id
		,claim.CompId
		,claim.EMP_Info_Id
		,emp.FullName
		,emp.MS_Branch_Id
		,empBranch.BranchName
		,claim.MS_Reim_Types_Id
		,reimb.ReimbursementType
		,claim.ClaimDate
		,claim.GSTRegNos
		,claim.CGST
		,claim.SGST
		,claim.Amount
		,claim.Description
		,claim.BillPath
		,claim.Status
		,claim.SubmittedDate
		,claim.ApprovedDate
		,claim.PaidDate
	from Reim_Claims AS claim 
		INNER JOIN EMP_Info AS emp ON emp.Id = claim.EMP_Info_Id
		INNER JOIN MS_Reim_Types AS reimb ON reimb.Id = claim.MS_Reim_Types_Id
		INNER JOIN MS_Branch AS empBranch ON empBranch.Id = emp.MS_Branch_Id
	where claim.CompId = @CompId
		AND emp.MS_Branch_Id = @BranchId
		AND claim.Active = 1