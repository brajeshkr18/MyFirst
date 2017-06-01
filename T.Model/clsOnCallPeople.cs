using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T.Model
{
    public class clsOnCallPeople
    {
        public clsOnCallPeople()
        {
            this.oclsPractices = new clsPractices();
        }

        public long id { get; set; }
        public long OnCallPersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? PhoneNumber { get; set; }
        public decimal? CellNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime StartsCall { get; set; }
        public DateTime EndCall { get; set; }
        public long PracticeId { get; set; }
        public string Practice { get; set; }
        public long SpecialityId { get; set; }
        public string Speciality { get; set; }
        public bool IsCredentialed { get; set; }
        public bool IsEmtala { get; set; }
        public string Picture { get; set; }
        public bool CanRequest { get; set; }
        public string CallCarrier { get; set; }
        public string UserId { get; set; }

        public clsPractices oclsPractices { get; set; }
        public List<clsSpeciality> lstSpeciality { get; set; }
        public List<clsPractices> lstPractices { get; set; }
    }
}
