using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Compras.Extensions {

    public static class CarrinhoExtensions {

        /// <summary>
        /// Verificar se existe condicao para conceder desconto e aplica-lo
        /// </summary>
        public static bool flagTemDesconto(this CarrinhoItemProdutoVW OItem, bool flagAssociadoAdimplente) {

            if (OItem.valorDescontoAssociado.toDecimal() == 0 && OItem.percentualDescontoAssociado.toDecimal() == 0) {

                return false;

            }

            if (flagAssociadoAdimplente == false) {

                return false;
            }

            return true;
        }

        /// <summary>
        /// Verificar se existe condicao para conceder desconto e aplica-lo
        /// </summary>
        public static decimal valorComDescontoUnitario(this CarrinhoItemProdutoVW OItem, bool flagAssociadoAdimplente) {

            if (!OItem.flagTemDesconto(flagAssociadoAdimplente)) {

                return OItem.valorProduto.toDecimal();

            }

            if (OItem.valorDescontoAssociado.toDecimal() > 0) {

                decimal valorComDesconto = decimal.Subtract(OItem.valorProduto.toDecimal(), OItem.valorDescontoAssociado.toDecimal());

                return valorComDesconto;

            }

            if (OItem.percentualDescontoAssociado.toDecimal() > 0) {

                decimal valorPercentualDesconto = OItem.valorProduto.toDecimal().valorPercentual(OItem.percentualDescontoAssociado.toDecimal());

                decimal valorComDesconto = decimal.Subtract(OItem.valorProduto.toDecimal(), valorPercentualDesconto);

                return valorComDesconto;

            }

            return OItem.valorProduto.toDecimal();
        }

        /// <summary>
        /// Verificar se existe condicao para conceder desconto e aplica-lo
        /// </summary>
        public static bool flagCalcularFrete(this List<CarrinhoItemProdutoVW> listaItensCarrinho) {

            if (listaItensCarrinho == null) {

                return false;

            }

            if (listaItensCarrinho.Any(x => x.flagCalcularFrete == true)) {

                return true;

            }


            return false;
        }

        /// <summary>
        /// Calcular o peso total dos itens adicionados no carrinho para calcular frete
        /// </summary>
        public static decimal pesoTotalFrete(this List<CarrinhoItemProdutoVW> listaItensCarrinho) {

            decimal pesoItens = listaItensCarrinho.Where(x => x.flagCalcularFrete == true)
                                                .Sum(x => decimal.Multiply(x.peso.toDecimal(), new decimal(x.qtde)));

            if (pesoItens == 0){

                pesoItens = new decimal(0.1);
            }

            return pesoItens;
        }

        /// <summary>
        /// Calcular o peso total dos itens adicionados no carrinho para calcular frete
        /// </summary>
        public static decimal valorTotalProdutos(this List<CarrinhoItemProdutoVW> listaItensCarrinho, bool flagAssociadoAdimplente) {

            var valorTotal = new decimal(0);

            foreach (var ItemCarrinho in listaItensCarrinho){

                decimal valorItem = decimal.Multiply(ItemCarrinho.valorComDescontoUnitario(flagAssociadoAdimplente), new decimal(1));

                valorTotal = decimal.Add(valorTotal, valorItem);

            }


            return valorTotal;
        }
    }
}
