using System;
using System.Linq;
using System.Web;
using BLL.Associados;
using BLL.NaoAssociados;
using BLL.PedidosTemp;
using BLL.Produtos;
using DAL.Associados;
using DAL.PedidosTemp;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoCadastroProdutoInclusaoVM {
        
        //Atributos
        private IAssociadoBL _IAssociadoBL;
        private INaoAssociadoBL _NaoAssociadoBL;
        private IProdutoBL _IProdutoBL;
        private IPedidoTempBL _IPedidoTempBL;
        private IPedidoProdutoTempBL _IPedidoProdutoTempBL;

        //Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
        private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();
        private IProdutoBL OProdutoBL => _IProdutoBL = _IProdutoBL ?? new ProdutoBL();
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();
        private IPedidoProdutoTempBL OPedidoProdutoTempBL => _IPedidoProdutoTempBL = _IPedidoProdutoTempBL ?? new PedidoProdutoTempBL();

        private PedidoTemp OPedidoTemp => this.OPedidoTempBL.carregar(HttpContext.Current.Session.SessionID);

        //
        public UtilRetorno adicionarProduto() {

            var ORetorno = UtilRetorno.newInstance(false);

            var idProduto = UtilRequest.getInt32("idProduto");

            var valorProdutoParam = Convert.ToDecimal(UtilRequest.getString("valorProduto"));

            var qtde = UtilRequest.getInt32("qtde");

            var observacoes = UtilRequest.getString("observacoes");

            ORetorno = this.validarDados();

            if (ORetorno.flagError) {
                return ORetorno;
            }
            
            var listaPedidoProduto = this.OPedidoTemp?.listaProdutos;
            
            var OPedidoProdutoTemp = listaPedidoProduto?.FirstOrDefault(x => x.idProduto == idProduto) ?? new PedidoProdutoTemp();
            
            if (OPedidoProdutoTemp.id > 0) {

                OPedidoProdutoTemp.qtde += qtde;
                
                this.OPedidoProdutoTempBL.salvar(OPedidoProdutoTemp);
                
                return ORetorno;

            }

            var OProduto = this.OProdutoBL.carregar(idProduto);

            OPedidoProdutoTemp.idPedido = this.OPedidoTemp.id;

            OPedidoProdutoTemp.idProduto = OProduto.id;

            OPedidoProdutoTemp.nomeProduto = OProduto.nome;

            OPedidoProdutoTemp.valorItem = valorProdutoParam;

            OPedidoProdutoTemp.peso = OProduto.peso;

            OPedidoProdutoTemp.qtde = qtde;

            OPedidoProdutoTemp.observacoes = observacoes;

            OPedidoProdutoTemp.flagCalcularFrete = OProduto.flagCalcularFrete;
            
            OPedidoProdutoTemp.valorGanhoDiario = OProduto.valorGanhoDiario;
            
            OPedidoProdutoTemp.qtdeDiasDuracao = OProduto.qtdeDiasDuracao;
            
            OPedidoProdutoTemp.dtFimGanhoDiario = OProduto.qtdeDiasDuracao > 0 ? DateTime.Now.AddDays(OProduto.qtdeDiasDuracao.toInt()) : (DateTime?) null;
            
            this.OPedidoProdutoTempBL.salvar(OPedidoProdutoTemp);
            
            return ORetorno;

        }

        //
        private UtilRetorno validarDados() {

            var ORetorno = UtilRetorno.newInstance(false);
            
            if (this.OPedidoTemp.idPessoa == 0) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Informe quem é o comprador antes de adicionar algum produto!");
                return ORetorno;
            }

            var OAssociado = this.OAssociadoBL.carregarAssociadoPessoa(this.OPedidoTemp.idPessoa.toInt()) ?? new Associado();

            var ONaoAssociado = this.ONaoAssociadoBL.carregarPorPessoa(this.OPedidoTemp.idPessoa.toInt()) ?? new Associado();

            if (OAssociado.id == 0 && ONaoAssociado.id == 0) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("O comprador informado não foi encontrado!");
                return ORetorno;
            }

            var idProduto = UtilRequest.getInt32("idProduto");

            var OProduto = this.OProdutoBL.carregar(idProduto);

            if (OProduto == null) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Escolha um produto para adicionar.");
                return ORetorno;
            }

            var qtde = UtilRequest.getInt32("qtde");

            if (qtde == 0) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Informe a quantidade do produto.");
                return ORetorno;
            }
            
            if (OProduto.flagSomenteAssociados == true && OAssociado.id == 0) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("O produto não pode ser adicionado pois está configurado somente para associados.");
                return ORetorno;
            }

            return ORetorno;

        }

	}

}