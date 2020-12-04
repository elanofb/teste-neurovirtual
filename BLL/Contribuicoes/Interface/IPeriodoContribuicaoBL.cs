using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface IPeriodoContribuicaoBL {

		PeriodoContribuicao carregar(int id);

		IQueryable<PeriodoContribuicao> listar(bool? ativo);

        bool salvar(PeriodoContribuicao OPeriodoContribuicao);

        UtilRetorno excluir(int id);

	}
}