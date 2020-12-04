using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using DAL.ContasBancarias;
using DAL.CuponsDesconto;
using DAL.Localizacao;
using DAL.Organizacoes;
using DAL.Permissao;

namespace DAL.Financeiro {

	public class TituloReceitaPagamento {
		
		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public virtual Organizacao Organizacao { get; set; }
		
        public int? idUnidade { get; set; }

		public int idTituloReceita { get; set; }
		
		public virtual TituloReceita TituloReceita { get; set; }

		public byte? idMeioPagamento { get; set; }

		public virtual MeioPagamento MeioPagamento { get; set; }

        public byte? idFormaPagamento { get; set; }

		public virtual FormaPagamento FormaPagamento { get; set; }

        public byte? idGatewayPagamento { get; set; }

		public virtual GatewayPagamento GatewayPagamento { get; set; }

		public byte? idStatusPagamento { get; set;}

		public virtual StatusPagamento StatusPagamento { get; set; }

        public bool? flagParcelamento { get; set; }
		
		public string descricaoParcela { get; set; }

		public byte? qtdeParcelas { get; set; }
		
		public byte? nroParcela { get; set; }
		
		public int? idParcelaPrincipal { get; set; }
		
		public string tokenTransacao { get; set; }

		public string codigoAutorizacao { get; set; }

        public string tid { get; set; }

        public int? idCentroCusto { get; set; }

        public virtual CentroCusto CentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public virtual MacroConta MacroConta { get; set; }

        public int? idCategoria { get; set; }

        public virtual CategoriaTitulo Categoria { get; set; }

        public int? idContaBancaria { get; set; }

        public virtual ContaBancaria ContaBancaria { get; set; }

        public string nroBanco { get; set; }
		
		public string nroAgencia { get; set; }

        public string nroDigitoAgencia { get; set; }
		
		public string nroConta { get; set; }
		
		public string nroDigitoConta { get; set; }

        public string nroOperacao { get; set; }
        
		public string nroCarteira { get; set; }
		
		public string nroCedente { get; set; }
		
		public string nroConvenio { get; set; }
		
		public string nroDocumento { get; set; }
		
		public string nossoNumero { get; set; }

        public string dacNossoNumero { get; set; }

        public int? idCupomDesconto { get; set; }

        public virtual CupomDesconto CupomDesconto { get; set; }

        public decimal? valorDescontoCupom{ get; set; }

        public int? idDescontoAntecipacao { get; set; }

        public decimal? valorDescontoAntecipacao{ get; set; }

        public decimal? valorDesconto{ get; set; }

        public string motivoDesconto{ get; set; }

		public decimal valorTarifasBancarias { get; set; }

		public decimal valorTarifasTransacao { get; set; }

		public decimal valorOutrasTarifas { get; set; }

		public decimal valorOriginal { get; set; }
		
		public string boletoUrl { get; set; }

		public string boletoCodigoBarras { get; set; }

        public bool? flagGerarRemessa { get; set; }

        public int? idArquivoRemessa { get; set; }

		public DateTime? dtVencimentoOriginal { get; set; }

		public DateTime? dtVencimento { get; set; }

		public DateTime? dtCompetencia { get; set; }

		public DateTime? dtPrevisaoPagamento { get; set; }

		public DateTime? dtPagamento { get; set; }

		public decimal? valorRecebido { get; set; }

		public decimal? valorJuros { get; set; }

		public decimal? percentualJuros { get; set; }
		
		public DateTime? dtCredito { get; set; }

        public DateTime? dtPrevisaoCredito { get; set; }

        public short anoCompetencia { get; set; }

        public byte? mesCompetencia { get; set; }

		public DateTime? dtBaixa { get; set; }

		public int? idUsuarioBaixa { get; set; }

	    public virtual UsuarioSistema UsuarioBaixa { get; set; }

        public bool? flagBaixaAutomatica { get; set; }

		public int? idCheckoutCompra { get; set; }

        public DateTime? dtFinalizacaoCheckout { get; set; }

        public string nomeRecibo { get; set; }

        public string documentoRecibo { get; set; }

        public string logradouroRecibo { get; set; }

        public string numeroRecibo { get; set; }

        public string complementoRecibo { get; set; }

        public string bairroRecibo { get; set; }

        public string cepRecibo { get; set; }

        public int? idEstadoRecibo { get; set; }

        public Estado EstadoRecibo { get; set; }

        public int? idCidadeRecibo { get; set; }

        public virtual Cidade CidadeRecibo { get; set; }

        public string nomeCidadeRecibo { get; set; }

        public string email { get; set; }

        public string telPrincipal { get; set; }

        public string telSecundario { get; set; }

		public DateTime dtCadastro { get; set; }
        
		public int? idUsuarioCadastro { get; set; }

		public UsuarioSistema UsuarioCadastro { get; set; }
		
		public DateTime? dtExclusao { get; set; }
		
		public int? idUsuarioExclusao { get; set; }

        public virtual UsuarioSistema UsuarioExclusao { get; set; }
		
		public string motivoExclusao { get; set; }

		public int? idConciliacao { get; set; }		

		public virtual ConciliacaoFinanceira Conciliacao { get; set; }

        public DateTime? dtConciliacao { get; set; }

	    public int? idUsuarioConciliacao { get; set; }

        public DateTime? dtAlteracao { get; set; }
		
		public int? idUsuarioAlteracao { get; set; }

		public bool ativo { get; set; }
	}

	//
	internal sealed class TituloReceitaPagamentoMapper : EntityTypeConfiguration<TituloReceitaPagamento> {

		public TituloReceitaPagamentoMapper() {

			this.ToTable("tb_titulo_receita_pagamento");
			
			this.HasKey(o => o.id);

		    this.Property(o => o.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			this.HasRequired(o => o.TituloReceita).WithMany(x => x.listaTituloReceitaPagamento).HasForeignKey(o => o.idTituloReceita);

			this.HasRequired(o => o.Organizacao).WithMany().HasForeignKey(o => o.idOrganizacao);
			
			this.HasOptional(o => o.MeioPagamento).WithMany().HasForeignKey(o => o.idMeioPagamento);

			this.HasOptional(o => o.FormaPagamento).WithMany().HasForeignKey(o => o.idFormaPagamento);

            this.HasOptional(o => o.GatewayPagamento).WithMany().HasForeignKey(o => o.idGatewayPagamento);

            this.HasOptional(o => o.StatusPagamento).WithMany().HasForeignKey(o => o.idStatusPagamento);

            this.HasOptional(o => o.ContaBancaria).WithMany().HasForeignKey(o => o.idContaBancaria);

		    this.HasOptional(o => o.CupomDesconto).WithMany().HasForeignKey(o => o.idCupomDesconto);

            this.HasOptional(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);

            this.HasOptional(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);

            this.HasOptional(o => o.Categoria).WithMany().HasForeignKey(o => o.idCategoria);

            this.HasOptional(o => o.CidadeRecibo).WithMany().HasForeignKey(o => o.idCidadeRecibo);
			
            this.HasOptional(o => o.EstadoRecibo).WithMany().HasForeignKey(o => o.idEstadoRecibo);

            this.HasOptional(o => o.UsuarioExclusao).WithMany().HasForeignKey(o => o.idUsuarioExclusao);

            this.HasOptional(o => o.UsuarioBaixa).WithMany().HasForeignKey(o => o.idUsuarioBaixa);

			this.HasOptional(o => o.Conciliacao).WithMany().HasForeignKey(o => o.idConciliacao);
			
			this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);

        }
	}
}
