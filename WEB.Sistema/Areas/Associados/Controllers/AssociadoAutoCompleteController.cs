using System;
using BLL.Associados;
using System.Linq;
using System.Web.Mvc;

namespace WEB.Areas.Associados.Controllers {

    public class AssociadoAutoCompleteController : Controller {

        //Atributos
        private IAssociadoConsultaBL _IAssociadoConsultaBL;
        
        //Propriedades
        private IAssociadoConsultaBL OAssociadoConsultaBL => _IAssociadoConsultaBL = _IAssociadoConsultaBL ?? new AssociadoConsultaBL();
        
        //Lista de associados para aucomplete de campos
        [ActionName("listar-json")]
        public ActionResult listarJson(string term) {
            
            var flagTipoPessoa = UtilRequest.getString("flagTipoPessoa");
            
            var query = this.OAssociadoConsultaBL.listar(0, "" , term, "");

            if (!flagTipoPessoa.isEmpty()) {
                query = query.Where(x => x.Pessoa.flagTipoPessoa == flagTipoPessoa);
            }

            var listaAssociados = query.Select(x => new { x.id, text = x.Pessoa.nome })
                                       .OrderBy(x => x.text).ToList();

            return Json(listaAssociados, JsonRequestBehavior.AllowGet);

        }
        
    }
}
