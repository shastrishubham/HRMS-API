


--- GetEmployeeDocumentsByDocId.sql.Sql
/*
DECLARE @empDocId UNIQUEIDENTIFIER = '3D360BA8-2B12-4DB7-BAC9-9ADFD3553E1E'

*/

SELECT  Id
	   ,FormDate
	   ,CompId
	   ,CreatedBy
	   ,EMP_Info_Id
	   ,DocsKey
	   ,DocsValue
	FROM EMP_Docs
	WHERE Id = @empDocId