﻿using EagleServicesWebApp.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web;

namespace EagleServicesWebApp.Models.Main
{
    #region EnumParameters
    //Change enum value to description value 
    internal static class Extensions
    {
        public static string ToDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
    public enum InspectionStatus
    {
        Servicable = 1,
        [Description("External Repair")]
        //[EnumMember(Value = "External Repair")]
        ExternalRepair,
        Scrap
    }
    public enum ProcessStatus
    {
        RFIDTagAssociation = 1,
        CleaningNDT,
        Inspection,
        Kitting,
        Storage,
        Building
    }
    #endregion

    #region DBMaster
    public class Engine
    {
        public int EngineID { get; set; }
        public string EngineName { get; set; }
    }
    public class Module
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string EngineName { get; set; }
    }
    #endregion
    public class Module_Rec
    {
        public string Module { get; set; }
        public int ModuleID { get; set; }
        public int Complete { get; set; }
        public int OutstandingCritical { get; set; }
        public int OutstandingNonCritical { get; set; }
        public int ExternalRepair { get; set; }
        public int Scrap { get; set; }

    }

    public class Part_Rec
    {
        public string PartName { get; set; }
        public string IsCritical { get; set; }
        public string TrolleyName { get; set; }
        public string StorageLocation { get; set; }
        public string InspectionStatus { get; set; }

        //Change into desc value from enum
        //public string InspectionStatusValue
        //{
        //    get { return InspectionStatus != null ? ((InspectionStatus)this.InspectionStatus).ToDescription() : ""; }
        //}
    }

    public class Dashboard_REC
    {
        public int OutModule { get; set; }
        public int OutPart { get; set; }
        public int OutCritical { get; set; }
        public string LastProcessStatus { get; set; }
        public int OverallCompletion { get; set; }
    }
    public class MainModel
    {
        public List<Engine> GetEngineList()
        {
            string sql = "select EngineID,EngineName from [dbo].[tblEngineMaster]";

            DatabaseContext db = new DatabaseContext();
            List<SqlParameter> oParameters = new List<SqlParameter>();
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var data = db.Database.SqlQuery<Engine>(sql, vSqlParameter).ToList();
            return data;

        }
        public List<Module> GetModuleList()
        {
            string sql = "select ModuleID,ModuleName, em.EngineName from [dbo].[tblModuleMaster] mm " +
                "   left join [dbo].[tblEngineMaster] em on em.EngineID=mm.EngineID ";

            DatabaseContext db = new DatabaseContext();
            List<SqlParameter> oParameters = new List<SqlParameter>();
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var moduleData = db.Database.SqlQuery<Module>(sql, vSqlParameter).ToList();
            return moduleData;

        }
        public List<Module_Rec> GetModuleData(int engine)
        {
            string sql = "EXEC SEL_ModuleData @engineID";

            DatabaseContext db = new DatabaseContext();
            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@engineID", engine));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var moduleData = db.Database.SqlQuery<Module_Rec>(sql, vSqlParameter).ToList();
            return moduleData;

        }
        public List<Part_Rec> GetPartData(int module)
        {
            string sql = "select PartName,Case when IsCritical=0 then 'NC' else 'C' end as 'IsCritical' ,tm.TrolleyName,tm.StorageLocation,ips.InspectionStatusName as 'InspectionStatus' " +
                            " from[dbo].[tblPartMaster] pm" +
                            " left join [dbo].[tblTrolleyMaster] tm on tm.TrolleyID =pm.TrolleyID " +
                            " left join [dbo].[tblInspectionStatus] ips on pm.InspectionStatusID =ips.InspectionStatusID " +
                            " where ModuleID = @moduleID ";

            DatabaseContext db = new DatabaseContext();
            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@moduleID", module));
            SqlParameter[] vSqlParameter = oParameters.ToArray();
            var partData = db.Database.SqlQuery<Part_Rec>(sql, vSqlParameter).ToList();
            return partData;

        }
        
        public List<Dashboard_REC> GetDashboardData(int engine)
        {
            try
            {
                string sql = "EXEC SEL_DashboardData @engineID";

                DatabaseContext db = new DatabaseContext();
                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@engineID", engine));
                SqlParameter[] vSqlParameter = oParameters.ToArray();
                var moduleData = db.Database.SqlQuery<Dashboard_REC>(sql, vSqlParameter).ToList();
                return moduleData;
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                return null;
            }
        }
        public bool RestEnigne(int enginID)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = " update [dbo].[tblPartMaster]" +
                " set EPC = null, InspectionStatusID = null, TrolleyID = null, ProcessStatusID=0 " +
                " where ModuleID in (select ModuleID  from [dbo].[tblModuleMaster] where EngineID =@engID)";

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@engID",enginID));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn > 0)
                    bReturn = true;
                else
                    bReturn = false;
            }
            catch (Exception ex)
            {
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }
    }
}