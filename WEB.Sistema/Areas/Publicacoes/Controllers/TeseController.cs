using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using DAL.Publicacoes;
using PagedList;
using WEB.Areas.Publicacoes.ViewModels;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class TeseController : Controller {

        //Atributos
        private INoticiaBL _NoticiaBL;

        //Propriedades
        private INoticiaBL ONoticiaBL => _NoticiaBL = _NoticiaBL ?? new NoticiaBL();

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            int? idPortal = UtilRequest.getInt32("idPortal");

            string ativo = UtilRequest.getString("flagAtivo");

            int tipoNoticia = (int)TipoNoticiaEnum.TESE;

            var lista = this.ONoticiaBL.listar(descricao, ativo, tipoNoticia, false, idPortal).OrderBy(x => x.descricao);

            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new NoticiaForm();

            ViewModel.Noticia = this.ONoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new Noticia();

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(NoticiaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            if (String.IsNullOrEmpty(ViewModel.Noticia.flagCompartilharFB)) ViewModel.Noticia.flagCompartilharFB = "N";

            ViewModel.Noticia.idTipoNoticia = TipoNoticiaConst.TESE;

            bool flagSucesso = this.ONoticiaBL.salvar(ViewModel.Noticia, ViewModel.OArquivo);

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
	        return Json(this.ONoticiaBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.ONoticiaBL.excluir(id));
        }
        
    }

}