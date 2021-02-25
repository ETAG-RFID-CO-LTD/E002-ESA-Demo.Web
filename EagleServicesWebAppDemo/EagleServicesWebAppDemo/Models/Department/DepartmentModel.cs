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
   public class Department_REC
   {
      [Key]
      [Required]
      [Display(Name = "Department")]
      public string Dept { get; set; }
      public string Description { get; set; }
      public string CreatedBy { get; set; }
      public DateTime CreatedDate { get; set; }
   }

    public class DepartmentModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public DepartmentModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }       

        #region CREATE
        public bool Insert(Department_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    " INSERT INTO tblDepartment " +
                    "      ( " +
                    "     Dept,Description " +                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @Dept,@Description " +                    "    )" +
                    ";" 
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                oParameters.Add(new SqlParameter("@Description", poRecord.Description));
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
        #endregion

        #region READ

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Department_REC> GetDepListByUserID(string UserID)
        {
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();
            if (UserID == "")
            {
                sSQL = "" +
                    "SELECT " +
                     "   Dept,Description " +
                    "    FROM tblDepartment with (nolock) " + " " +
                    "   ORDER BY Dept " +
                    ";";
            }
            else
            {
                sSQL = "" +
                        "SELECT " +
                         "   Dept,Description " +
                         "    FROM tblDepartment with (nolock) " + " WHERE  Dept not in (SELECT distinct Dept FROM tblDepartmentMapping with (nolock) WHERE UserID='" + UserID + "')" +
                         "   ORDER BY Dept " +
                        ";";

            }
            var vQuery = oRemoteDB.Database.SqlQuery<Department_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Department_REC> GetSelectedDepListByUserID(string UserID)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   Dept,Description " +
                         "    FROM tblDepartment with (nolock) " + " WHERE  Dept in (SELECT distinct Dept FROM tblDepartmentMapping with (nolock) WHERE UserID='" + UserID + "')" +
                         "   ORDER BY Dept " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Department_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Department_REC> GetList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   Dept,Description " +
                         "    FROM tblDepartment with (nolock) " + "" +
                         "   ORDER BY Dept " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Department_REC>(sSQL);

            return vQuery;
        }

        #endregion

        #region UPDATE
        public bool Update(Department_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    " UPDATE tblDepartment  " +
                    "  SET " +
                    "     Description=@Description " +
                    "      WHERE (Dept=@Dept) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));
                oParameters.Add(new SqlParameter("@Description", poRecord.Description));
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
        #endregion

        #region DELETE
        public bool Delete(Department_REC poRecord)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {

                DepartmentMappingModel deptMapping = new DepartmentMappingModel();
                bool mapped = deptMapping.CheckDeptAlreadyMapped(poRecord.Dept);

                if (mapped == false)
                    mapped = deptMapping.CheckDeptAlreadyInUse(poRecord.Dept);

                if (mapped)
                {
                    ErrorMessage = "Department deletion failed  <br> Department currently in use!";
                    return bReturn = false;
                } 
                else
                {
                    #region Perform delete function
                    string sSQL = "" +
                            "DELETE tblDepartment " +
                            "   WHERE (Dept=@Dept) " +
                             ";"
                                         ;

                    List<SqlParameter> oParameters = new List<SqlParameter>();
                    oParameters.Add(new SqlParameter("@Dept", poRecord.Dept));

                    SqlParameter[] vSqlParameter = oParameters.ToArray();

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


                    #endregion

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
    }
}