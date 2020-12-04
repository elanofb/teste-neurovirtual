using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro.Emails;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro {


    public class TituloReceitaPagamentoCobrancaBL : DefaultBL, ITituloReceitaPagamentoCobrancaBL {

        //Atributos
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

		//Carregamento do registro
		public UtilRetorno enviarEmailCobranca(int id) {

		    var OPagamento = this.OTituloReceitaPagamentoBL.carregar(id);

		    if (OPagamento == null) {
		        return UtilRetorno.newInstance(true, "O pagamento informado não pôde ser localizado.");
		    }

		    if (OPagamento.dtPagamento.HasValue) {
		        return UtilRetorno.newInstance(true, "Não é possível enviar o e-mail de cobrança para um pagamento já realizado.");
		    }

            if (OPagamento.TituloReceita.idTipoReceita == TipoReceitaConst.OUTROS && OPagamento.TituloReceita.dtQuitacao == null) {

                OPagamento = this.OTituloReceitaPagamentoBL.carregar(id);
            }

		    if (OPagamento.TituloReceita.emailPrincipal == null) {
		        return UtilRetorno.newInstance(true, "Nenhum e-mail foi encontrado para enviar a cobrança.");
		    }

            IEnvioCobrancaPagamento OEmail = EnvioCobrancaPagamento.factory(OPagamento.idOrganizacao, new List<string>{ OPagamento.TituloReceita.emailPrincipal }, null);

            var ORetorno = OEmail.enviar(OPagamento);

		    if (ORetorno.flagError) {
		        return UtilRetorno.newInstance(true, "Ocorreram problemas para disparar o e-mail de cobrança: " + ORetorno.listaErros.FirstOrDefault());
		    }

            return UtilRetorno.newInstance(false, "O e-mail de cobrança foi enviado com sucesso.");

		}

    }
}