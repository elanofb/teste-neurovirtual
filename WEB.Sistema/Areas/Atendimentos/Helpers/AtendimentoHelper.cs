using System.Collections.Generic;
using System.Web.Mvc;
using DAL.Atendimentos;

namespace WEB.Areas.Atendimentos.Helpers {

    public class AtendimentoHelper {

        //Atributos
        private static AtendimentoHelper _instance;

        //Propriedades
        public static AtendimentoHelper getInstance => _instance = _instance ?? new AtendimentoHelper();


        public SelectList selectListStatus(int? selected){

            var list = new List<object> {
                new { id = AtendimentoStatusConst.EM_ABERTO, nome = "Em Aberto" },
                new { id = AtendimentoStatusConst.EM_ATENDIMENTO, nome = "Em Atendimento" },
                new { id = AtendimentoStatusConst.AGUARDANDO_RETORNO, nome = "Aguardando Retorno" },
            };

            return new SelectList(list, "id", "nome", selected);
        }
    }
}