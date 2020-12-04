using System;
using System.Linq;
using BLL.ConfiguracoesEcommerce;
using BLL.Core.Events;
using BLL.Pedidos;
using BLL.Pessoas;
using BLL.Produtos;
using BLL.Services;
using DAL.Compras;
using DAL.Compras.Extensions;
using DAL.Pedidos;
using DAL.Pedidos.Extensions;
using DAL.Pessoas;

namespace BLL.Compras.Services {

    public class GeradorPedidoBL : DefaultBL {

        //Atributos
        private ICarrinhoItemConsultaBL _CarrinhoItemConsultaBL;
        private ICarrinhoResumoBL _CarrinhoResumoBL;
        private IPessoaBL _PessoaBL;
        private IProdutoBL _ProdutoBL;

        //Propriedades
        private ICarrinhoItemConsultaBL OCarrinhoItemConsultaBL => _CarrinhoItemConsultaBL = _CarrinhoItemConsultaBL ?? new CarrinhoItemConsultaBL();
        private ICarrinhoResumoBL OCarrinhoResumoBL => _CarrinhoResumoBL = _CarrinhoResumoBL ?? new CarrinhoResumoBL();
        private IPessoaBL OPessoaBL => _PessoaBL = _PessoaBL ?? new PessoaBL();
        private IProdutoBL OProdutoBL => _ProdutoBL = _ProdutoBL ?? new ProdutoBL();

