using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Financeiro;
using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosContribuicoes.ViewModels;
using MvcFlashMessages;
using BLL.Associados;
using DAL.Permissao.Security.Extensions;
using WEB.Extensions;

namespace WEB.Areas.AssociadosContribuicoes.Controllers{

    public class AssociadoContribuicaoPagamentoController : BaseSistemaController{

		//Atributos
		private IAssociadoContribuicaoBL _AssociadoContribuicaoBL; 
		private ITituloReceitaBaixaBL _TituloReceitaBaixaBL; 
		private ITituloReceitaBL _TituloReceitaBL; 

		//Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        private ITituloReceitaBaixaBL OTituloReceitaBaixaBL => this._TituloReceitaBaixaBL = this._TituloReceitaBaixaBL ?? new TituloReceitaContribuicaoBaixaBL();
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaContribuicaoBL();


        //Abrir o modal com formulario para registro da data de pagamento
		[HttpGet, ActionName("modal-registrar-pagamento")]
        public ActionResult modalDetalheAnuidade(int id){
			
			var OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(id) ?? new AssociadoContribuicao();

			var ViewModel = new RegistroPagamentoForm();

			ViewModel.AssociadoContribuicao = OAssociadoContribuicao;

			return PartialView(ViewModel);
        }

		//Listar as anuidades pendentes do associado
		[HttpPost, ActionName("salvar-pagamento")]
        public ActionResult salvarPagamento(RegistroPagamentoForm ViewModel){


			if (!ModelState.IsValid) {
				
				ViewModel.AssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(ViewModel.AssociadoContribuicao.id) ?? new AssociadoContribuicao();

				return PartialView("modal-registrar-pagamento", ViewModel);
			}

		    var dbTituloReceita = this.OTituloReceitaBL.carregarPorReceita(ViewModel.AssociadoContribuicao.id);

			ViewModel.AssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(ViewModel.AssociadoContribuicao.id) ?? new AssociadoContribuicao();

			ViewModel.TituloReceitaPagamento.idUsuarioBaixa = User.id();

			ViewModel.TituloReceitaPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;

			ViewModel.TituloReceitaPagamento.valorRecebido = decimal.Add(dbTituloReceita.valorTotal.toDecimal(), ViewModel.TituloReceitaPagamento.valorJuros.toDecimal());

			var listaPagamentos = new List<TituloReceitaPagamento>();

			listaPagamentos.Add(ViewModel.TituloReceitaPagamento);

			this.OTituloReceitaBaixaBL.liquidar(ViewModel.AssociadoContribuicao.id, listaPagamentos, User.id());

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O pagamento foi registrado com sucesso");

			return Json(new {error = false, message=""});

        }
        

    }
}
