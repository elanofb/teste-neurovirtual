using System.Web.Mvc;
using System.Linq;
using BLL.Associados;

namespace WEB.Areas.Associados.Helpers{
    public class CategoriaTipoAssociadoHelper{

        private static CategoriaTipoAssociadoHelper _instance;
        private ICategoriaTipoAssociadoBL _CategoriaTipoAssociadoBL;

        public static CategoriaTipoAssociadoHelper getInstance => _instance = _instance ?? new CategoriaTipoAssociadoHelper();
        private ICategoriaTipoAssociadoBL OCategoriaTipoAssociadoBL =>  _CategoriaTipoAssociadoBL = _CategoriaTipoAssociadoBL ?? new CategoriaTipoAssociadoBL();

        //
        public SelectList selectList(int? selected, bool flagCache = true) {

            var query = this.OCategoriaTipoAssociadoBL.listar("", "S").OrderBy(x => x.descricao);

            var lista = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }
}