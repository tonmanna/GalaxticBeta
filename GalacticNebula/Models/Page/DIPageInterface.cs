using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Page
{
    public interface DIPageInterface
    {
        IEnumerable<DIPage> GetAll(string server);
        IEnumerable<DIPage> GetbyParentid(int id, string server);
        IEnumerable<DIPage> GetbyNode(int id, string server);
        //DIPage Get(int id);
        //DIPage Add(DIPage item);
        //bool Update(DIPage item);
        //bool Delete(int id);
        string CheckParent(string id_parent, string server);
        string ChangeMode(string id_node, string id_parent, string mode_change, string server);
        string AddNode(string id_node, string namepage, string lang_id, string bLandingPage, string server);
        string UpdateNode(string namepage, string id_node, string name, string title, string keyword, string descript, string urlrewrite, string urlto, string checkwinop, string checkdisaccess, string lang_id, string server);
        string UpdateLayout(string id_layout, string type_layout, string id_node, string server);
        string DeleteNode(string id_node, string server);
    }
}