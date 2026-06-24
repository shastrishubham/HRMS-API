using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Recruitment.Generate_Docs
{
    public interface IEmployeeGeneratedDocAccess
    {
        int UpsertEmployeeGeneratedOfferLetter(EmployeeGeneratedDocument employeeGeneratedDocument);

        EmployeeGeneratedDocument GetEmployeeGeneratedDocDetail(Guid compId, Guid candidateId);

        List<EmployeeOfferLetterSalaryInfo> GetEmployeeOfferSalaryInfo(Guid candidateId, int documentId);

        List<EmployeeGeneratedDocument> GetGeneratedDocumentsByCompId(Guid compId);

        EmployeeGeneratedDocument GetGeneratedDocById(int documentId);

        List<EmployeeGeneratedDocument> GetDocGeneratedCandidatesByCompId(Guid compId);

        bool UpdateGeneratedDocStatusById(int documentId, string status);

        List<EmployeeGeneratedDocument> GetConfirmedCandidatesByCompId(Guid compId, string status);
    }
}
