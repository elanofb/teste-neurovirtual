using System.Linq;
using System.Web.Mvc;
using BLL.Produtos;

namespace WEB.Areas.Produtos.Helpers {

    public class UnidadeMedidaHelper {

        //Constanctes
	    private static UnidadeMedidaHelper _instance;
		private IUnidadeMedidaConsultaBL _UnidadeMedidaConsultaBL;

        //Atributos

        //Propriedades
	    public static UnidadeMedidaHelper getInstance => _instance = _instance ?? new UnidadeMedidaHelper();
        private IUnidadeMedidaConsultaBL OUnidadeMedidaConsultaBL => _UnidadeMedidaConsultaBL = _UnidadeMedidaConsultaBL ?? new UnidadeMedidaConsultaBL();

		//
		public SelectList selectList(int? selected) {

            var lista = this.OUnidadeMedidaConsultaBL.listar("",true)
                                        .Select(x => new { x.id, x.descricao})
		                                .ToList();

            return new SelectList(lista, "id", "descricao", selected);
		}
    }
}