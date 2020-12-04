using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {
    public class PedidoPessoaController : Controller {

        //Atributos
        private IPedidoBL _PedidoBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _PedidoBL = _PedidoBL ?? new PedidoBL();

        // GET: Pedidos/PedidoPessoa
        [ActionName("partial-listar-pedidos")]
        public ActionResult partialListarPedidos() {
            
            int idPessoa = UtilRequest.getInt32("idPessoa");
            
            var ViewModel = new PedidoPessoaLista();
            
            ViewModel.idPessoa = idPessoa;
            
            ViewModel.listaPedidos = this.OPedidoBL.listarPorPessoa(idPessoa).ToList();
            
            return PartialView(ViewModel);
            
        }        
        
        // GET: Pedidos/PedidoPessoa
        [ActionName("partial-listar-produtos-ativos")]
        public ActionResult partialListarProdutosAtivos() {
            
            var ViewModel = new ProdutosAtivosConsultaVM();
            
            ViewModel.carregar();          
            
            return View(ViewModel);
            
        } 
        
    }
}