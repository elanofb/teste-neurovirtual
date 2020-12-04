using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Ajudas {

	//
	public class AjudaCategoria {

	    public int id { get; set; }

	    public int? idRecursoGrupo { get; set; }

	    public string descricao { get; set; }

	    public DateTime? dtCadastro { get; set; }

	    public int? idUsuarioCadastro { get; set; }

	    public DateTime? dtAlteracao { get; set; }

	    public int? idUsuarioAlteracao { get; set; }

	    public bool? ativo { get; set; }

	    public bool? flagExcluido { get; set; }

    }

	/**
	*
	*/

	internal sealed class AjudaCategoriaMapper : EntityTypeConfiguration<AjudaCategoria> {

		public AjudaCategoriaMapper() {
			this.ToTable("datatb_ajuda_categoria");
			this.HasKey(o => o.id);
		}
	}
}