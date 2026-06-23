
--DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
--DECLARE @FromDt DATE = GETDATE();
--DECLARE @ToDt DATE = GETDATE();

SELECT goalApp.ApprovalId
      ,goalApp.FormDate
      ,goalApp.CompId
      ,goalApp.ApproverId
	  ,emp.FullName AS 'ApproverName'
      ,goalApp.MS_PerformanceCycles_Id
	  ,perfCycle.CycleName
      ,goalApp.MS_GoalCategories_CategoryId
	  ,goalCat.CategoryName
      ,goalApp.PMS_EmployeeGoals_GoalId
	  ,empGoal.GoalTitle
      ,goalApp.ApprovalStatus
      ,goalApp.Comment
      ,goalApp.Active
   FROM PMS_GoalApprovals AS goalApp
		INNER JOIN EMP_Info AS emp ON goalApp.ApproverId = emp.Id
		INNER JOIN MS_PerformanceCycles AS perfCycle ON goalApp.MS_PerformanceCycles_Id = perfCycle.CycleId
		INNER JOIN MS_GoalCategories AS goalCat ON goalCat.CategoryId = goalApp.MS_GoalCategories_CategoryId
		INNER JOIN PMS_EmployeeGoals AS empGoal ON empGoal.GoalId = goalApp.PMS_EmployeeGoals_GoalId
	WHERE    goalApp.CompId = @CompId
      AND CAST(goalApp.FormDate AS DATE) 
			BETWEEN @FromDt AND @ToDt
	 AND goalApp.Active = 1