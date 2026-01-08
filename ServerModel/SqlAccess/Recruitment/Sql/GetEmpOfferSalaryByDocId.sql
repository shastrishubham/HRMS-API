

/*
DECLARE @candidateId UNIQUEIDENTIFIER=  '85264BC4-DF2A-45DA-ACEA-66260B3421FE'
DECLARE @docId INT=  1
*/

SELECT 
	 EMP_OfferSL.Id
	,EMP_OfferSL.CompId
	,EMP_OfferSL.MS_SLHeads_Id
	,MS_SLHeads.IsEarningComponent
	,MS_SLHeads.SalaryHeadName
	,EMP_OfferSL.Amount AS 'AnnualAmount'
	FROM EMP_OfferSL
		INNER JOIN MS_SLHeads ON MS_SLHeads.Id = EMP_OfferSL.MS_SLHeads_Id
		WHERE Req_JbForm_Id = @candidateId
			AND EMP_GenDocs_Id = @docId
			AND IsAmmend = 0



