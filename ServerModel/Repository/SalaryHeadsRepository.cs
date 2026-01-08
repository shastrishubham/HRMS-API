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
    public class SalaryHeadsRepository
    {
        private IRespository<MS_SLHeads> respository = null;

        public SalaryHeadsRepository()
        {
            this.respository = new Repository<MS_SLHeads>();
        }

        public IEnumerable<SalaryHeadsInfo> GetAllSalaryHeadsByCompId(Guid compId)
        {
            IEnumerable<MS_SLHeads> salaryHeadsDb = this.respository.GetAll().Where(x=>x.Active == true && x.CompId == compId);
            return GetSalaryHeadsInformationFromSalaryHeadsDb(salaryHeadsDb);
        }

        #region DB Model to Server Model Data Binding
        private MS_SLHeads GetSalaryHeadsDbFromSalaryHeadsInformation(SalaryHeadsInfo salaryHeadsInfo, Guid existingSalaryHeadId)
        {
            MS_SLHeads salaryHeadsDb = new MS_SLHeads();
            salaryHeadsDb.Id = salaryHeadsInfo.Id;
            salaryHeadsDb.CompId = salaryHeadsInfo.CompId;
            salaryHeadsDb.SalaryHeadName = salaryHeadsInfo.SalaryHeadName;
            salaryHeadsDb.ShortSalaryHeadName = salaryHeadsInfo.ShortSalaryHeadName;
            salaryHeadsDb.IsEarningComponent = salaryHeadsInfo.IsEarningComponent;
            salaryHeadsDb.IsFixedComponent = salaryHeadsInfo.IsFixedComponent;
            //salaryHeadsDb.CreatedOn = salaryHeadsInfo.CreatedOn;
            //salaryHeadsDb.CreatedBy = salaryHeadsInfo.CreatedBy;
            //salaryHeadsDb.ModifiedOn = salaryHeadsInfo.ModifiedOn;
            //salaryHeadsDb.ModifiedBy = salaryHeadsInfo.ModifiedBy;
            salaryHeadsDb.Active = salaryHeadsInfo.Active;
            return salaryHeadsDb;
        }

        private SalaryHeadsInfo GetSalaryHeadsInformationFromSalaryHeadsDb(MS_SLHeads salaryHeadsDb)
        {
            SalaryHeadsInfo salaryHeadsInformation = new SalaryHeadsInfo();
            salaryHeadsInformation.Id = salaryHeadsDb.Id;
           // salaryHeadsInformation.MachineId = "AJINKYA-PC";
          //  salaryHeadsInformation.MachineIp = "0.0.0.0";
           // salaryHeadsInformation.CompId = salaryHeadsDb.CompId;
            salaryHeadsInformation.SalaryHeadName = salaryHeadsDb.SalaryHeadName;
           // salaryHeadsInformation.ShortSalaryHeadName = salaryHeadsDb.ShortSalaryHea/dName;
            salaryHeadsInformation.IsEarningComponent = salaryHeadsDb.IsEarningComponent;
            //salaryHeadsInformation.IsFixedComponent = salaryHeadsDb.IsFixedComponent;
            //salaryHeadsInformation.CreatedOn = salaryHeadsDb.CreatedOn;
            //salaryHeadsInformation.CreatedBy = salaryHeadsDb.CreatedBy;
            //salaryHeadsInformation.ModifiedOn = salaryHeadsDb.ModifiedOn;
            //salaryHeadsInformation.ModifiedBy = salaryHeadsDb.ModifiedBy;
            //salaryHeadsInformation.Active = salaryHeadsDb.Active;
            return salaryHeadsInformation;
        }

        private IEnumerable<SalaryHeadsInfo> GetSalaryHeadsInformationFromSalaryHeadsDb(IEnumerable<MS_SLHeads> salaryHeadsDb)
        {
            IEnumerable<SalaryHeadsInfo> salaryHeadsInfos = from a in salaryHeadsDb
                                                            select new SalaryHeadsInfo
                                                            {
                                                                Id = a.Id,
                                                               // MachineId = "AJINKYA-PC",
                                                                //MachineIp = "0.0.0.0",
                                                               /// CompId = a.CompId,
                                                                SalaryHeadName = a.SalaryHeadName,
                                                                ShortSalaryHeadName = a.ShortSalaryHeadName,
                                                                IsEarningComponent = a.IsEarningComponent,
                                                               // IsFixedComponent = a.IsFixedComponent,
                                                              //  CreatedOn = a.CreatedOn,
                                                              //  CreatedBy = a.CreatedBy,
                                                              //  ModifiedOn = a.ModifiedOn,
                                                              //  ModifiedBy = a.ModifiedBy,
                                                              //  Active = a.Active
                                                            };
            return salaryHeadsInfos;
        }
        #endregion
    }
}
