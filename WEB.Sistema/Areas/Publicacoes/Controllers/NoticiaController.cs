using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class NoticiaController : Controller {

        //Atributos
        private INoticiaBL _NoticiaBL;

        //Propriedades
        private INoticiaBL ONoticiaBL => _NoticiaBL = _NoticiaBL ?? new NoticiaBL();

        //Construtor
        public NoticiaController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagAtivo");

            int idPortal = UtilRequest.getInt32("idPortal");

            int tipoNoticia = (int)TipoNoticiaEnum.NOTICIA;

            var listaCargo = this.ONoticiaBL.listar(descricao, ativo, tipoNoticia).OrderByDescending(x => x.id);

            if (idPortal > 0){
                listaCargo = listaCargo.Where(x => x.idPortal == idPortal).OrderByDescending(x => x.id);
            }

            return View(listaCargo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new NoticiaForm();

            ViewModel.Noticia = this.ONoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new Noticia();

            if (ViewModel.Noticia == null) {
                return RedirectToAction("listar");
            }

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(NoticiaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            ViewModel.Noticia.idTipoNoticia = TipoNoticiaConst.NOTICIA;

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