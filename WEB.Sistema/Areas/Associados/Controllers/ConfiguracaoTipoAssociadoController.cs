using System;
using System.Web.Mvc;
using BLL.Associados;
using DAL.Associados;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Controllers {

	public class ConfiguracaoTipoAssociadoController : Controller {

        //Atributos
        private IConfiguracaoTipoAssociadoBL _IConfiguracaoTipoAssociadoBL;

        //Propriedades
        private IConfiguracaoTipoAssociadoBL OConfiguracaoTipoAssociadoBL => _IConfiguracaoTipoAssociadoBL = _IConfiguracaoTipoAssociadoBL ?? new ConfiguracaoTipoAssociadoBL();

        //
        [HttpGet, ActionName("partial-configuracoes-tipoassociado")]
        public PartialViewResult partialConfiguracoesTipoAssociado()
        {

            int idTipoAssociado = UtilRequest.getInt32("idTipoAssociado");
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0)
            {
                idOrganizacao = User.idOrganizacao();
            }

            if (idOrganizacao == 0)
            {
                return PartialView("sem-associacao");
            }

            ConfiguracaoTipoAssociadoForm ViewModel = new ConfiguracaoTipoAssociadoForm
            {
                ConfiguracaoTipoAssociado = this.OConfiguracaoTipoAssociadoBL.carregar(idTipoAssociado, idOrganizacao, false) ?? new ConfiguracaoTipoAssociado()
            };

            ViewModel.ConfiguracaoTipoAssociado.idTipoAssociado = idTipoAssociado;
            ViewModel.ConfiguracaoTipoAssociado.idOrganizacao = idOrganizacao;

            return PartialView(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult salvar(ConfiguracaoTipoAssociadoForm ViewModel)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("partial-configuracoes-tipoassociado", ViewModel);
            }

            if (User.idOrganizacao() > 0)
            {
                ViewModel.ConfiguracaoTipoAssociado.idOrganizacao = User.idOrganizacao();
            }

            if (ViewModel.ConfiguracaoTipoAssociado.idOrganizacao == 0)
            {
                return PartialView("sem-associacao");
            }

            this.OConfiguracaoTipoAssociadoBL.salvar(ViewModel.ConfiguracaoTipoAssociado);

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Operação realizada!", "As configurações foram salvas com sucesso."));

            return RedirectToAction("partial-configuracoes-tipoassociado", new { ViewModel.ConfiguracaoTipoAssociado.idTipoAssociado, ViewModel.ConfiguracaoTipoAssociado.idOrganizacao });

        }
    }
}