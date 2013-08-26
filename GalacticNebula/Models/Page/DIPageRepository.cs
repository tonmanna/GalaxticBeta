using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;


namespace GalacticNebula.Models.Page
{
    public class DIPageRepository : DIPageInterface
    {
        public DIPageRepository()
        {
        
        }
        private string nodeserver = WebConfigurationManager.AppSettings["nodeserver"].ToString();
        List<DIPage> pages = new List<DIPage>();



        public string ChangeMode(string id_node, string id_parent, string mode_change, string server)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/pages1?id_node=" + id_node + "&id_parent=" + id_parent + "&mode_change=" + mode_change + "&server=" + server);
            return result;
        }

        public string CheckParent(string id_parent, string server)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/chk_parent?id_parent=" + id_parent + "&server=" + server);
            return result;
        }


        public IEnumerable<DIPage> GetAll(string server)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/pages");

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            pages = json_serializer.Deserialize<List<DIPage>>(result);
            return pages.ToList();
        }

        public IEnumerable<DIPage> GetbyParentid(int id, string server)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/pages/?id=" + id + "&server=" + server);

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            pages = json_serializer.Deserialize<List<DIPage>>(result);
            return pages;
        }

        public IEnumerable<DIPage> GetbyNode(int id, string server)
        {
            string result = CGClass.HttpGet("http://" + nodeserver + "/selectNode/?id_node=" + id + "&server=" + server);

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            pages = json_serializer.Deserialize<List<DIPage>>(result);
            return pages;
        }

        public string AddNode(string id_node, string namepage, string lang_id, string bLandingPage, string server)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/AddNode", "id_node=" + id_node + "&namepage=" + namepage + "&lang_id=" + lang_id + "&bLandingPage=" + bLandingPage + "&server=" + server);
            return result;
        }

        public string UpdateNode(string namepage, string id_node, string name, string title, string keyword, string descript, string urlrewrite, string urlto, string checkwinop, string checkdisaccess, string lang_id, string server)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/UpdateNode", "namepage=" + namepage + "&id_node=" + id_node + "&name=" + name + "&title=" + title + "&keyword=" + keyword + "&descript=" + descript + "&urlrewrite=" + urlrewrite + "&urlto=" + urlto + "&checkwinop=" + checkwinop + "&checkdisaccess=" + checkdisaccess + "&lang_id=" + lang_id + "&server=" + server);
            return result;
        }

        public string UpdateLayout(string id_layout, string type_layout, string id_node, string server)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/UpdateLayout", "id_layout=" + id_layout + "&type_layout=" + type_layout + "&id_node=" + id_node + "&server=" + server);
            return result;
        }

        public string DeleteNode(string id_node, string server)
        {
            string result = CGClass.HttpPost("http://" + nodeserver + "/DeleteNode", "id_node=" + id_node + "&server=" + server);
            return result;
        }

        //public DIPage Get(int id)
        //{
        //    DIPage page = new DIPage();

        //    return page;
        //}

        //public DIPage Add(DIPage item)
        //{
        //    DIPage page = new DIPage();

        //    return page;
        //}

        //public bool Update(DIPage item)
        //{
        //    return false;
        //}

        //public bool Delete(int id)
        //{
        //    return false;
        //}
    }
    public class CGClass
    {

        public static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
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
