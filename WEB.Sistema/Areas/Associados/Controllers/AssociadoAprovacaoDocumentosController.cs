using System;
using System.ServiceModel;
using System.Web.Mvc;

using BLL.Associados.Interface;

namespace WEB.Areas.Associados.Controllers {

    [OrganizacaoFilter]
    public class AssociadoAprovacaoDocumentosController : Controller {
        private IAssociadoAprovacaoDocumentosBL _IAssociadoAprovacaoDocumentosBL;

        private IAssociadoAprovacaoDocumentosBL AprovacaoDocumentos => _IAssociadoAprovacaoDocumentosBL = _IAssociadoAprovacaoDocumentosBL ?? new AssociadoAprovacaoDocumentosBL();

        [HttpGet, ActionName("aprovacao-documentos")]
        public ActionResult aprovacaoDocumentos(int idAssociado) {
            UtilRetorno Retorno = new UtilRetorno();

            if (idAssociado == 0) {
                    
                return Json(
                    new {
                        error = true, 
                        message = "Não foi possível localizar o membro!"
                    }, JsonRequestBehavior.AllowGet
                );
                
            }
            
            Retorno = AprovacaoDocumentos.aprovacaoDocumentos(idAssociado);

            string message = string.Join("<br>", Retorno.listaErros);                 

            return Json(new {
                error   = Retorno.flagError,
                message = message
            }, JsonRequestBehavior.AllowGet);
            
        }
    }
}