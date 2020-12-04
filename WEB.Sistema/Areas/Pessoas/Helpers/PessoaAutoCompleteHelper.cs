using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using DAL.Pessoas;

namespace WEB.Areas.Pessoas.Helpers {

    public class PessoaAutoCompleteHelper {

        //Constanctes
	    private static PessoaAutoCompleteHelper _instance;
		private IPessoaVWBL _PessoaBL;

        //Propriedades
	    public static PessoaAutoCompleteHelper getInstance => _instance = _instance ?? new PessoaAutoCompleteHelper();
        private IPessoaVWBL OPessoaBL => _PessoaBL = _PessoaBL ?? new PessoaVWBL();

        //
        public SelectList selectListAssociadoNaoAssociado(int? selected) {

            var id = selected.toInt(); 
            
            if (id == 0) {
                return new SelectList(new List<object>());
            }

            var list = this.OPessoaBL.listar("")
                                     .Where(x => x.idPessoa == id && (x.flagCategoriaPessoa == CategoriaPessoaConst.ASSOCIADO || x.flagCategoriaPessoa == CategoriaPessoaConst.NAO_ASSOCIADO))
                                     .Select(x => new { value = x.idPessoa, text = x.nome })
                                     .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);

        }
    }
}