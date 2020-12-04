using System;
using System.Data.Entity.ModelConfiguration;

namespace DAL.AssociadosContribuicoes {

	//
	public class AssociadoContribuicaoResumoVW {

		public int id { get; set; }

		public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int? idAssociadoContribuicaoPrincipal { get; set; }

        public int? idAssociadoEstipulante { get; set; }

		public int idAssociado { get; set; }

        public int idPessoa { get; set; }
        
        public string nomeAssociado { get; set; }

		public int idContribuicao { get; set; }

        public string descricaoContribuicao { get; set; }

        public string descricaoPeriodoContribuicao { get; set; }

		public int idTipoAssociado { get; set; }
		
		public int idTipoAssociadoAtual { get; set; }

        public string descricaoTipoAssociado { get; set; }

		public decimal valorOriginal { get; set; }

		public decimal valorAtual { get; set; }

		public DateTime dtVencimentoOriginal { get; set; }

		public DateTime dtVencimentoAtual { get; set; }

        public DateTime? dtInicioVigencia { get; set; }

        public DateTime? dtFimVigencia { get; set; }

		public DateTime? dtPagamento { get; set; }

		public bool? flagIsento { get; set; }

        public string motivoIsencao { get; set; }

		public DateTime dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

        public string nomeUsuarioCadastro { get; set; }

        public int? idTituloReceita { get; set; }

        public decimal? valorTotalTitulo { get; set; }

        public int? qtdeParcelas { get; set; }

        public decimal? valorTotalRecebido { get; set; }

        public bool? flagDescontoAntecipacao { get; set; }


	    public bool flagPodeParcelar(){

	        return this.dtPagamento == null && qtdeParcelas <= 1;

	    }

	    public bool flagPodeIsentar(){

	        return this.dtPagamento == null && flagIsento != true;

	    }

        /// <summary>
        /// Informa se tem parcelamento na contribuicao
        /// </summary>
        /// <returns></returns>
	    public bool flagTemParcelamento(){

	        return !flagQuitado() && qtdeParcelas > 1;

	    }

        /// <summary>
        /// Informa se a contribuicao está paga ou foi isenta
        /// </summary>
	    public bool flagQuitado(){

	        return dtPagamento.HasValue || flagIsento == true;

	    }

        /// <summary>
        /// Informa se a contribuicao está vencida ou nao
        /// </summary>
	    public bool flagVencido(){

	        return !dtPagamento.HasValue && dtVencimentoAtual < DateTime.Today;

	    }
	}

	//
	internal sealed class AssociadoContribuicaoResumoVWMapper : EntityTypeConfiguration<AssociadoContribuicaoResumoVW> {

		public AssociadoContribuicaoResumoVWMapper() {

            this.ToTable("vw_associado_contribuicao_resumo");

            this.HasKey(o => o.id);

		}
	}
}

