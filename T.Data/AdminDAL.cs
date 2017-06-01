using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T.Model;

namespace T.Data
{
    public class AdminDAL
    {
        public List<clsRole> GetRoles()
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objgetRoles = maincontext.Web_Roles().ToList();
                List<clsRole> lstRoles = new List<clsRole>();

                if (objgetRoles.Count() > 0)
                {
                    foreach (var Roles in objgetRoles)
                    {
                        clsRole objRolesDetails = new clsRole();
                        objRolesDetails.RoleId = Roles.id;
                        objRolesDetails.Role = Roles.Role;
                        lstRoles.Add(objRolesDetails);
                    }

                    return lstRoles;
                }

                return lstRoles;
            }
        }
        public List<clsPractices> GetPractices(long Id)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objgetPractices = maincontext.Web_Practices(Id).ToList();
                List<clsPractices> lstPractices = new List<clsPractices>();

                if (objgetPractices.Count() > 0)
                {
                    foreach (var Practices in objgetPractices)
                    {
                        clsPractices objPracticesDetails = new clsPractices();
                        objPracticesDetails.id = Practices.id;
                        objPracticesDetails.Name = Practices.Name;
                        objPracticesDetails.Address = Practices.Address;
                        objPracticesDetails.URL = Practices.URL;
                        objPracticesDetails.PhoneNumber = (decimal)Practices.PhoneNumber;
                        objPracticesDetails.FaxNumber = (decimal)Practices.FaxNumber;
                        objPracticesDetails.RequiresBackup = Practices.RequiresBackup;
                        objPracticesDetails.BackupPolicy = Practices.BackupPolicy;
                        lstPractices.Add(objPracticesDetails);
                    }

                    return lstPractices;
                }

                return lstPractices;
            }
        }
        public List<clsSpeciality> GetSpeciality(long Id)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                var objgetSpeciality = maincontext.Web_Speciality(Id).ToList();
                List<clsSpeciality> lstSpeciality = new List<clsSpeciality>();

                if (objgetSpeciality.Count() > 0)
                {
                    foreach (var Speciality in objgetSpeciality)
                    {
                        clsSpeciality objSpecialityDetails = new clsSpeciality();
                        objSpecialityDetails.id = Speciality.id;
                        objSpecialityDetails.Speciality = Speciality.Speciality;
                        objSpecialityDetails.Description = Speciality.Description;
                        objSpecialityDetails.IsActive = Speciality.IsActive;
                        lstSpeciality.Add(objSpecialityDetails);
                    }

                    return lstSpeciality;
                }

                return lstSpeciality;
            }
        }

        public bool SaveOnCallPeople(ref clsOnCallPeople objOnCall)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var objsave = maincontext.Web_SaveOnCallPeople(objOnCall.id, objOnCall.OnCallPersonID.ToString(), objOnCall.FirstName, objOnCall.LastName, objOnCall.PhoneNumber, objOnCall.CellNo, objOnCall.Email, objOnCall.PracticeId, objOnCall.SpecialityId,objOnCall.IsEmtala, objOnCall.IsCredentialed, objOnCall.CanRequest, objOnCall.CallCarrier, objOnCall.UserId.ToString(), objOnCall.Picture);
              
                ///return objsave.SpecialityResult;
                return true;
              
            }
            return false;
        }
        public bool SavePractices(ref clsPractices objPractice)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {

                var obj = new Entity.tblPractice();
                //var obj = maincontext.tblPractices.FirstOrDefault();
                    if (obj != null)
                    {
                        if (objPractice.id > 0)
                        {
                            var practiceid = objPractice.id;
                            var objprac = new Entity.tblPractice();
                            objprac = maincontext.tblPractices.Single(a => a.id == practiceid);
                            objprac.Address = objPractice.Address;
                            objprac.Name = objPractice.Name;
                            objprac.FaxNumber = objPractice.FaxNumber;
                            objprac.PhoneNumber = objPractice.PhoneNumber;
                            objprac.URL = objPractice.URL == null ? string.Empty : ((objPractice.URL.StartsWith("http://") || objPractice.URL.StartsWith("https://")) ? objPractice.URL : ("http://" + objPractice.URL));
                            objprac.IsActive = true;
                            objprac.ModifiedDate = System.DateTime.Now;
                            objprac.CreatedBy = "101";
                            objprac.CreatedDate = System.DateTime.Now;
                            objprac.ModifiedBy = "101";
                            objprac.RequiresBackup = objPractice.RequiresBackup;
                            objprac.BackupPolicy=objPractice.BackupPolicy;
                            maincontext.SaveChanges();
                            return true;

                        }
                        else
                        {
                             var objpractices = new Entity.tblPractice();
                            objpractices.Address = objPractice.Address;
                            objpractices.id = objPractice.id;
                            objpractices.Name = objPractice.Name;
                            objpractices.FaxNumber = objPractice.FaxNumber;
                            objpractices.PhoneNumber = objPractice.PhoneNumber;
                            objpractices.URL = objPractice.URL;
                            objpractices.IsActive = true;
                            objpractices.ModifiedDate = System.DateTime.Now;
                            objpractices.CreatedBy = "101";
                            objpractices.CreatedDate = System.DateTime.Now;
                            objpractices.ModifiedBy = "101";
                            maincontext.tblPractices.Add(objpractices);
                            maincontext.SaveChanges();

                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                  
                }
              
            }
           
        public List<clsUser> GetUsers(long Id)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                List<clsUser> lstUsers = new List<clsUser>();
                lstUsers = (from user in maincontext.tblUsers.Where(user => user.id == (Id !=0 ? Id :user.id) &&  user.IsActive == true)
                                join Urole in maincontext.mtblRoles on user.RoleId equals Urole.id
                                select new clsUser
                                {
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                   Role = Urole.Role,
                                   Username = user.Username,
                                      Email = user.EmailId,
                                   Phone=user.Phone,
                                   RoleId=user.RoleId,
                                   id=user.id
                                }).ToList();
                 return lstUsers;

             
            }
        }


        public List<clsOnCallPeople> GetOnCallPeople(long Id)
        {
            using (var maincontext = new Entity.TelaCallEntities())
            {
                List<clsOnCallPeople> lstOnCallPeople = new List<clsOnCallPeople>();
                lstOnCallPeople = (from oncallPeople in maincontext.tblOnCallPeoples.Where(oncallPeople => oncallPeople.id == (Id != 0 ? Id : oncallPeople.id) && oncallPeople.IsActive == true)
                                   join practice in maincontext.tblPractices on oncallPeople.PracticeId equals practice.id into x
                                   from Y in x.DefaultIfEmpty()
                                   join speciality in maincontext.tblSpecialities on oncallPeople.SpecialityId equals speciality.id
                            select new clsOnCallPeople
                            {
                                FirstName = oncallPeople.FirstName,
                                LastName = oncallPeople.LastName,
                                PhoneNumber = (decimal)oncallPeople.PhoneNumber,
                                CellNo = (decimal)oncallPeople.CellNO,
                                Practice = Y.Name,
                                Speciality = speciality.Speciality,
                                SpecialityId=oncallPeople.SpecialityId,
                                PracticeId = oncallPeople.PracticeId,
                                IsCredentialed=oncallPeople.IsCredentialed,
                                CanRequest=oncallPeople.CanRequest,
                                id=oncallPeople.id,
                                CallCarrier=oncallPeople.CallCarrier,
                                Email =oncallPeople.EmailAddress 
                            }).ToList();
                return lstOnCallPeople;


            }
        }
    }
}
