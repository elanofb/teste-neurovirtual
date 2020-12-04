using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.PedidosTemp;
using BLL.Pessoas;
using BLL.Services;
using DAL.Pedidos;
using DAL.PedidosTemp;
using DAL.Pessoas;

namespace BLL.Pedidos {

    public class PedidoCadastroBL : DefaultBL, IPedidoCadastroBL {
        
		//Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoTempOperacaoBL _IPedidoTempOperacaoBL;
        private IPessoaBL _IPessoaBL;

		//Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IPedidoTempOperacaoBL OPedidoTempOperacaoBL => _IPedidoTempOperacaoBL = _IPedidoTempOperacaoBL ?? new PedidoTempOperacaoBL();
        private IPessoaBL OPessoaBL => _IPessoaBL = _IPessoaBL ?? new PessoaBL();
        
		//
		public PedidoCadastroBL() {
            
        }

		//
		public Pedido salvar(PedidoTemp OPedidoTemp) {

		    var OPedido = OPedidoTemp.ToJsonObject<Pedido>();
			
		    OPedido.id = 0;

			OPedido.dtVencimento = DateTime.Today;
			
			OPedido.dtFaturamento = DateTime.Today;

		    OPedido.Pessoa = this.OPessoaBL.query(1)
											.Where(x => x.id == OPedidoTemp.idPessoa)
											.Select(x => new {
																x.id, 
																x.nome,
																x.nroDocumento,
																x.emailPrincipal,
																x.emailSecundario,
																x.nroTelPrincipal,
																x.nroTelSecundario,
																listaEmails = x.listaEmails.Select(e => new {e.id, e.email, e.idTipoEmail, e.dtExclusao}).ToList(),
																listaTelefones = x.listaTelefones.Select(t => new {t.id, t.nroTelefone, t.dtExclusao}).ToList()
											}).FirstOrDefault()
											.ToJsonObject<Pessoa>() ?? new Pessoa();

		    OPedido.listaProdutos = OPedidoTemp.listaProdutos.ToListJsonObject<PedidoProduto>();
			
		    OPedido.listaProdutos.ForEach(x => {  x.id = 0; });

		    OPedido.valorProdutos = OPedido.listaProdutos.Sum(x => x.getValorTotalItem());

		    var OPedidoEntrega = OPedidoTemp.ToJsonObject<PedidoEntrega>();

            if (!OPedidoEntrega.cep.isEmpty()) {
                OPedidoEntrega.id = 0;
                OPedido.listaPedidoEntrega = new List<PedidoEntrega> { OPedidoEntrega };
            }

		    var flagSucesso = this.OPedidoBL.salvar(OPedido);

            if (flagSucesso) {

                this.OPedidoTempOperacaoBL.limpar(HttpContext.Current.Session.SessionID);
            }

            return OPedido;

        }
        
	}

}