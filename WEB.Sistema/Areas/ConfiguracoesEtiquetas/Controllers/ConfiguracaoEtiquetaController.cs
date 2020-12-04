using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.ConfiguracoesEtiquetas;
using BLL.Organizacoes;
using DAL.ConfiguracoesEtiquetas;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.ConfiguracoesEtiquetas.ViewModels;

namespace WEB.Areas.ConfiguracoesEtiquetas.Controllers{

    public class ConfiguracaoEtiquetaController : BaseSistemaController{
        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoEtiquetaBL _ConfiguracaoEtiquetaBL;

        //Propriedades
        private IOrganizacaoBL OOrganizacaoBL => _OrganizacaoBL = _OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoEtiquetaBL OConfiguracaoEtiquetaBL => _ConfiguracaoEtiquetaBL = _ConfiguracaoEtiquetaBL ?? new ConfiguracaoEtiquetaBL();


        //
        [HttpGet]
        public ActionResult listar(){

            if (User.idOrganizacao() > 0){
                return RedirectToAction("index");
            }

            var lista = this.OOrganizacaoBL.listar("", true).ToList();

            return View(lista);

        }

                //
        [HttpGet]
        public ActionResult index(){

            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var lista = this.OConfiguracaoEtiquetaBL.listar(idOrganizacao).OrderBy(x => x.descricao).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar(int? id, int? idOrganizacao) {

            ConfiguracaoEtiquetaForm ViewModel = new ConfiguracaoEtiquetaForm();

            ViewModel.ConfiguracaoEtiqueta = this.OConfiguracaoEtiquetaBL.carregar(id.toInt()) ?? new ConfiguracaoEtiqueta();

            ViewModel.ConfiguracaoEtiqueta.idOrganizacao = idOrganizacao;

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(ConfiguracaoEtiquetaForm ViewModel){

            if (User.idOrganizacao() > 0){
                ViewModel.ConfiguracaoEtiqueta.idOrganizacao = User.idOrganizacao();
            }

            if (!ModelState.IsValid){
                return View(ViewModel);
            }

            this.OConfiguracaoEtiquetaBL.salvar(ViewModel.ConfiguracaoEtiqueta);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("editar", new { id = ViewModel.ConfiguracaoEtiqueta.id });
        }

        /// 
        [HttpPost, ActionName("excluir")]
        public ActionResult excluir(int? id) {

            var Retorno = this.OConfiguracaoEtiquetaBL.excluir(id.toInt());

            if (!Retorno.flagError) {
                CacheService.getInstance.remover(ConfiguracaoEtiquetaBL.chaveCache);
            }
            
            return Json(Retorno);
        }
    }
}