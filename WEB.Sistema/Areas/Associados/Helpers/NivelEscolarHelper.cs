using System.Web.Mvc;
using System.Linq;
using BLL.Escolaridades;

namespace WEB.Areas.Associados.Helpers{
    public class NivelEscolarHelper{

        private static NivelEscolarHelper _instance;
        private INivelEscolarBL _NivelEscolarBL;

        public static NivelEscolarHelper getInstance => _instance = _instance ?? new NivelEscolarHelper();
        private INivelEscolarBL ONivelEscolarBL =>  _NivelEscolarBL = _NivelEscolarBL ?? new NivelEscolarBL();

        //
        public SelectList selectList(int? selected, bool flagCache = true) {

            var query = this.ONivelEscolarBL.listar("", "S").OrderBy(x => x.descricao);

            var lista = query.Select(x => new { value = x.id, text = x.descricao }).ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }
}