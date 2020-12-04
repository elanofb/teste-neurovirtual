using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Permissao;
using DAL.ContasBancarias;
using DAL.Financeiro;
using DAL.Organizacoes;

namespace DAL.Contribuicoes {

	public class Contribuicao {

		public int id { get; set; }

		public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; } 

		public string descricao { get; set; }

		public byte? mesInicioVigencia { get; set; }

		public short? anoInicioVigencia { get; set; }

        public DateTime? dtInicioVigencia { get; set; }

        public DateTime? dtValidade { get; set; }

		public int? idPeriodoContribuicao { get; set; }

		public virtual PeriodoContribuicao PeriodoContribuicao { get; set; }

		public int? idTipoVencimento { get; set; }

		public virtual TipoVencimento TipoVencimento { get; set; }

		public int? idTipoGeracao { get; set; }

		public virtual TipoGeracaoContribuicao TipoGeracaoContribuicao { get; set; }

        public bool? flagGerarParaTodos { get; set; }

        public bool? flagCobrancaProRata { get; set; }

        public bool? flagGerarCobrancaAutomatica { get; set; }

        public byte? qtdeDiasEnvioCobranca { get; set; }

        public bool? flagEnviarEmail { get; set; }

        public byte? qtdeLimiteParcelas { get; set; }

        public bool? flagCartaoCreditoPermitido { get; set; }

        public bool? flagBoletoBancarioPermitido { get; set; }

        public bool? flagDepositoPermitido { get; set; }

        public bool? flagGerarBoleto { get; set; }

        public int? idCentroCusto { get; set; }

        public virtual CentroCusto CentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public virtual MacroConta MacroConta { get; set; }

        public int? idCategoriaTitulo { get; set; }

        public virtual CategoriaTitulo CategoriaTitulo { get; set; }

        public int? idContaBancaria { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }

        public string codigoContabil { get; set; }

        public bool? flagConfigurarImpostos { get; set; }

        public decimal? icms { get; set; }

		public decimal? pis { get; set; }

		public decimal? cofins { get; set; }

		public decimal? iss { get; set; }
        
        public string emailCobrancaTitulo { get; set; }

        public string emailCobrancaHtml { get; set; }

        public string emailPagamentoTitulo { get; set; }

        public string emailPagamentoHtml { get; set; }

		public DateTime? dtCancelamento { get; set; }

		public int? idUsuarioCancelamento { get; set; }

		public string motivoCancelamento { get; set; }

		public DateTime dtCadastro { get; set; }

		public int? idUsuarioCadastro { get; set; }

		public UsuarioSistema UsuarioCadastro { get; set; }

		public DateTime dtAlteracao { get; set; }

		public int idUsuarioAlteracao { get; set; }

		public string ativo { get; set; }

        public bool flagVigente { get; set; }

		public virtual List<ContribuicaoTabelaPreco> listaTabelaPreco { get; set; }

		public virtual List<ContribuicaoPreco> listaContribuicaoPreco { get; set; }

		public virtual List<ContribuicaoVencimento> listaContribuicaoVencimento { get; set; }

        public List<VigenciaDTO> listaVigencia { get; set; }

		public Contribuicao() {

            this.listaTabelaPreco = new List<ContribuicaoTabelaPreco>();

			this.listaContribuicaoPreco = new List<ContribuicaoPreco>();

            this.listaContribuicaoVencimento = new List<ContribuicaoVencimento>();

		}


	}

	internal sealed class ContribuicaoMapper : EntityTypeConfiguration<Contribuicao> {

		public ContribuicaoMapper() {

			this.ToTable("tb_contribuicao");

            this.HasKey(x => x.id);

            this.HasOptional(x => x.PeriodoContribuicao).WithMany().HasForeignKey(x => x.idPeriodoContribuicao);

			this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.HasOptional(x => x.TipoVencimento).WithMany().HasForeignKey(x => x.idTipoVencimento);

            this.HasOptional(x => x.TipoGeracaoContribuicao).WithMany().HasForeignKey(x => x.idTipoGeracao);

            this.HasOptional(x => x.CentroCusto).WithMany().HasForeignKey(x => x.idCentroCusto);

            this.HasOptional(x => x.MacroConta).WithMany().HasForeignKey(x => x.idMacroConta);

            this.HasOptional(x => x.CategoriaTitulo).WithMany().HasForeignKey(x => x.idCategoriaTitulo);

            this.HasOptional(x => x.ContaBancaria).WithMany().HasForeignKey(x => x.idContaBancaria);
            
            this.HasRequired(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);

            this.Ignore(x => x.listaVigencia);
		}
	}
}