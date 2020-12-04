using System.Linq;
using System.Web.Mvc;
using BLL.Contatos;
using System.Collections.Generic;

namespace WEB.Helpers{
    
    public class TipoContatoHelper{

        // Atributos
        private static ITipoContatoVWConsultaBL _ITipoContatoVWConsultaBL;

        // Propriedades
        private static ITipoContatoVWConsultaBL OTipoContatoVWConsultaBL => _ITipoContatoVWConsultaBL = _ITipoContatoVWConsultaBL ?? new TipoContatoVWConsultaBL();

        //
        public static SelectList selectList(int? selected){
            
            var list = OTipoContatoVWConsultaBL.listar("").OrderBy(x => x.descricao);
            
            return new SelectList(list, "id", "descricao", selected);
            
        }

        public static MultiSelectList getSeleListMulti(List<int> selected) {
            
            var list = OTipoContatoVWConsultaBL.listar("").OrderBy(x => x.descricao);
            
            return new MultiSelectList(list, "id", "descricao", selected);
            
        }

    }
}