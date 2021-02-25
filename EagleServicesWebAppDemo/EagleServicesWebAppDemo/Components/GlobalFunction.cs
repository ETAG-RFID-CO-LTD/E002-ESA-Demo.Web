using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EagleServicesWebApp.Models;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Configuration;
using context = System.Web.HttpContext;
using System.Xml;

namespace EagleServicesWebApp.Components
{
    public static class GlobalFunction
    {
        
        public static class Constants
        {
            public const string DateFormat = "MM/dd/yyyy";
            public const string DateFormatGrid = "{0:MM/dd/yyyy}";
            public const string DateFormatGridRead = "{0:dd/MM/yyyy}";
            public const string DateFormatGridTimeRead = "{0:dd MMM yyyy hh:mm:ss}";
            public const string DateFormatReadOnly = "dd MMM yyyy";
            public const string DateFormatTimeRead = "dd/MM/yyyy hh:mm:ss";
        }

        #region Password Encrypt
        public static string CreateMD5Hash(string input)
        {

            byte[] clearBytes;
            clearBytes = new UnicodeEncoding().GetBytes(input);
            byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);
            string hashedText = BitConverter.ToString(hashedBytes);
            return hashedText;

        }

        #endregion

        #region Write Log File
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

        public static void WriteToFile(string Message)
        {
            string path = ConfigurationManager.AppSettings.Get("LogFilePath");
            string filepath = context.Current.Server.MapPath(path);
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            //string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\DataInformation\\ShopifyServiceLog_" + DateTime.Now.ToShortDateString().Replace('/', '_') + DateTime.Now.ToString("HHmmss") + ".txt";
            filepath = filepath + "Eagle_" + DateTime.Now.ToString("dd_MMM_yyyy").Replace('/', '_') + ".txt";

            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public static void SendErrorToText(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;

            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            hostIp = context.Current.Request.UserHostName.ToString();
            HostAdd = context.Current.Request.UserHostAddress.ToString();
            ErrorLocation = ex.Message.ToString();

            try
            {
                string sLogFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");

                string filepath = context.Current.Server.MapPath(sLogFilePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                //filepath = filepath + "AMS_" +DateTime.Today.ToString("dd-MM-yy") + ".txt";
                filepath = filepath + "Eagle_" + DateTime.Now.ToString("dd_MMM_yyyy").Replace('/', '_') + ".txt";

                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Dispose();

                }
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    string error = "Log Written Date:" + " " + DateTime.Now.ToString("dd_MMM_yyyy") + line + "Error Line No :L" + " " + ErrorlineNo + line + "Error Exception:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Message :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + ":" + HostAdd + line;
                    sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                    //sw.WriteLine("-------------------------------------------------------------------------------------");
                    //sw.WriteLine(line);
                    sw.WriteLine(error);
                    sw.WriteLine(ex.Message);
                    sw.WriteLine("--------------------------------*End*------------------------------------------");
                    sw.WriteLine(line);
                    sw.Flush();
                    sw.Close();

                }

            }
            catch (Exception e)
            {
                GlobalFunction.SendErrorToText(e);
                e.ToString();

            }
        }

        #endregion

        #region Call XML file For Common
        public static string GetStatus_MsgByKey(string _Key)
        {
            string fileName = ConfigurationManager.AppSettings.Get("xmlName");
            string result = string.Empty;
            var xmlString = AppDomain.CurrentDomain.BaseDirectory + "XML\\" + fileName.ToString();
            using (XmlReader reader = XmlReader.Create(xmlString))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        #region return Message according by Tag
                        if(reader.Name.ToString() == _Key)
                        {
                            result = reader.ReadString();
                            return result;
                        }
                        #endregion
                    }
                }

            }

            return result;

        }
        #endregion

    }
}