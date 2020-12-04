using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using WEB.Areas.Associados.ViewModels;
using DAL.Associados;
using System.Json;
using BLL.Caches;
using BLL.Organizacoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using UTIL.UtilClasses;
using WEB.Extensions;

namespace WEB.Areas.Associados.Controllers {

	public class TipoAssociadoController : Controller {

		private IOrganizacaoBL _OrganizacaoBL;
		private IOrganizacaoBL OOrganizacaoBL => this._OrganizacaoBL = this._OrganizacaoBL ?? new OrganizacaoBL();

		private ITipoAssociadoConsultaBL _TipoAssociadoConsultaBL;
		private ITipoAssociadoConsultaBL OTipoAssociadoConsultaBL => this._TipoAssociadoConsultaBL = this._TipoAssociadoConsultaBL ?? new TipoAssociadoConsultaBL();

		private ITipoAssociadoCadastroBL _TipoAssociadoCadastroBL;
		private ITipoAssociadoCadastroBL OTipoAssociadoCadastroBL => this._TipoAssociadoCadastroBL = this._TipoAssociadoCadastroBL ?? new TipoAssociadoCadastroBL();

		private ITipoAssociadoExclusaoBL _TipoAssociadoExclusaoBL;
		private ITipoAssociadoExclusaoBL OTipoAssociadoExclusaoBL => this._TipoAssociadoExclusaoBL = this._TipoAssociadoExclusaoBL ?? new TipoAssociadoExclusaoBL();


        [HttpGet]
        public ActionResult index() {
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                return RedirectToAction("listar");
            }
            var lista = this.OOrganizacaoBL.listar("", true).ToList();
            return View(lista);
        }

        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            string ativo = UtilRequest.getString("flagAtivo");
            string flagIsento = UtilRequest.getString("flagIsento");
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");

	        if (User.idOrganizacao() > 0) {
		        idOrganizacao = User.idOrganizacao();
	        }

	        var listaTipoAssociado = this.OTipoAssociadoConsultaBL
	            .listar(descricao, null, ativo, idOrganizacao)
	            .Select( x => new {
		            x.id
			        , x.nomeDisplay
		            , x.ativo
		            , x.descricao
		            , x.dtCadastro
			        , x.ContribuicaoPadrao
			        , x.valorTaxaInscricao
			        , x.Categoria
			        , x.flagIsento
	            }).OrderBy(x => x.descricao)
				  .ToPagedListJsonObject<TipoAssociado>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());


	        return View(listaTipoAssociado);
		}


        //
        [HttpGet, OrganizacaoFilter]
        public ActionResult editar(int? id) {
			TipoAssociadoForm ViewModel = new TipoAssociadoForm();
            int idOrganizacao = UtilRequest.getInt32("idOrganizacao");
            if (User.idOrganizacao() > 0) {
                idOrganizacao = User.idOrganizacao();
            }
            var OTipoAssociado = this.OTipoAssociadoConsultaBL.carregar(UtilNumber.toInt32(id), idOrganizacao) ?? new TipoAssociado();
		    OTipoAssociado.idOrganizacao = idOrganizacao;
			ViewModel.TipoAssociado = OTipoAssociado;
			return View(ViewModel);
		}

		//
		[HttpPost, OrganizacaoFilter]
		public ActionResult editar(TipoAssociadoForm ViewModel) {
            if (!ModelState.IsValid) {
				return View(ViewModel);
			}
            bool flagSucesso = this.OTipoAssociadoCadastroBL.salvar(ViewModel.TipoAssociado);
		    if(flagSucesso) {
		        CacheService.getInstance.remover(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                return RedirectToAction("listar", new {idOrganizacao = ViewModel.TipoAssociado.idOrganizacao});
		    }
            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Houve um problema ao salvar o registro. Tente novamente.");

			return View(ViewModel);
		}

		//
		[HttpPost, OrganizacaoFilter]
		public ActionResult excluir(int[] id) {
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;
			foreach (int idExclusao in id) { 
				var RetornoExclusao = this.OTipoAssociadoExclusaoBL.excluir(idExclusao);

				if (RetornoExclusao.flagError) { 
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}
		    CacheService.getInstance.remover(CacheService.TIPO_ASSOCIADO_DD_SIMPLES);

            return Json(Retorno);
		}

	    //GET
		[ActionName("carregar-options"), OrganizacaoFilter]
        public ActionResult carregarOptions(bool? flagPF, bool? flagPJ) {
		    var query = OTipoAssociadoConsultaBL.listar("", false, "S");
		    if (flagPF.HasValue) {
		        query = query.Where(x => x.flagPessoaFisica == flagPF.Value);
		    }
		    if (flagPJ.HasValue) {
		        query = query.Where(x => x.flagPessoaJuridica == flagPJ.Value);
		    }
            List<OptionSelect> listaItens = query.Select(
                x => new OptionSelect {
                    value = x.id.ToString(),
                    text = x.nomeDisplay
                }).ToList();

            return Json(listaItens, JsonRequestBehavior.AllowGet);
        }
	}
}