using ServerModel.Model.Masters;
using ServerModel.SqlAccess.MasterSetup.DocumentSetup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ServerModel.ServerModel.Masters.DocumentSetup
{
    public class DocumentSetupServer
    {
        #region Properties Interface

        public static IDocumentSetupAccess mDocumentSetupAccessT
            = new DocumentSetupAccessWrapper();

        #endregion


        public static List<DocumentUploadInfo> GetMasterDocumentsByCompId(Guid companyId)
        {
            List<DocumentUploadInfo> documents = mDocumentSetupAccessT.GetMasterDocumentsByCompId(companyId);
            if (documents.Any())
            {
                foreach (DocumentUploadInfo docInfo in documents)
                {
                    if (!string.IsNullOrEmpty(docInfo.DocPath))
                    {
                        docInfo.FileName = Path.GetFileName(docInfo.DocPath); ;
                    }
                }
            }
            return documents;
        }

        public static DocumentUploadInfo GetMasterDocumentById(int documentId)
        {
            DocumentUploadInfo document = mDocumentSetupAccessT.GetMasterDocumentById(documentId);
            return document;
        }

        public static int UpsertDocument(DocumentUploadInfo documentUpload)
        {
            return mDocumentSetupAccessT.UpsertDocument(documentUpload);
        }
    }
}
