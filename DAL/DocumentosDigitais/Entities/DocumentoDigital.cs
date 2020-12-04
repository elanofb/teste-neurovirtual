using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using System;
using DAL.Organizacoes;

namespace DAL.DocumentosDigitais {

	public class DocumentoDigital {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }

        public int idTipoDocumentoDigital { get; set; }

        public virtual TipoDocumentoDigital TipoDocumentoDigital { get; set; }

        public string titulo { get; set; }

        public string flagTipoPessoa { get; set; }

        public string htmlCorpo { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

    }

	internal sealed class DocumentoDigitalMapper : EntityTypeConfiguration<DocumentoDigital> {

		public DocumentoDigitalMapper() {

			this.ToTable("tb_documento_digital");

            this.HasKey(x => x.id);

            this.HasOptional(x => x.Organizacao).WithMany().HasForeignKey(x => x.idOrganizacao);

            this.HasRequired(x => x.TipoDocumentoDigital).WithMany().HasForeignKey(x => x.idTipoDocumentoDigital);

		}
	}
}