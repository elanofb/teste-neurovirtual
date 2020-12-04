using FluentValidation.Attributes;
using DAL.LinksUteis;
using System.Web;

namespace WEB.Areas.LinksUteis.ViewModels {

    [Validator(typeof(LinkUtilValidator))]
    public class LinkUtilForm {

        public LinkUtil LinkUtil { get; set; }

        public HttpPostedFileBase Arquivo { get; set; }

        public LinkUtilForm() {

            this.LinkUtil = new LinkUtil();

        }
    }
}