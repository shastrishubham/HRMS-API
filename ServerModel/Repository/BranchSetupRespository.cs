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
    public class BranchSetupRespository
    {
        private IRespository<MS_Branch> respository = null;

        public BranchSetupRespository()
        {
            this.respository = new Repository<MS_Branch>();
        }

        public IEnumerable<BranchInformation> GetAllBranchesByCompany(Guid compId)
        {
            var branches = (from a in this.respository.GetAll()
                            where a.CompId.Equals(compId) && a.Active == true
                            select new BranchInformation
                            {
                                Id = a.Id,
                                BranchName = a.BranchName,
                                BranchCode = a.BranchCode,
                                IsMainBranch = a.IsMainBranch
                            }).OrderBy(x => x.BranchName);
            return branches;
        }

        #region DB Model to Server Model Data Binding
        private MS_Branch GetBranchInfoDbFromBranchInformation(BranchInformation branchInformation, Guid existingBranchId)
        {
            MS_Branch branchInfoDb = new MS_Branch();
            
            return branchInfoDb;
        }

        private BranchInformation GetBranchInformationFromBranchInfoDb(MS_Branch empInfoDb)
        {
            BranchInformation branchInformation = new BranchInformation();
          
            return branchInformation;
        }
        #endregion
    }
}
