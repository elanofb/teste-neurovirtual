using System.Linq;
using System.Web.Mvc;
using BLL.Frete;
using DAL.Pedidos;

namespace WEB.Helpers{

    public class PedidoEntregaHelper{
        
        // Atributos
        public static PedidoEntregaHelper _intance;
        private ITransportadorBL _TransportadorBL;
        private ITipoFreteBL _TipoFreteBL;

        // Propriedades
        private ITransportadorBL OTransportadorBL => _TransportadorBL = _TransportadorBL ?? new TransportadorBL();
        private ITipoFreteBL OTipoFreteBL => _TipoFreteBL = _TipoFreteBL ?? new TipoFreteBL();
        public static PedidoEntregaHelper getInstance => _intance = _intance ?? new PedidoEntregaHelper();
        
        //
        public static SelectList getComboPedidoEntrega(string selected){
            var list = new[] { 
                new{value = PeriodoEntregaConst.MANHA, text = "MANHÃ"},
                new{value = PeriodoEntregaConst.TARDE, text = "TARDE"},
                new{value = PeriodoEntregaConst.INTEGRAL, text = "INTEGRAL"},
            };
            return new SelectList(list, "value", "text", selected);
        }

        //
        public SelectList getComboTransportador(int? selected){

            var lista = this.OTransportadorBL.listar("", "S")
                                        .Select(x => new { id = x.id, descricao = x.nome })
                                        .ToList();

            return new SelectList(lista, "id", "descricao", selected);
        }
            
        //
        public SelectList getComboTipoFrete(int? selected){

            var lista = this.OTipoFreteBL.listar("", true)
                                        .Select(x => new { id = x.id, descricao = x.descricaoCliente })
                                        .ToList();

            return new SelectList(lista, "id", "descricao", selected);
        }
                     
    }
}