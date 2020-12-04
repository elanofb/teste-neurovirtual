using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.Financeiro;
using BLL.Services;
using DAL.Associados;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Financeiro.Controllers {

	public class ReceitaBaixaController : Controller {

        //Atributos
        private ITituloReceitaBL _TituloReceitaBL;
        private IMembroSaldoConsultaBL _SaldoConsultaBL;

	    //Propriedades
        private ITituloReceitaBL OTituloReceitaBL => this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPadraoBL();
        private IMembroSaldoConsultaBL OSaldoConsultaBL => this._SaldoConsultaBL = this._SaldoConsultaBL ?? new MembroSaldoConsultaBL();

        /// <summary>
        /// Abre a modal pra registrar o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("modal-registrar-pagamento")]
        public ActionResult modelRegistrarPagamento(int id) {

            var ViewModel = new BaixaTituloReceitaForm();

            int idTituloReceita = id;

            ViewModel.TituloReceita = this.OTituloReceitaBL.carregar(idTituloReceita) ?? new TituloReceita();

            if (ViewModel.TituloReceita.id == 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A cobrança informada não foi localizada.");

                return PartialView(ViewModel);
            }

            if (ViewModel.TituloReceita.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A cobrança informada não foi localizada.");

                return PartialView(ViewModel);
            }

            if (ViewModel.TituloReceita.dtQuitacao.HasValue) {

                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A cobrança informada já foi quitada.");

                return PartialView(ViewModel);
            }


            ViewModel.TituloReceitaPagamento.valorRecebido = ViewModel.TituloReceita.valorTotal.toDecimal();

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Registra o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("registrar-pagamento")]
        public ActionResult registrarPagamento(BaixaTituloReceitaForm ViewModel) {

            ViewModel.TituloReceita = this.OTituloReceitaBL.carregar(ViewModel.TituloReceita.id) ?? new TituloReceita();

            if (!ModelState.IsValid) {
                return PartialView("modal-registrar-pagamento", ViewModel);
            }

            if (ViewModel.TituloReceita.idOrganizacao != User.idOrganizacao()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A cobrança informada não foi localizada.");

                return PartialView("modal-registrar-pagamento", ViewModel);
            }
            
            if (ViewModel.TituloReceita.dtQuitacao.HasValue) {

                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "A cobrança informada já foi quitada.");

                return PartialView("modal-registrar-pagamento", ViewModel);
            }

            if (ViewModel.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.BITLINK) {

                var Saldo = this.OSaldoConsultaBL.query(0, 1)
                                .Where(x => x.Membro.idPessoa == ViewModel.TituloReceita.idPessoa)
                                .Select(x => new {
                                                     x.id, 
                                                     x.idMembro, 
                                                     x.idPessoa, 
                                                     x.saldoAtual
                                                 })
                                .FirstOrDefault()
                                .ToJsonObject<MembroSaldo>() ?? new MembroSaldo();

                if (Saldo.saldoAtual < ViewModel.TituloReceitaPagamento.valorRecebido) {

                    this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "O usuário em questão não possui saldo suficiente para a operação.");

                    return PartialView("modal-registrar-pagamento", ViewModel);
                
                }
                
            }

            ViewModel.TituloReceitaPagamento.transferirDadosTitulo(ViewModel.TituloReceita);

			ViewModel.TituloReceitaPagamento.idUsuarioBaixa = User.id();

            if (ViewModel.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.DINHEIRO 
                || ViewModel.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO
                || ViewModel.TituloReceitaPagamento.idMeioPagamento == MeioPagamentoConst.TRANSFERENCIA_ELETRONICA) {
                ViewModel.TituloReceitaPagamento.valorTarifasBancarias = 0;
                ViewModel.TituloReceitaPagamento.valorTarifasTransacao = 0;
            }

			ViewModel.TituloReceitaPagamento.idStatusPagamento = StatusPagamentoConst.PAGO;

			ViewModel.TituloReceitaPagamento.valorRecebido = decimal.Add(ViewModel.TituloReceita.valorTotal.toDecimal(), ViewModel.TituloReceitaPagamento.valorJuros.toDecimal());

			var listaPagamentos = new List<TituloReceitaPagamento>();

			listaPagamentos.Add(ViewModel.TituloReceitaPagamento);

            TituloReceitaBaixaFactoryBL.getInstance.factory(ViewModel.TituloReceita).liquidar(ViewModel.TituloReceita, listaPagamentos);

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "O pagamento foi registrado com sucesso");

			return PartialView("modal-registrar-pagamento", ViewModel);

        }

    }
}