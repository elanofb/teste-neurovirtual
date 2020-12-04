using System.Web;
using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(BannerValidator))]
    public class BannerForm {

		public Banner Banner { get; set;}
		public HttpPostedFileBase OImagem { get; set; }

		public BannerForm() {

		}
    }

}