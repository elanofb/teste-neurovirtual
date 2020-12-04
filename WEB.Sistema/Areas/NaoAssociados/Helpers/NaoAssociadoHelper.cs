using System.Web.Mvc;
using System.Linq;
using BLL.NaoAssociados;

namespace WEB.Areas.NaoAssociados.Helpers{
    public class NaoAssociadoHelper{

        private static NaoAssociadoBL _NaoAssociadoBL;

        public static NaoAssociadoBL getService(){
            _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();
            return _NaoAssociadoBL;
        }

        //
        public static SelectList selectList(int? selected){

            var list = getService().listar("", "S")
                                   .Select( x => new { value = x.id, text = x.Pessoa.nome})
								   .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);

        }
        
        //
        public static SelectList SelectAllList(int? selected, string valorBusca, string ativo){
        
            var list = getService().listar(valorBusca, ativo)
                .Select( x => new { value = x.id, text = x.Pessoa.nome})
                .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);
        }

    }
}