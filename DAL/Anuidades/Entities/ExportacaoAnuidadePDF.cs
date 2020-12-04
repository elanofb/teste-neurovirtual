using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Anuidades {

	//
	public class ExportacaoAnuidadePDF {

		public int id { get; set; }

		public DateTime dtVencimento { get; set; }

		public string flagSomenteNaoPagos { get; set; }

		public int idAnuidade { get; set; }

		public virtual Anuidade Anuidade { get; set; }

		public int idTarefaProcessamento { get; set; }

		public int idUsuarioCadastro { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public DateTime dtCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public string flagExcluido { get; set; }
	}

	//
	internal sealed class ExportacaoAnuidadePDFMapper : EntityTypeConfiguration<ExportacaoAnuidadePDF> {

		public ExportacaoAnuidadePDFMapper() {
			this.ToTable("tb_exportacao_anuidade_pdf");
			this.HasKey(o => o.id);

			this.HasRequired(x => x.Anuidade).WithMany().HasForeignKey(x => x.idAnuidade);
		}
	}
}