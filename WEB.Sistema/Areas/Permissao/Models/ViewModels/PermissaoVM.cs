using System.Collections.Generic;
using DAL.Permissao;

namespace WEB.Areas.Permissao.ViewModels {

	public class PermissaoVM {

		public int idPerfilAcesso { get; set; }

		public PerfilAcesso PerfilAcesso { get; set; }

		public List<AcessoPermissao> listaPermissoes { get; set; }

		public List<AcessoRecursoGrupo> listaGrupos { get; set; }

		public List<AcessoRecurso> listaRecursos { get; set; }

		public List<AcessoRecursoAcao> listaRecursoAcao { get; set; }

		//Construtor
		public PermissaoVM() {
			this.PerfilAcesso = new PerfilAcesso();

			this.listaPermissoes = new List<AcessoPermissao>();

			this.listaGrupos = new List<AcessoRecursoGrupo>();

			this.listaRecursos = new List<AcessoRecurso>();

			this.listaRecursoAcao = new List<AcessoRecursoAcao>();
		}
	}

	
}