using System;
using System.Collections.Generic;
using System.Web.Mvc;
using UTIL.UtilClasses;
using BLL.Pessoas;

namespace WEB.Areas.Pedidos.Helpers {

    public class CompradorHelper {

        //Constanctes
	    private static CompradorHelper _instance;
        private IPessoaVWBL _IPessoaVWBL;

        //Atributos

        //Propriedades
	    public static CompradorHelper getInstance => _instance = _instance ?? new CompradorHelper();
        private IPessoaVWBL OPessoaVWBL => _IPessoaVWBL = _IPessoaVWBL ?? new PessoaVWBL();

		//
		public SelectList selectListAssociadoNaoAssociado(int? selected) {

		    var id = selected.toInt();

            if (id == 0) {
                return new SelectList(new List<object>());
            }

            var listaOpcoes = new List<object>();
            
            var OPessoa = this.OPessoaVWBL.carregar(id);

            if (OPessoa != null) {

                listaOpcoes.Add(new OptionSelect {value = OPessoa.idPessoa.ToString(), text = OPessoa.nome.ToUpper()});
                
            }
            
		    return new SelectList(listaOpcoes, "value", "text", selected);

		}
    }
}