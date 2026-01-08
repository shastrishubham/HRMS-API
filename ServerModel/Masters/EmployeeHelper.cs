using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model;
using ServerModel.Model.Employee;
using ServerModel.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Masters
{
    public class EmployeeHelper
    {
        IRespository<EMP_Shift> shiftRespository = null;
        IRespository<MS_Shift> shiftMasterRespository = null;
        IRespository<EMP_Info> empInfoRespository = null;
        IRespository<EMP_PersonalInfo> empPersonalInfoRespository = null;
        IRespository<EMP_Addr> empAddressInfoRespository = null;
        IRespository<EMP_Qualification> empQualificationRepository = null;
        IRespository<EMP_WorkExp> empWorkExperienceRepository = null;
        IRespository<EMP_FamilyInfo> empFamilyInfoRepository = null;
        IRespository<EMP_Docs> empDocInfoRepository = null;
        IRespository<EMP_Leaves> empLeaveSetupRepository = null;
        IRespository<EMP_AcctInfo> empAccountSetupRepository = null;
        IRespository<EMP_Images> empImageSetupRepository = null;
        IRespository<EMP_FaceEmbeddings> empFaceEmbeddingsSetupRepository = null;



        IRespository<MS_Branch> branchRespository= null;
        IRespository<MS_Dept> departmentRespository= null;
        IRespository<MS_Designation> designationRespository = null;
        IRespository<MS_Leave> leaveRepository = null;
        IRespository<MS_CompReg> compSetupRepository = null;


        EmployeeRespository employeeInfoRepository;
        EmployeePersonalRepository employeePersonalRespository;
        EmployeeQualificationRepository employeeQualificationRepository;
        EmployeeWorkExperienceRepository employeeWorkExperienceRepository;
        EmployeeLeaveSetupRepository employeeLeaveSetupRepository;
        EmployeeBankInfoRepository employeeBankInfoRepository;



        public EmployeeHelper()
        {
            shiftRespository = new Repository<EMP_Shift>();
            shiftMasterRespository = new Repository<MS_Shift>();
            empInfoRespository = new Repository<EMP_Info>();
            empPersonalInfoRespository = new Repository<EMP_PersonalInfo>();
            empAddressInfoRespository = new Repository<EMP_Addr>();
            empQualificationRepository = new Repository<EMP_Qualification>();
            empWorkExperienceRepository = new Repository<EMP_WorkExp>();
            empFamilyInfoRepository = new Repository<EMP_FamilyInfo>();
            empDocInfoRepository = new Repository<EMP_Docs>();
            empLeaveSetupRepository = new Repository<EMP_Leaves>();
            empAccountSetupRepository = new Repository<EMP_AcctInfo>();
            empImageSetupRepository = new Repository<EMP_Images>();
            empFaceEmbeddingsSetupRepository = new Repository<EMP_FaceEmbeddings>();

            branchRespository = new Repository<MS_Branch>();
            departmentRespository = new Repository<MS_Dept>();
            designationRespository = new Repository<MS_Designation>();
            leaveRepository = new Repository<MS_Leave>();
            compSetupRepository = new Repository<MS_CompReg>();

            employeeInfoRepository = new EmployeeRespository();
            employeePersonalRespository = new EmployeePersonalRepository();
            employeeQualificationRepository = new EmployeeQualificationRepository();
            employeeWorkExperienceRepository = new EmployeeWorkExperienceRepository();
            employeeLeaveSetupRepository = new EmployeeLeaveSetupRepository();
            employeeBankInfoRepository = new EmployeeBankInfoRepository();

        }

        public IEnumerable<EmployeeInformation> GetEmployeesByCompanyId(Guid compId)
        {
            try
            {
                var employees = (from empInfo in empInfoRespository.GetAll()
                                 join msbranch in branchRespository.GetAll() on empInfo.MS_Branch_Id equals msbranch.Id
                                 join msdepartment in departmentRespository.GetAll() on empInfo.MS_Dept_Id equals msdepartment.Id
                                    into msdept
                                 from msdepartment in msdept.DefaultIfEmpty()
                                 join msdesignation in designationRespository.GetAll() on empInfo.MS_Designation_Id equals msdesignation.Id
                                     into msdesg
                                 from msdesignation in msdesg.DefaultIfEmpty()

                                 where empInfo.CompId == compId && empInfo.IsActive == true
                                 select new EmployeeInformation
                                 {
                                     Id = empInfo.Id,
                                     FormDate = empInfo.FormDate,
                                     EmpId = empInfo.EmpId ?? string.Empty,
                                     EmpType = empInfo.EmpType ?? string.Empty,
                                     FirstName = empInfo.FirstName,
                                     MiddleName = empInfo.MiddleName,
                                     LastName = empInfo.LastName,
                                     FullName = empInfo.FullName,
                                     Gender = empInfo.Gender,
                                     FatherHusbandName = empInfo.FatherHusbandName,
                                     DOB = empInfo.DOB,
                                     DateOfJoining = empInfo.DateOfJoining,
                                     ProbationDuration = empInfo.ProbationDuration,
                                     ConfirmationDate = empInfo.ConfirmationDate,
                                     DateOfLeaving = empInfo.DateOfLeaving,
                                     PFNo = empInfo.PFNo,
                                     ESINo = empInfo.ESINo,
                                     PANNo = empInfo.PANNo,
                                     AadharNo = empInfo.AadharNo,
                                     MS_Branch_Id = empInfo.MS_Branch_Id,

                                     BranchName = msbranch.BranchName,
                                     MS_Dept_Id = empInfo.MS_Dept_Id,
                                     DepartmentName = msdepartment?.DepartmentName ?? "Not Yet Assigned",
                                     MS_Designation_Id = empInfo.MS_Designation_Id,
                                     DesignationName = msdesignation?.DesignationName ?? "Not Yet Assigned",

                                     SourceOfHire = empInfo.SourceOfHire ?? string.Empty,

                                 }).OrderByDescending(x => x.FormDate);

                return employees;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeInformation GetEmployeeInformationById(Guid employeeId)
        {
            var filteredEmpAccountInfo = empAccountSetupRepository.GetAll().Where(x => x.IsAmend == false && x.EMP_Info_Id == employeeId);

            EmployeeInformation employeeDetail = (from empInfo in empInfoRespository.GetAll()
                                                  join msbranch in branchRespository.GetAll() on empInfo.MS_Branch_Id equals msbranch.Id
                                                  join msdepartment in departmentRespository.GetAll() on empInfo.MS_Dept_Id equals msdepartment.Id
                                                     into msdept
                                                  from msdepartment in msdept.DefaultIfEmpty()
                                                  join msdesignation in designationRespository.GetAll() on empInfo.MS_Designation_Id equals msdesignation.Id
                                                      into msdesg
                                                  from msdesignation in msdesg.DefaultIfEmpty()

                                                  join empPersonalInfo in empPersonalInfoRespository.GetAll() on empInfo.Id equals empPersonalInfo.EMP_Info_Id
                                                    into empPersonal
                                                  from empPersonalInfo in empPersonal.DefaultIfEmpty()

                                                  join empAddressesInfo in empAddressInfoRespository.GetAll() on empInfo.Id equals empAddressesInfo.EMP_Info_Id
                                                    into empAddr
                                                  from empAddressesInfo in empAddr.DefaultIfEmpty()

                                                  join empQualificationInfo in empQualificationRepository.GetAll() on empInfo.Id equals empQualificationInfo.EMP_Info_Id
                                                     into empQualification
                                                  from empQualificationInfo in empQualification.DefaultIfEmpty()

                                                  join empWorkExperienceInfo in empWorkExperienceRepository.GetAll() on empInfo.Id equals empWorkExperienceInfo.EMP_Info_Id
                                                    into empWorkExperience
                                                  from empWorkExperienceInfo in empWorkExperience.DefaultIfEmpty()

                                                  join empFamilyInfo in empFamilyInfoRepository.GetAll() on empInfo.Id equals empFamilyInfo.EMP_Info_Id
                                                    into empFamily
                                                  from empFamilyInfo in empFamily.DefaultIfEmpty()

                                                  join empDocInfo in empDocInfoRepository.GetAll() on empInfo.Id equals empDocInfo.EMP_Info_Id
                                                  into empDoc
                                                  from empDocInfo in empDoc.DefaultIfEmpty()

                                                  join empAccountInfo in filteredEmpAccountInfo on empInfo.Id equals empAccountInfo.EMP_Info_Id
                                                  into empAccount
                                                  from empAccountInfo in empAccount.DefaultIfEmpty()

                                                  join empImageInfo in empImageSetupRepository.GetAll() on empInfo.Id equals empImageInfo.EMP_Info_Id
                                                 into empImage
                                                  from empImageInfo in empImage.DefaultIfEmpty()

                                                  where empInfo.Id == employeeId && empInfo.IsActive == true
                                                    
                                                  select new EmployeeInformation
                                                  {
                                                      Id = empInfo.Id,
                                                      FormDate = empInfo.FormDate,
                                                      EmpId = empInfo.EmpId,
                                                      EmpType = empInfo.EmpType,
                                                      FirstName = empInfo.FirstName,
                                                      MiddleName = empInfo.MiddleName,
                                                      LastName = empInfo.LastName,
                                                      FullName = empInfo.FullName,
                                                      Gender = empInfo.Gender,
                                                      FatherHusbandName = empInfo.FatherHusbandName,
                                                      DOB = empInfo.DOB,
                                                      DateOfJoining = empInfo.DateOfJoining,
                                                      ProbationDuration = empInfo.ProbationDuration,
                                                      ConfirmationDate = empInfo.ConfirmationDate,
                                                      DateOfLeaving = empInfo.DateOfLeaving,
                                                      PFNo = empInfo.PFNo,
                                                      ESINo = empInfo.ESINo,
                                                      PANNo = empInfo.PANNo,
                                                      AadharNo = empInfo.AadharNo,
                                                      MS_Branch_Id = empInfo.MS_Branch_Id,

                                                      BranchName = msbranch.BranchName,
                                                      MS_Dept_Id = empInfo.MS_Dept_Id,
                                                      DepartmentName = msdepartment?.DepartmentName ?? "Not Yet Assigned",
                                                      MS_Designation_Id = empInfo.MS_Designation_Id,
                                                      DesignationName = msdesignation?.DesignationName ?? "Not Yet Assigned",
                                                      SourceOfHire = empInfo.SourceOfHire ?? string.Empty,

                                                      employeePersonalInformation = empPersonalInfo ?? new EMP_PersonalInfo(),

                                                      employeeAddressesInformations = empAddr.ToList(), 

                                                      employeeQualificationInformations = empQualification.ToList(),

                                                      employeeWorkExperienceInformations = empWorkExperience.ToList(),

                                                      employeeFamilyInformations = empFamily.ToList(),

                                                      employeeDocuments = empDoc.ToList(),

                                                      employeeAccountInformation = empAccount.FirstOrDefault(),

                                                      employeeImages = empImage.ToList() ?? new List<EMP_Images>()

                                                  }).FirstOrDefault();

            return employeeDetail;
        }

        public DataResult AddUpdateEmployeePersonalInformation(EmployeePersonalInformation employeePersonalInformation)
        {
            if (employeePersonalInformation.EMP_Info_Id == null || employeePersonalInformation.EMP_Info_Id == Guid.Empty)
            {
                return new DataResult { IsSuccess = false, ErrorMessage = "Invalid Employee" };
            }

            return employeePersonalRespository.AddUpdateEmployeePersonalInformation(employeePersonalInformation);
        }

        public DataResult AddUpdateEmployeeQualificationInfo(EmployeeQualificationInformation employeeQualificationInformation)
        {
            DataResult dataResult = employeeQualificationRepository.AddUpdateEmployeeQualificationInfo(employeeQualificationInformation);

            if (dataResult.IsSuccess)
            {
                dataResult.data = employeeQualificationRepository.GetEmployeeQualificationsByEmployeeId(employeeQualificationInformation.EMP_Info_Id
                    ?? employeeQualificationInformation.EMP_Info_Id.Value);
            }

            return dataResult;
        }

        public DataResult AddUpdateEmployeeWorkExperienceInfo(EmployeeWorkExperienceInformation employeeWorkExperienceInformation)
        {
            DataResult dataResult = employeeWorkExperienceRepository.AddUpdateEmployeeWorkExperienceInfo(employeeWorkExperienceInformation);

            if (dataResult.IsSuccess)
            {
                dataResult.data = employeeWorkExperienceRepository.GetEmployeeWorkExperiencesByEmployeeId(employeeWorkExperienceInformation.EMP_Info_Id
                    ?? employeeWorkExperienceInformation.EMP_Info_Id.Value);
            }

            return dataResult;
        }

        public DataResult AddUpdateEmployeeLeaveSetup(EmployeeLeaveSetupInformation employeeLeaveSetupInformation)
        {
            DataResult dataResult = employeeLeaveSetupRepository.AddUpdateEmployeeLeaveSetup(employeeLeaveSetupInformation);

            return dataResult;
        }

        public DataResult GetEmployeeListByCompanyId(Guid compId)
        {
            DataResult dataResult = new DataResult
            {
                data = employeeInfoRepository.GetEmployeeListByCompanyId(compId),
                IsSuccess = true
            };

            return dataResult;
        }

        public IEnumerable<EmployeeLeaveSetupInformation> GetEmployeesLeaveDetails(Guid compId)
        {
            var empLeavesDetails = (from empLeave in empLeaveSetupRepository.GetAll()
                                    join empInfo in empInfoRespository.GetAll() on empLeave.EMP_Info_Id equals empInfo.Id

                                    join leavemaster in leaveRepository.GetAll() on empLeave.MS_Leave_Id equals leavemaster.Id

                                    where empLeave.Active == true
                                    select new EmployeeLeaveSetupInformation
                                    {
                                        Id = empLeave.Id,
                                        EMP_Info_Id = empLeave.EMP_Info_Id,
                                        EmployeeName = empInfo.FullName,
                                        MS_Leave_Id = empLeave.MS_Leave_Id,
                                        TotalLeaves = empLeave.TotalLeaves,
                                        AvailableLeaves = empLeave.AvailableLeaves
                                    });
            return empLeavesDetails;
        }

        public DataResult DeleteEmployeeLeaveSetup(EmployeeLeaveSetupInformation employeeLeaveSetupInformation)
        {
            DataResult dataResult = employeeLeaveSetupRepository.DeleteEmployeeLeaveSetup(employeeLeaveSetupInformation);

            return dataResult;
        }

        public bool UpsertEmployeeImage(EmployeeImages employeeImage)
        {
            if(employeeImage.EMP_Info_Id == Guid.Empty && employeeImage.Photo == null)
            {
                return false;
            }
            EMP_Images empImageDetail = empImageSetupRepository.GetAll().FirstOrDefault(x => x.EMP_Info_Id == employeeImage.EMP_Info_Id && x.MS_ImageType_Id == employeeImage.MS_ImageType_Id && x.Active == true);

            if(empImageDetail != null)
            {
                // update
                if(employeeImage.Active == false)
                {
                    empImageDetail.Active = false;

                    empImageSetupRepository.Update(empImageDetail);
                    empImageSetupRepository.Save();

                    return true;
                }
                empImageDetail.FormDate = DateTime.Now;
                empImageDetail.MS_ImageType_Id = employeeImage.MS_ImageType_Id;
                empImageDetail.Photo = employeeImage.Photo;

                empImageSetupRepository.Update(empImageDetail);
                empImageSetupRepository.Save();

                return true;
            }
            else
            {
                // insert
                EMP_Images images = new EMP_Images()
                {
                    Id = employeeImage.Id,
                    FormDate = DateTime.Now,
                    CompId = employeeImage.CompId,
                    EMP_Info_Id = employeeImage.EMP_Info_Id,
                    MS_ImageType_Id = employeeImage.MS_ImageType_Id,
                    Photo = employeeImage.Photo,
                    Active = true
                };

                empImageSetupRepository.Insert(images);
                empImageSetupRepository.Save();

                return true;
            }
        }

        public bool UpsertEmpImageEmbeddings(EmplmgEmbedding embedding)
        {
            if (embedding.EMP_Info_Id == Guid.Empty && embedding.Embedding == null)
            {
                return false;
            }
            EMP_FaceEmbeddings empImageDetail = empFaceEmbeddingsSetupRepository.GetAll().FirstOrDefault(x => x.EMP_Info_Id == embedding.EMP_Info_Id);

            if (empImageDetail != null)
            {
                // update
               
                empImageDetail.Formdate = DateTime.Now;
                empImageDetail.Embedding = embedding.Embedding;

                empFaceEmbeddingsSetupRepository.Update(empImageDetail);
                empFaceEmbeddingsSetupRepository.Save();

                return true;
            }
            else
            {
                // insert
                EMP_FaceEmbeddings faceEmbeddings = new EMP_FaceEmbeddings()
                {
                    Formdate = DateTime.Now,
                    CompId = embedding.CompId,
                    EMP_Info_Id = embedding.EMP_Info_Id,
                    Embedding = embedding.Embedding
                };

                empFaceEmbeddingsSetupRepository.Insert(faceEmbeddings);
                empFaceEmbeddingsSetupRepository.Save();

                return true;
            }
        }

    }
}
