using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsUser
    {
        public long UserId { get; set; }
        public string Username { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public long id { get; set; }
        public long? RoleId { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public List<clsRole> lstRoles { get; set; }
        //public List<clsSpeciality> lstSpeciality { get; set; }
        //public List<clsPractices> lstPractices { get; set; }
        //public List<clsUser> lstUsers { get; set; }
        //public clsOnCallPeople lstOnCallPeople { get; set; }
    }
}
