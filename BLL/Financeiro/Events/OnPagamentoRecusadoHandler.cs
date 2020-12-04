using BLL.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro.Emails;
using DAL.Financeiro;

namespace BLL.Financeiro.Events {

    public class OnPagamentoRecusadoHandler : IHandler<object> {

        //Atributos

        //Propridades

        //Chamador das ações do evento
        public void execute(object source) {

            var OPagamento = source as TituloReceitaPagamento;

            try {

                this.enviarEmail(OPagamento);

            } catch (Exception ex) {

                UtilLog.saveError(ex, $"Erro ao executar rotinas em OnPagamentoRecebidoHandler. Pagamento {OPagamento?.id}");

            }


        }
        /// <summary>
        /// Enviar e-mail de confirmação do recebimento do valor
        /// </summary>
        private void enviarEmail(TituloReceitaPagamento OPagamento) {

            var listaEmails = new List<string>();

            listaEmails.Add(OPagamento.TituloReceita.emailPrincipal);

            listaEmails = listaEmails.Where(UtilValidation.isEmail).ToList();

            if (!listaEmails.Any()) {
                return;
            }


            var Mensageiro = EnvioPagamentoRecusado.factory(OPagamento.idOrganizacao, listaEmails, null);

            Mensageiro.enviar(OPagamento);
        }


    }
}