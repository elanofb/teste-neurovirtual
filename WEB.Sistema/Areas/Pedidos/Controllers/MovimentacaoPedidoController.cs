using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pedidos;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

	public class MovimentacaoPedidoController : Controller {
		
		//Atributos
		private IMovimentacaoPedidoBL _IMovimentacaoPedidoBL;
		
		//Propriedades        	
		private IMovimentacaoPedidoBL OMovimentacaoPedidoBL => _IMovimentacaoPedidoBL = _IMovimentacaoPedidoBL ?? new MovimentacaoPedidoBL();
		
	    //Abertura do modal para seleção dos pedidos
		[ActionName("modal-selecionar-pedidos")]
		public ActionResult modalSelecionarPedidos(PedidoMovimentacaoForm ViewModel) {
            
			ViewModel.adicionarNaLista();
			
			ViewModel.montarQuery();
			
			return PartialView(ViewModel);

		}
		
	    //remover um pedido selecionado
		[HttpPost, ActionName("remover-pedidos")]
		public ActionResult removerPedidos(PedidoMovimentacaoForm ViewModel) {
            			
			ViewModel.removerDaLista();
			ModelState.Clear();
			ViewModel.montarQuery();
			
			return PartialView("modal-selecionar-pedidos",ViewModel);
			
		}
		
		[HttpPost, ActionName("movimentar-pedidos")]
		public ActionResult movimentarPedidos(PedidoMovimentacaoForm ViewModel) {
			
			if (!ModelState.IsValid){
				return View("modal-selecionar-pedidos", ViewModel);
			}
			
			ViewModel.montarQuery();
			
			var idsPedidosSelecionados = ViewModel.listaPedidos.Select(x => x.id).ToList();
			bool flagSucesso = this.OMovimentacaoPedidoBL.alterarStatus(idsPedidosSelecionados, ViewModel.idStatusPedido);
			
			if (!flagSucesso){
				this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível alterar o status dos pedidos!");
				return View("modal-selecionar-pedidos", ViewModel);
			}
			
			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Pedidos alterados com sucesso!");
			return Json(new {error = false, message = ""});									
			
		}

	}
}
