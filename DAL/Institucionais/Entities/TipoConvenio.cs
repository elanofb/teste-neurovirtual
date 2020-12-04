using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;
using DAL.Organizacoes;

namespace DAL.Institucionais {
	
    //
	public partial class TipoConvenio {

        public int id { get; set; }

        public string descricao { get; set; }

        public string chaveUrl { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }
    }

	//
	internal sealed class TipoConvenioMapper : EntityTypeConfiguration<TipoConvenio> {

		public TipoConvenioMapper() {
			this.ToTable("tb_tipo_convenio");
			this.HasKey(o => o.id);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
        }
	}
}