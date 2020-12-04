using System;
using System.Linq;
using BLL.Services;
using DAL.PedidosTemp;
using DAL.Permissao.Security.Extensions;
using EntityFramework.Extensions;

namespace BLL.PedidosTemp {

    public class PedidoTempOperacaoBL : DefaultBL, IPedidoTempOperacaoBL {
        
        // Atributos
        private IPedidoTempBL _IPedidoTempBL;
        private IPedidoProdutoTempBL _IPedidoProdutoTempBL;

        // Propriedades
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();
        private IPedidoProdutoTempBL OPedidoProdutoTempBL => _IPedidoProdutoTempBL = _IPedidoProdutoTempBL ?? new PedidoProdutoTempBL();


        public void limpar(string idSessao) {

            // Limpar pedidos temporários por sessão
            var queryPedidoTemp = this.OPedidoTempBL.query();

            queryPedidoTemp = queryPedidoTemp.condicoesSeguranca();

            queryPedidoTemp.Where(x => x.idSessao == idSessao).Delete();
            
            // Limpar produtos temporários por sessão
            var queryPedidoProdutoTemp = this.OPedidoProdutoTempBL.query();

            queryPedidoProdutoTemp = queryPedidoProdutoTemp.condicoesSeguranca();

            queryPedidoProdutoTemp.Where(x => x.PedidoTemp.idSessao == idSessao).Delete();


        }

    }

}
