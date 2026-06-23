using ServerModel.Model;
using ServerModel.Model.Recruitment;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.Recruitment.Generate_Docs
{
    public class EmployeeGeneratedDocWrapperAccess : IEmployeeGeneratedDocAccess
    {
        public List<EmployeeGeneratedDocument> GetConfirmedCandidatesByCompId(Guid compId, string status)
        {
            return EmployeeGeneratedDocAccess.GetConfirmedCandidatesByCompId(compId, status);
        }

        public List<EmployeeGeneratedDocument> GetDocGeneratedCandidatesByCompId(Guid compId)
        {
            return EmployeeGeneratedDocAccess.GetDocGeneratedCandidatesByCompId(compId);
        }

        public EmployeeGeneratedDocument GetEmployeeGeneratedDocDetail(Guid compId, Guid candidateId)
        {
            return EmployeeGeneratedDocAccess.GetEmployeeGeneratedDocDetail(compId, candidateId);
        }

        public List<EmployeeOfferLetterSalaryInfo> GetEmployeeOfferSalaryInfo(Guid candidateId, int documentId)
        {
            return EmployeeGeneratedDocAccess.GetEmployeeOfferSalaryInfo(candidateId, documentId);
        }

        public EmployeeGeneratedDocument GetGeneratedDocById(int documentId)
        {
            return EmployeeGeneratedDocAccess.GetGeneratedDocById(documentId);
        }

        public List<EmployeeGeneratedDocument> GetGeneratedDocumentsByCompId(Guid compId)
        {
            return EmployeeGeneratedDocAccess.GetGeneratedDocumentsByCompId(compId);
        }

        public bool UpdateGeneratedDocStatusById(int documentId, string status)
        {
            return EmployeeGeneratedDocAccess.UpdateGeneratedDocStatusById(documentId, status);
        }

        public int UpsertEmployeeGeneratedOfferLetter(EmployeeGeneratedDocument employeeGeneratedDocument)
        {
            return EmployeeGeneratedDocAccess.UpsertEmployeeGeneratedOfferLetter(employeeGeneratedDocument);
        }

        public DataResult GetCandidatesByInterviewStatusId(Guid compId, string interviewStatusIds)
        {
            return EmployeeGeneratedDocAccess.GetCandidatesByInterviewStatusId(compId, interviewStatusIds);
        }
    }
}
