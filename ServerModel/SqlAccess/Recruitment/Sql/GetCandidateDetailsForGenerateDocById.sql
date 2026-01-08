
---Sql.GetCandidateDetailsForGenerateDocById

/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @candidateId  UNIQUEIDENTIFIER = '949A5F1A-8E89-4DB9-AE3C-FBD8996D3B9D'

*/

SELECT 
	 genDoc.Id
	,candDetail.Id AS 'Req_JbForm_Id'
	,comp.CompanyName
	,comp.CompanyAddress
	,city.CityName AS 'CompCity'
	,state.StateName AS 'CompState'
	,country.CountryName AS 'CompCountry'
	,comp.Pincode
	,candDetail.FullName AS 'CandidateFullName'
	,'' AS 'CandidateAddress'
	,designation.DesignationName AS 'Job Title'
	,branch.BranchName AS 'Location'
	,reportMng.FullName + '-' + reportMngDesignation.DesignationName AS 'Reporting Manager'
	,genDoc.CTC AS 'CTC'
	,genDoc.Status
	,genDoc.ProbationPeriod
	,genDoc.NoticePeriod
	,genDoc.DateOfJoining
	,genDoc.DocCreatedOn
	,genDoc.DocExperiesOn
	,genDoc.DocPath
	,genDoc.MS_Doc_Id
	FROM Req_JbForm AS candDetail
		INNER JOIN Req_JbVacancy AS vacancy ON vacancy.Id = candDetail.Req_JbVacancy_Id
		INNER JOIN MS_Designation AS designation ON designation.Id = vacancy.MS_Designation_Id
		INNER JOIN MS_Branch AS branch ON branch.Id = vacancy.MS_Branch_Id
		LEFT JOIN EMP_Info AS reportMng ON reportMng.Id = vacancy.HiringManager_Id
		INNER JOIN MS_Designation AS reportMngDesignation ON reportMngDesignation.Id = reportMng.MS_Designation_Id
		
		INNER JOIN MS_CompReg AS comp ON comp.Id = candDetail.CompId
		LEFT JOIN MS_Cities AS city ON city.Id = comp.MS_City_Id
		LEFT JOIN MS_States AS state ON state.Id = comp.MS_State_Id
		LEFT JOIN MS_Country AS country ON city.Id = comp.MS_Country_Id

		LEFT JOIN EMP_GenDocs AS genDoc ON genDoc.Req_JbForm_Id = candDetail.Id
			AND genDoc.Ammend = 0
	
	WHERE candDetail.CompId = @compId
		AND candDetail.Id = @candidateId