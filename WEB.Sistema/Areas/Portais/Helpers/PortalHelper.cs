using System.Web.Mvc;
using System.Linq;
using BLL.Portais;

namespace WEB.Areas.Portais.Helpers{
    public class PortalHelper{

        //Atributos
        private static PortalHelper _instance;
        private IPortalBL _PortalBL;

        //Propriedades
        public static PortalHelper getInstance => _instance = _instance ?? new PortalHelper();
        private IPortalBL OPortalBL => _PortalBL = _PortalBL ?? new PortalBL();


        public SelectList selectList(int? selected, bool? ativo = true){

            var list = OPortalBL.listar("", ativo).OrderBy(x => x.descricao).ToList();
            return new SelectList(list, "id", "descricao", selected);
        }
    }
}