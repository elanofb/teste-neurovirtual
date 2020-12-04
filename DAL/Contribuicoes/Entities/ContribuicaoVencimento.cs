using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Contribuicoes {

	public class ContribuicaoVencimento {

        public int id { get; set; }

		public int idContribuicao { get; set; }

		public virtual Contribuicao Contribuicao { get; set; }

        public byte? diaVencimento { get; set; }

        public byte? mesVencimento { get; set; }

        public byte? diaInicioVigencia { get; set; }

        public byte? mesInicioVigencia { get; set; }

        public byte? diaFimVigencia { get; set; }

        public byte? mesFimVigencia { get; set; }

		public DateTime? dtVencimento { get; set; }

        public DateTime? dtInicioVigencia    { get; set; }

        public DateTime? dtFimVigencia { get; set; }

		public int idUsuarioCadastro { get; set; }

        public DateTime? dtCadastro { get; set; }

		public int? idUsuarioAlteracao { get; set; }

        public DateTime? dtAlteracao { get; set; }

		public int? idUsuarioExclusao { get; set; }

        public DateTime? dtExclusao { get; set; }

        public string motivoExclusao { get; set; }
	}

	internal sealed class ContribuicaoVencimentoMapper : EntityTypeConfiguration<ContribuicaoVencimento> {

		public ContribuicaoVencimentoMapper() {

			this.ToTable("tb_contribuicao_vencimento");

            this.HasKey(x => x.id);

            this.HasRequired(x => x.Contribuicao).WithMany( c => c.listaContribuicaoVencimento).HasForeignKey(x => x.idContribuicao);

		    this.Ignore(x => x.dtVencimento);

            this.Ignore(x => x.dtInicioVigencia);

            this.Ignore(x => x.dtFimVigencia);

		}
	}
}