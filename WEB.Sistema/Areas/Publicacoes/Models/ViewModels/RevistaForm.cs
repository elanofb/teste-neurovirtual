using System.Web;
using FluentValidation.Attributes;
using DAL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    [Validator(typeof(RevistaFormValidator))]
	public class RevistaForm {

		public Noticia Revista { get; set;}

		public HttpPostedFileBase Foto { get; set;}
		public HttpPostedFileBase Documento { get; set;}

		//Construtor
		public RevistaForm() {

		}
    }

}