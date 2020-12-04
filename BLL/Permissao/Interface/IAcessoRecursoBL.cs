using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Permissao;

namespace BLL.Permissao {
	
	public interface IAcessoRecursoBL {
	
		AcessoRecurso carregar(int id);
		IQueryable<AcessoRecurso> listar(int idRecursoGrupo, int idRecursoPai, string ativo);
		bool salvar(AcessoRecurso OAcessoRecurso);
		void reordenarRecurso(int idRecurso, int idRecursoPai, int idRecursoGrupo, int ordemExibicao);
		UtilRetorno excluir(int id);
	}
}
