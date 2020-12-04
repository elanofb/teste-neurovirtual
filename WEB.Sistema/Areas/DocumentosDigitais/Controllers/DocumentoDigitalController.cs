using System;
using System.Linq;
using System.Web.Mvc;
using BLL.DocumentosDigitais;
using BLL.Organizacoes;
using MvcFlashMessages;
using WEB.Areas.DocumentosDigitais.ViewModels;
using DAL.DocumentosDigitais;
using DAL.Permissao.Security.Extensions;
using PagedList;

namespace WEB.Areas.DocumentosDigitais.Controllers {

    public class DocumentoDigitalController : Controller {

        //Constantes

        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IDocumentoDigitalBL _DocumentoDigitalBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();
        private IDocumentoDigitalBL ODocumentoDigitalBL => _DocumentoDigitalBL = _DocumentoDigitalBL ?? new DocumentoDigitalBL();


        //GET 
        public ActionResult index() {

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("listar");
            }

            var lista = this.OOrganizacaoBL.listar("", true).ToList();

            return View(lista);
        }

        //GET 
        public ActionResult listar() {

            string valorBusca = UtilRequest.getString("valorBusca");
            bool? ativo = UtilRequest.getBool("flagAtivo");

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var listaDocumentosDigitais = this.ODocumentoDigitalBL.listar(valorBusca, 0, "", ativo)
                                              .Where(x => x.idOrganizacao == idOrganizacao || idOrganizacao == 0)
                                              .OrderBy(x => x.titulo);

            return View(listaDocumentosDigitais.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //GET 
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new DocumentoDigitalForm();
            
            ViewModel.DocumentoDigital = this.ODocumentoDigitalBL.carregar(UtilNumber.toInt32(id)) ?? new DocumentoDigital();

            return View(ViewModel);
        }

        //POST
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(DocumentoDigitalForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            var flagSucesso = this.ODocumentoDigitalBL.salvar(ViewModel.DocumentoDigital);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.DocumentoDigital.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }
        
        //GET 
        [HttpGet, ActionName("modal-documento-digital")]
        public ActionResult modalDocumentoDigital(int? id) {

            var ViewModel = new DocumentoDigitalForm();
            
            ViewModel.DocumentoDigital = this.ODocumentoDigitalBL.carregar(UtilNumber.toInt32(id));

            if (ViewModel.DocumentoDigital == null) {
                return Json(new { flagErro = true, message = "O documento informado não foi encontrado." }, JsonRequestBehavior.AllowGet);
            }

            return View(ViewModel);
        }

    }
}