using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using BLL.Institucionais;
using DAL.Institucionais;
using MvcFlashMessages;
using WEB.Areas.Institucionais.ViewModels;
using System.Json;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Institucionais.Controllers {

    public class ConvenioController : Controller {

        //Atributos
        private IConvenioBL _ConvenioBL;

        //Propriedades
        private IConvenioBL OConvenioBL => _ConvenioBL = _ConvenioBL ?? new ConvenioBL();

        //Construtor
        public ConvenioController() {
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            int? idTipoConvenio = UtilRequest.getInt32("idTipoConvenio");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaConvenio = this.OConvenioBL.listar(descricao, ativo, idTipoConvenio).OrderBy(x => x.descricao);

            return View(listaConvenio.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            ConvenioForm ViewModel = new ConvenioForm();

            var OConvenio = this.OConvenioBL.carregar(UtilNumber.toInt32(id)) ?? new Convenio();

            ViewModel.Convenio = OConvenio;

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConvenioForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            this.OConvenioBL.salvar(ViewModel.Convenio, ViewModel.OArquivo);

            bool flagSucesso = ViewModel.Convenio.id > 0;

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados forma salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.Convenio.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OConvenioBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.OConvenioBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}