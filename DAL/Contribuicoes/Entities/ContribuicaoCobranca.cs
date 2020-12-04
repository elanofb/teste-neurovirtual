using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;

namespace DAL.Contribuicoes {

	public class ContribuicaoCobranca {

        public int id { get; set; }

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

		public bool? flagSomenteVencidos { get; set; }

		public string mensagemCobranca { get; set; }

		public DateTime dtCobranca { get; set; }

        public int? idUsuarioCobranca { get; set; }

		public virtual UsuarioSistema UsuarioCobranca { get; set; }

        //
	    public ContribuicaoCobranca() {

	    }
	}

	internal sealed class ContribuicaoCobrancaMapper : EntityTypeConfiguration<ContribuicaoCobranca> {

		public ContribuicaoCobrancaMapper() {

            this.ToTable("tb_contribuicao_cobranca");

            this.HasKey(x => x.id);

            this.HasOptional(x => x.UsuarioCobranca).WithMany().HasForeignKey(x => x.idUsuarioCobranca);

		}
	}
}