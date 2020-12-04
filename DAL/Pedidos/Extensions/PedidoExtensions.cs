using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;
using DAL.Pedidos;
using DAL.Pessoas;

namespace DAL.Pedidos.Extensions {

    public static class PedidoExtensions {

        /// <summary>
        /// Extension method para capturar valor total dos items de uma lista
        /// </summary>
        public static decimal getValorTotal(this List<PedidoProduto> lista) {
            if (lista == null)
                return new decimal(0);
            return lista.Select(x => (x.valorItem.toDecimal() * x.qtde.toInt())).Sum();
        }

        /// <summary>
        /// Extension method para capturar valor total dos items de uma lista
        /// </summary>
        public static decimal getPesoTotal(this List<PedidoProduto> lista) {

            if (lista?.Any() == false) {
                return new decimal(0);
            }

            return lista.Select(x => x.qtde.toInt() * x.peso.toDecimal()).Sum();

        }

        public static int getQtdeItens(this List<PedidoProduto> lista) {
            if (lista == null)
                return 0;
            return lista.Select(x => x.qtde.toInt()).Sum();
        }

        /// <summary>
        /// Preencher os dados da pessoa no pedido
        /// </summary>
        public static Pedido transferirDadosPessoa(this Pedido OPedido, Pessoa OPessoa) {

            if (OPessoa == null) {

                return OPedido;

            }

            OPedido.nomePessoa = OPessoa.nome;

            OPedido.cpf = OPessoa.nroDocumento;

            OPedido.rg = OPessoa.rg;

            OPedido.email = OPessoa.emailPrincipal();

            OPedido.telPrincipal = OPessoa.telefonePrincipal();

            OPedido.telSecundario = OPessoa.telefoneSecundario();

            return OPedido;


        }
    }
}