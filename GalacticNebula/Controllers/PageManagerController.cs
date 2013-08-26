using GalacticNebula.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace GalacticNebula.Controllers
{
    public class PageManagerController : Controller
    {
        public bool check = false;
        //
        // GET: /PageManager/
        readonly DIPageInterface repository;
        public PageManagerController(DIPageRepository repository)
        {
            this.repository = repository;
        }
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListData(string id,string idpage)
        {
            List<DIPage> list;
            string server = Request.ServerVariables["SERVER_NAME"];
            if (idpage == null)
            {
                //DIPageRepository pages = new DIPageRepository();
                list = repository.GetAll(server).ToList();
            }
            else
            {
                int Id = Convert.ToInt32(idpage);
                //DIPageRepository pages = new DIPageRepository();
                list = repository.GetbyParentid(Id, server).ToList();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getNodeData(string id_node)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            int id = Convert.ToInt32(id_node);
            List<DIPage> list;
            list = repository.GetbyNode(id, server).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult changeMode(string id_parent, string mode_change, string id_node)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            string txt = "";
            if (id_node == id_parent)
            {
                return Content("Fail");
            }
            else
            {
             
                if (mode_change == "over")
                {
                    bool i = checkparent(id_node, id_parent);
                    if (i == true)
                    {
                        check = false;
                        return Content("Fail");
                    }
                    else
                    {

                        txt = repository.ChangeMode(id_node, id_parent, mode_change, server);
                    }
                }
                else
                {
                    if ((mode_change == "before")||(mode_change == "after"))
                    {
                        if (id_parent == "1")
                        {
                            return Content("Fail");
                        }
                    }
                    txt = repository.ChangeMode(id_node, id_parent, mode_change, server);
                }
            }
            return Content("Success");
        }

        [HttpPost]
        public ActionResult AddPage(string id_node, string namepage, string lang_id)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            string bLandingPage = "0";
            string id_newnode = repository.AddNode(id_node, namepage, lang_id, bLandingPage, server);

            return Content(id_newnode);
        }

        [HttpPost]
        public ActionResult EditPage(string namepage, string id_node, string name, string title, string keyword, string descript, string urlrewrite, string urlto, string checkwinop, string checkdisaccess, string lang_id)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            if (checkwinop == "true")
            {
                checkwinop = "1";
            }
            else
            {
                checkwinop = "0";
            }

            if (checkdisaccess == "true")
            {
                checkdisaccess = "1";
            }
            else
            {
                checkdisaccess = "0";
            }
            string txt = repository.UpdateNode(namepage, id_node, name, title, keyword, descript, urlrewrite, urlto, checkwinop, checkdisaccess, lang_id, server); 

            return Content("Success");
        }

        [HttpPost]
        public ActionResult DeletePage(string id_node)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            string content = "Success";
            if (id_node == "1")
            {
                content = "Fail";
            }
            else
            {
                string id_newnode = repository.DeleteNode(id_node, server);
            }

            return Content(content);
        }

        [HttpPost]
        public ActionResult updateLayout(string id_layout, string type_layout, string id_node)
        {
            string server = Request.ServerVariables["SERVER_NAME"];
            string id_newnode = repository.UpdateLayout(id_layout, type_layout, id_node, server);

            return Content(id_newnode);
        }

        public bool checkparent(string id_node, string id_parent)
        {
            if (id_parent != "1")
            {
                string server = Request.ServerVariables["SERVER_NAME"];
                string idnode_new = repository.CheckParent(id_parent, server);


                if (idnode_new == id_node)
                {
                    check = true;
                }
                else
                {
                    checkparent(id_node, idnode_new);
                }
            }

            return check;
        }

    }
}
