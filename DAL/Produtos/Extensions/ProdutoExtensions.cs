using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Produtos {

    public static class ProdutoExtensions {

        //
        public static decimal getValorComDescontoAssociado(this Produto OProduto) {

            var valorOriginal = OProduto.valor;

            if (OProduto.percentualDescontoAssociado > 0) {

                var valorDesconto = (OProduto.valor / 100) * OProduto.percentualDescontoAssociado;

                return valorOriginal - valorDesconto.toDecimal();

            }

            if (OProduto.valorDescontoAssociado > 0) {

                return valorOriginal - OProduto.valorDescontoAssociado.toDecimal();

            }

            return valorOriginal;

        }

        // Verificar se é produto ou serviço
        public static string produtoOuServico(this Produto OProduto) {

            string tipo = "";

            if (OProduto == null) {
                return tipo;
            }

            if (OProduto.TipoProduto.flagProduto == true) {
                tipo += "Produto";
            }

            if (OProduto.TipoProduto.flagProduto == true && OProduto.TipoProduto.flagServico == true) {
                tipo += "/";
            }

            if (OProduto.TipoProduto.flagServico == true) {
                tipo += "Serviço";
            }

            return tipo;
        }

        //
        public static decimal pesoTotalFrete(this List<Produto> listaProdutos) {

            if (listaProdutos == null) {
                return new decimal(0);
            }

            var pesoProdutosFrete = listaProdutos.Where(x => x.flagCalcularFrete == true).Sum(x => x.peso);

            return pesoProdutosFrete;
        }

        //Status descricao
        public static string exibirStatus(this Produto OProduto) {

            string descricaoAtivo = "Desativado";

            switch (OProduto.ativo) {

                case true:
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }

        //Status icone
        public static string exibirIconeStatus(this Produto OProduto) {

            string iconeAtivo = "fa-times-circle";

            switch (OProduto.ativo) {

                case true:
                    iconeAtivo = "fa-check";
                    break;
            }


            return iconeAtivo;
        }

        //Status cor
        public static string exibirCorStatus(this Produto OProduto) {

            string corAtivo = "text-red";

            switch (OProduto.ativo) {

                case true:
                    corAtivo = "text-green";
                    break;
            }


            return corAtivo;
        }
    }
}
