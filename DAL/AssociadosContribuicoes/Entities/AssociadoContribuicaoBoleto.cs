using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoBoleto {

		public Int64 id { get; set; }

		public int idAssociadoContribuicao { get; set; }

        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

		public int idAssociado { get; set; }

		public int idContribuicao { get; set; }

		public int idTipoAssociado { get; set; }

		public decimal? valorOriginal { get; set; }

		public decimal? valorAtual { get; set; }

        public DateTime? dtVencimentoOriginal { get; set; }

        public DateTime? dtVencimentoAtual { get; set; }

        public bool? flagIsento { get; set; }

		public int? idTituloReceita { get; set; }

		public int? idTituloReceitaPagamento { get; set; }

        public string descricaoTituloReceita { get; set; }

		public byte? idMeioPagamento { get; set; }

		public byte? idFormaPagamento { get; set; }

		public byte? idGatewayPagamento { get; set; }

        public string descricaoParcela { get; set; }

        public DateTime? dtVencimentoParcela { get; set; }

        public DateTime? dtExclusaoParcela { get; set; }

        public DateTime? dtPagamento { get; set; }

        public decimal? valorOriginalBoleto { get; set; }

        public decimal? valorRecebidoBoleto { get; set; }

        public decimal? valorJurosBoleto { get; set; }

        public string boletoUrl { get; set; }

	}

	//
	internal sealed class AssociadoContribuicaoBoletoMapper : EntityTypeConfiguration<AssociadoContribuicaoBoleto> {

		public AssociadoContribuicaoBoletoMapper() {

			this.ToTable("vw_associado_contribuicao_boleto");

            this.HasKey(o => o.id);

}
	}
}