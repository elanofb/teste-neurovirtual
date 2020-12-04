using System;
using System.Linq;
using DAL.Contribuicoes;

namespace BLL.Contribuicoes {

	public interface IContribuicaoBL {

	    IQueryable<Contribuicao> query(int? idOrganizacaoParam = null);

		Contribuicao carregar(int id);
        
		IQueryable<Contribuicao> listar(string valorBusca, string ativo);

		bool salvar(Contribuicao OContribuicao);

        bool existe(Contribuicao OContribuicao);

		bool excluir(int id);

    }

}