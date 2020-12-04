using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class IniciacaoCientificaController : Controller {

        //Atributos
        private INoticiaBL _NoticiaBL;

        //Propriedades
        private INoticiaBL ONoticiaBL => _NoticiaBL = _NoticiaBL ?? new NoticiaBL();

        //Construtor
        public IniciacaoCientificaController() {
        }

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            int? idPortal = UtilRequest.getInt32("idPortal");
            string ativo = UtilRequest.getString("flagAtivo");
            int tipoNoticia = (int)TipoNoticiaEnum.INICIACAO_CIENTIFICA;

            var listaCargo = this.ONoticiaBL.listar(descricao, ativo, tipoNoticia, false, idPortal).OrderBy(x => x.descricao);
            return View(listaCargo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new IniciacaoCientificaForm();

            ViewModel.ONoticia = this.ONoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new Noticia();
            
            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(IniciacaoCientificaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (String.IsNullOrEmpty(ViewModel.ONoticia.flagCompartilharFB)) {
                ViewModel.ONoticia.flagCompartilharFB = "N";
            }
            
            ViewModel.ONoticia.idTipoNoticia = TipoNoticiaConst.INICIACAO_CIENTIFICA;

            bool flagSucesso = this.ONoticiaBL.salvar(ViewModel.ONoticia, null);

            if (flagSucesso) {


                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));
                return RedirectToAction("editar", new { id = ViewModel.ONoticia.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.ONoticiaBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.ONoticiaBL.excluir(id));
        }

    }
}