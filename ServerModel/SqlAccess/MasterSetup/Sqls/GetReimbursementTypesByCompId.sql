


/*
---GetReimbursementTypesByCompId

DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
*/

SELECT a.Id
      ,a.CompId
      ,a.ReimbursementType
      ,a.MonthlyLimit
	  ,a.Frequency
	  ,a.IsDesignationSpecific
	  ,a.MS_Designation_Id
	  ,b.DesignationName
  FROM MS_Reim_Types as a
	LEFT JOIN MS_Designation as b on a.MS_Designation_Id = b.Id
  WHERE a.CompId = @CompId