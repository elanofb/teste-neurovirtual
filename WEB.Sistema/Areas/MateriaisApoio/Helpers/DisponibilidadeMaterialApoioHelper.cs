using System.Web.Mvc;
using System.Collections.Generic;
using DAL.MateriaisApoio;

namespace WEB.Areas.MateriaisApoio.Helpers{

    public class DisponibilidadeMaterialApoioHelper {
        
        //
        public static SelectList selectList(string selected){
            var list = new List<object>();

            list.Add(new { sigla = DisponibilidadeAssociadoConst.TODOS, descricao = "Todos" });
            list.Add(new { sigla = DisponibilidadeAssociadoConst.ASSOCIADOS, descricao = "Somente Associados" });
            list.Add(new { sigla = DisponibilidadeAssociadoConst.ASSOCIADOS_QUITES, descricao = "Associados Quites" });
            list.Add(new { sigla = DisponibilidadeAssociadoConst.ASSOCIADOS_ESPECIFICOS, descricao = "Associados Específicos" });

            return new SelectList(list, "sigla", "descricao", selected);
        }

    }
}