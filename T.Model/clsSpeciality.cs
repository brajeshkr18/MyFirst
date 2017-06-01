using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T.Model
{
    public class clsSpeciality
    {
        public long id { get; set; }
        public string Speciality { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }

}