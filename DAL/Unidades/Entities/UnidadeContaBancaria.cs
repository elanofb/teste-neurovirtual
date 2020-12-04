using System.Data.Entity.ModelConfiguration;
using DAL.Bancos;
using DAL.Entities;
using DAL.Financeiro;

namespace DAL.Unidades {

	//
	public class UnidadeContaBancaria : DefaultEntity {

        public int idUnidade { get; set; }
        public virtual Unidade Unidade { get; set; }

        public int? idBanco { get; set; }
        public virtual Banco Banco { get; set; }

        public string nroAgencia { get; set; }
        public string nroConta { get; set; }

        public string nomeTitular { get; set; }
        public string nroDocumentoTitular { get; set; }

	}

	//
	internal sealed class UnidadeContaBancariaMapper : EntityTypeConfiguration<UnidadeContaBancaria> {

		public UnidadeContaBancariaMapper() {
			this.ToTable("tb_unidade_conta_bancaria");
			this.HasKey(o => o.id);
            this.HasRequired(x => x.Unidade).WithMany().HasForeignKey(x => x.idUnidade);
            this.HasOptional(x => x.Banco).WithMany().HasForeignKey(x => x.idBanco);
		}
	}
}