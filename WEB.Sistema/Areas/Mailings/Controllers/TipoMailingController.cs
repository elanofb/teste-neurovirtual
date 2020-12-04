using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Mailings;
using DAL.Mailings;
using MvcFlashMessages;
using WEB.Areas.Mailings.ViewModels;

namespace WEB.Areas.Mailings.Controllers {

    public class TipoMailingController : Controller {

        // Atributos
        private ITipoMailingBL _TipoMailingBL { get; set; }

        // Propriedades
        private ITipoMailingBL OTipoMailingBL => this._TipoMailingBL = this._TipoMailingBL ?? new TipoMailingBL();

        //Listagem para consulta de Relacionamentos existentes
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            var listaOcorrencias = this.OTipoMailingBL.listar(ativo, descricao).OrderBy(x => x.descricao);

            return View(listaOcorrencias.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {
            TipoMailingForm ViewModel = new TipoMailingForm();

            ViewModel.TipoMailing = this.OTipoMailingBL.carregar(UtilNumber.toInt32(id)) ?? new TipoMailing();
            return View(ViewModel);
        }
        
        //
        [HttpPost]
        public ActionResult editar(TipoMailingForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OTipoMailingBL.salvar(ViewModel.TipoMailing);

            if(flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                return RedirectToAction("editar", new { ViewModel.TipoMailing.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

            return RedirectToAction("editar", new { id = ViewModel.TipoMailing.id });
        }

        //Excluir um ou mais registros
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach(int idExclusao in id) {
                bool flagSucesso = this.OTipoMailingBL.excluir(idExclusao);

                if(!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Algumas exclusões não puderam ser realizadas, tente novamente.";
                }
            }

            return Json(Retorno);
        }
    }
}