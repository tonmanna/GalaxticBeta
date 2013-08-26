using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Content
{
    [Serializable]
    public class DIContentMoreDetail
    {
        public int idcontenteditordetail;
        public int contenteditor_idcontenteditor;
        public int? parent_idcontenteditordetail;
        public int? ordernumber;
        public string textdata;
        public int? imgposition;
        public string imgpath;
        public int? imgthickboxshow;
        public int? mongodbconfiguration_idconfiguration;
        public string contentlinktype;
        public string contentlinktypevalue;

    }

}