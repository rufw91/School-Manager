using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Staff.Models;

namespace UmanyiSMS.Modules.Staff.Controller
{
    public class DataController
    {
        public static Task<bool> SaveNewStaffAsync(StaffModel newStaff)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                bool flag = newStaff.StaffID == 0;
                string commandText = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('Institution.Staff'); INSERT INTO [Staff] (StaffID,Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto,Designation) VALUES(" + (flag ? "@id" : "@staffID") + ",@name,@nationalID,@doa,@phoneNo,@email,@address,@city,@postalCode,@photo,@designation)\r\nCOMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new List<SqlParameter>
                {
                    new SqlParameter("@staffID", newStaff.StaffID),
                    new SqlParameter("@name", newStaff.Name),
                    new SqlParameter("@nationalID", newStaff.NationalID),
                    new SqlParameter("@doa", newStaff.DateOfAdmission),
                    new SqlParameter("@phoneNo", newStaff.PhoneNo),
                    new SqlParameter("@email", newStaff.Email),
                    new SqlParameter("@address", newStaff.Address),
                    new SqlParameter("@city", newStaff.City),
                    new SqlParameter("@postalCode", newStaff.PostalCode),
                    new SqlParameter("@photo", newStaff.SPhoto),
                    new SqlParameter("@designation", newStaff.Designation)
                });
            });
        }


        public static Task<bool> UpdateStaffAsync(StaffModel staff)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Staff] SET Name='",
                    staff.Name,
                    "', NationalID='",
                    staff.NationalID,
                    "', DateOfAdmission='",
                    staff.DateOfAdmission,
                    "', PhoneNo='",
                    staff.PhoneNo,
                    "', Email='",
                    staff.Email,
                    "', Address='",
                    staff.Address,
                    "', PostalCode='",
                    staff.PostalCode,
                    "', City='",
                    staff.City,
                    "', SPhoto=@photo WHERE StaffID=",
                    staff.StaffID
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@photo", staff.SPhoto)
                });
            });
        }


        public static Task<StaffModel> GetStaffAsync(int staffID)
        {
            return Task.Factory.StartNew<StaffModel>(() => GetStaff(staffID));
        }

        public static StaffModel GetStaff(int staffID)
        {
            StaffModel staffModel = new StaffModel();
            try
            {
                string commandText = "SELECT Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto,Designation FROM [Staff] WHERE StaffID='" + staffID + "'";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    staffModel.StaffID = staffID;
                    staffModel.Name = dataTable.Rows[0][0].ToString();
                    staffModel.NationalID = dataTable.Rows[0][1].ToString();
                    staffModel.DateOfAdmission = DateTime.Parse(dataTable.Rows[0][2].ToString());
                    staffModel.PhoneNo = dataTable.Rows[0][3].ToString();
                    staffModel.Email = dataTable.Rows[0][4].ToString();
                    staffModel.Address = dataTable.Rows[0][5].ToString();
                    staffModel.City = dataTable.Rows[0][6].ToString();
                    staffModel.PostalCode = dataTable.Rows[0][7].ToString();
                    staffModel.SPhoto = (byte[])dataTable.Rows[0][8];
                    staffModel.Designation = dataTable.Rows[0][9].ToString();
                }
            }
            catch
            {
            }
            return staffModel;
        }

        public static Task<ObservableCollection<StaffModel>> GetAllStaffAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<StaffModel>>(delegate
            {
                string commandText = "SELECT TOP 1000000 StaffID,Name,NationalID,DateOfAdmission,PhoneNo,Email,Address,City,PostalCode,SPhoto FROM [Staff]";
                ObservableCollection<StaffModel> observableCollection = new ObservableCollection<StaffModel>();
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new StaffModel
                        {
                            StaffID = (int)dataRow[0],
                            Name = dataRow[1].ToString(),
                            NationalID = dataRow[2].ToString(),
                            DateOfAdmission = DateTime.Parse(dataRow[3].ToString()),
                            PhoneNo = dataRow[4].ToString(),
                            Email = dataRow[5].ToString(),
                            Address = dataRow[6].ToString(),
                            City = dataRow[7].ToString(),
                            PostalCode = dataRow[8].ToString(),
                            SPhoto = dataRow[9] as byte[]
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static bool SearchAllStaffProperties(StaffModel staff, string searchText)
        {
            Regex.CacheSize = 14;
            return Regex.Match(staff.StaffID.ToString(), searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Name, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.NationalID, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Address, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.City, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.Email, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.PhoneNo, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(staff.PostalCode, searchText, RegexOptions.IgnoreCase).Success;
        }

    }
}
