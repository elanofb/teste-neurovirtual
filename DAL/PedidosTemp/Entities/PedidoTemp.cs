using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using DAL.CuponsDesconto;
using DAL.Localizacao;

namespace DAL.PedidosTemp {

	//
	public class PedidoTemp {

		public int id { get; set; }

	    public int? idOrganizacao { get; set; }

	    public int? idUnidade { get; set; }

	    public string idSessao { get; set; }

		public int? idAssociado { get; set; }
		
		public int? idNaoAssociado { get; set; }
		
	    public int? idPessoa { get; set; }

	    public int? idCupomDesconto { get; set; }

	    public virtual CupomDesconto CupomDesconto { get; set; }

	    public decimal valorProdutos { get; set; }

	    public decimal valorFrete { get; set; }

	    public decimal valorDesconto { get; set; }

	    public string idPais { get; set; }
        
	    public int? idEstado { get; set; }

	    public virtual Estado Estado { get; set; }
        
	    public int? idCidade { get; set; }

	    public virtual Cidade Cidade { get; set; }
        
	    public byte? idTipoEndereco { get; set; }
        
	    public string nomeCidade { get; set; }

	    public string cepOrigem { get; set; }

	    public string cep { get; set; }

	    public string logradouro { get; set; }

	    public string numero { get; set; }

	    public string complemento { get; set; }

	    public string bairro { get; set; }

        public DateTime? dtAgendamentoEntrega { get; set; }

        public string flagPeriodo { get; set; }

        public string nroRastreamentoEntrega { get; set; }

	    public int? idTipoFrete { get; set; }

	    public int? idTransportador { get; set; }

	    public bool flagFaturamentoCadastro { get; set; }

	    public int? idCentroCusto { get; set; }
        
	    public int? idMacroConta { get; set; }
        
	    public int? idCategoriaTitulo { get; set; }
        
	    public int? idContaBancaria { get; set; }
        
	    public string codigoContabil { get; set; }

	    public DateTime? dtVencimento { get; set; }

	    public DateTime? dtFaturamento { get; set; }

	    public bool? flagCartaoCreditoPermitido { get; set; }

	    public bool? flagBoletoBancarioPermitido { get; set; }

	    public bool? flagDepositoPermitido { get; set; }

	    public bool? flagPagamentoNaEntrega { get; set; }

		public DateTime dtCadastro { get; set; }

	    public int idUsuarioCadastro { get; set; }
        
	    public virtual List<PedidoProdutoTemp> listaProdutos { get; set; }

		//Construtor
		public PedidoTemp() {
			
		}
        
	}

	//
	internal sealed class PedidoTempMapper : EntityTypeConfiguration<PedidoTemp> {

		public PedidoTempMapper() {
			
			this.ToTable("temptb_pedido");
			
			this.HasKey(o => o.id);
		
		    this.HasOptional(x => x.CupomDesconto).WithMany().HasForeignKey(x => x.idCupomDesconto);
            	
		    this.HasOptional(o => o.Estado).WithMany().HasForeignKey(o => o.idEstado);

		    this.HasOptional(o => o.Cidade).WithMany().HasForeignKey(o => o.idCidade);

		}

	}

}