using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.Pedidos.Emails;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;

namespace BLL.Pedidos {

    public class PedidoCobrancaBL : IPedidoCobrancaBL {

        //Atritos
        private IPedidoBL _PedidoBL;
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _PedidoBL = _PedidoBL ?? new PedidoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPedidoBL();
        
        /// <summary>
        /// Envio de email de cobranca de pedido
        /// </summary>
        public UtilRetorno enviarEmailCobranca(int idPedido) {

            var OPedido = this.OPedidoBL.carregar(idPedido);

            if (OPedido == null) {
                return UtilRetorno.newInstance(true, "Pedido não localizado no sistema.");
            }

            OPedido.TituloReceita = this.OTituloReceitaBL.query()
                                                         .Where(x => x.idReceita == OPedido.id && x.dtExclusao == null)
                                                         .Select(x => new {x.id, x.descricao})
                                                         .FirstOrDefault()
                                                         .ToJsonObject<TituloReceita>() ?? new TituloReceita();

            IEnvioNovoPedido EnvioEmail = EnvioNovoPedido.factory(HttpContextFactory.Current.User.idOrganizacao(), new List<string> { OPedido.email }, null);

            var ORetorno = EnvioEmail.enviar(OPedido);

            if (!ORetorno.flagError) {
                
                return UtilRetorno.newInstance(false, "O e-mail foi enviado com sucesso.");
            }

            return UtilRetorno.newInstance(true, "Não foi possível enviar o e-mail de cobrança.");
        }
    }
}
