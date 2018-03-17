using System;
using System.Data;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.MySystem.ViewModels
{
	[PrincipalPermission(SecurityAction.Demand, Role = "SystemAdmin")]
	public class QueryEditorVM: ViewModelBase
	{
		private string _query;
		private DataTable result;
		private bool succResult = false;
		public QueryEditorVM()
		{
			InitVars();
			CreateCommands();
		}
		protected override void InitVars()
		{
			Query = "";
			Result = new DataTable();
		}

		protected override void CreateCommands()
		{
			ExecuteCommand = new RelayCommand(async o => {
				IsBusy = true;
				bool hasReturn = await HasReturn(_query);
				await Task.Factory.StartNew(() => {
					try {
						if (hasReturn)
							Result = DataAccessHelper.Helper.ExecuteNonQueryWithResult(_query);
						else {
							succResult = DataAccessHelper.Helper.ExecuteNonQuery(_query);
							if (succResult)
								MessageBox.Show("Succesfully completed operation.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
						}
					} catch (Exception e) {
						MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					
				});
				IsBusy = false; 
			}, o => true);
		}

		private Task<bool> HasReturn(string query)
		{
			return Task.Factory.StartNew<bool>(() => {
				bool temp = true;
				int c1 = -1, c2 = -1, c3 = -1;
				try {
					c1 = query.IndexOf("SELECT", StringComparison.InvariantCultureIgnoreCase);
				} catch {
				}

				try {
					c2 = query.IndexOf("INSERT", StringComparison.InvariantCultureIgnoreCase);
				} catch {
				}

				try {
					c2 = query.IndexOf("UPDATE", StringComparison.InvariantCultureIgnoreCase);
				} catch {
				}

				if (c2 != -1)
				if (c1 > c2)
					return false;
				if (c3 != -1)
				if (c1 > c3)
					return false;
				if (c1 == -1 && c2 == -1)
					return false;
				if (c1 == -1 && c3 == -1)
					return false;

				return temp;
			});
		}

		public string Query {
			get { return this._query; }

			set {
				if (value != this._query) {
					this._query = value;
					NotifyPropertyChanged("Query");
				}
			}
		}

		public DataTable Result {
			get { return this.result; }

			set {
				if (value != this.result) {
					this.result = value;
					NotifyPropertyChanged("Result");
				}
			}
		}

		public ICommand ExecuteCommand
       { get; private set; }

		public override void Reset()
		{
		}
	}
}
