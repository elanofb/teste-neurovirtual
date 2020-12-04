using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Arquivos;
using System;
using DAL.Organizacoes;

namespace DAL.Institucionais {

    //
	public class Convenio {

        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int idTipoConvenio { get; set; }

		public virtual TipoConvenio TipoConvenio { get; set; }

		public string titulo { get; set; }

		public string chaveUrl { get; set; }

		public string chamada { get; set; }

		public virtual ArquivoUpload Foto { get; set;}

		public Convenio() { 
		}
	}

	//
	internal sealed class ConvenioMapper : EntityTypeConfiguration<Convenio> {

		public ConvenioMapper() {

			this.ToTable("tb_convenio");

			this.HasKey(x => x.id);

			this.Ignore(x => x.Foto);

			this.HasRequired(o => o.TipoConvenio).WithMany().HasForeignKey(o => o.idTipoConvenio);

            this.HasOptional(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
        }
	}
}