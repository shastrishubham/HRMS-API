


/*
---GetTrainingsByCompId

DECLARE @CompId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
*/

SELECT training.Id
      ,training.CompId
      ,training.TrainingName
      ,training.TrainingShortName
      ,training.Description
      ,training.IsTrainingMandatory
      ,training.MS_Designation_Id
	  ,desig.DesignationName
  FROM MS_Training as training
	INNER JOIN MS_Designation as desig ON desig.Id = training.MS_Designation_Id
	WHERE training.CompId = @CompId