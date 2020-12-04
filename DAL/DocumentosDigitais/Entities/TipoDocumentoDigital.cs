using System.Data.Entity.ModelConfiguration;
using System;

namespace DAL.DocumentosDigitais {

	public class TipoDocumentoDigital {

        public int id { get; set; }

        public string descricao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool flagExcluido { get; set; }

    }

	internal sealed class TipoDocumentoDigitalMapper : EntityTypeConfiguration<TipoDocumentoDigital> {

		public TipoDocumentoDigitalMapper() {

            this.ToTable("datatb_tipo_documento_digital");

            this.HasKey(x => x.id);
		}
	}
}