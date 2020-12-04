using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.App_Infrastructure;

namespace WEB.Areas.Publicacoes.Controllers {

    [OrganizacaoFilter]
    public class ConteudoExclusaoController : BaseSistemaController {
        
        //Atributos
        private IConteudoExclusaoBL _ConteudoExclusaoBL;        
        
        //Propriedades
        private IConteudoExclusaoBL OConteudoExclusaoBL => _ConteudoExclusaoBL = _ConteudoExclusaoBL ?? new ConteudoExclusaoBL();        
        
        //Construtor
        public ConteudoExclusaoController() {

        }                
                
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OConteudoExclusaoBL.excluir(id));
        }
                
    }
}