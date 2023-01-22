using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Milkent.DAL
{
    public class DALPurchase
    {
        private string _ConnString;
        #region Query

        const String SQL_Add_Purchase = "SPW_AddPurchase";
        const String SQL_Add_PurchaseReceipt = "SPW_AddPurchaseReceipt";
        const String SQL_Read_SupplierPurchases = "SPW_ReadSupplierPurchases";
        const String SQL_Read_AllPurchases = "SPW_GetAllPurchases";
        const String SQL_Read_Unpaid_SupplierPurchases = "SPW_ReadUnpaidSupplierPurchases";
        const String SQL_Delete_Purchase = "SPW_DeletePurchase";
        const String SQL_Update_Purchase = "SPW_EditPurchase";
        const String SQL_Read_Purchase = "SPW_ReadPurchase";
        const String SQL_Insert_PurchaseBill_ID = "SPW_Insert_Bill_IDInPurchase";
        const String SQL_Read_OneReceiptPurchases = "SPW_ReadOneReceipt_Purchases";

        #endregion

        #region Parameters

        const String PARM_PU_ID = "@ID";
        const String PARM_PU_SupplierID = "@SupplierID";
        const String PARM_PU_Bill_ID = "@Bill_ID";
        const String PARM_UserID = "@UserID";
        const String PARM_PU_Milk = "@Milk";
        const String PARM_PU_Fat = "@Fat";
        const String PARM_PU_LR = "@LR";
        const String PARM_PU_TS = "@TS";
        const String PARM_PU_Date = "@Date";
        const String PARM_PU_ToDate = "@ToDate";
        const String PARM_PU_FromDate = "@FromDate";
        const String PARM_PU_Credit = "@Credit";
        const String PARM_PU_Total = "@Total";
        const String PARM_PU_PurchaseRate = "@PurchasePrice";
        const String PARM_PU_PartOfDay = "@PartOfDay";

        #endregion

        #region Constructor

        public DALPurchase()
        {
            _ConnString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        #endregion

        public void Add(MdlPurchase mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[11];
                parm[0] = new SqlParameter(PARM_PU_SupplierID, SqlDbType.Int)
                {
                    Value = mdl.SupplierID
                };
                parm[1] = new SqlParameter(PARM_PU_Milk, SqlDbType.Float)
                {
                    Value = mdl.Milk
                };
                parm[2] = new SqlParameter(PARM_PU_Fat, SqlDbType.Float)
                {
                    Value = mdl.Fat
                };
                parm[3] = new SqlParameter(PARM_PU_LR, SqlDbType.Float)
                {
                    Value = mdl.LR
                };

                parm[4] = new SqlParameter(PARM_PU_TS, SqlDbType.Float)
                {
                    Value = mdl.TS
                };
                parm[5] = new SqlParameter(PARM_PU_Date, SqlDbType.Date)
                {
                    Value = mdl.Date
                };
                parm[6] = new SqlParameter(PARM_PU_Credit, SqlDbType.Money)
                {
                    Value = mdl.Credit
                };
                parm[7] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                parm[8] = new SqlParameter(PARM_PU_PurchaseRate, SqlDbType.Money)
                {
                    Value = mdl.PurchasePrice
                };
                parm[9] = new SqlParameter(PARM_PU_PartOfDay, SqlDbType.NVarChar)
                {
                    Value = mdl.PartOfDay
                };
                parm[10] = new SqlParameter(PARM_UserID, SqlDbType.Int)
                {
                    Value = mdl.UserID
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Add_Purchase, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<MdlPurchase> DalGetAllSupplierPurchases(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlPurchase> mdlList = new List<MdlPurchase>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_SupplierPurchases, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPurchase RtrnVal = new MdlPurchase();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.Milk = Model.Common.CheckDoubleNull(oRow["Milk"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchasePrice"]);
                    RtrnVal.PartOfDay = Model.Common.CheckStringNull(oRow["PartOfDay"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);

                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPurchase> DalGetAllPurchases()
        {
            try
            {
                List<MdlPurchase> mdlList = new List<MdlPurchase>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllPurchases, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPurchase RtrnVal = new MdlPurchase();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.SupplierID = Model.Common.CheckIntegerNull(oRow["SupplierID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.Milk = Model.Common.CheckDoubleNull(oRow["Milk"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchasePrice"]);
                    RtrnVal.PartOfDay = Model.Common.CheckStringNull(oRow["PartOfDay"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);

                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MdlPurchase DalGetPurchase(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                MdlPurchase RtrnVal = new MdlPurchase();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Purchase, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                  
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.SupplierID = Model.Common.CheckIntegerNull(oRow["SupplierID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.Bill_ID = Model.Common.CheckIntegerNull(oRow["PurchaseBill_ID"]);
                    RtrnVal.Milk = Model.Common.CheckDoubleNull(oRow["Milk"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchasePrice"]);
                    RtrnVal.PartOfDay = Model.Common.CheckStringNull(oRow["PartOfDay"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);

                }
                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void EditPurchase(MdlPurchase mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[8];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[1] = new SqlParameter(PARM_PU_Milk, SqlDbType.Float)
                {
                    Value = mdl.Milk
                };
                parm[2] = new SqlParameter(PARM_PU_Fat, SqlDbType.Float)
                {
                    Value = mdl.Fat
                };
                parm[3] = new SqlParameter(PARM_PU_LR, SqlDbType.Float)
                {
                    Value = mdl.LR
                };

                parm[4] = new SqlParameter(PARM_PU_TS, SqlDbType.Float)
                {
                    Value = mdl.TS
                };
                parm[5] = new SqlParameter(PARM_PU_Credit, SqlDbType.Money)
                {
                    Value = mdl.Credit
                };
                parm[6] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                parm[7] = new SqlParameter(PARM_PU_PartOfDay, SqlDbType.NVarChar)
                {
                    Value = mdl.PartOfDay
                };
                 SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Update_Purchase, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int DAL_Delete_Purchase(int ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_Purchase, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPurchase> DAL_Get_UnpaidSupplierPurchases(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlPurchase> mdlList = new List<MdlPurchase>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Unpaid_SupplierPurchases, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPurchase RtrnVal = new MdlPurchase();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Milk = Model.Common.CheckDoubleNull(oRow["Milk"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchasePrice"]);
                    RtrnVal.PartOfDay = Model.Common.CheckStringNull(oRow["PartOfDay"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);

                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int DAL_AddPurchase_Bill(MdlPurchase mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[6];
                parm[0] = new SqlParameter(PARM_PU_ToDate, SqlDbType.Date)
                {
                    Value = mdl.ToDate
                };
                parm[1] = new SqlParameter(PARM_PU_FromDate, SqlDbType.Date)
                {
                    Value = mdl.FromDate
                };
                parm[2] = new SqlParameter(PARM_PU_Date, SqlDbType.Date)
                {
                    Value = mdl.Date
                };
                parm[3] = new SqlParameter(PARM_PU_SupplierID, SqlDbType.Int)
                {
                    Value = mdl.SupplierID
                };
                parm[4] = new SqlParameter(PARM_UserID, SqlDbType.Int)
                {
                    Value = mdl.UserID
                };
                parm[5] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                return (SqlHelper.SaveDataAndGetId(this._ConnString, CommandType.StoredProcedure, SQL_Add_PurchaseReceipt, parm));

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int DAL_Insert_Bill_ID_InPurchaseTbl(int ID,int Bill_ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[2];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                parm[1] = new SqlParameter(PARM_PU_Bill_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Insert_PurchaseBill_ID, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPurchase> DAL_GetAllPurchasesOf_OneReceipt(int Bill_ID)//Get all Purchases of a one Receipt
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_Bill_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                List<MdlPurchase> mdlList = new List<MdlPurchase>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_OneReceiptPurchases, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPurchase RtrnVal = new MdlPurchase();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.Milk = Model.Common.CheckDoubleNull(oRow["Milk"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchasePrice"]);
                    RtrnVal.PartOfDay = Model.Common.CheckStringNull(oRow["PartOfDay"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);

                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}