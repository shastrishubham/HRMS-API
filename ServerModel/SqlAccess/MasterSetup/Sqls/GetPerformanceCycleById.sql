

--Sql.GetPerformanceCycleById
/*
DECLARE @Id INT = 1

*/

SELECT [CycleId]
      ,[CompId]
      ,[CycleName]
      ,[StartDate]
      ,[EndDate]
      ,[ReviewType]
      ,[Status]
  FROM [MS_PerformanceCycles]
  WHERE CycleId = @Id