using System;
using System.Collections.Generic;
using ServerModel.Model.Masters;

namespace ServerModel.SqlAccess.MasterSetup.DocumentSetup
{
    public class DocumentSetupAccessWrapper : IDocumentSetupAccess
    {
        public DocumentUploadInfo GetMasterDocumentById(int documentId)
        {
            return DocumentSetupAccess.GetMasterDocumentById(documentId);
        }

        public List<DocumentUploadInfo> GetMasterDocumentsByCompId(Guid compId)
        {
            return DocumentSetupAccess.GetMasterDocumentsByCompId(compId);
        }

        public int UpsertDocument(DocumentUploadInfo documentUpload)
        {
            return DocumentSetupAccess.UpsertDocument(documentUpload);
        }
    }
}
