using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using BLL.Relacionamentos;

namespace WEB.Helpers{
    public class OcorrenciaRelacionamentoHelper{

        // Constantes

        // Atributos
        private static IOcorrenciaRelacionamentoVWConsultaBL _IOcorrenciaRelacionamentoVWConsultaBL;

        // Propriedades
        private static IOcorrenciaRelacionamentoVWConsultaBL OcorrenciaRelacionamentoVWConsultaBL => _IOcorrenciaRelacionamentoVWConsultaBL = _IOcorrenciaRelacionamentoVWConsultaBL ?? new OcorrenciaRelacionamentoVWConsultaBL();

        //
        public static SelectList selectList(int? selected){

            var list = OcorrenciaRelacionamentoVWConsultaBL.listar("")
                                                           .Select(x => new { x.id, x.descricao })
                                                           .OrderBy(x => x.descricao)
                                                           .AsNoTracking().ToList();

            return new SelectList(list, "id", "descricao", selected);
        }
        
        //
        public static MultiSelectList selectMultiList(int[] selected){

            var list = OcorrenciaRelacionamentoVWConsultaBL.listar("")
                                                           .Select(x => new { x.id, x.descricao })
                                                           .OrderBy(x => x.descricao)
                                                           .AsNoTracking().ToList();

            return new MultiSelectList(list, "id", "descricao", selected);
        }

    }
}