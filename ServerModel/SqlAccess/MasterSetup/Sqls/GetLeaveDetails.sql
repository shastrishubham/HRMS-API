
/*
--- GetLeaveDetails.Sql

DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680' 

*/

SELECT Id
      ,CompId
      ,LeaveName
      ,LeaveCode
      ,LeaveType
      ,Unit
      ,EffectiveAfterOnTypes
      ,CarryForward
      ,ApplicableFor
      ,MS_Branch_Id
      ,DurationAllowed
      ,Active
  FROM MS_Leave
  WHERE CompId = @CompId