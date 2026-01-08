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
    public class DepartmentSetupRepository
    {
        private IRespository<MS_Dept> respository = null;

        public DepartmentSetupRepository()
        {
            this.respository = new Repository<MS_Dept>();
        }

        public IEnumerable<DepartmentInfo> GetAllDepartmentByCompId(Guid compId)
        {
            IEnumerable<MS_Dept> departmentsDb = this.respository.GetAll().Where(x => x.Active == true && x.CompId == compId);
            return GetDepartmentInformationFromDepartmentDb(departmentsDb);
        }

        #region DB Model to Server Model Data Binding
        private MS_Dept GetDepartmentsDbFromDepartmentsDbInformation(DepartmentInfo departmentInfo, Guid existingDepartmentId)
        {
            MS_Dept departmentDb = new MS_Dept();
            departmentDb.MachineId = "AJINKYA-PC";
            departmentDb.MachineIp = "0.0.0.0";
            departmentDb.CompId = departmentInfo.CompId;
            departmentDb.DepartmentName = departmentInfo.DepartmentName;
            departmentDb.DepartmentCode = departmentInfo.DepartmentCode;
            departmentDb.DepartmentShortName = departmentInfo.DepartmentShortName;
            departmentDb.CreatedOn = departmentInfo.CreatedOn;
            departmentDb.CreatedBy = departmentInfo.CreatedBy;
            departmentDb.ModifiedOn = departmentInfo.ModifiedOn;
            departmentDb.ModifiedBy = departmentInfo.ModifiedBy;
            departmentDb.Active = true;
            return departmentDb;
        }

        private DepartmentInfo GetDesignationInformationFromDesignationDb(MS_Dept departmentDb)
        {
            DepartmentInfo departmentInformation = new DepartmentInfo();
            departmentInformation.Id = departmentDb.Id;
            departmentInformation.MachineId = "AJINKYA-PC";
            departmentInformation.MachineIp = "0.0.0.0";
            departmentInformation.CompId = departmentDb.CompId;
            departmentInformation.DepartmentName = departmentDb.DepartmentName;
            departmentInformation.DepartmentCode = departmentDb.DepartmentCode;
            departmentInformation.DepartmentShortName = departmentDb.DepartmentShortName;
            departmentInformation.CreatedOn = departmentDb.CreatedOn;
            departmentInformation.CreatedBy = departmentDb.CreatedBy;
            departmentInformation.ModifiedOn = departmentDb.ModifiedOn;
            departmentInformation.ModifiedBy = departmentDb.ModifiedBy;
            departmentInformation.Active = departmentDb.Active;
            return departmentInformation;
        }

        private IEnumerable<DepartmentInfo> GetDepartmentInformationFromDepartmentDb(IEnumerable<MS_Dept> departmentDb)
        {
            IEnumerable<DepartmentInfo> departmentInfos = from a in departmentDb
                                                            select new DepartmentInfo
                                                            {
                                                                Id = a.Id,
                                                                MachineId = "AJINKYA-PC",
                                                                MachineIp = "0.0.0.0",
                                                                CompId = a.CompId,
                                                                DepartmentName = a.DepartmentName,
                                                                DepartmentCode = a.DepartmentCode,
                                                                DepartmentShortName = a.DepartmentShortName,
                                                                CreatedOn = a.CreatedOn,
                                                                CreatedBy = a.CreatedBy,
                                                                ModifiedOn = a.ModifiedOn,
                                                                ModifiedBy = a.ModifiedBy,
                                                            };
            return departmentInfos;
        }
        #endregion
    }
}
