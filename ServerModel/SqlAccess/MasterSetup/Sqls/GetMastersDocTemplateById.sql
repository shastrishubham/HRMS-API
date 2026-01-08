

/**

---GetMastersDocTemplateById.sql

DECLARE @documentId INT = 1;

**/

SELECT Id
	  ,CompId
	  ,DocName
	  ,DocPath
  FROM MS_DocList
  WHERE Id = @documentId