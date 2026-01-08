using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Recruitment;
using ServerModel.SqlAccess.Recruitment.Generate_Docs;
using System;
using System.Collections.Generic;

namespace ServerModel.ServerModel.Recruitment
{
    public class RecruitmentServer
    {
        #region Properties Interface

        public static IEmployeeGeneratedDocAccess mEmpGenerateDocSetupAccessT = new EmployeeGeneratedDocWrapperAccess();

        #endregion

        public static int UpsertEmployeeGeneratedOfferLetter(EmployeeGeneratedDocument employeeGeneratedDocument)
        {
            return mEmpGenerateDocSetupAccessT.UpsertEmployeeGeneratedOfferLetter(employeeGeneratedDocument);
        }

        public static EmployeeGeneratedDocument GetEmployeeGeneratedDocDetail(Guid compId, Guid candidateId)
        {
            return mEmpGenerateDocSetupAccessT.GetEmployeeGeneratedDocDetail(compId, candidateId);
        }

        public static List<EmployeeOfferLetterSalaryInfo> GetEmployeeOfferSalaryInfo(Guid candidateId, int documentId)
        {
            return mEmpGenerateDocSetupAccessT.GetEmployeeOfferSalaryInfo(candidateId, documentId);
        }

        public static List<EmployeeGeneratedDocument> GetGeneratedDocumentsByCompId(Guid compId)
        {
            return mEmpGenerateDocSetupAccessT.GetGeneratedDocumentsByCompId(compId);
        }

        public static EmployeeGeneratedDocument GetGeneratedDocById(int documentId)
        {
            return mEmpGenerateDocSetupAccessT.GetGeneratedDocById(documentId);
        }

        public static List<EmployeeGeneratedDocument> GetDocGeneratedCandidatesByCompId(Guid compId)
        {
            return mEmpGenerateDocSetupAccessT.GetDocGeneratedCandidatesByCompId(compId);
        }

        public static bool UpdateGeneratedDocStatusById(int documentId, string status)
        {
            return mEmpGenerateDocSetupAccessT.UpdateGeneratedDocStatusById(documentId, status);
        }

        public static List<EmployeeGeneratedDocument> GetConfirmedCandidatesByCompId(Guid compId, string status)
        {
            return mEmpGenerateDocSetupAccessT.GetConfirmedCandidatesByCompId(compId, status);
        }

    }
}
