using System.Web.Mvc;
using System.Linq;
using BLL.Contratos;

namespace WEB.Areas.Contratos.Helpers {
    public class TipoContratoHelper{

        //Atributos
        private ITipoContratoBL _TipoContratoBL;
        private static TipoContratoHelper _instance;

        //Propriedades
        public ITipoContratoBL OTipoContratoBL => (_TipoContratoBL = _TipoContratoBL ?? new TipoContratoBL());
        public static TipoContratoHelper getInstance => _instance = _instance ?? new TipoContratoHelper();

        //
        public SelectList selectList(int selected){

            var list = OTipoContratoBL.listar("", true).OrderBy(x => x.descricao).ToList();

            return new SelectList(list, "id", "descricao", selected);
        }

    }
}