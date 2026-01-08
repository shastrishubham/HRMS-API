

/**

---GetMastersDocsTemplates.sql

DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';

**/

SELECT Id
	  ,CompId
	  ,DocName
	  ,DocPath
  FROM MS_DocList
  WHERE CompId = @CompId