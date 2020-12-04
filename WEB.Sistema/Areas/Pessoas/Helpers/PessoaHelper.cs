using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using DAL.Pessoas;
using UTIL.UtilClasses;

namespace WEB.Areas.Pessoas.Helpers {

    public class PessoaHelper {

        //Constanctes
	    private static PessoaHelper _instance;
		private IPessoaVWBL _PessoaBL;

        //Propriedades
	    public static PessoaHelper getInstance => _instance = _instance ?? new PessoaHelper();
        private IPessoaVWBL OPessoaBL => _PessoaBL = _PessoaBL ?? new PessoaVWBL();

		//
		public SelectList selectListFinanceiro(int? selected) {

		    var lista = this.OPessoaBL.listar("")
                //.Where(x => x.flagCategoriaPessoa != CategoriaPessoaConst.ASSOCIADO)
                .Select(x => new OptionSelect{ value = (x.flagCategoriaPessoa + "#" + x.idPessoa.ToString()), text = (x.descricaoCategoriaPessoa.ToUpper() + " - " +  x.nome.ToUpper())})
                .ToList();

		    lista = lista.OrderBy(x => x.text).ToList();

            return new SelectList(lista, "value", "text", selected);
		}

        //
        public SelectList selectListAssociadoNaoAssociado(int? selected) {

            var list = this.OPessoaBL.listar("")
                                     .Where(x => x.flagCategoriaPessoa == CategoriaPessoaConst.ASSOCIADO || x.flagCategoriaPessoa == CategoriaPessoaConst.NAO_ASSOCIADO)
                                     .Select(x => new { value = x.idReferencia, text = x.nome })
                                     .OrderBy(x => x.text).ToList();

            return new SelectList(list, "value", "text", selected);

        }
    }
}