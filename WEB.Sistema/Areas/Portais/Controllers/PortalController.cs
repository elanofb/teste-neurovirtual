using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Json;
using BLL.Portais;
using MvcFlashMessages;
using DAL.Permissao.Security.Extensions;
using DAL.Portais;
using WEB.App_Infrastructure;
using WEB.Areas.Portais.ViewModels;

namespace WEB.Areas.Portais.Controllers {

    public class PortalController : BaseSistemaController {
        //Constantes

        //Atributos
        private IPortalBL _PortalBL;

        //Propriedades
        private IPortalBL OPortalBL => _PortalBL = _PortalBL ?? new PortalBL();

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            bool? ativo = UtilRequest.getBool("flagAtivo");
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var query = this.OPortalBL.listar(descricao,ativo);

            if (idOrganizacao > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
            }

            return View(query.OrderBy(x => x.descricao).ToPagedList(UtilRequest.getNroPagina(),20));
        }

        //
        [HttpGet, OrganizacaoFilter]
        public ActionResult editar(int? id) {

            PortalForm ViewModel = new PortalForm();
            
            ViewModel.Portal = this.OPortalBL.carregar(UtilNumber.toInt32(id)) ?? new Portal();

            return View(ViewModel);
        }

        //
        [HttpPost, OrganizacaoFilter]
        public ActionResult editar(PortalForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OPortalBL.salvar(ViewModel.Portal);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do banco foram salvos com sucesso."));
			    return RedirectToAction("editar", new {id = ViewModel.Portal.id});	
			}
            
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
        }

        //
		[HttpPost]
		[ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OPortalBL.alterarStatus(id));
		}

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idPortal in id) {

                var RetornoItem = this.OPortalBL.excluir(idPortal);

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);

        }
        
    }
}