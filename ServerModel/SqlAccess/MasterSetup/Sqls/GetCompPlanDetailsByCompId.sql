


/*
---GetCompPlanDetailsByCompId.Sql

DECLARE @CompId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
*/

SELECT Id
	  ,FormDate
      ,CompId
      ,MS_Module_Id
	  ,ActiveFrom
	  ,ActiveTo
      ,Active
  FROM MS_CompPlan
  WHERE CompId = @CompId