        //Eventos
        private EventAggregator eventoPedidoCriado = OnPedidoCadastrado.getInstance;

        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno salvar(Pedido NovoPedido, bool flagAssociadoAdimplente) {

            var listaItensCarrinho = this.OCarrinhoItemConsultaBL.listarResumo(NovoPedido.idOrganizacao.toInt(), NovoPedido.idPessoa.toInt(), NovoPedido.idSessao).ToList();
            
            if (listaItensCarrinho == null || !listaItensCarrinho.Any()){
                return UtilRetorno.newInstance(true, "Seu pedido deve ter ao menos 1 item.");
            }

            var idsProdutos = listaItensCarrinho.Where(x => x.idProduto > 0)
                                                .Select(x => x.idProduto.toInt())
                                                .ToList();

            var listaProdutos = this.OProdutoBL.query(1)
                                    .Where(x => idsProdutos.Contains(x.id))
                                    .ToList();

            var CarrinhoResumo = OCarrinhoResumoBL.carregarExistente(NovoPedido.idOrganizacao.toInt(), NovoPedido.idPessoa.toInt(), NovoPedido.idSessao) ?? new CarrinhoResumo();

            var ConfiguracaoEcommerce = ConfiguracaoEcommerceBL.getInstance.carregar(NovoPedido.idOrganizacao.toInt());

            var OPessoa = this.OPessoaBL.query().Where(x => x.id == NovoPedido.idPessoa)
                              .Select(x => new{
                                  x.id,
                                  x.nome,
                                  x.nroDocumento,
                                  x.rg,
                                  listaEmails = x.listaEmails.Where(em => em.dtExclusao == null).ToList(),
                                  listaTelefones = x.listaTelefones.Where(tel => tel.dtExclusao == null).ToList()
                              }).FirstOrDefault()
                                .ToJsonObject<Pessoa>();

            if (OPessoa == null){
                
                return UtilRetorno.newInstance(true, "Não foi possível carregar os dados da pessoa.");
            }

            NovoPedido = NovoPedido.transferirDadosPessoa(OPessoa);

            NovoPedido.idStatusPedido = StatusPedidoConst.EM_ABERTO;

            NovoPedido.valorProdutos = listaItensCarrinho.valorTotalProdutos(flagAssociadoAdimplente);

            NovoPedido.valorFrete = CarrinhoResumo.valorFrete;

            NovoPedido.flagFreteGratis = CarrinhoResumo.flagFreteGratis;

            NovoPedido.idCentroCusto = ConfiguracaoEcommerce.idCentroCusto;

            NovoPedido.idMacroConta = ConfiguracaoEcommerce.idMacroConta;

            NovoPedido.idCategoriaTitulo = ConfiguracaoEcommerce.idCategoriaTitulo;

            NovoPedido.flagCartaoCreditoPermitido = ConfiguracaoEcommerce.flagCartaoCreditoPermitido;

            NovoPedido.flagBoletoBancarioPermitido = ConfiguracaoEcommerce.flagBoletoBancarioPermitido;

            NovoPedido.flagDepositoPermitido = ConfiguracaoEcommerce.flagDepositoPermitido;

            NovoPedido.qtdeLimiteParcelas = ConfiguracaoEcommerce.qtdeLimiteParcelas;

            NovoPedido.dtVencimento = DateTime.Now.AddDays(ConfiguracaoEcommerce.qtdeDiasVencimento.toInt() > 0 ? ConfiguracaoEcommerce.qtdeDiasVencimento.toInt() : 5);

            NovoPedido.flagFaturamentoCadastro = true;

            NovoPedido.ativo = "S";

            NovoPedido.flagExcluido = "N";

            NovoPedido.dtCadastro = DateTime.Now;

            NovoPedido.dtAlteracao = NovoPedido.dtCadastro;


            foreach (var OItemCarrinho in listaItensCarrinho) {

                var Produto = listaProdutos.FirstOrDefault(x => x.id == OItemCarrinho.idProduto.toInt());

                if (Produto == null) {
                    continue;
                }
                
                var PedidoProduto = new PedidoProduto();

                PedidoProduto.idProduto = OItemCarrinho.idProduto.toInt();

                PedidoProduto.qtde = OItemCarrinho.qtde;

                PedidoProduto.flagCalcularFrete = OItemCarrinho.flagCalcularFrete;

                PedidoProduto.nomeProduto = OItemCarrinho.descricaoItem.abreviar(100);

                PedidoProduto.peso = OItemCarrinho.peso;

                PedidoProduto.valorItem = OItemCarrinho.valorComDescontoUnitario(flagAssociadoAdimplente);

                PedidoProduto.valorOriginal = OItemCarrinho.valorProduto;

                PedidoProduto.valorDesconto = decimal.Subtract(PedidoProduto.valorOriginal.toDecimal(), PedidoProduto.valorItem.toDecimal());

                PedidoProduto.dtCadastro = NovoPedido.dtCadastro;

                PedidoProduto.dtAlteracao = NovoPedido.dtCadastro;

                PedidoProduto.qtdeDiasDuracao = Produto.qtdeDiasDuracao;
                
                PedidoProduto.valorGanhoDiario = Produto.valorGanhoDiario;
                
                PedidoProduto.qtdePontosPlanoCarreira = Produto.qtdePontosPlanoCarreira.toDecimal();
                
                PedidoProduto.ativo = "S";

                PedidoProduto.flagExcluido = "N";

                NovoPedido.listaProdutos.Add(PedidoProduto);

            }

            if (listaItensCarrinho.flagCalcularFrete()) {

                var OPedidoEntrega = new PedidoEntrega();

                OPedidoEntrega.cepOrigem = ConfiguracaoEcommerce.cepOrigemFrete;

                OPedidoEntrega.cep = CarrinhoResumo.cepDestino;

                OPedidoEntrega.idTipoFrete = CarrinhoResumo.idTipoFrete;

                OPedidoEntrega.idTransportador = 1;

                OPedidoEntrega.logradouro = CarrinhoResumo.logradouroEntrega;

                OPedidoEntrega.numero = CarrinhoResumo.numeroEntrega;

                OPedidoEntrega.complemento = CarrinhoResumo.complementoEntrega;

                OPedidoEntrega.bairro = CarrinhoResumo.bairroEntrega;

                OPedidoEntrega.nomeCidade = CarrinhoResumo.nomeCidadeEntrega;

                OPedidoEntrega.idCidade = CarrinhoResumo.idCidadeEntrega;

                OPedidoEntrega.idEstado = CarrinhoResumo.idEstadoEntrega;

                OPedidoEntrega.idPais = "BRA";

                OPedidoEntrega.dtCadastro = NovoPedido.dtCadastro;

                OPedidoEntrega.dtAlteracao = NovoPedido.dtCadastro;

                OPedidoEntrega.flagExcluido = "N";

                NovoPedido.listaPedidoEntrega.Add(OPedidoEntrega);
            }

            db.Configuration.AutoDetectChangesEnabled = false;

            db.Configuration.ValidateOnSaveEnabled = false;

            db.Pedido.Add(NovoPedido);

            db.SaveChanges();

            if (NovoPedido.id == 0) {

                return UtilRetorno.newInstance(true, "Não foi possível registrar o seu pedido, tente novamente mais tarde!");

            }

            eventoPedidoCriado.subscribe(new PedidoCadastradoHandler());

            eventoPedidoCriado.publish(NovoPedido as object);

            return UtilRetorno.newInstance(false, "", NovoPedido);
        }


    }
}
