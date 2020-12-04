using System.Web.Mvc;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.Helpers{
    public class GatewayPagamentoHelper{

        //Atributos
        private static IGatewayPagamentoBL _GatewayPagamentoBL;
        private static GatewayPagamentoHelper _getInstance;

        //Propriedades
        public static GatewayPagamentoHelper getInstance => _getInstance = _getInstance ?? new GatewayPagamentoHelper();
        private IGatewayPagamentoBL OGatewayPagamentoBL => _GatewayPagamentoBL = _GatewayPagamentoBL ?? new GatewayPagamentoBL();

        //
        public SelectList selectList(int? selected) {

            var list = this.OGatewayPagamentoBL.listar("", true);
            
			return new SelectList(list, "id", "descricao", selected);
        }
    }
}