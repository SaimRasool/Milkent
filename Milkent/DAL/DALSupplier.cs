using Milkent.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Milkent.DAL
{
    public class DALSupplier
    {
        private string _ConnString;
        #region Query

        const String SQL_Add_Supplier = "[SPW_AddSupplier]";
        const String SQL_Add_SupplierPayment = "[SPW_AddSupplierPayment]";
        const String SQL_Read_Supplier = "SPW_ReadSupplier";
        const String SQL_Read_SupplierPayment = "SPW_GetSupplierPayment";
        const String SQL_Read_PurchaseReceipt = "SPW_ReadPurchaseReceipt";
        const String SQL_Delete_Supplier = "SPW_DeleteSupplier";
        const String SQL_Delete_SupplierPurchaseReceipt = "SPW_DeleteSupplierPurchaseReceipt";
        const String SQL_Delete_SupplierPayment = "SPW_DeleteSupplierPayment";
        const String SQL_Update_Supplier = "[SPW_EditSupplier]";
        const String SQL_Update_SupplierPayment = "[SPW_EditSupplierPayment]";
        const String SQL_Read_AllSupplier = "SPW_ReadAllSupplier";
        const String SQL_Read_AllSupplierPurchaseReceipt = "SPW_ReadAllSuplierPurchaseReceipt";
        const String SQL_Read_OnePurchaseReceiptPayment = "SPW_ReadOnePurchaseReceiptPayment";

        #endregion

        #region Parameters

        const String PARM_SUP_ID = "@ID";
        const String PARM_SUP_Name = "@Name";
        const String PARM_SUP_Address = "@Address";
        const String PARM_SUP_Phone = "@PhoneNo";
        const String PARM_SUP_Type = "@Type";
        const String PARM_SUP_Image = "@Image";
        const String PARM_PU_PurchaseRate = "@PurchasePrice";
        const String PARM_PY_Date = "@Date";
        const String PARM_PY_Bill_ID = "@Bill_ID";
        const String PARM_PY_Credit = "@Credit";
        const String PARM_PY_Balance = "@Balance";


        #endregion

        #region Constructor

        public DALSupplier()
        {
            _ConnString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        #endregion

        public void Add(MdlSupplier mdl)
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
                parm[3] = new SqlParameter(PARM_SUP_Type, SqlDbType.NVarChar)
                {
                    Value = mdl.Type
                };

                parm[4] = new SqlParameter(PARM_SUP_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                parm[5] = new SqlParameter(PARM_PU_PurchaseRate, SqlDbType.Money)
                {
                    Value = mdl.PurchasePrice
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Add_Supplier, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public MdlSupplier DAL_Read_Supplier(int ID)
        {
            try
            {
                MdlSupplier RtrnVal = new MdlSupplier();
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Supplier, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Name = Model.Common.CheckStringNull(oRow["Name"]);
                    RtrnVal.Address = Model.Common.CheckStringNull(oRow["Address"]);
                    RtrnVal.PhoneNo = Model.Common.CheckStringNull(oRow["PhoneNo"]);
                    RtrnVal.Type = Model.Common.CheckStringNull(oRow["Type"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchaseRate"]);
                }

                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void DAL_Update_Supplier(MdlSupplier mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[7];
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
                parm[3] = new SqlParameter(PARM_SUP_Type, SqlDbType.NVarChar)
                {
                    Value = mdl.Type
                };

                parm[4] = new SqlParameter(PARM_SUP_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                parm[5] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[6] = new SqlParameter(PARM_PU_PurchaseRate, SqlDbType.Money)
                {
                    Value = mdl.PurchasePrice
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Update_Supplier, parm);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MdlSupplier> DalGetAllSuplier()
        {
            try
            {
                List<MdlSupplier> mdlList = new List<MdlSupplier>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllSupplier, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlSupplier RtrnVal = new MdlSupplier();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Name = Model.Common.CheckStringNull(oRow["Name"]);
                    RtrnVal.Address = Model.Common.CheckStringNull(oRow["Address"]);
                    RtrnVal.PhoneNo = Model.Common.CheckStringNull(oRow["PhoneNo"]);
                    RtrnVal.Type = Model.Common.CheckStringNull(oRow["Type"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                    RtrnVal.PurchasePrice = Model.Common.CheckDoubleNull(oRow["PurchaseRate"]);
                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int Dal_Delete_Supplier(int ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_Supplier, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPurchase> DAL_GetAll_SupplierPurchasesReceipt(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                List<MdlPurchase> mdlList = new List<MdlPurchase>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllSupplierPurchaseReceipt, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPurchase RtrnVal = new MdlPurchase();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
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

        public MdlPurchase DAL_Get_SupplierReceipt(int Bill_ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                MdlPurchase RtrnVal = new MdlPurchase();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_PurchaseReceipt, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                   
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserID = Model.Common.CheckIntegerNull(oRow["UserID"]);
                    RtrnVal.SupplierID = Model.Common.CheckIntegerNull(oRow["SupplierID"]);
                    RtrnVal.Total = Model.Common.CheckDoubleNull(oRow["Total"]);
                    RtrnVal.Credit = Model.Common.CheckDoubleNull(oRow["Credit"]);
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

        public int Dal_Delete_SupplierPurchaseReceipt(int ID /*Bill_ID*/)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_SupplierPurchaseReceipt, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<MdlPayment> DAL_Get_PaymentOfPurchaseReceipt(int Bill_ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = Bill_ID
                };
                List<MdlPayment> mdlList = new List<MdlPayment>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_OnePurchaseReceiptPayment, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlPayment RtrnVal = new MdlPayment();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Bill_ID = Model.Common.CheckIntegerNull(oRow["PurchaseBill_ID"]);
                    RtrnVal.Credit_Debit = Model.Common.CheckDoubleNull(oRow["Credit"]);
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

        public int DAL_AddSupplierPayment(MdlPayment mdl)
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
                parm[2] = new SqlParameter(PARM_PY_Credit, SqlDbType.Money)
                {
                    Value = mdl.Credit_Debit
                };
                parm[3] = new SqlParameter(PARM_PY_Balance, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                return (SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Add_SupplierPayment, parm));

            }
            catch (Exception)
            {

                throw;
            }

        }

        public void DAL_EditSupplierPayment(MdlPayment mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[3];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                parm[1] = new SqlParameter(PARM_PY_Credit, SqlDbType.Money)
                {
                    Value = mdl.Credit_Debit
                };
                parm[2] = new SqlParameter(PARM_PY_Balance, SqlDbType.Money)
                {
                    Value = mdl.Total
                };
                SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Update_SupplierPayment, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public MdlPayment DAL_Get_SupplierPayment(int ID)
        {
            try
            {
                MdlPayment RtrnVal = new MdlPayment();
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_SupplierPayment, parm);
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

        public int DAL_Delete_SupplierPayment(int ID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_SUP_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveDataa(this._ConnString, CommandType.StoredProcedure, SQL_Delete_SupplierPayment, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}