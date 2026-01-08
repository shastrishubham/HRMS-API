

---GetEmployeeReimbursementsByBranchAndMonth.Sql



--DECLARE @BranchId INT  = 5004;
--DECLARE @Month INT = 08;
--DECLARE @Year INT= 2025;
--DECLARE @CompId UNIQUEIDENTIFIER = 'C225A53F-9ECB-4B9A-88DC-EAEF6115C2A4';

SELECT DISTINCT
	rc.CompId,
    rc.Id AS 'Reim_Claims_Id',
    rc.EMP_Info_Id,
	emp.MS_Branch_Id AS 'EMP_Info_MSBranchId',
	emp.FullName,
    rc.ClaimDate,
	rc.CGST,
	rc.SGST,
    rc.Amount,
	rembType.ReimbursementType,
	rc.ApprovedDate,
    ra.Approver_Emp_Id,
	empApp.FullName AS 'Approver',
	ISNULL(prRemb.PayrollStatus, 'NotProcessed') AS PayrollStatus,
    prRemb.Comment
FROM 
    Reim_Claims rc
		INNER JOIN Reim_Appro_Claims AS ra ON rc.Id = ra.Reim_Claims_Id
		INNER JOIN EMP_Info AS emp ON emp.Id = rc.EMP_Info_Id
		INNER JOIN EMP_Info AS empApp ON empApp.Id = ra.Approver_Emp_Id
		INNER JOIN MS_Reim_Types AS rembType ON rembType.Id = rc.MS_Reim_Types_Id
		LEFT JOIN PR_Reimbursement AS prRemb ON prRemb.[Reim_Claims_Id] = rc.Id
			AND MONTH(prRemb.PayrollCreationDt) = @Month
			AND prRemb.IsAmmend = 0
WHERE 
    emp.MS_Branch_Id = @BranchId
    AND ra.Status = 'Approved'
	AND rc.Status = 'Approved'
    AND MONTH(rc.ApprovedDate) = @Month
    AND YEAR(rc.ApprovedDate) = @Year
	AND rc.CompId = @CompId
