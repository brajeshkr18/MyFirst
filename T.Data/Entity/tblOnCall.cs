//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOnCall
    {
        public tblOnCall()
        {
            this.tblActivecalls = new HashSet<tblActivecall>();
        }
    
        public long id { get; set; }
        public long OnCAllPersonID { get; set; }
        public long Speciality { get; set; }
        public System.DateTime StartsCall { get; set; }
        public System.DateTime EndsCall { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual ICollection<tblActivecall> tblActivecalls { get; set; }
        public virtual tblOnCallPeople tblOnCallPeople { get; set; }
        public virtual tblSpeciality tblSpeciality { get; set; }
    }
}
