
/*
--GetDesignationsByCompId

DECLARE @CompId uniqueidentifier = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680'

*/

SELECT Id
      ,MachineId
      ,MachineIp
      ,CompId
      ,DesignationName
      ,DesignationCode
      ,DesignationShortName
      ,Active
  FROM MS_Designation
  WHERE CompId = @compId