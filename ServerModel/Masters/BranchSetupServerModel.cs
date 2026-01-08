using ServerModel.Database;
using ServerModel.Model.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters
{
    public class BranchSetupServerModel
    {
        HRMSEntities dbContext;
        public BranchSetupServerModel()
        {
            dbContext = new HRMSEntities();
        }

        //public BranchInfo GetBranchInfo(int compId)
        //{
        //    var branchInfo = (from a in dbContext.MS_Branch
        //                 where a.CompId == compId && a.Active == true
        //                 select new BranchInfo
        //                 {
        //                     Id = a.Id,
        //                     BranchName = a.BranchName,
        //                     BranchCode = a.BranchCode,
        //                     BranchAddress = a.BranchAddress,
        //                     PostalCode = a.PostalCode,
        //                     IsMainBranch = a.IsMainBranch
        //                 }).FirstOrDefault();
        //    return branchInfo;
        //}

        public void AddUpdateBranch(BranchInfo branchInfo)
        {
            var branch = dbContext.MS_Branch.Where(x => x.Id == branchInfo.Id).FirstOrDefault();
            if (branch == null)
            {
                // create new record
                MS_Branch newBranch = new MS_Branch();
                newBranch.BranchName = branchInfo.BranchName;
                newBranch.BranchCode = branchInfo.BranchCode;
                newBranch.BranchAddress = branchInfo.BranchAddress;
                newBranch.CountryId = branchInfo.CountryId;
                newBranch.CityId = branchInfo.CityId;
                newBranch.PostalCode = branchInfo.PostalCode;
                newBranch.IsMainBranch = branchInfo.IsMainBranch;
               newBranch.CreatedOn = DateTime.Now;
                newBranch.ModifiedOn = DateTime.Now;
                newBranch.ModifiedBy = newBranch.CreatedBy;
                newBranch.Active = true;
                newBranch.MachineId = "AJINKYA-PC";
                newBranch.MachineIp = "1";
                newBranch.CompId = Guid.Empty;
                dbContext.MS_Branch.Add(newBranch);
                dbContext.SaveChanges();
            }
            else
            {
                // update record
                branch.BranchName = branchInfo.BranchName;
                branch.BranchCode = branchInfo.BranchCode;
                branch.BranchAddress = branchInfo.BranchAddress;
                branch.CountryId = branchInfo.CountryId;
                branch.CityId = branchInfo.CityId;
                branch.PostalCode = branchInfo.PostalCode;
                branch.IsMainBranch = branchInfo.IsMainBranch;
                branch.ModifiedOn = DateTime.Now;

                dbContext.SaveChanges();
            }
        }
    }
}
