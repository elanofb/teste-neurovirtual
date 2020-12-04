using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Permissao {

	[Serializable]
	public class RecursoSistemaVW {

		public int idRecursoGrupo { get; set; }

		public string areaGrupo { get; set; }

		public string controllerGrupo { get; set; }
		
		public string actionGrupo { get; set; }

		public int idRecurso { get; set; }

		public int idRecursoPai { get; set; }

		public string nomeDisplay { get; set; }

		public string areaRecurso { get; set; }

		public string controllerRecurso { get; set; }

		public string actionPadrao { get; set; }

		public string flagRecursoLiberado { get; set; }

		public string areaAcao { get; set; }

		public string controllerAcao { get; set; }

		public string nomeAcao { get; set; }

		public string metodoRequest { get; set; }
	}

	//
	internal sealed class RecursoSistemaVWMapper : EntityTypeConfiguration<RecursoSistemaVW> {

		public RecursoSistemaVWMapper() {
			this.ToTable("vw_recursos_sistema");
			this.HasKey(o => o.idRecurso);
		}
	}
}