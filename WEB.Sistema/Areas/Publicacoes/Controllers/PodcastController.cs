using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using DAL.Publicacoes;
using PagedList;
using WEB.Areas.Publicacoes.ViewModels;
using System.Json;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class PodcastController : Controller {

        //Atributos
        private IPublicacaoBL _PodcastBL;

        //Propriedades
        private IPublicacaoBL OPodcastBL => _PodcastBL = _PodcastBL ?? new PodcastBL();


        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            int? idPortal = UtilRequest.getInt32("idPortal");

            string ativo = UtilRequest.getString("flagAtivo");

            var lista = this.OPodcastBL.listar(descricao, ativo, idPortal).OrderBy(x => x.descricao);

            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new NoticiaForm();

            ViewModel.Noticia = this.OPodcastBL.carregar(UtilNumber.toInt32(id)) ?? new Noticia();

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(NoticiaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (String.IsNullOrEmpty(ViewModel.Noticia.flagCompartilharFB)) ViewModel.Noticia.flagCompartilharFB = "N";

            ViewModel.Noticia.idTipoNoticia = TipoNoticiaConst.PODCAST;

            bool flagSucesso = this.OPodcastBL.salvar(ViewModel.Noticia, ViewModel.OArquivo);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.Noticia.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OPodcastBL.alterarStatus(id));
        }


        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

			var Retorno = new JsonMessage();
			Retorno.error = false;
			Retorno.message = "Os dados foram removidos com sucesso.";

			foreach(var idPodcast in id ){
				var RetornoItem = this.OPodcastBL.excluir(idPodcast);

				if (RetornoItem.flagError) { 
					Retorno.error = true;
					Retorno.message = RetornoItem.listaErros.FirstOrDefault();
				}
			}

            return Json(Retorno);
        }
    }
}