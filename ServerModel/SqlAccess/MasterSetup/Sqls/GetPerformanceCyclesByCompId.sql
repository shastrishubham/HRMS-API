

--Sql.GetPerformanceCyclesByCompId
/*
DECLARE @CompId UNIQUEIDENTIFIER = NEWID();

*/

SELECT [CycleId]
      ,[CompId]
      ,[CycleName]
      ,[StartDate]
      ,[EndDate]
      ,[ReviewType]
      ,[Status]
  FROM [MS_PerformanceCycles]
  WHERE CompId = @CompId