using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Projects.Models;

namespace UmanyiSMS.Modules.Projects.Controller
{
    public class DataController
    {
        public static DonorModel GetDonor(int donorID)
        {
            DonorModel donorModel = new DonorModel();
            string commandText = "SELECT DonorID,NameOfDonor FROM [Donor] WHERE DonorID=" + donorID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count != 0)
            {
                donorModel.DonorID = donorID;
                donorModel.NameOfDonor = dataTable.Rows[0][1].ToString();
            }
            return donorModel;
        }

        public static Task<bool> SaveNewDonation(DonationModel donation, string type)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = "INSERT INTO [Donation] (DonorID,AmountDonated,DateDonated,DonateTo) VALUES(@donorID,@amount,@dod,@dnt)";
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, new ObservableCollection<SqlParameter>
                {
                    new SqlParameter("@donorID", donation.DonorID),
                    new SqlParameter("@amount", donation.Amount),
                    new SqlParameter("@dod", donation.DateDonated.ToString("g")),
                    new SqlParameter("@dnt", donation.DonateTo.ToString())
                });
            });
        }

        public static Task<bool> SaveNewDonorAsync(DonorModel newDonor)
        {
            return Task.Run<bool>(delegate
            {
                string commandText = string.Concat(new string[]
                {
                    "INSERT INTO [Donor] (NameOfDonor,PhoneNo) VALUES('",
                    newDonor.NameOfDonor,
                    "','",
                    newDonor.PhoneNo,
                    "')"
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            });
        }


        public static Task<ObservableCollection<DonorListModel>> GetAllDonorsAsync()
        {
            return Task.Run<ObservableCollection<DonorListModel>>(delegate
            {
                ObservableCollection<DonorListModel> observableCollection = new ObservableCollection<DonorListModel>();
                string commandText = "SELECT d.DonorID, d.NameOfDonor,d.PhoneNo, ISNULL(SUM(CONVERT(decimal(18,0),dn.AmountDonated)),0) FROM [Donor] d LEFT OUTER JOIN [Donation] dn ON(d.DonorID=dn.DonorID) GROUP BY d.DonorID,d.NameOfDonor,d.PhoneNo";
                var dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new DonorListModel
                    {
                        DonorID = int.Parse(dataRow[0].ToString()),
                        NameOfDonor = dataRow[1].ToString(),
                        PhoneNo = dataRow[2].ToString(),
                        TotalDonations = decimal.Parse(dataRow[3].ToString())
                    });
                }
                return observableCollection;
            });
        }

        internal static Task<bool> RemoveDonationAsync(int selectedDonationID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "DELETE FROM [Donation] WHERE DonationID=@did";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@did", selectedDonationID));
                return DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
            });
        }
    

        public static Task<bool> SaveNewProject(ProjectModel newProject)
        {
            return Task.Run<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id INT; SET @id=dbo.GetNewID('dbo.ProjectHeader')INSERT INTO [ProjectHeader] ([ProjectID],[NameOfProject],[StartDateTime],[EndDateTime],[Budget],[Description]) VALUES(@id,@nameOfProject,@starts,@ends,@budget,@description1)\r\n";
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                observableCollection.Add(new SqlParameter("@nameOfProject", newProject.Name));
                observableCollection.Add(new SqlParameter("@starts", newProject.StartDate.ToString("g")));
                observableCollection.Add(new SqlParameter("@ends", newProject.EndDate.ToString("g")));
                observableCollection.Add(new SqlParameter("@budget", newProject.Budget));
                observableCollection.Add(new SqlParameter("@description1", newProject.Description));
                int num = 0;
                foreach (ProjectDetailModel current in newProject.Tasks)
                {
                    string text2 = "@name" + num;
                    string text3 = "@allocation" + num;
                    string text4 = "@starts" + num;
                    string text5 = "@ends" + num;
                    string text6 = text;
                    text = string.Concat(new string[]
                    {
                        text6,
                        "\r\nINSERT INTO [ProjectDetail] (ProjectID,Name,Allocation,StartDate,EndDate) VALUES(@id,",
                        text2,
                        ",",
                        text3,
                        ",",
                        text4,
                        ",",
                        text5,
                        ")"
                    });
                    observableCollection.Add(new SqlParameter(text2, current.Name));
                    observableCollection.Add(new SqlParameter(text3, current.Allocation));
                    observableCollection.Add(new SqlParameter(text4, current.StartDate.ToString("g")));
                    observableCollection.Add(new SqlParameter(text5, current.EndDate.ToString("g")));
                    num++;
                }
                text += "\r\nCOMMIT";
                return DataAccessHelper.Helper.ExecuteNonQuery(text, observableCollection);
            });
        }

        public static Task<ObservableCollection<ProjectBaseModel>> GetAllProjectsDisplay()
        {
            return Task.Run<ObservableCollection<ProjectBaseModel>>(() => GetProjectsDisplay(new DateTime(2015, 1, 1), DateTime.Now.AddDays(1.0)));
        }

        public static Task<ObservableCollection<ProjectListModel>> GetAllProjects()
        {
            return Task.Run<ObservableCollection<ProjectListModel>>(() => GetProjects(new DateTime(2015, 1, 1), DateTime.Now.AddDays(1.0)));
        }

        private static ObservableCollection<ProjectListModel> GetProjects(DateTime startDate, DateTime endDate)
        {
            ObservableCollection<ProjectListModel> observableCollection = new ObservableCollection<ProjectListModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT p.ProjectID, p.NameOfProject,p.Budget, ISNULL(SUM(CONVERT(decimal(18,0),pd.Allocation)),0), p.StartDateTime,p.EndDateTime FROM [ProjectHeader] p LEFT OUTER JOIN [ProjectDetail]pd ON (p.ProjectID=pd.ProjectID) WHERE p.StartDateTime >= CONVERT(datetime,'",
                startDate.Day,
                "/",
                startDate.Month,
                "/",
                startDate.Year,
                " 00:00:00.000') AND p.EndDateTime<= CONVERT(datetime,'",
                endDate.Day,
                "/",
                endDate.Month,
                "/",
                endDate.Year,
                " 23:59:59.998') GROUP BY p.ProjectID, p.NameOfProject,p.Budget,p.StartDateTime,p.EndDateTime"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new ProjectListModel
                {
                    ProjectID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString(),
                    Budget = decimal.Parse(dataRow[2].ToString()),
                    CurrentAllocation = decimal.Parse(dataRow[3].ToString()),
                    StartDate = DateTime.Parse(dataRow[4].ToString()),
                    EndDate = DateTime.Parse(dataRow[5].ToString())
                });
            }
            return observableCollection;
        }

        private static ObservableCollection<ProjectBaseModel> GetProjectsDisplay(DateTime startDate, DateTime endDate)
        {
            ObservableCollection<ProjectBaseModel> observableCollection = new ObservableCollection<ProjectBaseModel>();
            string commandText = string.Concat(new object[]
            {
                "SELECT [ProjectID], [NameOfProject] FROM [ProjectHeader] WHERE [StartDateTime] >= CONVERT(datetime,'",
                startDate.Day,
                "/",
                startDate.Month,
                "/",
                startDate.Year,
                " 00:00:00.000') AND [EndDateTime]<= CONVERT(datetime,'",
                endDate.Day,
                "/",
                endDate.Month,
                "/",
                endDate.Year,
                " 23:59:59.998')"
            });
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                observableCollection.Add(new ProjectBaseModel
                {
                    ProjectID = int.Parse(dataRow[0].ToString()),
                    Name = dataRow[1].ToString()
                });
            }
            return observableCollection;
        }

        public static Task<ObservableCollection<ProjectTaskModel>> GetProjectTasksAsync(int projectID)
        {
            return Task.Run<ObservableCollection<ProjectTaskModel>>(delegate
            {
                ObservableCollection<ProjectTaskModel> observableCollection = new ObservableCollection<ProjectTaskModel>();
                string commandText = "SELECT ProjectDetailID,[Name],[Allocation],StartDate,EndDate FROM [ProjectDetail] WHERE ProjectID=" + projectID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new ProjectTaskModel
                        {
                            TaskID = int.Parse(dataRow[0].ToString()),
                            NameOfTask = dataRow[1].ToString(),
                            Allocation = decimal.Parse(dataRow[2].ToString()),
                            StartDate = DateTime.Parse(dataRow[3].ToString()),
                            EndDate = DateTime.Parse(dataRow[4].ToString())
                        });
                    }
                }
                return observableCollection;
            });
        }

        public static Task<bool> SaveNewProjectTimeLineAsync(int projectID, ObservableCollection<ProjectTaskModel> allTasks)
        {
            return Task.Run<bool>(delegate
            {
                string text = "DELETE FROM [ProjectDetail] WHERE ProjectID=" + projectID;
                bool flag = DataAccessHelper.Helper.ExecuteNonQuery(text);
                ObservableCollection<SqlParameter> observableCollection = new ObservableCollection<SqlParameter>();
                text = "";
                for (int i = 0; i < allTasks.Count; i++)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "INSERT INTO [ProjectDetail] (ProjectID,[Name],Allocation,StartDate,EndDate) VALUES(@projID,@nam",
                        i,
                        ",@all",
                        i,
                        ",@startd",
                        i,
                        ",@endd",
                        i,
                        ")\r\n"
                    });
                    observableCollection.Add(new SqlParameter("@nam" + i, allTasks[i].NameOfTask));
                    observableCollection.Add(new SqlParameter("@all" + i, allTasks[i].Allocation));
                    observableCollection.Add(new SqlParameter("@startd" + i, allTasks[i].StartDate.ToString("g")));
                    observableCollection.Add(new SqlParameter("@endd" + i, allTasks[i].EndDate.ToString("g")));
                }
                observableCollection.Add(new SqlParameter("@projID", projectID));
                return flag && DataAccessHelper.Helper.ExecuteNonQuery(text, observableCollection);
            });
        }

        public static Task<ObservableCollection<DonationModel>> GetDonationsAsync(int? donorID, DateTime? from, DateTime? to)
        {
            return Task.Run<ObservableCollection<DonationModel>>(() => GetDonations(donorID, from, to));
        }

        private static ObservableCollection<DonationModel> GetDonations(int? donorID, DateTime? from, DateTime? to)
        {
            ObservableCollection<DonationModel> observableCollection = new ObservableCollection<DonationModel>();
            ObservableCollection<DonationModel> result;
            try
            {
                string text = "SELECT dn.DonationID,dn.DonorID,d.NameOfDonor,dn.AmountDonated, dn.DonateTo,dn.[Type],dn.DateDonated FROM [Donation] dn LEFT OUTER JOIN [Donor]d ON(dn.DonorID=d.DonorID)";
                if (donorID.HasValue)
                {
                    text = text + " WHERE dn.DonorID =" + donorID.Value;
                    if (from.HasValue && to.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND dn.DateDonated BETWEEN CONVERT(datetime,'",
                            from.Value.Day.ToString(),
                            "/",
                            from.Value.Month.ToString(),
                            "/",
                            from.Value.Year.ToString(),
                            " 00:00:00.000') AND CONVERT(datetime,'",
                            to.Value.Day.ToString(),
                            "/",
                            to.Value.Month.ToString(),
                            "/",
                            to.Value.Year.ToString(),
                            " 23:59:59.998')"
                        });
                    }
                }
                else if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE dn.DateDonated BETWEEN CONVERT(datetime,'",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000') AND CONVERT(datetime,'",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998')"
                    });
                }
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new DonationModel
                    {
                        DonationID = int.Parse(dataRow[0].ToString()),
                        DonorID = int.Parse(dataRow[1].ToString()),
                        NameOfDonor = dataRow[2].ToString(),
                        Amount = decimal.Parse(dataRow[3].ToString()),
                        DonateTo = dataRow[4].ToString(),
                        DateDonated = DateTime.Parse(dataRow[6].ToString())
                    });
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<DonationModel>();
            }
            return result;
        }

    }
}
