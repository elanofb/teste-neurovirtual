using System.Web.Mvc;
using System.Linq;
using BLL.Contratos;

namespace WEB.Areas.Contratos.Helpers{
    public class StatusContratoHelper{

        //Atributos
        private static IStatusContratoBL _StatusContratoBL;

        //Propriedades
        private static IStatusContratoBL OStatusContratoBL { get { return (_StatusContratoBL = _StatusContratoBL ?? new StatusContratoBL()); } }

        //
        public static SelectList selectList(int selected){
            var list = OStatusContratoBL.listar("", "S").OrderBy(x => x.descricao).ToList();
            return new SelectList(list, "id", "descricao", selected);
        }

    }
}