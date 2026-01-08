


/*
---GetAssetDetailsByCompId.Sql

DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
*/

SELECT Id
      ,CompId
      ,AssetName
	  ,IsSubmitToBack
      ,Active
  FROM MS_Asset
  WHERE CompId = @CompId