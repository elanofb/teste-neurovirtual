using System;
using System.Linq;
using System.Collections.Generic;
using DAL.Permissao;

namespace BLL.Permissao {

	public interface IAcessoPermissaoBL {

		IQueryable<AcessoPermissao> listar(int idPerfilAcesso, int idRecurso, int idRecursoAcao);

		IQueryable<RecursoPermissaoVW> listarPermissoes(int idPerfilAcesso, int idOrganizacao);

		IQueryable<RecursoSistemaVW> listarRecursos();

		void salvarPermissoesAcoes(int idPerfil, int idRecursoAcao);

		void salvarPermissoesRecursos(int idPerfil,List<AcessoRecurso> listaRecursos);

		UtilRetorno excluir(int id);
		
		UtilRetorno excluir(int idPerfil, int idRecursoAcao);
		
	}
}
