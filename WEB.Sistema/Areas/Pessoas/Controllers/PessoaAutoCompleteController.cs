using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Pessoas;
using DAL.Pessoas;

namespace WEB.Areas.Pessoas.Controllers {

    public class PessoaAutoCompleteController : Controller {

        //Constantes

        //Atributos
        private IPessoaVWBL _IPessoaVWBL;

        //Propriedades
        private IPessoaVWBL OPessoaVWBL => _IPessoaVWBL = _IPessoaVWBL ?? new PessoaVWBL();


        //
        [ActionName("listar-json-associados-nao-associados")]
        public ActionResult listarJsonAssociadoNaoAssociado(string term) {
            
            var listaMembros = this.OPessoaVWBL.listar(term)
                                   .Where(x => x.flagCategoriaPessoa == CategoriaPessoaConst.ASSOCIADO || x.flagCategoriaPessoa == CategoriaPessoaConst.NAO_ASSOCIADO)
                                   .OrderBy(x => x.nome).Select(x => new { id = x.idPessoa, text = x.nome }).ToList();

            return Json(listaMembros, JsonRequestBehavior.AllowGet);

        }
        
        //
        [ActionName("carregar-associados-nao-associados")]
        public ActionResult carregarAssociadoNaoAssociado(int? id) {

            var DadosPessoa = new PessoaVW();
            
            if (id > 0) {

                DadosPessoa = this.OPessoaVWBL.carregar(id.toInt()) ?? new PessoaVW();

            }
            
            return Json(new { DadosPessoa, error = false , message = "" }, JsonRequestBehavior.AllowGet);
            

        }


    }
    
}