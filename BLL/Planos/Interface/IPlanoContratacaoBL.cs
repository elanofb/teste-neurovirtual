using System;
using System.Linq;
using DAL.Planos;

namespace BLL.Planos {
	public interface IPlanoContratacaoBL {

        PlanoContratacao carregar(int id);
		IQueryable<PlanoContratacao> listar(string valorBusca, int idPlano = 0, int idStatus = 0);
		bool salvar(PlanoContratacao OPlanoContratacao);
        bool inserir(PlanoContratacao OPlanoContratacao);
		bool atualizar(PlanoContratacao OPlanoContratacao);
		UtilRetorno excluir(int id);
        bool existe(int idPessoa, int idPlano);
        bool atualizarStatus(int id, int status);
	}
}
