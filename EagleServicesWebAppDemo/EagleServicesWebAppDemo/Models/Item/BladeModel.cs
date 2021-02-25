using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EagleServicesWebApp.Models.Item
{
    public class EngineRigDetails_Rec
    {
        public string EneSerialNo { get; set; }
        public string Model { get; set; }
        public string BuildNumber { get; set; }
        public string HubDisPartNo { get; set; }
        public string HubDisSerialNo { get; set; }
        public int SlotsNumber { get; set; }
        public string Module { get; set; }
        public string Stage { get; set; }
        public string OperatorName { get; set; }
        public string OperatorID { get; set; }
        public Int16 FixedStatus { get; set; }
        public string RigComments { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public bool isDeleteBlade { get; set; }
    }

    public class EngineBladeDetails_Rec
    {
        public string EneSerialNo { get; set; }
        public int SlotOrder { get; set; }
        public string BladeNo { get; set; }

        [Key]
        [Required]
        [Display(Name = "Part No")]
        public string BladePartNo { get; set; }

        [Key]
        [Required]
        [Display(Name = "Serial No")]
        public string BladeSerialNo { get; set; }

        [Key]
        [Required]
        [Display(Name = "Radial")]
        public string RadialWt { get; set; }

        [Key]
        [Required]
        [Display(Name = "Tangential")]
        public string TangentialWt { get; set; }

        [Key]
        [Required]
        [Display(Name = "Axial")]
        public string AxialWt { get; set; }
        public string BladeComment { get; set; }
        public Int16 FixedStatus { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string FixedStatusStr { get; set; }

        public string oldBladeSerialNo { get; set; }
    }

    public class BladeModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public BladeModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }


        public System.Data.Entity.Infrastructure.DbRawSqlQuery<EngineBladeDetails_Rec> GetBladeDataBySerialNo(string EngSN)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        " SELECT " +
                        "    SlotOrder,BladeNo,BladePartNo,BladeSerialNo,RadialWt,TangentialWt,AxialWt, " +
                        "    ISNULL(BladeComment,'') BladeComment,FixedStatus,CASE When FixedStatus = 1 Then 'TRUE' ELSE 'FALE' END As FixedStatusStr " +
                        "  FROM  [dbo].[tblEngineBladeDetails] with (NOLOCK) where EneSerialNo='" + EngSN + "' order by SlotOrder asc" +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<EngineBladeDetails_Rec>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<EngineRigDetails_Rec> GetRigDataBySerialNo(string EngSN)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        " SELECT " +
                        "    EneSerialNo, Model, " +
                        "    ISNULL(BuildNumber,'') BuildNumber,ISNULL(HubDisPartNo,'') HubDisPartNo, " +
                        "   ISNULL(HubDisSerialNo,' ') HubDisSerialNo,SlotsNumber , ISNULL(Module,' ') Module," +
                        "  ISNULL(Stage,' ') Stage,ISNULL(OperatorName,' ') OperatorName,ISNULL(OperatorID,' ') OperatorID,FixedStatus,ISNULL(RigComments,' ') RigComments  " +
                        "  FROM  [dbo].[tblEngineRigDetails] with (NOLOCK) where EneSerialNo='" + EngSN + "'" +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<EngineRigDetails_Rec>(sSQL);

            return vQuery;
        }


        public System.Data.Entity.Infrastructure.DbRawSqlQuery<EngineBladeDetails_Rec> GetSlotOrderResult(string EngSN)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        " SELECT SlotOrder" +
                        "  FROM  [dbo].[tblEngineBladeDetails] with (NOLOCK) where EneSerialNo='" + EngSN + "' order by SlotOrder asc" +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<EngineBladeDetails_Rec>(sSQL);

            return vQuery;
        }

        public bool UpdateEngineRigDetails(EngineRigDetails_Rec poRecord)//just update engine's rig data
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "UPDATE tblEngineRigDetails " +
                    "   SET " +
                    "      BuildNumber = @BuildNumber, " +
                    "      HubDisPartNo = @HubDisPartNo, " +
                    "      HubDisSerialNo = @HubDisSerialNo, " +
                    "      SlotsNumber = @SlotsNumber, " +
                    "      Module = @Module, " +
                    "      Stage = @Stage, " +
                    "      OperatorName = @OperatorName, " +
                    "      OperatorID = @OperatorID, " +
                    "      FixedStatus = @FixedStatus, " +
                    "      RigComments = @RigComments, " +
                    "      UpdatedDate = @UpdatedDate, " +
                    "      UpdatedBy = @UpdatedBy " +
                    "   WHERE (EneSerialNo = @EneSerialNo) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();

                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@BuildNumber", (object)poRecord.BuildNumber ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@HubDisPartNo", (object)poRecord.HubDisPartNo ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@HubDisSerialNo", (object)poRecord.HubDisSerialNo ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@SlotsNumber", poRecord.SlotsNumber));
                oParameters.Add(new SqlParameter("@Module", (object)poRecord.Module ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@Stage", (object)poRecord.Stage ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@OperatorName", (object)poRecord.OperatorName ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@OperatorID", (object)poRecord.OperatorID ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@FixedStatus", poRecord.FixedStatus));
                oParameters.Add(new SqlParameter("@RigComments", (object)poRecord.RigComments ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@UpdatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                if (nReturn == 1)
                {
                    oTransaction.Commit(); //(WEK-20200512)as blade data are auto saved, now just need to update rig, no need to update or delete blade table,
                   // if (poRecord.FixedStatus == 0)
                   // {
                   //     string sBladeSQL = "" +
                   //"UPDATE tblEngineBladeDetails " +
                   //"   SET " +
                   //"      FixedStatus = @FixedStatus, " +
                   //"      UpdatedDate = @UpdatedDate, " +
                   //"      UpdatedBy = @UpdatedBy " +
                   //"   WHERE (EneSerialNo = @EneSerialNo) " +
                   //";"
                   //;
                   //     List<SqlParameter> oParameters1 = new List<SqlParameter>();

                   //     oParameters1.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                   //     oParameters1.Add(new SqlParameter("@FixedStatus", poRecord.FixedStatus));
                   //     oParameters1.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                   //     oParameters1.Add(new SqlParameter("@UpdatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));

                   //     SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                   //     if (oRemoteDB.Database.ExecuteSqlCommand(sBladeSQL, vSqlParameter1) >= 1)
                   //     {
                   //         if (poRecord.isDeleteBlade)
                   //         {
                   //             string sBladedeleteSQL = "" +
                   //             "DELETE FROM tblEngineBladeDetails WHERE SlotOrder NOT IN " +
                   //             " (SELECT TOP " + poRecord.SlotsNumber + " SlotOrder FROM tblEngineBladeDetails where EneSerialNo = @EneSerialNo order by SlotOrder desc)" +
                   //             ";"
                   //              ;


                   //             List<SqlParameter> oParameters2 = new List<SqlParameter>();

                   //             oParameters2.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));

                   //             SqlParameter[] vSqlParameter2 = oParameters2.ToArray();

                   //             if (oRemoteDB.Database.ExecuteSqlCommand(sBladedeleteSQL, vSqlParameter2) >= 1)
                   //             {
                   //                 oTransaction.Commit();
                   //             }
                   //             else
                   //             {
                   //                 bReturn = false;
                   //                 oTransaction.Rollback();
                   //             }
                   //         }
                   //         else
                   //         {
                   //             oTransaction.Commit();
                   //         }
                   //     }
                   //     else
                   //     {
                   //         bReturn = false;
                   //         oTransaction.Rollback();
                   //     }
                   // }
                   // else
                   // {
                   //     if (poRecord.isDeleteBlade)
                   //     {
                   //         string sBladedeleteSQL = "" +
                   //         "DELETE FROM tblEngineBladeDetails WHERE SlotOrder NOT IN " +
                   //         " (SELECT TOP " + poRecord.SlotsNumber + " SlotOrder FROM tblEngineBladeDetails where EneSerialNo = @EneSerialNo order by SlotOrder desc)" +
                   //         ";"
                   //          ;


                   //         List<SqlParameter> oParameters2 = new List<SqlParameter>();

                   //         oParameters2.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));

                   //         SqlParameter[] vSqlParameter2 = oParameters2.ToArray();

                   //         if (oRemoteDB.Database.ExecuteSqlCommand(sBladedeleteSQL, vSqlParameter2) >= 1)
                   //         {
                   //             oTransaction.Commit();
                   //         }
                   //         else
                   //         {
                   //             bReturn = false;
                   //             oTransaction.Rollback();
                   //         }
                   //     }
                   //     else
                   //     {
                   //         oTransaction.Commit();
                   //     }
                   // }
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
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

        public bool EngineBladeDelete(EngineBladeDetails_Rec poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                //----------
                string sSQL = "" +
                     " DELETE tblEngineBladeDetails WHERE (EneSerialNo = @EneSerialNo and BladeSerialNo = @BladeSerialNo);"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@BladeSerialNo", poRecord.BladeSerialNo));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                if (nReturn >= 1)
                {
                    oTransaction.Commit();

                }
                else
                {
                    oTransaction.Rollback();
                    //ErrorMessage = nReturn.ToString() + "<br>" + "Failed to delete record!"  ;
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

        public bool EngineBladeInsert(EngineBladeDetails_Rec poRecord)
        {
            bool bReturn = true;

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                string sSQL = "" +
                    "INSERT INTO tblEngineBladeDetails " +
                    "      ( " +
                    "      EneSerialNo,SlotOrder,BladeNo,BladePartNo,BladeSerialNo,RadialWt,TangentialWt,AxialWt,BladeComment,FixedStatus,CreatedDate,CreatedBy " +
                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @EneSerialNo,@SlotOrder,@BladeNo,@BladePartNo,@BladeSerialNo,@RadialWt,@TangentialWt,@AxialWt,@BladeComment,@FixedStatus,@CreatedDate,@CreatedBy " +
                    "      )" +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@SlotOrder", poRecord.SlotOrder));
                oParameters.Add(new SqlParameter("@BladeNo", poRecord.BladeNo));
                oParameters.Add(new SqlParameter("@BladePartNo", poRecord.BladePartNo));
                oParameters.Add(new SqlParameter("@BladeSerialNo", poRecord.BladeSerialNo));
                oParameters.Add(new SqlParameter("@RadialWt", poRecord.RadialWt));
                oParameters.Add(new SqlParameter("@TangentialWt", poRecord.TangentialWt));
                oParameters.Add(new SqlParameter("@AxialWt", poRecord.AxialWt));
                oParameters.Add(new SqlParameter("@BladeComment", (object)poRecord.BladeComment ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@FixedStatus", poRecord.FixedStatus));
                oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                if (oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter) > 0)
                {
                    oTransaction.Commit();
                }
                else
                {
                    bReturn = false;
                    oTransaction.Rollback();
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                oTransaction.Rollback();
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool EngineBladeUpdate(EngineBladeDetails_Rec poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "UPDATE tblEngineBladeDetails " +
                    "   SET " +
                    "      EneSerialNo = @EneSerialNo, " +
                    "      BladePartNo = @BladePartNo, " +
                    "      BladeSerialNo = @BladeSerialNo, " +
                    "      RadialWt = @RadialWt, " +
                    "      TangentialWt = @TangentialWt, " +
                    "      AxialWt = @AxialWt, " +
                    "      BladeComment = @BladeComment, " +
                    "      FixedStatus = @FixedStatus, " +
                    "      UpdatedDate = @UpdatedDate, " +
                    "      UpdatedBy = @UpdatedBy " +
                    "   WHERE (EneSerialNo = @EneSerialNo and BladeSerialNo = @oldBladeSerialNo) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();

                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@BladePartNo", poRecord.BladePartNo));
                oParameters.Add(new SqlParameter("@BladeSerialNo", poRecord.BladeSerialNo));
                oParameters.Add(new SqlParameter("@RadialWt", poRecord.RadialWt));
                oParameters.Add(new SqlParameter("@TangentialWt", poRecord.TangentialWt));
                oParameters.Add(new SqlParameter("@AxialWt", poRecord.AxialWt));
                oParameters.Add(new SqlParameter("@FixedStatus", poRecord.FixedStatus));
                oParameters.Add(new SqlParameter("@BladeComment", (object)poRecord.BladeComment ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@oldBladeSerialNo", poRecord.oldBladeSerialNo));
                oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@UpdatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                if (nReturn == 1)
                {
                    oTransaction.Commit();
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
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
    }
}