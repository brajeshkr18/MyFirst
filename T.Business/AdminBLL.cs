using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T.Model;
using T.Data;


namespace T.Business
{
    public class AdminBLL
    {
        public List<clsRole> GetRoles()
        {
            List<clsRole> lstRoles = new List<clsRole>();

            return lstRoles = (new AdminDAL()).GetRoles();

        }
        public List<clsPractices> GetPractices(long Id)
        {
            List<clsPractices> lstPractices = new List<clsPractices>();

            return lstPractices = (new AdminDAL()).GetPractices(Id);

        }
        public List<clsUser> GetUsers(long id)
        {
            List<clsUser> lstUsers = new List<clsUser>();
            return lstUsers = (new AdminDAL()).GetUsers(id);

        }
        public bool SaveOnCallPeople(ref clsOnCallPeople objOnCall)
        {
            if ((new AdminDAL()).SaveOnCallPeople(ref objOnCall))
            {
                return true;
            }
            return false;
        }
        public List<clsSpeciality> GetSpeciality(long Id)
        {
            return (new AdminDAL()).GetSpeciality(Id);
        }
        public bool SavePractices(ref clsPractices objPractice)
        {
            if ((new AdminDAL()).SavePractices(ref objPractice))
            {
                return true;
            }
            return false;
        }
        public List<clsOnCallPeople> GetOnCallPeople(long Id)
        {
            List<clsOnCallPeople> lstOnCallPeople = new List<clsOnCallPeople>();

            return lstOnCallPeople = (new AdminDAL()).GetOnCallPeople(Id);

        }
    }
}
