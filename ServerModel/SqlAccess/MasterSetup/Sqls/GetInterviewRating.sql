


/*
---GetInterviewRatings

DECLARE @compId UNIQUEIDENTIFIER = 'D33691FC-7EB9-4D1C-888C-4F4B6E204680';
*/

SELECT Id
      ,CompId
      ,InterviewRate
      ,Active
  FROM MS_InterviewRate
  WHERE CompId = @compId
	AND Active = 1