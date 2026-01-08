using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;

namespace ServerModel.SqlAccess.MasterSetup.DocumentSetup
{
    public interface IDocumentSetupAccess
    {
        int UpsertDocument(DocumentUploadInfo documentUpload);

        List<DocumentUploadInfo> GetMasterDocumentsByCompId(Guid compId);

        DocumentUploadInfo GetMasterDocumentById(int documentId);
    }
}
