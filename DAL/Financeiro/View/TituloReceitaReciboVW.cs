using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.Financeiro {

	//
	public class TituloReceitaReciboVW {

        public int id { get; set; }

        public string nomeRecibo{ get; set; }
		
        public string documentoRecibo{ get; set; }

        public string descricao{ get; set; }

        public decimal? valorTotal{ get; set; }

        public decimal? valorDesconto { get; set; }

        public decimal? valorTotalJuros { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public string organizacaoDddTelPrincipal{ get; set; }
        public string organizacaoDddTelSecundario{ get; set; }
        public string organizacaoNroTelPrincipal{ get; set; }
        public string organizacaoNroTelSecundario{ get; set; }

        public string organizacaoCep{ get; set; }
        public string organizacaoLogradouro{ get; set; }
        public string organizacaoNumero{ get; set; }
        public string organizacaoComplemento{ get; set; }
        public string organizacaoBairro{ get; set; }
        public string organizacaoNomeCidade{ get; set; }
        public string organizacaoSiglaEstado{ get; set; }
    }

	//
	internal sealed class TituloReceitaReciboVWMapper : EntityTypeConfiguration<TituloReceitaReciboVW> {

		public TituloReceitaReciboVWMapper() {
			
            this.ToTable("vw_titulo_receita_recibo");

			this.HasKey(o => o.id);

		}
	}
}