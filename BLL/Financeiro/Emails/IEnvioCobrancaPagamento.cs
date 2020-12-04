
using System;
using DAL.Financeiro;

namespace BLL.Financeiro.Emails {

	public interface IEnvioCobrancaPagamento {

		UtilRetorno enviar(TituloReceitaPagamento OPagamento);

	}
}