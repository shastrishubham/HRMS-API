/*
--- GetGoalCategoriesById.Sql

DECLARE @Id INT = 1;

*/

SELECT CategoryId
      ,CompId
      ,CategoryName
  FROM MS_GoalCategories
  WHERE CategoryId = @Id