using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Caches;
using BLL.Configuracoes;
using BLL.Organizacoes;
using BLL.Permissao;
using DAL.Organizacoes;
using PagedList;
using WEB.App_Infrastructure;
using WEB.Areas.Associacoes.ViewModels;
using MvcFlashMessages;
using DAL.Documentos;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;
using WEB.Helpers;

namespace WEB.Areas.Associacoes.Controllers {

    public class AssociacaoController : BaseSistemaController {

		//Atributos
		private IOrganizacaoBL _AssociacaoBL;
		private IUsuarioSistemaBL _UsuarioSistemaBL;
        private IConfiguracaoSistemaBL _ConfiguracaoSistemaBL;

        //Propriedades
        private IOrganizacaoBL OAssociacaoBL => this._AssociacaoBL = this._AssociacaoBL ?? new OrganizacaoBL();
        private IUsuarioSistemaBL OUsuarioSistemaBL => this._UsuarioSistemaBL = this._UsuarioSistemaBL ?? new UsuarioSistemaBL();
        private IConfiguracaoSistemaBL OConfiguracaoSistemaBL => this._ConfiguracaoSistemaBL = this._ConfiguracaoSistemaBL ?? new ConfiguracaoSistemaBL();

        //Construtor

        //Listagem das unidades
        public ActionResult listar() {

            bool? flagStatus = UtilRequest.getBool("flagStatus");

            string flagTipoSaida = UtilRequest.getString("flagTipoSaida");

            string valorBusca = UtilRequest.getString("valorBusca");

            int idOrganizacaoGestora = UtilRequest.getInt32("idOrganizacaoGestora");

            int idStatusOrganizacao = UtilRequest.getInt32("idStatusOrganizacao");

            var query = this.OAssociacaoBL.listar(valorBusca, flagStatus);

		    if (idOrganizacaoGestora > 0) {
		        query = query.Where(x => x.idOrganizacaoGestora == idOrganizacaoGestora);
		    }

            if (idStatusOrganizacao > 0) {
                query = query.Where(x => x.idStatusOrganizacao == idStatusOrganizacao);
		    }

            if (flagTipoSaida == TipoSaidaHelper.EXCEL) {

                var OAssociacaoExportacao = new AssociacaoExportacao();
                OAssociacaoExportacao.baixarExcel(query.OrderBy(x => x.Pessoa.nome).ToList());

                return null;
            }

            var ViewModel = new AssociacaoVM();

            ViewModel.listaAssociacoes = query.OrderBy(x => x.Pessoa.nome).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            var idsOrganizacoes = ViewModel.listaAssociacoes.Select(x => x.id).ToList();

            ViewModel.listaConfiguracaoSistema = OConfiguracaoSistemaBL.listar(0).Where(x => idsOrganizacoes.Contains(x.idOrganizacao.Value)).ToList();
            
            return View(ViewModel);
		}

		//Formulário para cadastro e edição de registros
		public ActionResult editar(int? id) {

		    var ViewModel = new AssociacaoForm{
		        Associacao = this.OAssociacaoBL.carregar(UtilNumber.toInt32(id)) ?? new Organizacao()
		    };

		    ViewModel.Associacao.Pessoa = ViewModel.Associacao.Pessoa ?? new Pessoa();

            ViewModel.tratarEnderecos();

            return View(ViewModel);
		}

		//Salvamento da unidade na base de dados
		[HttpPost]
		public ActionResult editar(AssociacaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

            ViewModel.Associacao.Pessoa.idTipoDocumento = TipoDocumentoConst.CNPJ;
            ViewModel.Associacao.Pessoa.flagTipoPessoa = "J";

            var flagSucesso = this.OAssociacaoBL.salvar(ViewModel.Associacao, ViewModel.Logotipo);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados da Associacao foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.Associacao.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);			
		}

        public JsonResult loadVisaoAssociacao() {

            int idOrganizacaoVisao = String.IsNullOrEmpty(UtilRequest.getString("idOrganizacaoVisao")) ? 0 : UtilRequest.getInt32("idOrganizacaoVisao");

            if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR && !User.flagMultiOrganizacao()) {
                return Json(new { error = true, message = "O usuário não tem permissão para mudar de associação." });
            }


            if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR && User.flagMultiOrganizacao()) {

                if (User.flagMultiOrganizacao() && idOrganizacaoVisao == 0) {
                    return Json(new { error = true, message = "O usuário não tem permissão para mudar de associação." });
                }

                var OUsuarioSistema = OUsuarioSistemaBL.carregar(User.id());
                if (OUsuarioSistema.listaUsuarioOrganizacao.All(x => x.idOrganizacao != idOrganizacaoVisao) && OUsuarioSistema.idOrganizacao != idOrganizacaoVisao) {
                    return Json(new { error = true, message = "O usuário não tem permissão para mudar de associação." });
                }
            }

            Organizacao OAssociacao = this.OAssociacaoBL.carregar(idOrganizacaoVisao);

            CacheService.getInstance.limparCacheOrganizacao(idOrganizacaoVisao);

            if (idOrganizacaoVisao > 0 && OAssociacao != null) {

                User.setOrganizacao(OAssociacao.id.ToString(), OAssociacao.Pessoa.nome);

                return Json(new { error = false, message = "Id informada: " + idOrganizacaoVisao });

            }

            User.setOrganizacao(idOrganizacaoVisao.ToString());

            User.setUnidade("0");

            return Json(new { error = false, message = "Id informada: " + idOrganizacaoVisao });
        }

		//
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
            return Json(this.OAssociacaoBL.alterarStatus(id));
        }

        [ActionName("partial-lista-associacoes-vinculadas")]
        public ActionResult partialListaAssociacoesVinculadas(int? idOrganizacaoGestora) {

            if (idOrganizacaoGestora.toInt() == 0) {
                return View(new List<Organizacao>());
            }

            var listaOrganizacoes = this.OAssociacaoBL.listar("", null, true).Where(x => x.idOrganizacaoGestora == idOrganizacaoGestora).OrderBy(x => x.Pessoa.nome);

            return View(listaOrganizacoes.ToList());
        }
    }
}