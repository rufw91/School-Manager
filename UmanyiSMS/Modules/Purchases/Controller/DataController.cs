using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.Controller
{
    public class DataController
    {
        public static bool SearchAllItemProperties(ItemModel item, string searchText)
        {
            if (item == null)
                return false;
            Regex.CacheSize = 14;
            return Regex.Match(item.Description, searchText, RegexOptions.IgnoreCase).Success || Regex.Match(item.ItemID.ToString(), searchText, RegexOptions.IgnoreCase).Success;
        }
        internal static Task<ItemCategoryModel> GetAccountAsync(int accountID)
        {
            return Task.Factory.StartNew<ItemCategoryModel>(() =>
            {
                string text = "SELECT ItemCategoryID,Description FROM [Sales].[ItemCategory] WHERE ItemCategoryID = @catID";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@catID", accountID));
                var result = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text, paramColl);

                if (result.Rows.Count == 0)
                    return null;
                ItemCategoryModel temp2 = new ItemCategoryModel();
                temp2.ItemCategoryID = int.Parse(result.Rows[0][0].ToString());
                temp2.Description = result.Rows[0][1].ToString();
                return temp2;

            });

        }

        public static Task<ItemModel> GetItemAsync(long itemID)
        {
            return Task.Factory.StartNew<ItemModel>(() => GetItem(itemID));
        }

        internal static ItemModel GetItem(long itemID)
        {
            ItemModel itemModel = new ItemModel();
            string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Cost FROM [Item] WHERE ItemID =@itid";
            var paramColl = new List<SqlParameter>();
            paramColl.Add(new SqlParameter("@itid", itemID));
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText,paramColl);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                itemModel.ItemID = long.Parse(dataRow[0].ToString());
                itemModel.Description = dataRow[1].ToString();
                itemModel.DateAdded = DateTime.Parse(dataRow[2].ToString());
                itemModel.ItemCategoryID = int.Parse(dataRow[3].ToString());
                itemModel.Cost = int.Parse(dataRow[4].ToString());
            }
            return itemModel;
        }


        public static Task<bool> RemoveSupplierAsync(int supplierID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "DELETE FROM [Supplier] WHERE SupplierID=@suppid;";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@suppid", supplierID));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText,paramColl);
            });
        }

        public static Task<int> GetLastSupplierPaymentIDAsync(int supplierID, DateTime datePaid)
        {
            return Task.Factory.StartNew<int>(delegate
            {
                string commandText = "SELECT SupplierPaymentID FROM [SupplierPayment] WHERE SupplierID=@suppID AND CONVERT(date,DatePaid)=CONVERT(date,@dtp)";
               
                int result;
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@suppID", supplierID));
                paramColl.Add(new SqlParameter("@dtp", datePaid));
                int.TryParse(DataAccessHelper.Helper.ExecuteScalar(commandText, paramColl), out result);
                return result;
            });
        }

        public static Task<bool> SaveNewItemAsync(ItemModel item)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "INSERT INTO [Item] (ItemID,Description,DateAdded,ItemCategoryID,Cost) "+
                "VALUES(@itid,@desc,@det,@catid,@cost)";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@itid", item.ItemID));
                paramColl.Add(new SqlParameter("@desc", item.Description));
                paramColl.Add(new SqlParameter("@det", item.DateAdded));
                paramColl.Add(new SqlParameter("@catid", item.ItemCategoryID));
                paramColl.Add(new SqlParameter("@cost", item.Cost));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText,paramColl);
            });
        }


        public static Task<ObservableCollection<PurchaseModel>> GetItemReceiptsAsync(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Factory.StartNew<ObservableCollection<PurchaseModel>>(() => GetItemReceipts(includeAllDetails, supplierID, startTime, endTime));
        }

        private static ObservableCollection<PurchaseModel> GetItemReceipts(bool includeAllDetails, int? supplierID, DateTime? startTime, DateTime? endTime)
        {
            string text;
            if (supplierID.HasValue)
            {
                text = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled,ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [ItemReceiptHeader] sh LEFT OUTER JOIN [ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID) WHERE sh.SupplierID =" + supplierID;
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " AND sh.OrderDate BETWEEN '",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998'\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            else
            {
                text = "SELECT sh.ItemReceiptID,sh.OrderDate,TotalAmt,SupplierID,IsCancelled, ISNULL(SUM(ISNULL(sd.Quantity,0)),0),RefNo FROM [ItemReceiptHeader] sh LEFT OUTER JOIN [ItemReceiptDetail] sd ON(sh.ItemReceiptID=sd.ItemReceiptID)";
                if (startTime.HasValue && endTime.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE sh.OrderDate BETWEEN '",
                        startTime.Value.Day.ToString(),
                        "/",
                        startTime.Value.Month.ToString(),
                        "/",
                        startTime.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        endTime.Value.Day.ToString(),
                        "/",
                        endTime.Value.Month.ToString(),
                        "/",
                        endTime.Value.Year.ToString(),
                        " 23:59:59.998'\r\n GROUP BY sh.ItemReceiptID,sh.OrderDate, TotalAmt,SupplierID,IsCancelled,RefNo"
                    });
                }
            }
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
            ObservableCollection<PurchaseModel> result;
            if (dataTable.Rows.Count == 0)
            {
                result = new ObservableCollection<PurchaseModel>();
            }
            else
            {
                ObservableCollection<PurchaseModel> observableCollection = new ObservableCollection<PurchaseModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PurchaseModel purchaseModel = new PurchaseModel();
                    purchaseModel.PurchaseID = int.Parse(dataRow[0].ToString());
                    purchaseModel.OrderDate = DateTime.Parse(dataRow[1].ToString());
                    if (supplierID.HasValue)
                    {
                        purchaseModel.SupplierID = supplierID.Value;
                    }
                    else
                    {
                        purchaseModel.SupplierID = int.Parse(dataRow[3].ToString());
                    }
                    purchaseModel.IsCancelled = bool.Parse(dataRow[4].ToString());
                    purchaseModel.NoOfItems = decimal.Parse(dataRow[5].ToString());
                    purchaseModel.RefNo = dataRow[6].ToString();
                    if (includeAllDetails)
                    {
                        purchaseModel.Items = GetItemsReceiptItems(purchaseModel.PurchaseID);
                        purchaseModel.NoOfItems = purchaseModel.Items.Count;
                    }
                    observableCollection.Add(purchaseModel);
                }
                result = observableCollection;
            }
            return result;
        }

        public static ObservableCollection<ItemPurchaseModel> GetItemsReceiptItems(int saleId)
        {
            ObservableCollection<ItemPurchaseModel> observableCollection = new ObservableCollection<ItemPurchaseModel>();
            string commandText = "SELECT sod.ItemID,p.Description,sod.UnitCost,sod.Quantity FROM [ItemReceiptDetail] sod LEFT OUTER JOIN [Item] p ON( sod.ItemID = p.ItemID) WHERE sod.ItemReceiptID = " + saleId;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                long itemID = long.Parse(dataRow[0].ToString());
                decimal buyingPrice;
                decimal.TryParse(dataRow[2].ToString(), out buyingPrice);
                decimal quantity;
                decimal.TryParse(dataRow[3].ToString(), out quantity);
                observableCollection.Add(new ItemPurchaseModel(itemID, dataRow[1].ToString(), quantity, buyingPrice));
            }
            return observableCollection;
        }



        public static Task<ObservableCollection<ItemModel>> GetAllItemsAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<ItemModel>>(delegate
            {
                ObservableCollection<ItemModel> observableCollection = new ObservableCollection<ItemModel>();
                string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Cost FROM [Item]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemModel
                    {
                        ItemID = long.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        DateAdded = DateTime.Parse(dataRow[2].ToString()),
                        ItemCategoryID = int.Parse(dataRow[3].ToString()),
                        Cost = decimal.Parse(dataRow[4].ToString())
                    });
                }
                return observableCollection;
            });
        }


        public static Task<ObservableCollection<ItemCategoryModel>> GetAllItemCategoriesAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<ItemCategoryModel>>(delegate
            {
                ObservableCollection<ItemCategoryModel> observableCollection = new ObservableCollection<ItemCategoryModel>();
                string commandText = "SELECT ItemCategoryID,Description FROM [ItemCategory]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemCategoryModel
                    {
                        ItemCategoryID = int.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<ObservableCollection<SupplierBaseModel>> GetAllSuppliersAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<SupplierBaseModel>>(delegate
            {
                ObservableCollection<SupplierBaseModel> observableCollection = new ObservableCollection<SupplierBaseModel>();
                string commandText = "SELECT SupplierID,NameOfSupplier FROM [Supplier]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierBaseModel
                    {
                        SupplierID = int.Parse(dataRow[0].ToString()),
                        NameOfSupplier = dataRow[1].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<bool> UpdateItemAsync(ItemModel item)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText ="UPDATE [Item] SET Description=@desc, DateAdded=@det, ItemCategoryID=@catid, Cost=@cost WHERE ItemID=@itid";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@itid", item.ItemID));
                paramColl.Add(new SqlParameter("@desc", item.Description));
                paramColl.Add(new SqlParameter("@det", item.DateAdded));
                paramColl.Add(new SqlParameter("@catid", item.ItemCategoryID));
                paramColl.Add(new SqlParameter("@cost", item.Cost));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText,paramColl);
            });
        }

        public static Task<bool> SaveNewPurchaseAsync(PurchaseModel currentPurchase)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.ItemReceiptHeader')\r\n" +
                "INSERT INTO [ItemReceiptHeader] (ItemReceiptID,SupplierID,OrderDate,RefNo,IsCancelled) VALUES(@id,@suppid,@det,@refno,@isc)";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@suppid", currentPurchase.SupplierID));
                paramColl.Add(new SqlParameter("@refno", currentPurchase.RefNo));
                paramColl.Add(new SqlParameter("@det", currentPurchase.OrderDate));
                paramColl.Add(new SqlParameter("@isc", currentPurchase.IsCancelled));
                int index = 0;
                foreach (ItemPurchaseModel current in currentPurchase.Items)
                {
                    text += "\r\nINSERT INTO [ItemReceiptDetail] (ItemReceiptID,ItemID,UnitCost,Quantity,LineTotal) " +
                    "VALUES(@id,@itid" + index + ",@cost" + index + ",@qty" + index + ",@tot" + index + ")";

                    paramColl.Add(new SqlParameter("@itid" + index, current.ItemID));
                    paramColl.Add(new SqlParameter("@cost" + index, current.Cost));
                    paramColl.Add(new SqlParameter("@qty" + index, current.Quantity));
                    paramColl.Add(new SqlParameter("@tot" + index, current.TotalAmt));
                    index++;
                }
                text += "\r\nCOMMIT";
                
                DataAccessHelper.Helper.ExecuteNonQuery(text,paramColl);
                return true;
            });
        }

        public static Task<bool> UpdateSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Supplier] SET NameOfSupplier='",
                    newSupplier.NameOfSupplier,
                    "', PhoneNo='",
                    newSupplier.PhoneNo,
                    "', AltPhoneNo='",
                    newSupplier.AltPhoneNo,
                    "', Email='",
                    newSupplier.Email,
                    "', Address='",
                    newSupplier.Address,
                    "', PostalCode='",
                    newSupplier.PostalCode,
                    "', City='",
                    newSupplier.City,
                    "', PINNo='",
                    newSupplier.PINNo,
                    "' WHERE SupplierID=",
                    newSupplier.SupplierID
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewItemCategoryAsync(ItemCategoryModel itemCategory)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "INSERT INTO [ItemCategory] (Description) VALUES(@desc)";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@desc", itemCategory.Description));
                bool succ = DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
                return succ;
            });
        }

        public static Task<bool> SaveNewSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "INSERT INTO [Supplier] (NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) " +
                        "VALUES(@nam,@phone,@altphone,@email,@address,@postcode,@city,@pinno)";

                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@nam", newSupplier.NameOfSupplier));
                paramColl.Add(new SqlParameter("@phone", newSupplier.PhoneNo));
                paramColl.Add(new SqlParameter("@altphone", newSupplier.AltPhoneNo));
                paramColl.Add(new SqlParameter("@email", newSupplier.Email));
                paramColl.Add(new SqlParameter("@address", newSupplier.Address));
                paramColl.Add(new SqlParameter("@postcode", newSupplier.PostalCode));
                paramColl.Add(new SqlParameter("@city", newSupplier.City));
                paramColl.Add(new SqlParameter("@pinno", newSupplier.PINNo));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
            });
        }

        public static Task<ObservableCollection<SupplierPaymentModel>> GetSupplierPaymentsAsync(int? supplierId, DateTime? from, DateTime? to)
        {
            return Task.Factory.StartNew<ObservableCollection<SupplierPaymentModel>>(() => GetSupplierPayments(supplierId, from, to));
        }

        private static ObservableCollection<SupplierPaymentModel> GetSupplierPayments(int? supplierId, DateTime? from, DateTime? to)
        {
            ObservableCollection<SupplierPaymentModel> observableCollection = new ObservableCollection<SupplierPaymentModel>();
            ObservableCollection<SupplierPaymentModel> result;
            try
            {
                string text = "SELECT sp.SupplierPaymentID,sp.SupplierID,s.NameOfSupplier, sp.AmountPaid,sp.DatePaid,sp.Notes FROM [SupplierPayment] sp LEFT OUTER JOIN [Supplier] s ON (sp.SupplierID=s.SupplierID)";
                if (supplierId.HasValue)
                {
                    text = text + " WHERE sp.SupplierID =" + supplierId;
                    if (from.HasValue && to.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND sp.DatePaid BETWEEN '",
                            from.Value.Day.ToString(),
                            "/",
                            from.Value.Month.ToString(),
                            "/",
                            from.Value.Year.ToString(),
                            " 00:00:00.000' AND '",
                            to.Value.Day.ToString(),
                            "/",
                            to.Value.Month.ToString(),
                            "/",
                            to.Value.Year.ToString(),
                            " 23:59:59.998'"
                        });
                    }
                }
                else if (from.HasValue && to.HasValue)
                {
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " WHERE sp.DatePaid BETWEEN '",
                        from.Value.Day.ToString(),
                        "/",
                        from.Value.Month.ToString(),
                        "/",
                        from.Value.Year.ToString(),
                        " 00:00:00.000' AND '",
                        to.Value.Day.ToString(),
                        "/",
                        to.Value.Month.ToString(),
                        "/",
                        to.Value.Year.ToString(),
                        " 23:59:59.998'"
                    });
                }
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierPaymentModel(int.Parse(dataRow[0].ToString()), int.Parse(dataRow[1].ToString()), dataRow[2].ToString(), decimal.Parse(dataRow[3].ToString()), DateTime.Parse(dataRow[4].ToString()), dataRow[5].ToString()));
                }
                result = observableCollection;
            }
            catch
            {
                result = new ObservableCollection<SupplierPaymentModel>();
            }
            return result;
        }

        public static Task<bool> SaveNewSupplierPaymentAsync(SupplierPaymentModel newPayment)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "INSERT INTO [SupplierPayment] (SupplierID,DatePaid,AmountPaid,Notes) "+
                    "VALUES(@suppID,@det,@amt,@notes)";
                var paramColl = new List<SqlParameter>();
                paramColl.Add(new SqlParameter("@suppID", newPayment.SupplierID));
                paramColl.Add(new SqlParameter("@det", newPayment.DatePaid));
                paramColl.Add(new SqlParameter("@amt", newPayment.AmountPaid));
                paramColl.Add(new SqlParameter("@notes", newPayment.Notes));
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
            });
        }

        public static Task<SupplierModel> GetSupplierAsync(int supplierID)
        {
            return Task.Factory.StartNew<SupplierModel>(() => GetSupplier(supplierID));
        }

        public static SupplierModel GetSupplier(int supplierID)
        {
            SupplierModel supplierModel = new SupplierModel();
            string commandText = "SELECT SupplierID, NameOfSupplier, PhoneNo, AltPhoneNo, Email, Address, PostalCode, City, PINNo FROM [Supplier] WHERE SupplierID=" + supplierID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count >= 1)
            {
                DataRow dataRow = dataTable.Rows[0];
                supplierModel.SupplierID = int.Parse(dataRow[0].ToString());
                supplierModel.NameOfSupplier = dataRow[1].ToString();
                supplierModel.PhoneNo = dataRow[2].ToString();
                supplierModel.AltPhoneNo = dataRow[3].ToString();
                supplierModel.Email = dataRow[4].ToString();
                supplierModel.Address = dataRow[5].ToString();
                supplierModel.PostalCode = dataRow[6].ToString();
                supplierModel.City = dataRow[7].ToString();
                supplierModel.PINNo = dataRow[8].ToString();
            }
            return supplierModel;
        }

        public static Task<ObservableCollection<SupplierPaymentModel>> GetRecentSupplierPaymentsAsync(SupplierBaseModel selectedSupplier)
        {
            return Task.Factory.StartNew<ObservableCollection<SupplierPaymentModel>>(delegate
            {
                ObservableCollection<SupplierPaymentModel> observableCollection = new ObservableCollection<SupplierPaymentModel>();
                string commandText = "SELECT TOP 20 SupplierPaymentID,AmountPaid, DatePaid,Notes FROM [SupplierPayment] " +
                    "WHERE SupplierID =@suppID ORDER BY [DatePaid] desc";
                var paramColl = new ObservableCollection<SqlParameter>();
                paramColl.Add(new SqlParameter("@suppID", selectedSupplier.SupplierID));
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText, paramColl);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SupplierPaymentModel pmt = new SupplierPaymentModel();
                    pmt.SupplierPaymentID = int.Parse(dataRow[0].ToString());
                    pmt.AmountPaid = decimal.Parse(dataRow[1].ToString());
                    pmt.SupplierID = selectedSupplier.SupplierID;
                    pmt.NameOfSupplier = selectedSupplier.NameOfSupplier;
                    pmt.DatePaid = DateTime.Parse(dataRow[2].ToString());
                    pmt.Notes = dataRow[3].ToString();
                    observableCollection.Add(pmt);
                }
                return observableCollection;
            });
        }
        
        public static Task<ObservableCollection<SupplierModel>> GetAllSuppliersFullAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<SupplierModel>>(delegate
            {
                ObservableCollection<SupplierModel> observableCollection = new ObservableCollection<SupplierModel>();
                string commandText = "SELECT SupplierID,NameOfSupplier,PhoneNo,AltPhoneNo,Email,Address,PostalCode,City,PINNo FROM [Supplier]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new SupplierModel
                    {
                        SupplierID = int.Parse(dataRow[0].ToString()),
                        NameOfSupplier = dataRow[1].ToString(),
                        PhoneNo = dataRow[2].ToString(),
                        AltPhoneNo = dataRow[3].ToString(),
                        Email = dataRow[4].ToString(),
                        Address = dataRow[5].ToString(),
                        PostalCode = dataRow[6].ToString(),
                        City = dataRow[7].ToString(),
                        PINNo = dataRow[8].ToString()
                    });
                }
                return observableCollection;
            });
        }

        public static Task<SupplierStatementModel> GetSupplierStatementAsync(int supplierID, DateTime? startTime, DateTime? endTime)
        {
            return Task.Factory.StartNew<SupplierStatementModel>(delegate
            {
                SupplierStatementModel result;
                if (supplierID <= 0)
                {
                    result = new SupplierStatementModel();
                }
                else
                {
                    SupplierStatementModel suppStatementModel = new SupplierStatementModel();
                   
                    string text3 = "SELECT ItemReceiptID, OrderDate, TotalAmt FROM [ItemReceiptHeader]  WHERE [SupplierID] =" + supplierID;
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text3;
                        text3 = string.Concat(new string[]
                        {
                            text2,
                            " AND OrderDate BETWEEN CONVERT(datetime,'",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(datetime,'",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                        });
                    }

                    string text4 = "SELECT SupplierPaymentID,DatePaid,AmountPaid FROM [Supplierpayment] WHERE SupplierID=" + supplierID;
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        text4 += " AND DatePaid BETWEEN CONVERT(datetime,'" + startTime.Value.ToString("dd-MM-yyyy") +
                            " 00:00:00.000') AND CONVERT(datetime,'" + endTime.Value.ToString("dd-MM-yyyy") + " 23:59:59.998')";
                    }

                    
                    DataTable dataTable2 = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text3);
                    DataTable dataTable3 = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text4);
                    ObservableCollection<TransactionModel> observableCollection = new ObservableCollection<TransactionModel>();
                    
                    foreach (DataRow dataRow in dataTable2.Rows)
                    {
                        DateTime transactionDateTime;
                        DateTime.TryParse(dataRow[1].ToString(), out transactionDateTime);
                        decimal num;
                        decimal.TryParse(dataRow[2].ToString(), out num);
                        observableCollection.Add(new TransactionModel(TransactionTypes.Credit, "INV-" + dataRow[0].ToString(), transactionDateTime, num));
                        suppStatementModel.TotalPayments += num;
                        suppStatementModel.TotalDue -= num;
                    }

                    foreach (DataRow dataRow in dataTable3.Rows)
                    {
                        observableCollection.Add(new TransactionModel(TransactionTypes.Debit, "PMT-" + dataRow[0].ToString(), DateTime.Parse(dataRow[1].ToString()), decimal.Parse(dataRow[2].ToString())));
                        suppStatementModel.TotalSales += decimal.Parse(dataRow[2].ToString());
                        suppStatementModel.TotalDue += decimal.Parse(dataRow[2].ToString());
                    }

                    IEnumerable<TransactionModel> enumerable = from fruit in observableCollection
                                                               orderby fruit.TransactionDateTime
                                                               select fruit;
                    suppStatementModel.BalanceBroughtForward = GetCurrentSupplierBalanceAsync(supplierID, startTime.Value).Result;
                    suppStatementModel.Transactions.Add(new TransactionModel(TransactionTypes.Credit, "0", DateTime.Now, suppStatementModel.BalanceBroughtForward));
                    foreach (TransactionModel current in enumerable)
                    {
                        suppStatementModel.Transactions.Add(current);
                    }
                    suppStatementModel.SupplierID = supplierID;
                    suppStatementModel.From = startTime.Value;
                    suppStatementModel.To = endTime.Value;
                    suppStatementModel.TotalDue = GetCurrentSupplierBalanceAsync(supplierID, endTime.Value).Result;
                    result = suppStatementModel;
                }
                return result;
            });
        }

        private static Task<decimal> GetCurrentSupplierBalanceAsync(int supplierID, DateTime date)
        {
            return Task.Factory.StartNew<decimal>(delegate
            {
                string commandText = "DECLARE  @pur1 decimal=(SELECT SUM(ISNULL(TotalAmt,0)) FROM  [ItemReceiptHeader] WHERE SupplierID =@supplierID AND OrderDate <CONVERT(datetime,@dt));\r\n" +
                         
                        "DECLARE  @pay decimal=(SELECT SUM(ISNULL(AmountPaid,0)) FROM  [SupplierPayment] WHERE SupplierID=@supplierID AND DatePaid <CONVERT(datetime,@dt));\r\n" +
                        "SELECT (select ISNULL(@pur1,0)-ISNULL(@pay,0));";
                decimal result;
                var paramColl = new ObservableCollection<SqlParameter>();
                paramColl.Add(new SqlParameter("@supplierID", supplierID));
                paramColl.Add(new SqlParameter("@dt", date));
                decimal.TryParse(DataAccessHelper.Helper.ExecuteScalar(commandText, paramColl), out result);
                return result;
            });
        }


    }
}
