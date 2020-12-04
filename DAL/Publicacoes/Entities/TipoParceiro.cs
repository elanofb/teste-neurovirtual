using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Organizacoes;

namespace DAL.Publicacoes {
	/**
	*
	*/

	public partial class TipoParceiro {

	    public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public string descricao { get; set; }

	    public DateTime dtCadastro { get; set; }

	    public DateTime? dtAlteracao { get; set; }

	    public int idUsuarioCadastro { get; set; }

	    public int? idUsuarioAlteracao { get; set; }

	    public bool? ativo { get; set; }

	    public bool? flagExcluido { get; set; }

    }

	/**
	*
	*/

	internal sealed class TipoParceiroMapper : EntityTypeConfiguration<TipoParceiro> {

		public TipoParceiroMapper() {
			this.ToTable("tb_tipo_parceiro");
			this.HasKey(o => o.id);

		    this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);
        }
	}
}