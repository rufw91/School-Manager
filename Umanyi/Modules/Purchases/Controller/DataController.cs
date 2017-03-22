using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Controllers;
using UmanyiSMS.Modules.Purchases.Models;

namespace UmanyiSMS.Modules.Purchases.Controller
{
    public class DataController
    {

        public static Task<ItemModel> GetItemAsync(long itemID)
        {
            return Task.Factory.StartNew<ItemModel>(() => GetItem(itemID));
        }

        internal static ItemModel GetItem(long itemID)
        {
            ItemModel itemModel = new ItemModel();
            string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,StartQuantity FROM [Item] WHERE ItemID =" + itemID;
            DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                itemModel.ItemID = long.Parse(dataRow[0].ToString());
                itemModel.Description = dataRow[1].ToString();
                itemModel.DateAdded = DateTime.Parse(dataRow[2].ToString());
                itemModel.ItemCategoryID = int.Parse(dataRow[3].ToString());
                itemModel.Price = decimal.Parse(dataRow[4].ToString());
                itemModel.Cost = 1m;
                itemModel.StartQuantity = decimal.Parse(dataRow[6].ToString());
            }
            return itemModel;
        }


        public static Task<bool> RemoveSupplierAsync(int supplierID)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText = "DELETE FROM [Supplier] WHERE SupplierID=" + supplierID + ";";
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<int> GetLastSupplierPaymentIDAsync(int supplierID, DateTime datePaid)
        {
            return Task.Factory.StartNew<int>(delegate
            {
                string commandText = string.Concat(new object[]
                {
                    "SELECT SupplierPaymentID FROM [SupplierPayment] WHERE SupplierID=@suppID AND CONVERT(date,DatePaid)=CONVERT(date,@dtp)"
                });
                int result;
                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();
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
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [Item] (ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,StartQuantity) VALUES(",
                    item.ItemID,
                    ",'",
                    item.Description,
                    "','",
                    item.DateAdded.ToString("g"),
                    "',",
                    item.ItemCategoryID,
                    ",",
                    item.Price,
                    ",",
                    item.Cost,
                    ",",
                    item.StartQuantity,
                    ")"
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
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
                    purchaseModel.OrderTotal = decimal.Parse(dataRow[2].ToString());
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
            string commandText = "SELECT sod.ItemID,p.Description,sod.UnitCost,sod.Quantity FROM Sales.ItemReceiptDetail sod LEFT OUTER JOIN Sales.Item p ON( sod.ItemID = p.ItemID) WHERE sod.ItemReceiptID = " + saleId;
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



        public static Task<ObservableCollection<ItemListModel>> GetAllItemsWithCurrentQuantityAsync()
        {
            return Task.Factory.StartNew<ObservableCollection<ItemListModel>>(delegate
            {
                ObservableCollection<ItemListModel> observableCollection = new ObservableCollection<ItemListModel>();
                string commandText = "SELECT ItemID,Description,DateAdded,ItemCategoryID,Price,Cost,StartQuantity,dbo.GetCurrentQuantity(ItemID) FROM [Item]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemListModel
                    {
                        ItemID = long.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        DateAdded = DateTime.Parse(dataRow[2].ToString()),
                        ItemCategoryID = int.Parse(dataRow[3].ToString()),
                        Price = decimal.Parse(dataRow[4].ToString()),
                        Cost = decimal.Parse(dataRow[5].ToString()),
                        StartQuantity = decimal.Parse(dataRow[6].ToString()),
                        CurrentQuantity = decimal.Parse(dataRow[7].ToString())
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
                string commandText = "SELECT ItemCategoryID,Description,ISNULL(ParentCategoryID,0) FROM [ItemCategory]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new ItemCategoryModel
                    {
                        ItemCategoryID = int.Parse(dataRow[0].ToString()),
                        Description = dataRow[1].ToString(),
                        ParentCategoryID = int.Parse(dataRow[2].ToString())
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
                string commandText = string.Concat(new object[]
                {
                    "UPDATE [Item] SET Description='",
                    item.Description,
                    "', DateAdded='",
                    item.DateAdded.ToString("g"),
                    "', ItemCategoryID=",
                    item.ItemCategoryID,
                    ", Price=",
                    item.Price,
                    ", Cost=",
                    item.Cost,
                    ", StartQuantity=",
                    item.StartQuantity,
                    " WHERE ItemID=",
                    item.ItemID
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
            });
        }

        public static Task<bool> SaveNewPurchaseAsync(PurchaseModel currentPurchase)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = string.Concat(new object[]
                {
                    "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.ItemReceiptHeader')\r\nINSERT INTO [ItemReceiptHeader] (ItemReceiptID,SupplierID,OrderDate,RefNo,IsCancelled) VALUES(@id,",
                    currentPurchase.SupplierID,
                    ",'",
                    currentPurchase.OrderDate.ToString("g"),
                    "','",
                    currentPurchase.RefNo,
                    "',",
                    currentPurchase.IsCancelled ? "1" : "0",
                    ")"
                });
                foreach (ItemPurchaseModel current in currentPurchase.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [ItemReceiptDetail] (ItemReceiptID,ItemID,UnitCost,Quantity,LineTotal) VALUES(@id,",
                        current.ItemID,
                        ",",
                        current.Cost,
                        ",",
                        current.Quantity,
                        ",",
                        current.TotalAmt,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.Helper.ExecuteNonQuery(text);
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
                string commandText = "INSERT INTO [ItemCategory] (Description,ParentCategoryID) VALUES(@desc,@parCatID)";
                ObservableCollection<SqlParameter> paramColl = new ObservableCollection<SqlParameter>();
                paramColl.Add(new SqlParameter("@desc", itemCategory.Description));
                paramColl.Add(new SqlParameter("@parCatID", itemCategory.ParentCategoryID));
                bool succ = DataAccessHelper.Helper.ExecuteNonQuery(commandText, paramColl);
                return succ;
            });
        }

        public static Task<bool> SaveNewSupplierAsync(SupplierModel newSupplier)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string commandText;
                if (newSupplier.SupplierID <= 0)
                {
                    commandText = string.Concat(new string[]
                    {
                        "INSERT INTO [Supplier] (NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) VALUES('",
                        newSupplier.NameOfSupplier,
                        "','",
                        newSupplier.PhoneNo,
                        "','",
                        newSupplier.AltPhoneNo,
                        "','",
                        newSupplier.Email,
                        "','",
                        newSupplier.Address,
                        "','",
                        newSupplier.PostalCode,
                        "','",
                        newSupplier.City,
                        "','",
                        newSupplier.PINNo,
                        "')"
                    });
                }
                else
                {
                    commandText = string.Concat(new object[]
                    {
                        "INSERT INTO [Supplier] (SupplierID, NameOfSupplier,PhoneNo,AltPhoneNo,Email, Address, PostalCode, City,PINNo) VALUES(",
                        newSupplier.SupplierID,
                        ",'",
                        newSupplier.NameOfSupplier,
                        "','",
                        newSupplier.PhoneNo,
                        "','",
                        newSupplier.AltPhoneNo,
                        "','",
                        newSupplier.Email,
                        "','",
                        newSupplier.Address,
                        "','",
                        newSupplier.PostalCode,
                        "','",
                        newSupplier.City,
                        "','",
                        newSupplier.PINNo,
                        "')"
                    });
                }
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
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
                string commandText = string.Concat(new object[]
                {
                    "INSERT INTO [SupplierPayment] (SupplierID,DatePaid,AmountPaid,Notes) VALUES(",
                    newPayment.SupplierID,
                    ",'",
                    newPayment.DatePaid.ToString("g"),
                    "',",
                    newPayment.AmountPaid,
                    ",'",
                    newPayment.Notes,
                    "')"
                });
                return DataAccessHelper.Helper.ExecuteNonQuery(commandText);
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

        public static Task<ObservableCollection<StockTakingBaseModel>> GetAllStockTakings()
        {
            return Task.Factory.StartNew<ObservableCollection<StockTakingBaseModel>>(delegate
            {
                ObservableCollection<StockTakingBaseModel> observableCollection = new ObservableCollection<StockTakingBaseModel>();
                string commandText = "SELECT StockTakingID,DateTaken FROM [StockTakingHeader]";
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    observableCollection.Add(new StockTakingBaseModel
                    {
                        StockTakingID = int.Parse(dataRow[0].ToString()),
                        DateTaken = new DateTime?(DateTime.Parse(dataRow[1].ToString()))
                    });
                }
                return observableCollection;
            });
        }

        public static Task<StockTakingResultsModel> GetStockTakingResults(int stockTakingID)
        {
            return Task.Factory.StartNew<StockTakingResultsModel>(delegate
            {
                StockTakingResultsModel stockTakingResultsModel = new StockTakingResultsModel();
                string commandText = "SELECT std.ItemID,i.Description,std.AvailableQuantity,std.Expected,std.VarianceQty,CASE (dbo.GetCurrentQuantity([std].[ItemID])) \r\nWHEN 0 THEN 0\r\nELSE \r\nstd.VariancePc/dbo.GetCurrentQuantity([std].[ItemID]) END FROM [StockTakingDetail] std LEFT OUTER JOIN [Item] i ON( std.ItemID = i.ItemID) WHERE std.StockTakingID = " + stockTakingID;
                DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(commandText);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ItemStockTakingResultsModel itemStockTakingResultsModel = new ItemStockTakingResultsModel();
                    itemStockTakingResultsModel.ItemID = long.Parse(dataRow[0].ToString());
                    itemStockTakingResultsModel.Description = dataRow[1].ToString();
                    itemStockTakingResultsModel.Counted = decimal.Parse(dataRow[2].ToString());
                    itemStockTakingResultsModel.Expected = decimal.Parse(dataRow[3].ToString());
                    itemStockTakingResultsModel.VarianceQty = decimal.Parse(dataRow[4].ToString());
                    itemStockTakingResultsModel.VariancePc = decimal.Parse(dataRow[5].ToString());
                    stockTakingResultsModel.Items.Add(itemStockTakingResultsModel);
                }
                return stockTakingResultsModel;
            });
        }

        public static Task<bool> SaveNewStockTakingAsync(StockTakingModel newStockTaking)
        {
            return Task.Factory.StartNew<bool>(delegate
            {
                string text = "BEGIN TRANSACTION\r\nDECLARE @id int; SET @id = dbo.GetNewID('dbo.StockTakingHeader')\r\nINSERT INTO [StockTakingHeader] (StockTakingID,DateTaken) VALUES(@id,'" + newStockTaking.DateTaken.Value.ToString("g") + "')";
                foreach (ItemStockTakingModel current in newStockTaking.Items)
                {
                    object obj = text;
                    text = string.Concat(new object[]
                    {
                        obj,
                        "\r\nINSERT INTO [StockTakingDetail] (StockTakingID,ItemID,AvailableQuantity) VALUES(@id,",
                        current.ItemID,
                        ",",
                        current.AvailableQuantity,
                        ")"
                    });
                }
                text += "\r\nCOMMIT";
                DataAccessHelper.Helper.ExecuteNonQuery(text);
                return true;
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
                    string text = "SELECT BookReceiptID,DateReceived,ISNULL(TotalAmt,0) FROM [BookReceiptHeader] WHERE [SupplierID] =" + supplierID;
                    if (startTime.HasValue && endTime.HasValue)
                    {
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            " AND DateReceived BETWEEN CONVERT(datetime,'",
                            startTime.Value.ToString("dd-MM-yyyy"),
                            " 00:00:00.000') AND CONVERT(datetime,'",
                            endTime.Value.ToString("dd-MM-yyyy"),
                            " 23:59:59.998')"
                        });
                    }
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

                    DataTable dataTable = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text);
                    DataTable dataTable2 = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text3);
                    DataTable dataTable3 = DataAccessHelper.Helper.ExecuteNonQueryWithResultTable(text4);
                    ObservableCollection<TransactionModel> observableCollection = new ObservableCollection<TransactionModel>();
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        observableCollection.Add(new TransactionModel(TransactionTypes.Credit, "BK-" + dataRow[0].ToString(), DateTime.Parse(dataRow[1].ToString()), decimal.Parse(dataRow[2].ToString())));
                        suppStatementModel.TotalSales += decimal.Parse(dataRow[2].ToString());
                        suppStatementModel.TotalDue += decimal.Parse(dataRow[2].ToString());
                    }
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
                         "DECLARE  @pur2 decimal=(SELECT SUM(ISNULL(TotalAmt,0)) FROM  [BookReceiptHeader] WHERE SupplierID =@supplierID AND DateReceived <CONVERT(datetime,@dt));\r\n" +
                        "DECLARE  @pay decimal=(SELECT SUM(ISNULL(AmountPaid,0)) FROM  [SupplierPayment] WHERE SupplierID=@supplierID AND DatePaid <CONVERT(datetime,@dt));\r\n" +
                        "SELECT (select (ISNULL(@pur1,0)+ISNULL(@pur2,0))-ISNULL(@pay,0));";
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
