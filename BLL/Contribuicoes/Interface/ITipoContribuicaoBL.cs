using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface ITipoContribuicaoBL {

		TipoContribuicao carregar(int id);

        IQueryable<TipoContribuicao> listar(string ativo);

        bool salvar(TipoContribuicao OTipoContribuicao);

	}
}