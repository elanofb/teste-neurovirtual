using DAL.Diretorias;
using System.Web;

namespace WEB.Areas.Diretorias.ViewModels {

    public class DiretoriaMembroForm {

        public DiretoriaMembro DiretoriaMembro { get; set; }

        public HttpPostedFileBase OImagem { get; set; }

        public DiretoriaMembroForm() {

        }
    }
}