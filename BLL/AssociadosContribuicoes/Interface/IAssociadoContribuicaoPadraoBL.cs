using System;
using DAL.Associados;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

	public interface IAssociadoContribuicaoPadraoBL {

	    UtilRetorno vincularPrimeiraContribuicao(Associado OAssociado);

        bool salvar(AssociadoContribuicao OAssociadoContribuicao);

	}
}
