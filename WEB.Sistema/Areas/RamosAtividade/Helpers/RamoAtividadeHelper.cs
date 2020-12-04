using System.Collections;
using System.Linq;
using System.Web.Mvc;
using BLL.RamosAtividade;

namespace WEB.Areas.RamosAtividade.Helpers{

    public class RamoAtividadeHelper{

        //Constantes
        private static RamoAtividadeHelper _instance; 

        //Atributos
         private static IRamoAtividadeBL _RamoAtividadeBL;

        //Propriedades
        public static RamoAtividadeHelper getInstance => _instance = _instance ?? new RamoAtividadeHelper();
        private IRamoAtividadeBL ORamoAtividadeBL   => _RamoAtividadeBL = _RamoAtividadeBL ?? new RamoAtividadeBL();
       

        //Carregar combo de selecao dos tipos de associados
        public SelectList selectList(int? selected) {


            var listaItens = ORamoAtividadeBL.listar("", true)
                                    .Select(x => new {
                                        x.id,
                                        descricao = x.descricao
                                    }).OrderBy(x => x.descricao).ToList();

            return new SelectList((IEnumerable) listaItens, "id", "descricao", selected);
        }

    }
}