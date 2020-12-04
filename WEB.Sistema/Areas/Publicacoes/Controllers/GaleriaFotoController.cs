using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class GaleriaFotoController : Controller {

        //Atributos
        private IGaleriaFotoBL _GaleriaFotoBL;

        //Propriedades
        private IGaleriaFotoBL OGaleriaFotoBL => _GaleriaFotoBL = _GaleriaFotoBL ?? new GaleriaFotoBL();

        //Construtor
        public GaleriaFotoController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagAtivo");

            int idPortal = UtilRequest.getInt32("idPortal");
            
            var queryGalerias = this.OGaleriaFotoBL.listar(descricao, ativo);

            if (idPortal > 0){
                queryGalerias = queryGalerias.Where(x => x.idPortal == idPortal).OrderBy(x => x.descricao);
            }

            var listaGalerias = queryGalerias.OrderBy(x => x.titulo).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(listaGalerias);

        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new GaleriaFotoForm();

            ViewModel.GaleriaFoto = this.OGaleriaFotoBL.carregar(UtilNumber.toInt32(id)) ?? new GaleriaFoto();
            
            if (ViewModel.GaleriaFoto.id == 0) {

                ViewModel.GaleriaFoto.dtGaleria = DateTime.Today;
            }

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(GaleriaFotoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }
            
            bool flagSucesso = this.OGaleriaFotoBL.salvar(ViewModel.GaleriaFoto, ViewModel.OArquivo);;

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.GaleriaFoto.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OGaleriaFotoBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OGaleriaFotoBL.excluir(id));
        }
    }
}