using ServerModel.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Model.Employee
{
    public class EmployeeImages : EMP_Images
    {
        public string PhotoBase64 { get; set; }
    }
}
