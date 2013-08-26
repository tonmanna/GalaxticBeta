using GalacticNebula.Models;
using GalacticNebula.Models.Content;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GalacticNebula.Controllers
{
    public class ContentManagerController : Controller
    {

        readonly DIContentInterface contentrepo;
        readonly DIContentMoreDetailInterface contentmorerepo;
        public ContentManagerController(DIContentInterface contentrepo,DIContentMoreDetailInterface contentmorerepo)
        {
            this.contentrepo = contentrepo;
            this.contentmorerepo = contentmorerepo;
        }


        protected string RenderPartialToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult Index()
        {
            List<DIContent> list = contentrepo.ShowContent("1").ToList();
            if (list.Count() != 0)
            {
                string editor = "";
                string script = "";
                string preview = "";
                string tab = "";
                showContentEditor sh = new showContentEditor();
                string idcontent = list[0].idcontenteditor.ToString();

                sh.trick = list[0].imgthickboxshow.ToString();
                sh.id = list[0].idcontenteditor;
                sh.order = list[0].ordernumber;
                string html = System.Web.HttpUtility.HtmlDecode(list[0].textdata);
                sh.text = html;
                sh.imgpath = list[0].imgpath;
                editor = editor + RenderPartialToString("_contentEditor", sh);
                script = script + RenderPartialToString("_contentEditorScript", sh);
                if (list[0].imgposition == 1)
                {
                    preview = preview + RenderPartialToString("_contentImgLeft", sh);
                }
                else if (list[0].imgposition == 2)
                {
                    preview = preview + RenderPartialToString("_contentImgCenter", sh);
                }
                else if (list[0].imgposition == 3)
                {
                    preview = preview + RenderPartialToString("_contentImgRight", sh);
                }

                //if (list[0].bmoredetail == 0)
                //{
                //    tab = "<ul><li class='k-state-active'>พิมพ์ข้อความ</li><li id='more' style='display:none'>รายละเอียด</li></ul>";
                //}
                //else
                //{
                //    tab = "<ul><li class='k-state-active'>พิมพ์ข้อความ</li><li id='more'>รายละเอียด</li></ul>";
                //}

                List<DIContent> list2 = contentrepo.ShowChildContent(idcontent).ToList();
                foreach (var data in list2)
                {
                    sh.order = data.ordernumber;
                    sh.trick = data.imgthickboxshow.ToString();
                    sh.id = data.idcontenteditor;
                    string html2 = System.Web.HttpUtility.HtmlDecode(data.textdata);
                    sh.text = html2;
                    sh.imgpath = data.imgpath;
                    editor = editor + RenderPartialToString("_contentEditor", sh);
                    script = script + RenderPartialToString("_contentEditorScript", sh);
                    if (data.imgposition == 1)
                    {
                        preview = preview + RenderPartialToString("_contentImgLeft", sh);
                    }
                    else if (data.imgposition == 2)
                    {
                        preview = preview + RenderPartialToString("_contentImgCenter", sh);
                    }
                    else if (data.imgposition == 3)
                    {
                        preview = preview + RenderPartialToString("_contentImgRight", sh);
                    }
                }
                ViewBag.drawEdit = "<input type='hidden' id='hidden' value='" + list[0].idcontenteditor + "'/>" + editor;
                //ViewBag.drawUL = tab;
                ViewBag.draw = preview;
                ViewBag.drawMoreDetail = "<img id='linkvaluepreview' src='/ContentManager/ImageIcon?id=" + list[0].ContentLinkTypeValue + "'/>";
                ViewBag.script = script;
            }
            return View();
        }

        public ActionResult MoreDetail(string id)
        {
            ViewBag.drawMore = "xxx";
            return View();
        }

        [HttpGet]
        public JsonResult SelectContent(string idcontenteditor,string type)
        {
            if (type == "content")
            {
                List<DIContent> list = contentrepo.SelectContent(idcontenteditor).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<DIContentMoreDetail> list = contentmorerepo.SelectContentDetail(idcontenteditor).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult SelectSeo(string idcontenteditor)
        {
            List<DIContentSeo> list = contentmorerepo.SelectContentDetailSeo(idcontenteditor).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Content_textImg()
        {
            ViewBag.message = "Page!!";
            return View();
        }

        [HttpPost]
        public ActionResult AddMoreDetail(string idcontenteditor)
        {
            List<DIContentMoreDetail> list = contentmorerepo.AddContentMoreDeatil(idcontenteditor).ToList();
            showContentEditor sh = new showContentEditor();
            string detaileditor = "";
            string scriptdetaileditor = "";
            string firstnode = "";
            string linktype = "";
            foreach (var data in list)
            {
                if (data.ordernumber == 1)
                {
                    firstnode = data.idcontenteditordetail.ToString();
                    linktype = data.contentlinktype;
                }
                sh.trick = data.imgthickboxshow.ToString();
                sh.id = data.idcontenteditordetail;
                sh.order = data.ordernumber;
                string html = System.Web.HttpUtility.HtmlDecode(data.textdata);
                sh.text = html;
                sh.imgpath = data.imgpath;
                detaileditor = detaileditor + RenderPartialToString("_contentEditorMoreDetail", sh);
                scriptdetaileditor = scriptdetaileditor + RenderPartialToString("_contentEditorMoreDetailScript", sh);
            }
            detaileditor = "<input type='hidden' id='morehidden' value='" + firstnode + "'/>" + detaileditor;
            string htmlandscript = detaileditor + "<....>" + scriptdetaileditor + "<....>" + linktype;
            return Content(htmlandscript);
        }

        [HttpPost]
        public ActionResult AddContent(string idcontenteditor)
        {
            string id_newcontent = contentrepo.AddContent(idcontenteditor);
            return Content(id_newcontent);
        }

        [HttpPost]
        public ActionResult Addcontentblock(string idcontenteditor,string type)
        {
            string id_newcontent = "";
            if (type == "content")
            {
                id_newcontent = contentrepo.Addcontentblock(idcontenteditor);
            }
            else if (type == "moredetail")
            {
                id_newcontent = contentmorerepo.Addcontentmore(idcontenteditor);
            }

            return Content(id_newcontent);
        }

        [HttpPost]
        public ActionResult AddSeo(string title, string keyword, string description, string urlrewrite, string idcontenteditor)
        {
            string seodata = contentmorerepo.AddSeo(title, keyword, description, urlrewrite, idcontenteditor);
            return Content(seodata);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateContent(string idcontenteditor, string textdata, string type)
        {
            string updatecontent = "";
            string html = System.Web.HttpUtility.HtmlEncode(textdata);
            html = html.Replace("&", "${amp}");
            if (type == "content")
            {
                updatecontent = contentrepo.UpdateContent(idcontenteditor, html);
            }
            else if(type == "moredetail")
            {
                updatecontent = contentmorerepo.UpdateDatatext(idcontenteditor, html);
            }
            return Content(updatecontent);
        }

        [HttpPost]
        public ActionResult UpdatePosition(string idcontenteditor, string position, string type)
        {
            string updateposition = "";
            if (type == "content")
            {
                updateposition = contentrepo.UpdateImgPosition(idcontenteditor, position);
            }
            else if (type == "moredetail")
            {
                updateposition = contentmorerepo.UpdateImgPosition(idcontenteditor, position);
            }
            return Content(updateposition);
        }

        [HttpPost]
        public ActionResult UpdateSize(string imgpath,string idcontenteditor,string type)
        {
            string updatesize = "";
            if (type == "content")
            {
                updatesize = contentrepo.UpdateImgSize(imgpath, idcontenteditor);
            }
            else if (type == "moredetail")
            {
                updatesize = contentmorerepo.UpdateImgSize(imgpath, idcontenteditor);
            }
            return Content(updatesize);
        }

        [HttpPost]
        public ActionResult UpdateTrickbox(string idcontenteditor,string check,string type)
        {
            string updatesize = "";
            if (type == "content")
            {
                updatesize = contentrepo.UpdateTrickbox(idcontenteditor, check);
            }
            else if (type == "moredetail")
            {
                updatesize = contentmorerepo.UpdateTrickbox(idcontenteditor, check);
            }
            
            return Content(updatesize);
        }

        [HttpPost]
        public ActionResult UpdateLinkType(string idcontenteditordetail, string linktypevalue)
        {
            string updatelinktype = contentmorerepo.UpdateLinkType(idcontenteditordetail, linktypevalue);
            return Content(updatelinktype);
        }

        [HttpPost]
        public ActionResult UpdateLinkTypeValue(string idcontenteditordetail, string linktypepath)
        {
            string updatelinkvalue = contentmorerepo.UpdateLinkTypeValue(idcontenteditordetail, linktypepath);
            return Content(updatelinkvalue);
        }

        [HttpPost]
        public ActionResult UpdateOrder(string idcontenteditor, string idcontenteditordes, string type)
        {
            string updatesize = "";
            if(type=="content")
            {
                updatesize = contentrepo.SetUpdownContent(idcontenteditor, idcontenteditordes);
            }
            else if(type == "moredetail")
            {
                updatesize = contentmorerepo.SetUpdownContent(idcontenteditor, idcontenteditordes);
            }
            return Content(updatesize);
        }


        [HttpPost]
        public ActionResult DeleteImg(string idcontenteditor,string type)
        {
            string delimg = "";
            if (type == "content")
            {
                delimg = contentrepo.DeleteImg(idcontenteditor);
            }
            else if (type == "moredetail")
            {
                delimg = contentmorerepo.DeleteImg(idcontenteditor);
            }
            
            return Content(delimg);
        }

        [HttpPost]
        public ActionResult DeleteBlock(string idcontenteditor,string type)
        {
            string delblock = "";
            if (type == "content")
            {
                delblock = contentrepo.DeleteBlock(idcontenteditor);
            }
            else if (type == "moredetail")
            {
                delblock = contentmorerepo.DeleteBlock(idcontenteditor);
            }
            return Content(delblock);
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<HttpPostedFileBase> files)
        {
            return
                from a in files
                where a != null
                select string.Format("{0} ({1} bytes)", Path.GetFileName(a.FileName), a.ContentLength);
        }

        [HttpPost]
        public ActionResult uploadpic(IEnumerable<HttpPostedFileBase> displayImage, string id, string type)
        {
            TempData["UploadedFiles"] = GetFileInfo(displayImage);
            var filelocal = displayImage.First();
            var contenttype = filelocal.ContentLength;
            byte[] data = new byte[filelocal.ContentLength];
            filelocal.InputStream.Read(data, 0, filelocal.ContentLength);

            NameValueCollection nvc = new NameValueCollection();
            
            string uploaddata = "";
            if(type == "content")
            {
                nvc.Add("ideditor", id);
                uploaddata = contentrepo.UploadImage(data, "displayImage", filelocal.FileName, filelocal.ContentType, nvc);
            }
            else if(type == "moredetail")
            {
                nvc.Add("idContentEditorDetail", id);
                uploaddata = contentmorerepo.UploadImage(data, "displayImage", filelocal.FileName, filelocal.ContentType, nvc);
            }
            return Json(new { data = uploaddata }, "text/plain");
        }

        [HttpGet]
        public JsonResult getIconResource()
        {
            List<DIiconresource> list=  contentmorerepo.SelectIcon().ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //public static void UploadPhoto(string name)
        //{
        //    var connectionString = "mongodb://localhost:27017";
        //    var client = new MongoClient(connectionString);
        //    var server = client.GetServer();
        //    var database = server.GetDatabase("IMAGE");
        //    string fileName = name;
        //    using (var fs = new FileStream(fileName, FileMode.Open))
        //    {
        //        var gridFsInfo = database.GridFS.Upload(fs, "KOARA");
        //        var fileId = gridFsInfo.Id;
        //    }

        //}

        [HttpGet]     
        public FileResult Image(string id)
        {
            string[] dt = id.Split('.');
            string imgtype = "";
            if(dt[1] == "jpg")
            {
                imgtype = "image/jpg";
            }
            else if (dt[1] == "png")
            {
                imgtype = "image/png";
            }
            else if (dt[1] == "jpeg")
            {
                imgtype = "image/jpeg";
            }

            byte[] br = ShowPhoto(id,"img");

            if (br.Length != 0)
            {
                return File(br, imgtype);
            }
            else
            {
                return File("C:\\Noimage","image/jpg");
            }
        }


        [HttpGet]
        public FileResult ImageIcon(string id)
        {
            string[] dt = id.Split('.');
            string imgtype = "";
            if (dt[1] == "jpg")
            {
                imgtype = "image/jpg";
            }
            else if (dt[1] == "png")
            {
                imgtype = "image/png";
            }
            else if (dt[1] == "jpeg")
            {
                imgtype = "image/jpeg";
            }

            byte[] br = ShowPhoto(id, "imgicon");

            if (br.Length != 0)
            {
                return File(br, imgtype);
            }
            else
            {
                return File("C:\\Noimage", "image/jpg");
            }
        }

        public static byte[] ShowPhoto(string name,string img)
        {
            try
            {
                string imgdb = "";
                var connectionString = "mongodb://192.168.3.123:27017";
                var client = new MongoClient(connectionString);
                var server = client.GetServer();
                if (img == "img")
                {
                    imgdb = "imagedb";
                }
                else if(img == "imgicon")
                {
                    imgdb = "uploadicon";
                }
                var database = server.GetDatabase(imgdb);
                var file = database.GridFS.FindOne(Query.EQ("filename", name));
                using (var stream = file.OpenRead())
                {

                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    return bytes;
                }
            }
            catch(Exception ex)
            {
                byte[] dum = new byte[0];
                return dum;
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LoginUpload(IEnumerable<HttpPostedFileBase> displayImage)
        //{
        //    if (displayImage != null)
        //    {
        //        TempData["UploadedFiles"] = GetFileInfo(displayImage);
        //        var filelocal = displayImage.First();
        //        byte[] data = new byte[filelocal.ContentLength];
        //        filelocal.InputStream.Read(data, 0, filelocal.ContentLength);

        //        NameValueCollection nvc = new NameValueCollection();
        //        nvc.Add("id", "TTR");
        //        nvc.Add("btn-submit-photo", "Upload");

        //        HttpUploadFile("http://localhost:3000/upload", data, "displayImage", filelocal.FileName, "image/jpeg", nvc);

        //    }

        //    return RedirectToRoute(new
        //    {
        //        controller = "MainBackend",
        //        action = "Index"
        //    });
        //}

        


     
    }
}
