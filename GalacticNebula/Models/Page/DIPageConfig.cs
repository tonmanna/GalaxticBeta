using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Page
{
    public class DIPageConfig
    {
        public DIPage Page_Refference;
        public int langid;
        public string name;
        public string title;
        public string keywords;
        public string description;
        public string URLRewrite;
        public bool showonmenu;
        public bool newwindows;
        public bool disableAccess;
        public string URLTarget;
        public bool loginrequire;
    }
}