using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EagleServicesWebApp.Models
{

    public class ImgObjResult
    {
        #region Properties

        /// <summary>
        /// Gets or sets Image ID.
        /// </summary>
        public string ItemNo { get; set; }

        /// <summary>
        /// Gets or sets Image name.
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// Gets or sets Image extension.
        /// </summary>
        public string file_ext { get; set; }

        /// <summary>
        /// Get or sets Image 
        /// </summary>
        public string file_base6 { get; set; }
        #endregion
    }

    public class ItemPartMapping
    {
        public Int64 PartDetailsID { get; set; }
        public string PartItemNo { get; set; }
        public string VPN { get; set; }
        public string PWPN { get; set; }
        //public  string  SN { get; set; }
        //public string QCStatus { get; set; }

        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public ItemPartMapping()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        public List<ItemPartMapping> GetItemPartMapping(string itemNo)
        {
            string sql = "select * from tblPartMasterDetails where PartItemNo = @ItemNo";

            DatabaseContext db = new DatabaseContext();
            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", itemNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var itemPartMapping = db.Database.SqlQuery<ItemPartMapping>(sql, vSqlParameter).ToList();
            return itemPartMapping;

        }
        public bool Insert(ItemPartMapping poRecord)
        {
            bool bReturn = true;
            //------------------------------
            string sSQL = "INS_PartMasterDetailsForImport @PartItemNo,@VPN,@PWPN ";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@PartItemNo", poRecord.PartItemNo));
            oParameters.Add(new SqlParameter("@VPN", poRecord.VPN == "" || poRecord.VPN == null ? "N/A" : poRecord.VPN));
            oParameters.Add(new SqlParameter("@PWPN", poRecord.PWPN == "" || poRecord.PWPN == null ? "N/A" : poRecord.PWPN));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {

            }
            else
            {
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }
        public bool Update(ItemPartMapping poRecord)
        {
            bool bReturn = true;
            //------------------------------
            string sSQL = "UPD_PartMasterDetails @detialID,@PartItemNo,@VPN,@PWPN ";
            //"Update [dbo].[tblPartMasterDetails] " +
            //"      set " +
            //"       VPN =@VPN," +
            //"       PWPN=@PWPN " +
            //"where PartDetailsID=@detialID and PartItemNo=@PartItemNo" +
            //";"
            ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@detialID", poRecord.PartDetailsID));
            oParameters.Add(new SqlParameter("@PartItemNo", poRecord.PartItemNo));
            oParameters.Add(new SqlParameter("@VPN", poRecord.VPN == "" || poRecord.VPN == null ? "N/A" : poRecord.VPN));
            oParameters.Add(new SqlParameter("@PWPN", poRecord.PWPN == "" || poRecord.PWPN == null ? "N/A" : poRecord.PWPN));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {

            }
            else
            {
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }
        public bool Delete(ItemPartMapping poRecord)
        {
            bool bReturn = true;
            //------------------------------
            string sSQL = "" +
                "delete from [dbo].[tblPartMasterDetails] " +
                "      where " +
                "       PartDetailsID = @PartDetailsID and PartItemNo=@itemNo " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@PartDetailsID", poRecord.PartDetailsID));
            oParameters.Add(new SqlParameter("@itemNo", poRecord.PartItemNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {

            }
            else
            {
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

    }



    public class ItemModel
    {
        //public string EngSN { get; set; }
        [Required(ErrorMessage = "please PartItemNo")]
        public string PartItemNo { get; set; }
        [Required(ErrorMessage = "please IPCRef")]
        public string IPCRef { get; set; }
        [Required(ErrorMessage = "please Nomenclature")]
        public string Nomenclature { get; set; }
        [Required(ErrorMessage = "please PartType")]
        public string PartType { get; set; }
        public bool PwelStd { get; set; }
        public string PwelStdStr { get; set; }
        public string SN { get; set; }
        public string PartStatus { get; set; }
        public string Remarks { get; set; }
        public string Photo { get; set; }
        public string CreatedDateStr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedDateStr { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<ItemPartMapping> PartMappings { get; set; }

        public List<ItemModel> GetItemList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            //string sSQL = "select *,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblItem where PartType = 'Engine Part';";
            string sSQL = "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks,CreatedDate,ISNULL (CONVERT(varchar(50), CreatedDate, 120), '') as CreatedDateStr,CreatedBy,UpdatedDate,ISNULL (CONVERT(varchar(50), UpdatedDate, 120), '') as UpdatedDateStr,UpdatedBy, case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblPartMaster ; ";//where PartType = 'Engine Part'
            var itemList = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL).AsEnumerable().AsQueryable().ToList();
            return itemList;
        }

        public ItemModel GetItem(string ItemNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "SELECT [PartItemNo],[IPCRef],[Nomenclature],[PartType]" +
                          " ,[PwelStd],[PartStatus],[SN],[Remarks]" +
                          " ,[CreatedDate],ISNULL (CONVERT(varchar(50), CreatedDate, 120), '') as CreatedDateStr,[CreatedBy],UpdatedDate,ISNULL (CONVERT(varchar(50), UpdatedDate, 120), '') as UpdatedDateStr, ISNULL([UpdatedBy],'') as UpdatedBy " +
                          " FROM [tblPartMaster] where PartItemNo = @ItemNo;";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", ItemNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            //int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            var item = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL, vSqlParameter).AsQueryable().SingleOrDefault();
            return item;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemModel> GetItemDetailByEdit(string ItemPartNo, string EngineSerialNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT * FROM [dbo].[ItemDetailByEngineEdit](@ItemNo,@engineSN) " +
                        ";"
        ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", ItemPartNo));
            oParameters.Add(new SqlParameter("@engineSN", EngineSerialNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL, vSqlParameter);//.AsQueryable().SingleOrDefault();
            //var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL);
            return vQuery;
        }


        public bool CheckItem(string ItemNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "SELECT [PartItemNo]" +
                          " FROM [tblPartMaster] where PartItemNo = @ItemNo;";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", ItemNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            //int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            var item = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL, vSqlParameter).AsQueryable().SingleOrDefault();

            if (item != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ItemModel GetItemPhoto(string ItemNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "SELECT [Photo]" +
                          " FROM [tblPartPhoto] where PartItemNo = @ItemNo;";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", ItemNo));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            //int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            var item = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL, vSqlParameter).AsQueryable().SingleOrDefault();
            return item;
        }

        public bool Delete(string itemNo)
        {

            DatabaseContext db = new DatabaseContext();
            var oTransaction = db.Database.BeginTransaction();

            try
            {
                string sql = "delete from tblPartMasterDetails where [PartItemNo] = @ItemNo";
                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@ItemNo", itemNo));
                var result = db.Database.ExecuteSqlCommand(sql, oParameters.ToArray());

                sql = "delete from tblPartMaster where [PartItemNo] = @ItemNo " +
                    " IF NOT EXISTS (SELECT EneSerialNo FROM [dbo].[tblEnginePartDetails] WHERE PartItemNo=@ItemNo) " +
                        "BEGIN" +
                        " DELETE FROM[dbo].[tblPartPhoto] WHERE PartItemNo =@ItemNo END";
                List<SqlParameter> nParameters = new List<SqlParameter>();
                nParameters.Add(new SqlParameter("@ItemNo", itemNo));
                result = db.Database.ExecuteSqlCommand(sql, nParameters.ToArray());

                if (result > 0)
                {
                    oTransaction.Commit();
                    return true;
                }
                {
                    oTransaction.Rollback();
                    return false;
                }
            }
            catch
            {
                oTransaction.Rollback();
                return false;
            }
        }
        public bool Insert(ItemModel item)
        {
            DatabaseContext db = new DatabaseContext();

            string sql = "INSERT INTO [dbo].[tblPartMaster] " +
                           "([PartItemNo],[IPCRef],[Nomenclature] " +
                           ",[PartType],[PwelStd],[PartStatus],[SN] " +
                           ",[Remarks],[CreatedDate],[CreatedBy] " +
                           //",[UpdatedDate],[UpdatedBy]" +
                           ") " +
                          "Values( " +
                              " @ItemNo , @IPCRef , @Nomenclature ,@PartType ,@PwelStd , " +
                              " @PartStatus  ,@SN ,  @Remarks , " +
                              " @ImportedDate , @ImportedBy  " +
                              //" ,@ImgUploadedDate ,  @ImgUploadedBy " +
                              ");" +
                              " " +
                              " if exists(SELECT PartItemNo " +
                              "FROM [dbo].[tblPartPhoto] WHERE PartItemNo=@ItemNo) " +
                            " Update [dbo].[tblPartPhoto] " +
                            "set Photo=@Photo, " +
                            "ModifiedBy=@ImportedBy , " +
                            "ModifiedDate=@ImportedDate " +
                            "where PartItemNo=@ItemNo " +
                            "else " +
                              "INSERT INTO [dbo].[tblPartPhoto] " +
                              "(PartItemNo,Photo,ModifiedBy,ModifiedDate) " +
                              "Values(@ItemNo,@Photo,@ImportedBy,@ImportedDate) " +
                              ";";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ItemNo", item.PartItemNo));
            oParameters.Add(new SqlParameter("@IPCRef", item.IPCRef));
            oParameters.Add(new SqlParameter("@Nomenclature", item.Nomenclature));
            oParameters.Add(new SqlParameter("@PartType", item.PartType));
            oParameters.Add(new SqlParameter("@PwelStd", item.PwelStd));
            oParameters.Add(new SqlParameter("@PartStatus", (item.PartStatus == null || item.PartStatus == "") ? "" : item.PartStatus));
            oParameters.Add(new SqlParameter("@SN", (item.SN == null || item.SN == "") ? "" : item.SN));
            oParameters.Add(new SqlParameter("@Remarks", (item.Remarks == null || item.Remarks == "") ? "" : item.Remarks));
            oParameters.Add(new SqlParameter("@Photo", item.Photo == null ? "" : item.Photo.Trim()));
            oParameters.Add(new SqlParameter("@ImportedDate", item.CreatedDate));
            oParameters.Add(new SqlParameter("@ImportedBy", item.CreatedBy));
            //oParameters.Add(new SqlParameter("@ImgUploadedDate", null));
            //oParameters.Add(new SqlParameter("@ImgUploadedBy", ""));


            SqlParameter[] vSqlParameter = oParameters.ToArray();

            var result = db.Database.ExecuteSqlCommand(sql, vSqlParameter);
            if (result > 0)
            {
                if (item.PartMappings != null)
                {
                    if (item.PartMappings.Count > 0)
                    {
                        foreach (ItemPartMapping itemPartMapping in item.PartMappings)
                        {
                            string sSQL = "INS_PartMasterDetailsForImport @PartItemNo,@VPN,@PWPN ";

                            List<SqlParameter> nParameters = new List<SqlParameter>();
                            nParameters.Add(new SqlParameter("@PartItemNo", item.PartItemNo));
                            nParameters.Add(new SqlParameter("@VPN", itemPartMapping.VPN == "" || itemPartMapping.VPN == null ? "N/A" : itemPartMapping.VPN));
                            nParameters.Add(new SqlParameter("@PWPN", itemPartMapping.PWPN == "" || itemPartMapping.PWPN == null ? "N/A" : itemPartMapping.PWPN));
                            SqlParameter[] nSqlParameter = nParameters.ToArray();

                            DatabaseContext oRemoteDB = new DatabaseContext();
                            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, nSqlParameter);

                            if (nReturn == 1)
                            {

                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Update(ItemModel item)
        {
            DatabaseContext db = new DatabaseContext();
            var updatePhotoQuery = "";

            if (item.Photo.Trim() != "samephoto")
            {
                updatePhotoQuery = " if exists(SELECT PartItemNo " +
                        "FROM [tblPartPhoto] WHERE PartItemNo=@ItemNo) " +
                    " Update [dbo].[tblPartPhoto] " +
                    "set Photo=@Photo, " +
                    "ModifiedBy=@ImgUploadedBy , " +
                    "ModifiedDate=@ImgUploadedDate " +
                    "where PartItemNo=@ItemNo " +
                    "else " +
                    "   Insert into [dbo].[tblPartPhoto] ([PartItemNo],[Photo],[ModifiedBy],[ModifiedDate]) " +
                    " values (@ItemNo,@Photo,@ImgUploadedBy,@ImgUploadedDate) " +
                    ";";
            }
            string sql = "update tblPartMaster set IPCRef= @IPCRef ,  Nomenclature =  @Nomenclature ," +
                              "PartType= @PartType , PwelStd = @PwelStd , PartStatus =  @PartStatus, [SN] =@SN," +
                              "Remarks = @Remarks ," +
                              " [CreatedDate] = @ImportedDate , [CreatedBy] = @ImportedBy ," +
                              " [UpdatedDate]=  @ImgUploadedDate , [UpdatedBy] = @ImgUploadedBy" +
                              " where [PartItemNo] = @ItemNo ; " +
                              " " +
                               updatePhotoQuery;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            //oParameters.Add(new SqlParameter("@EngSN", item.EngSN));
            oParameters.Add(new SqlParameter("@IPCRef", item.IPCRef));
            oParameters.Add(new SqlParameter("@Nomenclature", item.Nomenclature));
            oParameters.Add(new SqlParameter("@PartType", item.PartType));
            oParameters.Add(new SqlParameter("@PwelStd", item.PwelStd));
            oParameters.Add(new SqlParameter("@PartStatus", (item.PartStatus == null || item.PartStatus == "") ? "" : item.PartStatus));
            oParameters.Add(new SqlParameter("@SN", (item.SN == null || item.SN == "") ? "" : item.SN));
            oParameters.Add(new SqlParameter("@Remarks", (item.Remarks == null || item.Remarks == "") ? "" : item.Remarks));
            if (item.Photo.Trim() != "samephoto")
            {
                oParameters.Add(new SqlParameter("@Photo", item.Photo.Trim()));
            }
            oParameters.Add(new SqlParameter("@ImportedDate", item.CreatedDate));
            oParameters.Add(new SqlParameter("@ImportedBy", item.CreatedBy));
            oParameters.Add(new SqlParameter("@ImgUploadedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@ImgUploadedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
            oParameters.Add(new SqlParameter("@ItemNo", item.PartItemNo));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            var result = db.Database.ExecuteSqlCommand(sql, vSqlParameter);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}