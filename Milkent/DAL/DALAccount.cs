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
    public class DALAccount
    {
        private string _ConnString;

        #region Query

        const String SQL_Add_Account = "SPW_AddAccount";
        const String SQL_Read_Account = "SPW_ReadAccount";
        const String SQL_Read_Milk = "SPW_ReadMilk";
        const String SQL_Delete_Account = "SPW_DeleteAccount";
        const String SQL_Update_Account = "SPW_EditAccount";
        const String SQL_Read_AllAccount = "[SPW_ReadAllAccount]";
        const String SQL_Email_Exist = "[SPW_IsEmailExist]";

        #endregion

        #region Parameters

        const String PARM_ID = "@ID";
        const String PARM_Name = "@Name";
        const String PARM_Password = "@Password";
        const String PARM_Role = "@Role";
        const String PARM_Email = "@Email";
        const String PARM_Image = "@Image";

        #endregion

        #region Constructor

        public DALAccount()
        {
            _ConnString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }
        #endregion

        public void Add(MdlLogin mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[5];
                parm[0] = new SqlParameter(PARM_Name, SqlDbType.NVarChar)
                {
                    Value = mdl.UserName
                };
                parm[1] = new SqlParameter(PARM_Email, SqlDbType.NVarChar)
                {
                    Value = mdl.Email
                };
                parm[2] = new SqlParameter(PARM_Password, SqlDbType.NVarChar)
                {
                    Value = mdl.Password
                };
                parm[3] = new SqlParameter(PARM_Role, SqlDbType.NVarChar)
                {
                    Value = mdl.Role
                };
                parm[4] = new SqlParameter(PARM_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Add_Account, parm);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<MdlLogin> DalGetAllAccount()
        {
            try
            {
                List<MdlLogin> mdlList = new List<MdlLogin>();
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_AllAccount, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    MdlLogin RtrnVal = new MdlLogin();
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserName = Model.Common.CheckStringNull(oRow["UserName"]);
                    RtrnVal.Password = Model.Common.CheckStringNull(oRow["Password"]);
                    RtrnVal.Role = Model.Common.CheckStringNull(oRow["Role"]);
                    RtrnVal.Email = Model.Common.CheckStringNull(oRow["Email"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                    mdlList.Add(RtrnVal);
                }
                return mdlList;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public MdlLogin DAL_Read_Account(int ID)
        {
            try
            {
                MdlLogin RtrnVal = new MdlLogin();
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Account, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.UserName = Model.Common.CheckStringNull(oRow["UserName"]);
                    RtrnVal.Password = Model.Common.CheckStringNull(oRow["Password"]);
                    RtrnVal.Role = Model.Common.CheckStringNull(oRow["Role"]);
                    RtrnVal.Email = Model.Common.CheckStringNull(oRow["Email"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                }

                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void DAL_Update_Account(MdlLogin mdl)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[5];
                parm[0] = new SqlParameter(PARM_Name, SqlDbType.NVarChar)
                {
                    Value = mdl.UserName
                };
                parm[1] = new SqlParameter(PARM_Email, SqlDbType.NVarChar)
                {
                    Value = mdl.Email
                };
                parm[2] = new SqlParameter(PARM_Password, SqlDbType.NVarChar)
                {
                    Value = mdl.Password
                };
                parm[3] = new SqlParameter(PARM_Image, SqlDbType.NVarChar)
                {
                    Value = mdl.Image
                };
                parm[4] = new SqlParameter(PARM_ID, SqlDbType.Int)
                {
                    Value = mdl.ID
                };
                SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Update_Account, parm);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DAL_Delete_Account(int ID)
        {
            try
            {

                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_ID, SqlDbType.Int)
                {
                    Value = ID
                };
                return SqlHelper.SaveData(this._ConnString, CommandType.StoredProcedure, SQL_Delete_Account, parm);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public MdlLogin DAL_IsEmailExist(string str)
        {
            MdlLogin RtrnVal = new MdlLogin();
            try
            {
                SqlParameter[] parm = new SqlParameter[1];
                parm[0] = new SqlParameter(PARM_Email, SqlDbType.NVarChar)
                {
                    Value = str
                };
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Email_Exist, parm);
                foreach (DataRow oRow in oTable.Rows)
                {
                    RtrnVal.ID = Model.Common.CheckIntegerNull(oRow["ID"]);
                    RtrnVal.Email = Model.Common.CheckStringNull(oRow["Email"]);
                    RtrnVal.Password = Model.Common.CheckStringNull(oRow["Password"]);
                    RtrnVal.UserName = Model.Common.CheckStringNull(oRow["UserName"]);
                    RtrnVal.Role = Model.Common.CheckStringNull(oRow["Role"]);
                    RtrnVal.Image = Model.Common.CheckStringNull(oRow["Image"]);
                }
                return RtrnVal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public double DAL_Read_Milk()
        {
            try
            {
                double Milk=0;
                DataTable oTable = SqlHelper.ExecuteTable(this._ConnString, CommandType.StoredProcedure, SQL_Read_Milk, null);
                foreach (DataRow oRow in oTable.Rows)
                {
                    Milk = Model.Common.CheckIntegerNull(oRow["Milk"]);
                }

                return Milk;
            }
            catch (Exception)
            {
                throw;
            }

        }
       

    }
}