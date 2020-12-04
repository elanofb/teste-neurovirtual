using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class VideoController : Controller {

        //Atributos
        private IVideoBL _VideoBL;

        //Propriedades
        private IVideoBL OVideoBL => _VideoBL = _VideoBL ?? new VideoBL();

        //Construtor
        public VideoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagAtivo");

            int idPortal = UtilRequest.getInt32("idPortal");


            var listaRegistros = this.OVideoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            if (idPortal > 0) {
                listaRegistros = listaRegistros.Where(x => x.idPortal == idPortal).OrderBy(x => x.descricao);
            }

            return View(listaRegistros.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new VideoForm();

            ViewModel.Video = this.OVideoBL.carregar(UtilNumber.toInt32(id)) ?? new Video();

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(VideoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            bool flagSucesso = this.OVideoBL.salvar(ViewModel.Video, null);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.Video.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OVideoBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OVideoBL.excluir(id));
        }
    }
}