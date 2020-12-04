using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Configuracoes;
using BLL.Organizacoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Configuracao.ViewModels;

namespace WEB.Areas.Configuracao.Controllers {

    public class ConfiguracaoImagemController : Controller {


        //Atributos
        private IOrganizacaoBL _OrganizacaoBL;
        private IConfiguracaoImagemBL _ConfiguracaoImagemBL;

		//Propriedades
        private IOrganizacaoBL OOrganizacaoBL => _OrganizacaoBL = _OrganizacaoBL ?? new OrganizacaoBL();
        private IConfiguracaoImagemBL OConfiguracaoImagemBL => _ConfiguracaoImagemBL = _ConfiguracaoImagemBL ?? new ConfiguracaoImagemBL();

        [HttpGet]
        public ActionResult listar() {

            if (User.idOrganizacao() > 0) {
                return RedirectToAction("editar", new { idOrganizacao = User.idOrganizacao() });
            }

            var lista = this.OOrganizacaoBL.listar("", true).ToList();

            return View(lista);
        }

        //
        [HttpGet]
        public ActionResult editar() {

            var idOrganizacao = UtilRequest.getInt32("idOrganizacao");

            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }

            var ViewModel = new ConfiguracaoImagemForm();

            ViewModel.preLoad(idOrganizacao);

            return View(ViewModel);
        }

        //
		[HttpPost, ActionName("salvar-imagem")]
        public ActionResult salvarImagem(ConfiguracaoImagemForm ViewModel){

            if (User.idOrganizacao() > 0) {

                ViewModel.idOrganizacao = User.idOrganizacao();

            }

            if (ViewModel.SistemaTopo != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.SistemaTopo, ConfiguracaoImagemBL.IMAGEM_TOPO_SISTEMA, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro na imagem do topo do sistema: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração da imagem do topo do sistema foi realizada com sucesso!");
		        }
            }

            if (ViewModel.SistemaRodape != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.SistemaRodape, ConfiguracaoImagemBL.IMAGEM_RODAPE_SISTEMA, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro na imagem do rodapé do sistema: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração da imagem do rodapé do sistema foi realizada com sucesso!");
		        }
            }

		    if (ViewModel.SistemaEmail != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.SistemaEmail, ConfiguracaoImagemBL.IMAGEM_EMAIL_SISTEMA, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro no logotipo dos e-mails do sistema: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração do logotipo para e-mails do sistema foi realizada com sucesso!");
		        }
            }

		    if (ViewModel.SistemaLogin != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.SistemaLogin, ConfiguracaoImagemBL.IMAGEM_LOGIN_SISTEMA, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro no logotipo da tela de login do sistema: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração do logotipo da tela de login do sistema foi realizada com sucesso!");
		        }
            }

            if (ViewModel.SistemaPrint != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.SistemaPrint, ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro no logotipo das telas de impressão do sistema: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração do logotipo para as telas de impressão do sistema foi realizada com sucesso!");
		        }
            }

            if (ViewModel.BgLogin != null) {

		        var Retorno = OConfiguracaoImagemBL.salvar(ViewModel.BgLogin, ConfiguracaoImagemBL.IMAGEM_BG_LOGIN, ViewModel.idOrganizacao);

		        if (Retorno.flagError) {
		            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, $"Erro na imagem cadastrada para o fundo da tela de login: {Retorno.listaErros.FirstOrDefault()}");
		        } else {
		            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A configuração da imagem para a tela de login do sistema foi realizada com sucesso!");
		        }
            }

            return RedirectToAction("editar", new {ViewModel.idOrganizacao});
        }
	}
}