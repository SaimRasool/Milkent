using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Model
{
    public class Common
    {
        #region DBNULL
        public static string CheckStringNull(object obj)
        {
            try
            {

                if (obj == null)
                {
                    return string.Empty;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return obj.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckStringNull", ex);
            }
        }

        public static int CheckIntegerNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if ((obj == DBNull.Value) || (obj is string && string.IsNullOrEmpty((string)obj)))
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToInt32(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }
        public static byte CheckByteNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if ((obj == DBNull.Value) || (obj is string && string.IsNullOrEmpty((string)obj)))
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToByte(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }
        public static long CheckLongNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToInt64(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckLongNull", ex);
            }
        }

        public static double CheckDoubleNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToDouble(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDoubleNull", ex);
            }
        }

        public static bool CheckBooleanNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return false;
                    }
                    else
                    {
                        return System.Convert.ToBoolean(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckBooleanNull", ex);
            }
        }

        public static short CheckShortNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return ((short)(obj));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckIntegerNull", ex);
            }
        }
        public static DateTime CheckDateTimeNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return System.DateTime.Now;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return System.DateTime.Now;
                    }
                    else
                    {
                        return (Convert.ToDateTime(obj));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDateTimeNull", ex);
            }
        }

        public static Decimal CheckDecimalNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return System.Convert.ToDecimal(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckDecimalNull", ex);
            }
        }
        public static float CheckfloatNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return 0;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return 0;
                    }
                    else
                    {
                        return float.Parse(obj.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckfloatNull", ex);
            }
        }

        public static string FndUnitDescirption(bool IsShowOnWizard, bool InkItem, bool FilmItem, bool PlateItem, string LargeFormatType, int USAUK)
        {
            string UnitDesc = "N/A";
            if (IsShowOnWizard == true)
                UnitDesc = "Sheet(s)";
            else if (FilmItem == true)
                UnitDesc = "Film(s)";
            else if (PlateItem == true)
                UnitDesc = "Plate(s)";
            else if (LargeFormatType == "LF" && USAUK == 1)
                UnitDesc = "SQFT";
            else if (LargeFormatType == "LF" && USAUK == 1)
                UnitDesc = "SQM";

            return (UnitDesc);
        }

        public static byte[] CheckImageNull(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    if (obj == DBNull.Value)
                    {
                        return null;
                    }
                    else
                    {
                        return (System.Byte[])(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CheckfloatNull", ex);
            }
        }

        public static void CreateFolder(string Path)
        {
            Path = Path;
            if (Directory.Exists(Path) == false)
            {
                Directory.CreateDirectory(Path);
            }

        }
        #endregion
    }

}
