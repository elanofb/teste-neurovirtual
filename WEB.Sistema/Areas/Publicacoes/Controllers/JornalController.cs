using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Publicacoes;
using PagedList;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class JornalController : Controller {

        //Atributos
        private IJornalBL _JornalBL;

        //Propriedades
        private IJornalBL OJornalBL => _JornalBL = _JornalBL ?? new JornalBL();

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            int? idPortal = UtilRequest.getInt32("idPortal");

            string ativo = UtilRequest.getString("flagAtivo");

            int tipoJornal = TipoNoticiaConst.JORNAL;

            var listaCargo = this.OJornalBL.listar(descricao, ativo, tipoJornal, false, idPortal).OrderByDescending(x => x.id);

            return View(listaCargo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new JornalForm();

            ViewModel.Jornal = this.OJornalBL.carregar(UtilNumber.toInt32(id)) ?? new Jornal();

            if (ViewModel.Jornal == null) {
                return RedirectToAction("listar");
            }

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(JornalForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            ViewModel.Jornal.idTipoNoticia = TipoNoticiaConst.JORNAL;

            bool flagSucesso = this.OJornalBL.salvar(ViewModel.Jornal, ViewModel.arrayArquivos);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.Jornal.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OJornalBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            return Json(this.OJornalBL.excluir(id));
        }
        
    }

}