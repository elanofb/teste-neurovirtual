using System.Web.Mvc;
using BLL.Atendimentos;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoAcaoInicioController : Controller {

        //Atributos
	    private IAtendimentoConsultaBL _IAtendimentoConsultaBL;
        private IAtendimentoAcaoBL _IAtendimentoAcaoBL;

        //Propriedades
	    private IAtendimentoConsultaBL OAtendimentoConsultaBL => _IAtendimentoConsultaBL = _IAtendimentoConsultaBL ?? new AtendimentoConsultaBL();
        private IAtendimentoAcaoBL OAtendimentoAcaoBL => _IAtendimentoAcaoBL = _IAtendimentoAcaoBL ?? new AtendimentoAcaoBL();

        //
        [ActionName("iniciar-atendimento")]
        public JsonResult iniciarAtendimento(int id) {

            var OAtendimento = this.OAtendimentoConsultaBL.carregar(id);

            if (OAtendimento == null) {

                return Json(new { error = true, message = "O atendimento informado não foi encontrado." }, JsonRequestBehavior.AllowGet);

            }

            this.OAtendimentoAcaoBL.iniciarAtendimento(id);

            string urlRedirect = Url.Action("detalhe", "Atendimento", new{id});

            
            return Json(new { error = false, urlRedirect }, JsonRequestBehavior.AllowGet);

        }


    }
}