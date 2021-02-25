using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EagleServicesWebApp.Components;

using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Models
{

    public class DepartmentMapping_REC
    {

        [Key]
        [Required]
        [Display(Name = "User ID")]
        public string UserID { get; set; }


        [Key]
        [Required]
        [Display(Name = "Department")]
        public string Dept { get; set; }

        public string DepartmentListings { get; set; }
    }

    public class DepartmentMappingModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public DepartmentMappingModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        #region CREATE
        public bool Insert(DepartmentMapping_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    " INSERT INTO tblDepartmentMapping " +
                    "      ( " +
                    "     UserID,Dept " + "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @UserID,@Dept " + "    )" +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
                oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn == 1)
                {
                    bReturn = true;
                }
                else
                {
                    ErrorMessage = "Failed to insert record!";
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool InsertWithCollection(List<DepartmentMapping_REC> poRecordCollections)
        {
            string sSQL = string.Empty;
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            //------------------------------
            try
            {

                #region New Record Insert
                sSQL = "" +
                     " INSERT INTO tblDepartmentMapping " +
                      "      ( " +
                      "     UserID,Dept " + "      ) " +
                     "      VALUES " +
                     "      (" +
                     "      @UserID,@Dept " + "    )" +
                     ";"
                     ;

                foreach (var _poRecord in poRecordCollections)
                {
                    #region Insert 
                    DepartmentMapping_REC poRecord = new DepartmentMapping_REC();
                    poRecord.UserID = _poRecord.UserID;
                    poRecord.Dept = _poRecord.Dept;
                    List<SqlParameter> oParameters = new List<SqlParameter>();
                    oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
                    oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                    SqlParameter[] vSqlParameter = oParameters.ToArray();

                    
                    int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                    if (nReturn == 1)
                    {
                        bReturn = true;
                    }
                    else
                    {
                        ErrorMessage = "Failed to insert record!";
                        throw new Exception("Failed to insert record!");
                        
                    }
                    #endregion
                }
                #endregion

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }
        #endregion

        #region READ
        //select distinct   from tblDepartment
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<DepartmentMapping_REC> GetDepartmentList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();


            string sSQL = "" +
            "SELECT " +
             "   Distinct dbo.DepartmentList() as DepartmentListings " +
             "    FROM tblDepartment " + ";" 
              ;

            var vQuery = oRemoteDB.Database.SqlQuery<DepartmentMapping_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<DepartmentMapping_REC> GetList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();


            string sSQL = "" +
            "SELECT " +
             "   Distinct UserID,dbo.DepartmentListByUserID (UserID) as DepartmentListings " +
             "    FROM tblUser WHERE isAdmin=0 " + "" +
             "   ORDER BY UserID asc " +
            ";"
              ;

            var vQuery = oRemoteDB.Database.SqlQuery<DepartmentMapping_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<DepartmentMapping_REC> GetDepartmentListByUserID(string UserID)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   Dept,UserID " +
                         "    FROM tblDepartmentMapping with (nolock) " + " WHERE UserID='" + UserID +"'" +
                         "   GROUP BY Dept,UserID " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<DepartmentMapping_REC>(sSQL);

            return vQuery;
        }
        #endregion

        #region UPDATE
        public bool Update(DepartmentMapping_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    " UPDATE tblDepartmentMapping  " +
                    "  SET " +
                    "     Dept=@Dept " +
                    "      WHERE (UserID=@UserID) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn == 1)
                {
                    bReturn = true;
                }
                else
                {
                    ErrorMessage = "Failed to update record!";
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool UpdateWitheCollection(List<DepartmentMapping_REC> poRecordCollections, string DepartmentListings)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                int nReturn = 0;
                string sSQL = "";
                if (DepartmentListings == "0" || DepartmentListings == null)
                    nReturn = 1;
                else
                {
                    #region Remove Old
                   sSQL = "" +
                        "DELETE tblDepartmentMapping " +
                         "   WHERE (UserID=@UserID) " +
                             ";"
                          ;

                    List<SqlParameter> oDelParameters = new List<SqlParameter>();
                    oDelParameters.Add(new SqlParameter("@UserID", poRecordCollections[0].UserID));

                    SqlParameter[] vSqlDelParameter = oDelParameters.ToArray();


                    nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlDelParameter);

                    #endregion
                }

                if (nReturn > 0)
                {
                    nReturn = 0;
                    sSQL = string.Empty;

                    #region Insert Updated Record

                    #region Insert Query
                    sSQL = "" +
                          " INSERT INTO tblDepartmentMapping " +
                          "      ( " +
                          "     UserID,Dept " + "      ) " +
                          "      VALUES " +
                          "      (" +
                          "      @UserID,@Dept " + "    )" +
                          ";"
                          ;
                    #endregion

                    #region Collections looping 
                    foreach (var _poRecord in poRecordCollections)
                    {
                        #region Insert 
                        DepartmentMapping_REC poRecord = new DepartmentMapping_REC();
                        poRecord.UserID = _poRecord.UserID;
                        poRecord.Dept = _poRecord.Dept;
                        List<SqlParameter> oParameters = new List<SqlParameter>();
                        oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
                        oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                        SqlParameter[] vSqlParameter = oParameters.ToArray();


                        nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                        if (nReturn == 1)
                        {
                            bReturn = true;
                        }
                        else
                        {
                            ErrorMessage = "Failed to insert record!";
                            throw new Exception("Failed to insert record!");

                        }
                        #endregion
                    }
                    #endregion

                    #endregion

                    #region Transaction Rollback or Commit
                    if(bReturn == true)
                    {
                        oTransaction.Commit();
                        bReturn = true;
                    }
                    else
                    {
                        oTransaction.Rollback();
                        bReturn = false;
                    }
                    #endregion

                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to delete record!";

                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                oTransaction.Rollback();
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }
        #endregion

        #region DELETE
        public bool DeleteByUserID(DepartmentMapping_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "DELETE tblDepartmentMapping " +
                    "   WHERE (UserID=@UserID); " 
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn > 0)
                {
                    oTransaction.Commit();
                    bReturn = true;
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to delete record!";

                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool DeleteByUserIDAndDept(DepartmentMapping_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "DELETE tblDepartmentMapping " +
                    "   WHERE (UserID=@UserID) AND (Dept=@Dept) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
                oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn > 0)
                {
                    oTransaction.Commit();
                    bReturn = true;
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to delete record!";

                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool CheckDeptAlreadyMapped(string Dept)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                   "select count(*) from tblDepartmentMapping " +
                   "   WHERE Dept=@Dept " +
                   ";"
                   ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@Dept", Dept));

            SqlParameter[] vSqlParameter = oParameters.ToArray();
            //int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            var noOfDeptUsed = oRemoteDB.Database.SqlQuery<int>(sSQL, vSqlParameter).SingleOrDefault();

            if (noOfDeptUsed > 0)
            {
                return true;
            }

            return false;
        }

        public bool CheckDeptAlreadyInUse(string Dept)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                   "select count(*) from tblAsset " +
                   "   WHERE Dept=@Dept " +
                   ";"
                   ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@Dept", Dept));

            SqlParameter[] vSqlParameter = oParameters.ToArray();
            //int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            var noOfDeptUsed = oRemoteDB.Database.SqlQuery<int>(sSQL, vSqlParameter).SingleOrDefault();

            if (noOfDeptUsed > 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}