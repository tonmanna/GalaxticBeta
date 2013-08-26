using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using GalacticNebula.Models.Content;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.IO;


namespace GalacticNebula.Models.Content
{
    public class DIContentRepository : DIContentInterface
    {
        public DIContentRepository()
        {
        
        }
        private string nodeserver = WebConfigurationManager.AppSettings["nodeserver"].ToString();
        List<DIContent> content = new List<DIContent>();

        public IEnumerable<DIContent> ShowContent(string layout)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/Content/selectContent?layout=" + layout);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            content = json_serializer.Deserialize<List<DIContent>>(result);
            return content.ToList();
        }

        public IEnumerable<DIContent> ShowChildContent(string idcontenteditor)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/Content/selectParentContent?idContentEditor=" + idcontenteditor);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            content = json_serializer.Deserialize<List<DIContent>>(result);
            return content.ToList();
        }

        public IEnumerable<DIContent> SelectContent(string idcontenteditor)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/Content/SelectById?idContentEditor=" + idcontenteditor);
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            content = json_serializer.Deserialize<List<DIContent>>(result);
            return content.ToList();
        }



        public string AddContent(string idcontenteditor)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/Addcontent", "idContentEditor=" + idcontenteditor);
            return result;
        }

        public string Addcontentblock(string idcontenteditor)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/Addcontentblock", "idContentEditor=" + idcontenteditor);
            return result;
        }

        public string UpdateContent(string idcontenteditor, string textdata)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/UpdateContent", "idcon=" + idcontenteditor + "&text=" + textdata);
            return result;
        }

        public string UploadImage(byte[] data, string name, string filename, string contentType, NameValueCollection nvc)
        {
            //string result = CGClass.HttpPostFS("http://" + nodeserver + "/Upload",stream);
            string result = CGClass.HttpUploadFile("http://" + nodeserver + "/Content/Upload", data, name, filename, contentType, nvc);
            return result;
        }

        public string UpdateImgPosition(string idcontenteditor, string position)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/Upposition", "idContentEditor=" + idcontenteditor + "&imgPosition=" + position);
            return result;
        }

        public string UpdateImgSize(string imgpath, string idcontenteditor)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/UpdateSize", "idContentEditor=" + idcontenteditor + "&imgPath=" + imgpath);
            return result;
        }

        public string UpdateTrickbox(string idcontenteditor, string check)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/UpdateTrickbox", "id=" + idcontenteditor + "&thick=" + check);
            return result;
        }

        public string SetUpdownContent(string idcontenteditor, string idcontenteditordes)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/SetUpdownContent", "idcontenteditor=" + idcontenteditor + "&idcontenteditordes=" + idcontenteditordes);
            return result;
        }

        public string DeleteImg(string idcontenteditor)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/DeleteImg", "idContentEditor=" + idcontenteditor);
            return result;
        }

        public string DeleteBlock(string idcontenteditor)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/Content/DeleteBlock", "idContentEditor=" + idcontenteditor);
            return result;
        }
       
    }
    public class CGClass
    {
        public static string HttpUploadFile(string url, byte[] data, string name, string filename, string contentType, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, name, filename, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(data, 0, data.Length);
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            if (wresp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(wresp.GetResponseStream());
            return sr.ReadToEnd();
        }

        public static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.Timeout = 2000;
            //byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd();
        }
        public static string HttpGet(string URI)
        {
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                req.Timeout = 2000;
                System.Net.WebResponse resp = req.GetResponse();
                string result = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
                return result;
            }
            catch
            {
                return "NodeJS Problem";
            }
        }
    }

}
