using System;
using DAL.Impostos;

namespace BLL.Financeiro {

	public interface ICalculadorImpostoTituloBL {
		UtilRetorno calcularImpostoTitulo(int idTabelaImposto, TituloImposto OTituloImposto);
	}
}
