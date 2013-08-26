using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Page
{
    [Serializable]
    public class DIPage
    {
        public int idpage;
        public int? orderpage;
        public int? haschild;
        public int? parent_idpage;
        public int? idpageConfig;
        public int? LangConfigData_idLangConfigData;
        public string namepage;
        public string name;
        public string title;
        public string keyword;
        public string description;
        public string urlrewrite;
        public int? showonmenu {
            set
            {
                if(value == 1)
                    bshowmenu = true;
                else
                    bshowmenu = false;
            }
        }
        public bool bshowmenu;
        public int? newwindows{
            set
            {
                if (value == 1)
                    bnewwindows = true;
                else
                    bnewwindows = false;
            }
        }
        public bool bnewwindows;
        public int? disableAccess;
        public string URLTarget;
        public int? loginrequire;
        public int? Page_idpage;
    }

}