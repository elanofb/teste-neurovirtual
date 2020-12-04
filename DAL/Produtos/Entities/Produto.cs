using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Entities;
using DAL.Fornecedores;
using DAL.Arquivos;

namespace DAL.Produtos {

    //
    public class Produto {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int idTipoProduto { get; set; }

        public virtual TipoProduto TipoProduto { get; set; }

        public int? idFornecedor { get; set; }

        public Fornecedor OFornecedor { get; set; }

        public string nome { get; set; }

        public string descricaoResumida { get; set; }

        public string descricaoCompleta { get; set; }

        public decimal valor { get; set; }

        public decimal peso { get; set; }

        public decimal? largura { get; set; }

        public decimal? altura { get; set; }

        public decimal? comprimento { get; set; }

        public decimal? icms { get; set; }

        public decimal? pis { get; set; }

        public decimal? cofins { get; set; }

        public decimal? iss { get; set; }

        public string codigoContabil { get; set; }

        public decimal? percentualDescontoAssociado { get; set; }

        public decimal? valorDescontoAssociado { get; set; }
        
        public int? qtdeDiasDuracao { get; set; }
        
        public decimal? qtdePontosPlanoCarreira { get; set; }
        
        public bool? flagRedeAfiliados { get; set; }
        
        public decimal? qtdeMaximoBinario { get; set; }
        
        public decimal? valorGanhoDiario { get; set; }
        
        public bool? flagPlanoRecomendado { get; set; }
        
        public bool? flagCalcularFrete { get; set; }
        
        public bool? flagFreteGratis { get; set; }

        public bool? flagInserirImpostos { get; set; }

        public bool? flagOnline { get; set; }

        public bool? flagParaTodos { get; set; }

        public bool? flagSomenteAssociados { get; set; }

        public bool? flagSomenteAssociadosQuites { get; set; }

        public bool? flagCortesia { get; set; }

        public bool? flagControleEstoque { get; set; }

        public bool? flagValorConfiguravel { get; set; }

        public int qtde { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public int idUsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public bool? ativo { get; set; }

        public bool? flagExcluido { get; set; }

        public virtual List<ArquivoUpload> listaFotos { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
		public Produto() {

            this.listaFotos = new List<ArquivoUpload>();

        }

        /// <summary>
        /// Flag que identifica se há descontos configurados no produto ou nao
        /// </summary>
        public bool flagDesconto() {

            if (valorDescontoAssociado.toDecimal() == 0 && percentualDescontoAssociado.toDecimal() == 0) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Valor com desconto
        /// </summary>
        public decimal valorComDesconto() {

            if (percentualDescontoAssociado.toDecimal() == 0 && valorDescontoAssociado.toDecimal() == 0) {
                return this.valor;
            }

            if (valorDescontoAssociado.toDecimal() > 0) {

                decimal valorComDesconto = decimal.Subtract(valor, valorDescontoAssociado.toDecimal());

                return valorComDesconto;
            }



            decimal valorDesconto = valor.valorPercentual(this.percentualDescontoAssociado.toDecimal());

            decimal valorComDescontoPercentual = decimal.Subtract(valor, valorDesconto);

            return valorComDescontoPercentual;

        }
    }

    //
    internal sealed class ProdutoMapper : EntityTypeConfiguration<Produto> {

        public ProdutoMapper() {

            this.ToTable("tb_produto");

            this.HasKey(x => x.id);

            this.Ignore(x => x.listaFotos);

            this.HasRequired(x => x.TipoProduto).WithMany().HasForeignKey(x => x.idTipoProduto);

            this.HasOptional(x => x.OFornecedor).WithMany().HasForeignKey(x => x.idFornecedor);

        }
    }
}