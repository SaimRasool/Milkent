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
    public class DALSales
    {
        private string _ConnString;

        #region Query

        const String SQL_Add_Sale = "SPW_AddSale";
        const String SQL_Read_CustomerSales = "SPW_ReadCustomerSales";
        const String SQL_Read_AllSales = "SPW_GetAllSales";
        const String SQL_Delete_Sale = "SPW_DeleteSale";
        const String SQL_Update_Sale = "SPW_EditSale";
        const String SQL_Read_Sale = "SPW_ReadSale";
        const String SQL_Read_Unpaid_CustomerSale = "SPW_ReadUnpaidCustomerSales";
        const String SQL_Insert_SaleBill_IDinSalesTbl = "SPW_Insert_Bill_IDInSaleTbl";
        const String SQL_Read_SalesOf_OneReceipt = "SPW_Read_SalesOf_OneReceipt";
        const String SQL_Add_SaleReceipt = "SPW_AddSaleReceipt";
        #endregion

        #region Parameters

        const String PARM_PU_ID = "@ID";
        const String PARM_PU_Bill_ID = "@Bill_ID";
        const String PARM_UserID = "@UserID";
        const String PARM_PU_CustomerID = "@CustomerID";
        const String PARM_PU_MilkCredit = "@MilkCredit";
        const String PARM_PU_Fat = "@Fat";
        const String PARM_PU_LR = "@LR";
        const String PARM_PU_TS = "@TS";
        const String PARM_PU_Date = "@Date";
        const String PARM_PU_CashDebit = "@CashDebit";
        const String PARM_PU_Total = "@Total";
        const String PARM_PU_SalesPrice = "@SalePrice";
        const String PARM_PU_PartOfDay = "@PartOfDay";
        const String PARM_PU_ToDate = "@ToDate";
        const String PARM_PU_FromDate = "@FromDate";
        #endregion

        #region Constructor

        public DALSales()
        {
            _ConnString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        #endregion

        public void Add(MdlSales mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[10];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = mdl.CustomerID
                };
                parm[1] = new SqlParameter(PARM_PU_MilkCredit, SqlDbType.Float)
                {
                    Value = mdl.MilkCredit
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
                parm[6] = new SqlParameter(PARM_PU_CashDebit, SqlDbType.Money)
                {
                    Value = mdl.CashDebit
                };
                parm[7] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                parm[8] = new SqlParameter(PARM_PU_SalesPrice, SqlDbType.Money)
                {
                    Value = mdl.SalePrice
                };
                parm[9] = new SqlParameter(PARM_PU_PartOfDay, SqlDbType.NVarChar)
                {
                    Value = mdl.PartOfDay
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Add_Sale, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<MdlSales> DAL_GetAllCustomerSales(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlSales> mdlList = new List<MdlSales>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_CustomerSales, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSales RtrnVal = new MdlSales();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.MilkCredit = Model.Common.CheckDoubleNull(oRow["MilkCredit"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["CashDebit"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SalePrice"]);
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

        public MdlSales DAL_GetSale(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                MdlSales RtrnVal = new MdlSales();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Sale, parm);
                foreach (DataRow oRow in oTable.Rows)
                {

                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.Bill_ID = Model.Common.CheckIntegerNull(oRow["SaleBill_ID"]);
                    RtrnVal.MilkCredit = Model.Common.CheckDoubleNull(oRow["MilkCredit"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["CashDebit"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SalePrice"]);
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

        public void DAL_EditSale(MdlSales mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[8];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[1] = new SqlParameter(PARM_PU_MilkCredit, SqlDbType.Float)
                {
                    Value = mdl.MilkCredit
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
                parm[5] = new SqlParameter(PARM_PU_CashDebit, SqlDbType.Money)
                {
                    Value = mdl.CashDebit
                };
                parm[6] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                parm[7] = new SqlParameter(PARM_PU_PartOfDay, SqlDbType.NVarChar)
                {
                    Value = mdl.PartOfDay
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Update_Sale, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int DAL_DeleteSales(int ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                
                 return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_Sale, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlSales> DAL_GetAllSales()
        {
            try
            {
                List<MdlSales> mdlList = new List<MdlSales>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllSales, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSales RtrnVal = new MdlSales();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.MilkCredit = Model.Common.CheckDoubleNull(oRow["MilkCredit"]);
                    RtrnVal.CustomerID = Model.Common.CheckIntegerNull(oRow["CustomerID"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["CashDebit"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SalePrice"]);
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

        public List<MdlSales> DAL_Get_UnpaidCustomerSales(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlSales> mdlList = new List<MdlSales>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Unpaid_CustomerSale, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSales RtrnVal = new MdlSales();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.MilkCredit = Model.Common.CheckDoubleNull(oRow["MilkCredit"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["CashDebit"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SalePrice"]);
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

        public int DAL_AddSales_Bill(MdlSales mdl)
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
                parm[3] = new SqlParameter(PARM_PU_CustomerID, SqlDbType.Int)
                {
                    Value = mdl.CustomerID
                };
                parm[4] = new SqlParameter(PARM_UserID, SqlDbType.Int)
                {
                    Value = mdl.UserID
                };
                parm[5] = new SqlParameter(PARM_PU_Total, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                return (SqlHelper.SaveDataAndGetId(this._ConnString, CommandType.StoredProcedure, SQL_Add_SaleReceipt, parm));

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int DAL_Insert_Bill_ID_InSalesTbl(int ID, int Bill_ID)
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
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Insert_SaleBill_IDinSalesTbl, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlSales> DAL_GetAllSalesOf_OneReceipt(int Bill_ID)//Get all Purchases of a one Receipt
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_PU_Bill_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                List<MdlSales> mdlList = new List<MdlSales>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_SalesOf_OneReceipt, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSales RtrnVal = new MdlSales();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.MilkCredit = Model.Common.CheckDoubleNull(oRow["MilkCredit"]);
                    RtrnVal.Fat = Model.Common.CheckDoubleNull(oRow["Fat"]);
                    RtrnVal.LR = Model.Common.CheckDoubleNull(oRow["LR"]);
                    RtrnVal.TS = Model.Common.CheckDoubleNull(oRow["TS"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["CashDebit"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SalePrice"]);
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