using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.Associados;
using DAL.Financeiro;
using DAL.Pessoas;
using DAL.CuponsDesconto;
using DAL.ContasBancarias;
using DAL.Permissao;

namespace DAL.Pedidos {

    //
    public class Pedido {

        public int id { get; set; }

        public int? idOrganizacao { get; set; }

        public int? idUnidade { get; set; }

        public int? idAssociado { get; set; }

        public Associado Associado { get; set; }
        
        public int? idNaoAssociado { get; set; }
        
        public Associado NaoAssociado { get; set; }
        
        public int? idPessoa { get; set; }

        public virtual Pessoa Pessoa { get; set; }

        public string nomePessoa { get; set; }

        public string cpf { get; set; }

        public string rg { get; set; }

        public string email { get; set; }

        public string telPrincipal { get; set; }

        public string telSecundario { get; set; }

        public int? idCupomDesconto { get; set; }

        public virtual CupomDesconto CupomDesconto { get; set; }

        public decimal valorProdutos { get; set; }

        public decimal? valorFrete { get; set; }

        public bool? flagFreteGratis { get; set; }

        public decimal? valorDesconto { get; set; }

        public decimal? valorDescontoCupom { get; set; }

        public int idStatusPedido { get; set; }

        public virtual StatusPedido StatusPedido { get; set; }

        public virtual List<PedidoProduto> listaProdutos { get; set; }

        public string idSessao { get; set; }

        public DateTime dtCadastro { get; set; }

        public DateTime dtAlteracao { get; set; }

        public DateTime? dtQuitacao { get; set; }

        public DateTime? dtAtendimento { get; set; }

        public DateTime? dtPreparacao { get; set; }

        public DateTime? dtExpedicao { get; set; }

        public DateTime? dtCancelamento { get; set; }

        public DateTime? dtFinalizado { get; set; }

        public bool flagFaturamentoCadastro { get; set; }

        public byte? idMeioPagamento { get; set; }
        
        public MeioPagamento MeioPagamento { get; set; }
        
        public int? idCentroCusto { get; set; }

        public CentroCusto CentroCusto { get; set; }

        public int? idMacroConta { get; set; }

        public MacroConta MacroConta { get; set; }

        public int? idCategoriaTitulo { get; set; }

        public CategoriaTitulo CategoriaTitulo { get; set; }

        public int? idContaBancaria { get; set; }

        public ContaBancaria ContaBancaria { get; set; }

        public string codigoContabil { get; set; }

        public DateTime? dtVencimento { get; set; }

        public DateTime? dtFaturamento { get; set; }

        public byte? qtdeLimiteParcelas { get; set; }

        public bool? flagCartaoCreditoPermitido { get; set; }

        public bool? flagBoletoBancarioPermitido { get; set; }

        public bool? flagDepositoPermitido { get; set; }

        public bool? flagPagamentoNaEntrega { get; set; }
        
        public bool? flagPagamentoBitlink { get; set; }

        public int? idUsuarioCadastro { get; set; }
        
        public UsuarioSistema UsuarioCadastro { get; set; }

        public int? idUsuarioAlteracao { get; set; }

        public string ativo { get; set; }

        public string flagExcluido { get; set; }

        public List<PedidoEntrega> listaPedidoEntrega { get; set; }
        
        public TituloReceita TituloReceita { get; set; }
        
        //Construtor
        public Pedido() {

            this.listaProdutos = new List<PedidoProduto>();

            this.listaPedidoEntrega = new List<PedidoEntrega>();

        }

        //
        public decimal getValorTotal() {

            decimal vlFrete = this.valorFrete ?? 0;
            decimal valorTotal = Decimal.Add(this.valorProdutos, vlFrete);

            valorTotal = Decimal.Subtract(valorTotal, UtilNumber.toDecimal(this.valorDesconto));

            valorTotal = valorTotal > 0 ? valorTotal : 0;

            return valorTotal;
        }
        
    }

    //
    internal sealed class PedidoMapper : EntityTypeConfiguration<Pedido> {

        public PedidoMapper() {

            this.ToTable("tb_pedido");

            this.HasKey(o => o.id);

            this.HasRequired(t => t.StatusPedido).WithMany().HasForeignKey(t => t.idStatusPedido);
            
            this.HasOptional(t => t.Associado).WithMany().HasForeignKey(t => t.idAssociado);
            
            this.HasOptional(t => t.NaoAssociado).WithMany().HasForeignKey(t => t.idNaoAssociado);
            
            this.HasOptional(t => t.Pessoa).WithMany().HasForeignKey(t => t.idPessoa);

            this.HasOptional(t => t.CupomDesconto).WithMany().HasForeignKey(t => t.idCupomDesconto);

            this.HasOptional(x => x.CentroCusto).WithMany().HasForeignKey(x => x.idCentroCusto);

            this.HasOptional(x => x.MacroConta).WithMany().HasForeignKey(x => x.idMacroConta);

            this.HasOptional(x => x.CategoriaTitulo).WithMany().HasForeignKey(x => x.idCategoriaTitulo);

            this.HasOptional(x => x.ContaBancaria).WithMany().HasForeignKey(x => x.idContaBancaria);

            this.HasOptional(x => x.MeioPagamento).WithMany().HasForeignKey(x => x.idMeioPagamento);
            
            this.HasOptional(x => x.UsuarioCadastro).WithMany().HasForeignKey(x => x.idUsuarioCadastro);

            this.Ignore(x => x.TituloReceita);
        }
    }
}