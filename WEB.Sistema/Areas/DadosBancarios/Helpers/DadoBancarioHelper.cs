using System;
using System.Web.Mvc;
using System.Linq;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;

namespace WEB.Areas.DadosBancarios.Helpers {

    public class DadoBancarioHelper {

        //Constanctes
        private static DadoBancarioHelper _instance;

        //Atributos
        private IDadoBancarioConsultaBL _DadoBancarioConsultaBL;

        //Propriedades
        public static DadoBancarioHelper getInstance => _instance = _instance ?? new DadoBancarioHelper();
        private IDadoBancarioConsultaBL ODadoBancarioConsultaBL => _DadoBancarioConsultaBL = _DadoBancarioConsultaBL ?? new DadoBancarioConsultaBL();

        //Combo de conta bancaria
        public SelectList selectList(int? selected, int? idPessoa, int[] exclude = null) {
        
            var query = this.ODadoBancarioConsultaBL.listar("", true).Select(x => new { x.id, x.idPessoa, x.Banco.descricao, x.nroAgencia, x.nroConta });

            if (exclude != null && exclude.Length > 0) {

                query = query.Where(x => !exclude.Contains(x.id));

            }

            if (idPessoa > 0) {
                
                query = query.Where(x => x.idPessoa == idPessoa);
                
            }
            
            var lista = query
                .Select(x => new { value = x.id, text = String.Concat(x.descricao, " - Ag:", x.nroAgencia, " Cc:",x.nroConta) })
                .OrderBy(x => x.text).ToList();

            return new SelectList(lista, "value", "text", selected);
        }

    }
}