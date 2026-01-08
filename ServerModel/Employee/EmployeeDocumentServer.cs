using ServerModel.Model.Employee;
using ServerModel.SqlAccess.EmployeeSetup.DocumentInfo;
using System;
using System.Collections.Generic;

namespace ServerModel.Employee
{
    public class EmployeeDocumentServer
    {
        #region Properties Interface

        public static IEmployeeDocumentAccess mEmpDocumentnfoAccessT
            = new EmployeeDocumentWrapperAccess();

        #endregion

        public static List<EmployeeDocument> GetEmployeeDocuments(Guid employeeId)
        {
            return mEmpDocumentnfoAccessT.GetEmployeeDocuments(employeeId);
        }

        public static EmployeeDocument GetEmployeeDocument(Guid documentId)
        {
            return mEmpDocumentnfoAccessT.GetEmployeeDocument(documentId);
        }

        public static bool UpsertEmployeeDocument(EmployeeDocument employeeDocument)
        {
            return mEmpDocumentnfoAccessT.UpsertEmployeeDocument(employeeDocument);
        }
    }
}
