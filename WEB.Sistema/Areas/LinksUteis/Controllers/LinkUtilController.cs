using BLL.LinksUteis;
using DAL.LinksUteis;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.App_Infrastructure;
using WEB.Areas.LinksUteis.ViewModels;

namespace WEB.Areas.LinksUteis.Controllers {
    
    public class LinkUtilController : BaseSistemaController {

        //Constantes

        //Atributos
        private ILinkUtilBL _LinkUtilBL;

        //Propriedades
        private ILinkUtilBL OLinkUtilBL => this._LinkUtilBL = this._LinkUtilBL ?? new LinkUtilBL();

        //
        [HttpGet]
        public ActionResult listar() {
            bool? ativo = UtilRequest.getBool("flagAtivo");
            int? idPortal = UtilRequest.getInt32("idPortal");
            string valorBusca = UtilRequest.getString("valorBusca");

            var lista = this.OLinkUtilBL.listar(valorBusca, ativo, idPortal).OrderBy(x => x.descricao).ToList();

            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            LinkUtilForm ViewModel = new LinkUtilForm();
            ViewModel.LinkUtil = this.OLinkUtilBL.carregar(UtilNumber.toInt32(id)) ?? new LinkUtil();

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(LinkUtilForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            bool flagSucesso = this.OLinkUtilBL.salvar(ViewModel.LinkUtil, ViewModel.Arquivo);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.LinkUtil.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OLinkUtilBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            JsonMessage Retorno = new JsonMessage();
            Retorno = this.OLinkUtilBL.delete(id);

            return Json(Retorno);
        }
    }
}