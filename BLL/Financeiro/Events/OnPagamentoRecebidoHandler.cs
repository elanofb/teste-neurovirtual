using BLL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro.Emails;
using BLL.Notificacoes;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro.Events {

    public class OnPagamentoRecebidoHandler : IHandler<object> {

        //Atributos

        //Servicos


        //Chamador das ações do evento
        public void execute(object source) {

            var OPagamentoParam = source as TituloReceitaPagamento ?? new TituloReceitaPagamento();

            var OPagamento = this.carregarDadosPagamento(OPagamentoParam.id);
            
            if (OPagamento == null) {
                
                throw new Exception("Não foi possível executar o manipulador OnPagamentoRecebidoHandler pois o objeto é nulo.");

            }
            
            try {

                this.liquidar(OPagamento);

            } catch(Exception ex) {

                UtilLog.saveError(ex, $"Erro ao executar a rotina de baixa no titulo em OnPagamentoRecebidoHandler. Pagamento {OPagamento.id}");

            }

            try {

                this.enviarEmail(OPagamento);
                
            }catch (Exception ex){

                UtilLog.saveError(ex, $"Erro ao executar a rotina de envio de e-mail em OnPagamentoRecebidoHandler. Pagamento {OPagamento.id}");

            }
            

        }
        /// <summary>
        /// Enviar e-mail de confirmação do recebimento do valor
        /// </summary>
        private void enviarEmail(TituloReceitaPagamento OPagamento) {

            if (OPagamento.valorTotalComDesconto() <= 0){

                return;

            }

            var listaEmails = new List<string>();

            listaEmails.Add(OPagamento.TituloReceita.emailPrincipal);

            listaEmails = listaEmails.Where(UtilValidation.isEmail).ToList();

            if (!listaEmails.Any()) {
                return;
            }


            var Mensageiro = EnvioPagamentoRecebido.factory(OPagamento.idOrganizacao, listaEmails, null);

            Mensageiro.enviar(OPagamento);
        }

        /// <summary>
        /// Executar serviço que verificará que o título foi totalmente quitado
        /// </summary>
        private void liquidar(TituloReceitaPagamento OPagamento) {

            var OTituloBL = TituloReceitaBaixaFactoryBL.getInstance.factory(OPagamento.TituloReceita);

            OTituloBL.liquidar(OPagamento.TituloReceita);
        }


        /// <summary>
        /// 
        /// </summary>
        private TituloReceitaPagamento carregarDadosPagamento(int idPagamento) {

            ITituloReceitaPagamentoConsultaBL PagamentoConsultaBL = new TituloReceitaPagamentoConsultaBL(); 
            
            var query = PagamentoConsultaBL.query(0)
                                                .Where(x => x.id == idPagamento)
                                                .Select(x => new {
                                                                     x.id,
                                                                     x.idOrganizacao,
                                                                     x.idTituloReceita,
                                                                     x.valorDesconto,
                                                                     x.valorDescontoAntecipacao,
                                                                     x.valorDescontoCupom,
                                                                     x.valorJuros,
                                                                     x.valorTarifasBancarias,
                                                                     x.valorTarifasTransacao,
                                                                     x.valorOriginal,
                                                                     x.valorRecebido,
                                                                     x.dtVencimento,
                                                                     x.dtVencimentoOriginal,
                                                                     x.dtPagamento,
                                                                     x.descricaoParcela,
                                                                     TituloReceita = new {
                                                                                             x.TituloReceita.id,
                                                                                             x.TituloReceita.idPessoa,
                                                                                             x.TituloReceita.idOrganizacao,
                                                                                             x.TituloReceita.idTipoReceita,
                                                                                             x.TituloReceita.idReceita,
                                                                                             x.TituloReceita.valorTotal,
                                                                                             x.TituloReceita.valorDesconto,
                                                                                             x.TituloReceita.valorJuros,
                                                                                             x.TituloReceita.dtQuitacao,
                                                                                             x.TituloReceita.emailPrincipal,
                                                                                             x.TituloReceita.nomePessoa,
                                                                                             x.TituloReceita.descricao
                                                                                         }
                                                                 
                                                                 });

            var Registro = query.FirstOrDefault().ToJsonObject<TituloReceitaPagamento>();

            return Registro;
        }

    }
}