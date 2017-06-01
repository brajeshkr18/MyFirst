
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace T.Model
{
    public class clsPractices
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
         public string URL { get; set; }
         public decimal? PhoneNumber { get; set; }
         public decimal? FaxNumber { get; set; }
        public bool IsActive { get; set; }
        public bool? RequiresBackup { get; set; }
        public string BackupPolicy { get; set; }
    }
   

}