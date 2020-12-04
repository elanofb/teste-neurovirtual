using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.SegmentosAtuacao {

	public class SegmentoAtuacao {
        
		public int id { get; set; }

        public bool flagPessoaFisica { get; set; }

        public bool flagPessoaJuridica { get; set; }

		public string descricao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime? dtAlteracao { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

		public bool? ativo { get; set; }
		
		public DateTime? dtExclusao { get; set; }

		public int? idUsuarioExclusao { get; set; }

	}

	internal sealed class SegmentoAtuacaoMapper : EntityTypeConfiguration<SegmentoAtuacao> {

		public SegmentoAtuacaoMapper() {
			this.ToTable("tb_segmento_atuacao");
			this.HasKey(o => o.id);
		}
	}
}