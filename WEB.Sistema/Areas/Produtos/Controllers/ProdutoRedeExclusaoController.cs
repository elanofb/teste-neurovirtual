using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using WEB.App_Infrastructure;

namespace WEB.Areas.Produtos.Controllers {

    public class ProdutoRedeExclusaoController : BaseSistemaController {
        
        //Atributos
        private IProdutoRedeConfiguracaoExclusaoBL _ExclusaoBL;
        
        //Servicos
        private IProdutoRedeConfiguracaoExclusaoBL ExclusaoBL => _ExclusaoBL = _ExclusaoBL ?? new ProdutoRedeConfiguracaoExclusaoBL();
        
        [ActionName("excluir")]
        public ActionResult excluir(int id) {

            var Retorno = ExclusaoBL.excluir(id);

            return Json(new {error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault()});
        }
    }

}
