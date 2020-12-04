using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using PagedList;
using WEB.Areas.Pedidos.ViewModels;
using BLL.Organizacoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Pedidos.Controllers {

    [OrganizacaoFilter]
    public class ProdutosAtivosController : Controller {
            
        //Atributos
        private IOrganizacaoBL _IOrganizacaoBL;
        private IPedidoProdutoBL _PedidoProdutoBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => _IOrganizacaoBL = _IOrganizacaoBL ?? new OrganizacaoBL();
        private IPedidoProdutoBL OPedidoProdutoBL => _PedidoProdutoBL = _PedidoProdutoBL ?? new PedidoProdutoBL();
        
        //
        public ActionResult index() {
                
            var ViewModel = new ProdutosAtivosConsultaVM();
            
            ViewModel.carregar();            
            
            return View(ViewModel);
            
        }

    }
    
}