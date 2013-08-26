using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Content
{
    [Serializable]
    public class DIContentSeo
    {
        public int idcontenteditorseo;
        public string title;
        public string keywords;
        public string description;
        public string urlrewrite;
        public int contenteditordetail_idcontenteditordetail;
    }

}