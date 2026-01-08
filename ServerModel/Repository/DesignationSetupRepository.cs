using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class DesignationSetupRepository
    {
        private IRespository<MS_Designation> respository = null;

        public DesignationSetupRepository()
        {
            this.respository = new Repository<MS_Designation>();
        }

        public IEnumerable<DesignationInfo> GetAllDesignationByCompId(Guid compId)
        {
            IEnumerable<MS_Designation> designationsDb = this.respository.GetAll().Where(x => x.Active == true && x.CompId == compId);
            return GetDesignationInformationFromDesignationDb(designationsDb);
        }

        #region DB Model to Server Model Data Binding
        private MS_Designation GetDesignationDbFromDesignationInformation(DesignationInfo designationInfo, Guid existingDesignationId)
        {
            MS_Designation designationDb = new MS_Designation();
            designationDb.MachineId = "AJINKYA-PC";
            designationDb.MachineIp = "0.0.0.0";
            designationDb.CompId = designationInfo.CompId;
            designationDb.DesignationName = designationInfo.DesignationName;
            designationDb.DesignationCode = designationInfo.DesignationCode;
            designationDb.DesignationShortName = designationInfo.DesignationShortName;
            //designationDb.CreatedOn = designationInfo.CreatedOn;
            //designationDb.CreatedBy = designationInfo.CreatedBy;
            //designationDb.ModifiedOn = designationInfo.ModifiedOn;
            //designationDb.ModifiedBy = designationInfo.ModifiedBy;
            designationDb.Active = true;
            return designationDb;
        }

        private DesignationInfo GetDesignationInformationFromDesignationDb(MS_Designation designationDb)
        {
            DesignationInfo designationInformation = new DesignationInfo();
            designationInformation.Id = designationDb.Id;
            designationInformation.MachineId = "AJINKYA-PC";
            designationInformation.MachineIp = "0.0.0.0";
            //designationInformation.CompId = designationDb.CompId;
            designationInformation.DesignationName = designationDb.DesignationName;
            designationInformation.DesignationCode = designationDb.DesignationCode;
            designationInformation.DesignationShortName = designationDb.DesignationShortName;
            //designationInformation.CreatedOn = designationDb.CreatedOn;
            //designationInformation.CreatedBy = designationDb.CreatedBy;
            //designationInformation.ModifiedOn = designationDb.ModifiedOn;
            //designationInformation.ModifiedBy = designationDb.ModifiedBy;
            //designationInformation.Active = designationDb.Active;
            return designationInformation;
        }

        private IEnumerable<DesignationInfo> GetDesignationInformationFromDesignationDb(IEnumerable<MS_Designation> designationsDb)
        {
            IEnumerable<DesignationInfo> designationsInfos = from a in designationsDb
                                                             select new DesignationInfo
                                                             {
                                                                Id = a.Id,
                                                                MachineId = "AJINKYA-PC",
                                                                MachineIp = "0.0.0.0",
                                                              //  CompId = a.CompId,
                                                                DesignationName = a.DesignationName,
                                                                DesignationCode = a.DesignationCode,
                                                                DesignationShortName = a.DesignationShortName,
                                                                //CreatedOn = a.CreatedOn,
                                                                //CreatedBy = a.CreatedBy,
                                                                //ModifiedOn = a.ModifiedOn,
                                                                //ModifiedBy = a.ModifiedBy,
                                                            };
            return designationsInfos;
        }
        #endregion
    }
}
