using System;
using System.Data.Entity.ModelConfiguration;
using DAL.Localizacao;
using System.Collections.Generic;
using System.Linq;
using DAL.ContasBancarias;
using DAL.DadosBancarios;
using DAL.Permissao;
using DAL.Pessoas;

namespace DAL.Financeiro {

	public class TituloDespesa {

		public int id { get; set; }

        public int idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

		public int? idTituloDespesaOrigem { get; set; }
		public TituloDespesa TituloDespesaOrigem { get; set; }

		public string descricao { get; set; }

	    public int? idTipoDespesa { get; set; }
		public TipoDespesa TipoDespesa { get; set; }
		
        public int? idDespesa { get; set; }

        public int? idPessoa { get; set; }
        public virtual Pessoa Pessoa { get; set; }

        public string flagCategoriaPessoa { get; set; }

        public int? idCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        public int? idMacroConta { get; set; }
        public virtual MacroConta MacroConta { get; set; }

        public int? idCategoria { get; set; }
		public virtual CategoriaTitulo Categoria { get; set; }

        public int? idContaBancaria { get; set; }
		public virtual ContaBancaria ContaBancaria { get; set; }

        public int? idPeriodoRepeticao { get; set; }
		public virtual PeriodoRepeticao PeriodoRepeticao { get; set; }

        public int? qtdeRepeticao { get; set; }

        public int? idAgrupador { get; set; }
		
		public int?                 idModoPagamento      { get; set; }
		public ModoPagamentoDespesa ModoPagamentoDespesa { get; set; }

		public int?         idContaBancariaFavorecida { get; set; }
		public DadoBancario ContaBancariaFavorecida   { get; set; }
		
        public string nomePessoaCredor { get; set; }

		public string documentoPessoaCredor { get; set; }

		public string nroTelPrincipalCredor { get; set; }
		
		public string nroTelSecundarioCredor { get; set; }

		public string emailPrincipalCredor { get; set; }
		
		public string nomePagador { get; set; }
		
		public string nroDocumentoPagador { get; set; }

        public short? anoCompetencia { get; set; }

        public byte? mesCompetencia { get; set; }

        public DateTime? dtDespesa { get; set; }

        public DateTime? dtVencimento { get; set; }
		
        public DateTime? dtPrevisaoPagamento { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public decimal? valorTotal { get; set; }

        public decimal? valorJuros { get; set; }
		
        public decimal? valorMulta { get; set; }
		
        public decimal? valorDesconto { get; set; }
        
		public string observacao { get; set; }

        public string flagFixa { get; set; }

        public int? nroNotaFiscal { get; set; }

        public int? nroContabil { get; set; }

		public string nroDocumento { get; set; }

        public string nroContrato { get; set; }

		public string codigoBoleto { get; set; }
		
        public DateTime dtCadastro { get; set; }

        public int idUsuarioCadastro { get; set; }

        public virtual UsuarioSistema UsuarioCadastro { get; set; }

        public DateTime? dtAlteracao { get; set; }

        public int? idUsuarioAlteracao { get; set; }
        
        public string motivoExclusao { get; set; }

        public DateTime? dtExclusao { get; set; }

        public int? idUsuarioExclusao { get; set; }

        public virtual UsuarioSistema UsuarioExclusao { get; set; }

        public virtual List<TituloDespesaPagamento> listaTituloDespesaPagamento { get; set; }

        //Constructor
        public TituloDespesa(){
			this.listaTituloDespesaPagamento = new List<TituloDespesaPagamento>();
        }
		
		/// <summary>
		/// Retornar a lista de pagamentos removendo os registros inválidos
		/// </summary>
		/// <returns></returns>
		public List<TituloDespesaPagamento> retornarListaPagamentos() {

			var lista = this.listaTituloDespesaPagamento.Where(x => x.dtExclusao == null).ToList();

			return lista;
		}
	}

	//
	internal sealed class TituloDespesaMapper : EntityTypeConfiguration<TituloDespesa> {

		public TituloDespesaMapper() {
			
            this.ToTable("tb_titulo_despesa");
			
			this.HasKey(o => o.id);

			this.Property(x => x.valorTotal).HasPrecision(10, 2);
			
			this.Property(x => x.valorJuros).HasPrecision(10, 2);

			this.HasRequired(o => o.UsuarioCadastro).WithMany().HasForeignKey(o => o.idUsuarioCadastro);
			
			this.HasOptional(o => o.TituloDespesaOrigem).WithMany().HasForeignKey(o => o.idTituloDespesaOrigem);
            this.HasOptional(o => o.Pessoa).WithMany().HasForeignKey(o => o.idPessoa);
            this.HasOptional(o => o.CentroCusto).WithMany().HasForeignKey(o => o.idCentroCusto);
            this.HasOptional(o => o.MacroConta).WithMany().HasForeignKey(o => o.idMacroConta);
            this.HasOptional(o => o.Categoria).WithMany().HasForeignKey(o => o.idCategoria);
			this.HasOptional(o => o.PeriodoRepeticao).WithMany().HasForeignKey(o => o.idPeriodoRepeticao);
			this.HasOptional(o => o.ContaBancaria).WithMany().HasForeignKey(o => o.idContaBancaria);
			this.HasOptional(o => o.UsuarioExclusao).WithMany().HasForeignKey(o => o.idUsuarioExclusao);
			this.HasOptional(o => o.TipoDespesa).WithMany().HasForeignKey(o => o.idTipoDespesa);
			this.HasOptional(o => o.ModoPagamentoDespesa).WithMany().HasForeignKey(o => o.idModoPagamento);
			this.HasOptional(o => o.ContaBancariaFavorecida).WithMany().HasForeignKey(o => o.idContaBancariaFavorecida);

			this.Ignore(x => x.nomePagador);
			
			this.Ignore(x => x.nroDocumentoPagador);
		}
	}
}