using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;
using BLL.Services;
using DAL.Produtos;
using WEB.App_Infrastructure;

namespace WEB.Areas.Produtos.Controllers {

    public class ProdutoRedeConsultaController : BaseSistemaController {
        
        //Atributos
        private IProdutoRedeConfiguracaoConsultaBL _ConsultaBL;
        
        //Servicos
        private IProdutoRedeConfiguracaoConsultaBL ConsultaBL => _ConsultaBL = _ConsultaBL ?? new ProdutoRedeConfiguracaoConsultaBL();
        
        // GET
        [ActionName("partial-lista-configuracao")]
        public ActionResult partialListaConfiguracao(int idProduto) {

            var listaRegistros = ConsultaBL.listar(idProduto)
                                           .Where(x => idProduto > 0)
                                           .Select(x => new {
                                                                x.id,
                                                                x.idProduto,
                                                                x.nivel,
                                                                x.percentualComissao,
                                                                x.dtCadastro
                                                            })
                                            .OrderBy(x => x.nivel)
                                           .ToListJsonObject<ProdutoRedeConfiguracao>();
        
            return PartialView(listaRegistros);
        }
    }

}
