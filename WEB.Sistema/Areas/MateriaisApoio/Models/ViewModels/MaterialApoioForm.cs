using DAL.MateriaisApoio;
using FluentValidation.Attributes;
using System.Web;


namespace WEB.Areas.MateriaisApoio.ViewModels {

    [Validator(typeof(MaterialApoioFormValidator))]
    public class MaterialApoioForm {

		public MaterialApoio MaterialApoio { get; set; }
        public HttpPostedFileBase OArquivo { get; set; }

        public MaterialApoioForm() {
        }


    }

}