using System.Web.Mvc;
using BLL.Notificacoes;

namespace WEB.Areas.Notificacoes.Controllers {

    [OrganizacaoFilter] 
    public class TemplateMensagemExclusaoController : Controller {

        // Atributos
        private ITemplateMensagemExclusaoBL _TemplateMensagemExclusaoBL;

        // Propriedades
        private ITemplateMensagemExclusaoBL OTemplateMensagemExclusaoBL => _TemplateMensagemExclusaoBL = _TemplateMensagemExclusaoBL ?? new TemplateMensagemExclusaoBL();

        public TemplateMensagemExclusaoController() {
        }

        [HttpPost]
        public ActionResult excluir(int id) {
            
            return Json(this.OTemplateMensagemExclusaoBL.excluir(new[] { id }), JsonRequestBehavior.AllowGet);
            
        }

    }
}