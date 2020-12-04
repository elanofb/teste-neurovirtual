using BLL.Empresas;
using DAL.Empresas;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Empresas.ViewModels;

namespace WEB.Areas.Empresas.Controllers
{
    public class EmpresaPorteController : Controller
    {
        //Constantes

        //Atributos
        private IEmpresaPorteBL _EmpresaPorteBL;

        //Propriedades
        private IEmpresaPorteBL OEmpresaPorteBL => _EmpresaPorteBL = _EmpresaPorteBL ?? new EmpresaPorteBL();

        //
        public EmpresaPorteController() {
        }


        //Listagem para consulta de empresas existentes
        [HttpGet]
        public ActionResult listar() {

            bool? ativo = UtilRequest.getBool("flagAtivo");

            string valorBusca = UtilRequest.getString("valorBusca");

            var listaEmpresasPorte = this.OEmpresaPorteBL.listar(User.idOrganizacao(), valorBusca, ativo).OrderBy(x => x.descricao);

            return View(listaEmpresasPorte.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        /**
		*
		*/
        [HttpGet]
        public ActionResult editar(int? id) {

            EmpresaPorteForm ViewModel = new EmpresaPorteForm();

            ViewModel.EmpresaPorte = this.OEmpresaPorteBL.carregar(UtilNumber.toInt32(id)) ?? new EmpresaPorte();

            return View(ViewModel);
        }


        /**
		*
		*/
        [HttpPost]
        public ActionResult editar(EmpresaPorteForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OEmpresaPorteBL.salvar(ViewModel.EmpresaPorte);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.OEmpresaPorteBL.alterarStatus(id));
        }

        //Excluir um ou mais registros
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idItem in id) {

                var RetornoItem = this.OEmpresaPorteBL.excluir(idItem, User.id());

                if (RetornoItem.flagError == true) {
                    Retorno.error = true;
                    Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
                }
            }

            return Json(Retorno);
        }
    }
}