using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace GalacticNebula.Models.Content
{
    public interface DIContentInterface
    {
        IEnumerable<DIContent> ShowContent(string layout);
        IEnumerable<DIContent> ShowChildContent(string idcontenteditor); 
        IEnumerable<DIContent> SelectContent(string idcontenteditor); 
        string AddContent(string idcontenteditor);
        string Addcontentblock(string idcontenteditor);
        string UpdateContent(string idcontenteditor, string textdata);
        string UploadImage(byte[] data, string name, string filename, string contentType, NameValueCollection nvc);
        string UpdateImgPosition(string idcontenteditor, string position);
        string UpdateImgSize(string imgpath, string idcontenteditor);
        string UpdateTrickbox(string idcontenteditor, string check);
        string SetUpdownContent(string idcontenteditor, string idcontenteditordes);
        string DeleteImg(string idcontenteditor);
        string DeleteBlock(string idcontenteditor);
    }
}