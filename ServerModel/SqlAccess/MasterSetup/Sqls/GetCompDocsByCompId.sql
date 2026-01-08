


/*
---GetCompDocsByCompId.Sql

DECLARE @CompId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000';
*/

SELECT Id
	  ,FormDate
      ,CompId
      ,DocumentName
	  ,DocumentPath
      ,Active
  FROM MS_CompDocs
  WHERE CompId = @CompId