using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Content
{
    public interface DIContentMoreDetailInterface
    {

        IEnumerable<DIContentMoreDetail> AddContentMoreDeatil(string idcontenteditordetail);
        IEnumerable<DIContentMoreDetail> SelectContentDetail(string idcontenteditordetail);
        IEnumerable<DIContentSeo> SelectContentDetailSeo(string idcontenteditordetail);
        IEnumerable<DIiconresource> SelectIcon();
        string Addcontentmore(string idcontenteditordetail);
        string AddSeo(string text, string keyword, string description, string urlrewrite, string idcontenteditordetail);
        string UpdateDatatext(string idcontenteditordetail, string textdata);
        string UploadImage(byte[] data, string name, string filename, string contentType, NameValueCollection nvc);
        string UpdateImgPosition(string idcontenteditordetail, string position);
        string UpdateImgSize(string imgpath, string idcontenteditordetail);
        string UpdateTrickbox(string idcontenteditordetail, string check);
        string UpdateLinkType(string idcontenteditordetail, string linktypevalue);
        string UpdateLinkTypeValue(string idcontenteditordetail, string linktypepath);
        string DeleteImg(string idcontenteditordetail);
        string DeleteBlock(string idcontenteditordetail);
        string SetUpdownContent(string idcontenteditordetail, string idcontenteditordetaildes);
    }
}