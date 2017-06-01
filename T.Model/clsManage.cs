using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsManage
    {
        public clsManage()
        {
            this.oclsEmergency = new EmergencyModel();
        }
        public List<clsRole> lstRoles { get; set; }
        public List<clsSpeciality> lstSpeciality { get; set; }
        public List<clsPractices> lstPractices { get; set; }
        public List<clsUser> lstUsers { get; set; }
        public List<clsOnCallPeople> lstOnCallPeople { get; set; }
        public EmergencyModel oclsEmergency { get; set; }
    }
}
