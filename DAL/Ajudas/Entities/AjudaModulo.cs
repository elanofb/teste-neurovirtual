using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;

namespace DAL.Ajudas {

	//
	public class AjudaModulo {

	    public int id { get; set; }

	    public int? idCategoriaAjuda { get; set; }

        public virtual AjudaCategoria AjudaCategoria { get; set; }

        public string titulo { get; set; }

	    public string chamada { get; set; }

	    public string descricao { get; set; }

	    public string embedVideo { get; set; }

	    public int? ordem { get; set; }

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

	internal sealed class AjudaModuloMapper : EntityTypeConfiguration<AjudaModulo> {

		public AjudaModuloMapper() {
			this.ToTable("datatb_ajuda_modulo");
			this.HasKey(o => o.id);

		    this.HasOptional(x => x.AjudaCategoria).WithMany().HasForeignKey(x => x.idCategoriaAjuda);
		}
	}
}