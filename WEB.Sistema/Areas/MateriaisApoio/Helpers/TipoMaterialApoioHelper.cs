using System.Web.Mvc;
using BLL.MateriaisApoio;

namespace WEB.Areas.MateriaisApoio.Helpers{

    public class TipoMaterialApoioHelper {

        private static ITipoMaterialApoioBL _ITipoMaterialApoioBL;

        private static ITipoMaterialApoioBL getService(){
			return (_ITipoMaterialApoioBL = _ITipoMaterialApoioBL ?? new TipoMaterialApoioBL() );
        }

        //
        public static SelectList selectList(byte? selected){
            var list = getService().listar("", "S");
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}