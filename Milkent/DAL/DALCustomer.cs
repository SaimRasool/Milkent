using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Milkent.DAL
{
    public class DALCustomer
    {
        private string _ConnString;

        #region Query

        const String SQL_Add_Customer = "SPW_AddCustomer";
        const String SQL_Read_Customer = "SPW_ReadCustomer";
        const String SQL_Delete_Customer = "SPW_DeleteCustomer";
        const String SQL_Update_Customer = "SPW_EditCustomer";
        const String SQL_Read_AllCustomer = "[SPW_ReadAllCustomer]";
        const String SQL_Update_CustomerPayment = "SPW_EditCustomerrPayment";
        const String SQL_Read_AllCustomerSalesReceipt = "SPW_ReadAllCustomerReceipt";
        const String SQL_Read_SaleReceipt = "SPW_ReadSaleReceipt";
        const String SQL_Read_OneSalesReceiptPayment = "SPW_ReadOneSalesReceiptPayment";
        const String SQL_Read_DeleteCustomerSalesReceipt = "SPW_DeleteCustomerSalesReceipt";
        const String SQL_Read_DeleteCustomerPayment = "SPW_DeleteCustomerPayment";
        const String SQL_Read_AddCustomerPayment = "SPW_AddCustomerPayment";
        const String SQL_Read_CustomerPayment = "SPW_GetCustomerPayment";
        #endregion

        #region Parameters

        const String PARM_SUP_ID = "@ID";
        const String PARM_SUP_Name = "@Name";
        const String PARM_SUP_Address = "@Address";
        const String PARM_SUP_Phone = "@PhoneNo";
        const String PARM_SUP_Image = "@Image";
        const String PARM_PU_SalesPrice = "@SalePrice";
        const String PARM_PY_Date = "@Date";
        const String PARM_PY_Bill_ID = "@Bill_ID";
        const String PARM_PY_Debit = "@Debit";
        const String PARM_PY_Balance = "@Balance";
        #endregion

        #region Constructor

        public DALCustomer()
        {
            _ConnString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        #endregion

        public void Add(MdlCustomer mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[5];
                parm[0] = new SqlParameter(PARM_SUP_Name, SqlDbType.NVarChar)
                {
                    Value = mdl.Name
                };
                parm[1] = new SqlParameter(PARM_SUP_Address, SqlDbType.NVarChar)
                {
                    Value = mdl.Address
                };
                parm[2] = new SqlParameter(PARM_SUP_Phone, SqlDbType.NVarChar)
                {
                    Value = mdl.PhoneNo
                };
                parm[3] = new SqlParameter(PARM_SUP_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                parm[4] = new SqlParameter(PARM_PU_SalesPrice, SqlDbType.Money)
                {
                    Value = mdl.SalePrice
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Add_Customer, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<MdlCustomer> DalGetAllCustomer()
        {
            try
            {
                List<MdlCustomer> mdlList = new List<MdlCustomer>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllCustomer, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlCustomer RtrnVal = new MdlCustomer();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Name = Model.Common.CheckStringNull(oRow["Name"]);
                    RtrnVal.Address = Model.Common.CheckStringNull(oRow["Address"]);
                    RtrnVal.PhoneNo = Model.Common.CheckStringNull(oRow["PhoneNo"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SaleRate"]);
                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MdlCustomer DAL_Read_Customer(int ID)
        {
            try
            {
                MdlCustomer RtrnVal = new MdlCustomer();
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Customer, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Name = Model.Common.CheckStringNull(oRow["Name"]);
                    RtrnVal.Address = Model.Common.CheckStringNull(oRow["Address"]);
                    RtrnVal.PhoneNo = Model.Common.CheckStringNull(oRow["PhoneNo"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                    RtrnVal.SalePrice = Model.Common.CheckDoubleNull(oRow["SaleRate"]);
                }

                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void DAL_Update_Customer(MdlCustomer mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[6];
                parm[0] = new SqlParameter(PARM_SUP_Name, SqlDbType.NVarChar)
                {
                    Value = mdl.Name
                };
                parm[1] = new SqlParameter(PARM_SUP_Address, SqlDbType.NVarChar)
                {
                    Value = mdl.Address
                };
                parm[2] = new SqlParameter(PARM_SUP_Phone, SqlDbType.NVarChar)
                {
                    Value = mdl.PhoneNo
                };

                parm[3] = new SqlParameter(PARM_SUP_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                parm[4] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[5] = new SqlParameter(PARM_PU_SalesPrice, SqlDbType.Money)
                {
                    Value = mdl.SalePrice
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Update_Customer, parm);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DAL_Delete_Customer(int ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_Customer, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }



        public List<MdlSales> DAL_GetAll_CustomerSalesReceipt(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlSales> mdlList = new List<MdlSales>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllCustomerSalesReceipt, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSales RtrnVal = new MdlSales();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["Debit"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);
                    RtrnVal.ToDate = Model.Common.CheckDateTimeNull(oRow["ToDate"]);
                    RtrnVal.FromDate = Model.Common.CheckDateTimeNull(oRow["FromDate"]);

                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MdlSales DAL_Get_SaleReceipt(int Bill_ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                MdlSales RtrnVal = new MdlSales();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_SaleReceipt, parm);
                foreach (DataRow oRow in oTable.Rows)
                {

                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.CustomerID = Model.Common.CheckIntegerNull(oRow["SupplierID"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.CashDebit = Model.Common.CheckDoubleNull(oRow["Debit"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);
                    RtrnVal.ToDate = Model.Common.CheckDateTimeNull(oRow["ToDate"]);
                    RtrnVal.FromDate = Model.Common.CheckDateTimeNull(oRow["FromDate"]);

                }
                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int Dal_Delete_CustomerSalesReceipt(int Bill_ID /*Bill_ID*/)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Read_DeleteCustomerSalesReceipt, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPayment> DAL_Get_PaymentOfSaleReceipt(int Bill_ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                List<MdlPayment> mdlList = new List<MdlPayment>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_OneSalesReceiptPayment, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPayment RtrnVal = new MdlPayment();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Bill_ID = Model.Common.CheckIntegerNull(oRow["PurchaseBill_ID"]);
                    RtrnVal.Credit_Debit = Model.Common.CheckDoubleNull(oRow["Debit"]);
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

        public int DAL_AddCustomerPayment(MdlPayment mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[4];
                parm[0] = new SqlParameter(PARM_PY_Date, SqlDbType.Date)
                {
                    Value = mdl.Date
                };
                parm[1] = new SqlParameter(PARM_PY_Bill_ID, SqlDbType.Int)
                {
                    Value = mdl.Bill_ID
                };
                parm[2] = new SqlParameter(PARM_PY_Debit, SqlDbType.Money)
                {
                    Value = mdl.Credit_Debit
                };
                parm[3] = new SqlParameter(PARM_PY_Balance, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                return (SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Read_AddCustomerPayment, parm));

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DAL_EditCustomerPayment(MdlPayment mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[3];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[1] = new SqlParameter(PARM_PY_Debit, SqlDbType.Money)
                {
                    Value = mdl.Credit_Debit
                };
                parm[2] = new SqlParameter(PARM_PY_Balance, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Update_CustomerPayment, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public MdlPayment DAL_Get_CustomerPayment(int ID)
        {
            try
            {
                MdlPayment RtrnVal = new MdlPayment();
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_CustomerPayment, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Bill_ID = Model.Common.CheckIntegerNull(oRow["PurchaseBill_ID"]);
                    RtrnVal.Credit_Debit = Model.Common.CheckDoubleNull(oRow["Credit"]);
                    RtrnVal.Date = Model.Common.CheckDateTimeNull(oRow["Date"]);
                }
                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int DAL_Delete_CustomerPayment(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Read_DeleteCustomerPayment, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}