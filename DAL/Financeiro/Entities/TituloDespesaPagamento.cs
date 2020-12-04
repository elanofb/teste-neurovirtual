using System;
using System.Data.Entity.ModelConfiguration;
using DAL.ContasBancarias;
using DAL.DadosBancarios;
using DAL.Permissao;

namespace DAL.Financeiro {

	//
	public class TituloDespesaPagamento {

		public int id { get; set; }
        public int idOrganizacao { get; set; }
        public int? idUnidade { get; set; }
        public string descParcela { get; set; }

        public int idTituloDespesa { get; set; }
        public virtual TituloDespesa TituloDespesa { get; set; }

        public byte? idMeioPagamento { get; set; }
        public virtual MeioPagamento MeioPagamento { get; set; }

        public byte? idFormaPagamento { get; set; }
        public virtual FormaPagamento FormaPagamento { get; set; }

        public int? idModoPagamento { get; set; }
		public ModoPagamentoDespesa ModoPagamento { get; set; }
		
		public int? idContaBancariaFavorecida { get; set; }
		public DadoBancario ContaBancariaFavorecida { get; set; }

        public byte? idStatusPagamento { get; set;}
        public virtual StatusPagamento StatusPagamento { get; set; }

        public int? idCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        public int? idMacroConta { get; set; }
        public virtual MacroConta MacroConta { get; set; }

        public int? idCategoria { get; set; }
        public virtual CategoriaTitulo Categoria { get; set; }

	    public int? idContaBancaria { get; set; }
	    public virtual ContaBancaria ContaBancaria { get; set; }
		
		public int? idArquivoRemessa { get; set; }

        public decimal valorOriginal { get; set; }
		
		public decimal? valorJuros { get; set; }
        
		public decimal? valorMulta { get; set; }
		
		public decimal? valorDesconto { get; set; }
        
		public decimal valorOutrasTarifas { get; set; }
		
		public decimal? valorPago { get; set; }

        public short? anoCompetencia { get; set; }
        
		public byte? mesCompetencia { get; set; }
        
		public DateTime? dtCompetencia { get; set; }
        
		public DateTime? dtVencimento { get; set; }
		
		public DateTime? dtPrevisaoPagamento { get; set; }
        
		public DateTime? dtDebito { get; set; }
        
		public DateTime? dtPagamento { get; set; }
		
        public string flagPago { get; set; }
		
		public int? idConciliacao { get; set; }
		
		public virtual ConciliacaoFinanceira Conciliacao { get; set; }
		
		
	    public DateTime? dtConciliacao { get; set; }
	    public int? idUsuarioConciliacao { get; set; }
        public bool? flagReembolso { get; set; }
        public string codigoAutorizacao { get; set; }
		
		public string nroBanco { get; set; }
		public string nroAgencia { get; set; }
		public string nroDigitoAgencia { get; set; }
		public string nroConta { get; set; }
		public string nroDigitoConta { get; set; }
		public string nroCarteira { get; set; }
		public string nroDocumento { get; set; }
		public string nossoNumero { get; set; }
		public string dacNossoNumero { get; set; }
		
		public int? nroNotaFiscal { get; set; }
		public string nroContrato { get; set; }
		public string codigoBoleto { get; set; }

        public DateTime? dtBaixa { get; set; }
        public int? idUsuarioBaixa { get; set; }
        public virtual UsuarioSistema UsuarioBaixa { get; set; }

        public DateTime dtCadastro { get; set; }
        public int? idUsuarioCadastro { get; set; }
		public UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }
        public int? idUsuarioAlteracao { get; set; }

        public DateTime? dtExclusao { get; set; }
        public int? idUsuarioExclusao { get; set; }
	    public virtual UsuarioSistema UsuarioExclusao { get; set; }
        public string motivoExclusao { get; set; }
        
		public bool ativo { get; set; }
	}

	//
	internal sealed class TituloDespesaPagamentoMapper : EntityTypeConfiguration<TituloDespesaPagamento> {

		public TituloDespesaPagamentoMapper() {
			this.ToTable("tb_titulo_despesa_pagamento");
			this.HasKey(o => o.id);

			this.HasRequired(o => o.TituloDespesa).WithMany(x => x.listaTituloDespesaPagamento).HasForeignKey(o => o.idTituloDespesa);
            this.HasOptional(o => o.MeioPagamento).WithMany().HasForeignKey(o => o.idMeioPagamento);
            this.HasOptional(o => o.FormaPagamento).WithMany().HasForeignKey(o => o.idFormaPagamento);
			this.HasOptional(o => o.ModoPagamento).WithMany().HasForeignKey(o => o.idModoPagamento);
			this.HasOptional(o => o.StatusPagamento).WithMany().HasForeignKey(o => o.idStatusPagamento);
            this.HasOptional(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);
            this.HasOptional(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);
            this.HasOptional(o => o.Categoria).WithMany().HasForeignKey(o => o.idCategoria);
		    this.HasOptional(o => o.ContaBancaria).WithMany().HasForeignKey(o => o.idContaBancaria);
            this.HasOptional(o => o.UsuarioBaixa).WithMany().HasForeignKey(o => o.idUsuarioBaixa);
			this.HasOptional(o => o.UsuarioExclusao).WithMany().HasForeignKey(o => o.idUsuarioExclusao);
			this.HasOptional(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);
			this.HasOptional(o => o.Conciliacao).WithMany().HasForeignKey(o => o.idConciliacao);
			this.HasOptional(o => o.ContaBancariaFavorecida).WithMany().HasForeignKey(o => o.idContaBancariaFavorecida);
        }
	}
}