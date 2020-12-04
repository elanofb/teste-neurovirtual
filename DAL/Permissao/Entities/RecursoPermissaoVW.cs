using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Permissao {

	[Serializable]
	public class RecursoPermissaoVW {

        public int? idOrganizacao { get; set; }

		public int idPerfilAcesso { get; set; }

		public string descricaoPerfilAcesso { get; set; }

		public int idPermissao { get; set; }

		public int idRecursoGrupo { get; set; }

		public string descricaoGrupo { get; set; }

		public int idRecurso { get; set; }

		public string areaRecurso { get; set; }

		public string controllerRecurso { get; set; }

		public string actionPadraoRecurso { get; set; }

		public string nomeDisplay { get; set; }

		public int idRecursoAcao { get; set; }

		public string areaAcao { get; set; }

		public string controllerAcao { get; set; }

		public string nomeAcao { get; set; }

		public string metodoRequest { get; set; }
	}

	//
	internal sealed class RecursoPermissaoVWMapper : EntityTypeConfiguration<RecursoPermissaoVW> {

		public RecursoPermissaoVWMapper() {
			this.ToTable("vw_recursos_permissao");
			this.HasKey(o => o.idPermissao);
		}
	}
}