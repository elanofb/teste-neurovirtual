using System;
using BLL.Associados;
using BLL.Pedidos;
using BLL.Produtos;
using DAL.Associados;
using DAL.Pedidos;
using DAL.Produtos;
using FluentValidation;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoDetalhesProdutoFormValidator : AbstractValidator<PedidoDetalhesProdutoForm> {

        //Atributos Serviços
        private IAssociadoBL _IAssociadoBL;
        private IProdutoBL _IProdutoBL;
        private IPedidoBL _IPedidoBL;

        //Propriedades Serviços
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
        private IProdutoBL OProdutoBL => _IProdutoBL = _IProdutoBL ?? new ProdutoBL();
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();

        // Propriedades
        private Pedido OPedido { get; set; }
        private Produto OProduto { get; set; }
        private Associado OAssociado { get; set; }

        //
        public PedidoDetalhesProdutoFormValidator() {

            RuleFor(x => x.OPedidoProduto.idProduto)
                .NotEmpty().WithMessage("Escolha um produto para adicionar.");

            RuleFor(x => x.OPedidoProduto.qtde)
                .GreaterThan(0).WithMessage("Informe a quantidade do produto.");
            
            When(x => x.OPedidoProduto.idProduto > 0, () => {

                RuleFor(x => x.OPedidoProduto.idProduto)
                    .Must((x, idProduto) => validarProdutoSomenteAssociados(x))
                    .WithMessage("O produto não pode ser adicionado pois está configurado somente para associados.");

                RuleFor(x => x.OPedidoProduto.idProduto)
                    .Must((x, idProduto) => validarProdutoSomenteQuites(x))
                    .WithMessage("O produto não pode ser adicionado pois está configurado somente para associados quites (adimplentes).");

            });

        }

        private void carregarObjetos(PedidoDetalhesProdutoForm ViewModel) {

            this.OPedido = this.OPedidoBL.carregar(ViewModel.OPedidoProduto.idPedido);

            this.OProduto = this.OProdutoBL.carregar(ViewModel.OPedidoProduto.idProduto);

            this.OAssociado = this.OAssociadoBL.carregarAssociadoPessoa(OPedido.idPessoa.toInt()) ?? new Associado();

        }

        private bool validarProdutoSomenteAssociados(PedidoDetalhesProdutoForm ViewModel) {

            if (this.OAssociado == null) {
                this.carregarObjetos(ViewModel);
            }
            
            if (this.OProduto.flagSomenteAssociados == true && this.OAssociado.id == 0) {
                return false;
            }

            return true;

        }

        private bool validarProdutoSomenteQuites(PedidoDetalhesProdutoForm ViewModel) {

            if (this.OAssociado == null) {
                this.carregarObjetos(ViewModel);
            }
            

            return true;

        }

    }

}