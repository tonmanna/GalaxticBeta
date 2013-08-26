using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Content
{
    [Serializable]
    public class DIContent
    {
        public int idcontenteditor;
        public int? parent_idcontenteditor;
        public int? ordernumber;
        public string textdata;
        public int? imgposition;
        public string imgpath;
        public int? imgthickboxshow;
        public int? iconresource_idiconresource;
        public int? mongodbconfiguration_idconfiguration;
        public int? bmoredetail;
        public string ContentLinkTypeValue;
    }

}