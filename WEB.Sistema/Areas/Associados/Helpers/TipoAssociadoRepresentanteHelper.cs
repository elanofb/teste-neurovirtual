using System.Linq;
using System.Web.Mvc;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers {

    public class TipoAssociadoRepresentanteHelper {

        private static TipoAssociadoRepresentanteBL _TipoAssociadoRepresentanteBL;

        public static TipoAssociadoRepresentanteBL getService() {
            _TipoAssociadoRepresentanteBL = _TipoAssociadoRepresentanteBL ?? new TipoAssociadoRepresentanteBL();
            return _TipoAssociadoRepresentanteBL;
        }

        public static SelectList selectList(int? selected) {
            var list = getService().listar("", null, "S").OrderBy(x => x.descricao).Select(x => new { x.id, x.descricao }).ToList();
            return new SelectList(list, "id", "descricao", selected);
        }
    }
}