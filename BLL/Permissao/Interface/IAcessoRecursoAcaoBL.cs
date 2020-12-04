using System;
using System.Linq;
using DAL.Permissao;

namespace BLL.Permissao {
	
	public interface IAcessoRecursoAcaoBL {
	
		AcessoRecursoAcao carregar(int id);
		IQueryable<AcessoRecursoAcao> listar(int idRecursoGrupo, int idRecurso, string ativo);
		UtilRetorno validar(AcessoRecursoAcao OAcessoRecursoAcao);
		bool salvar(AcessoRecursoAcao OAcessoRecursoAcao);
		UtilRetorno excluir(int id);
	}
}
