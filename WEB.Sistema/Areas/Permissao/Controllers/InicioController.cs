using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BLL.Permissao;
using DAL.Permissao;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.Permissao.ViewModels;

namespace WEB.Areas.Permissao.Controllers {

	public class InicioController : BaseSistemaController {
		
		//Constantes

		//Atributos
		private IAcessoRecursoGrupoBL _AcessoRecursoGrupoBL;
		private IAcessoRecursoBL _AcessoRecursoBL;
		private IAcessoRecursoAcaoBL _AcessoRecursoAcaoBL;
		private IAcessoPermissaoBL _AcessoPermissaoBL;
		private PerfilAcessoBL _PerfilAcessoBL;

        //Propriedades
        private IAcessoRecursoGrupoBL OAcessoRecursoGrupoBL => this._AcessoRecursoGrupoBL = this._AcessoRecursoGrupoBL ?? new AcessoRecursoGrupoBL();

        private IAcessoRecursoBL OAcessoRecursoBL => this._AcessoRecursoBL = this._AcessoRecursoBL ?? new AcessoRecursoBL();

        private IAcessoRecursoAcaoBL OAcessoRecursoAcaoBL => this._AcessoRecursoAcaoBL = this._AcessoRecursoAcaoBL ?? new AcessoRecursoAcaoBL();

        private IPerfilAcessoBL OPerfilAcessoBL => this._PerfilAcessoBL = this._PerfilAcessoBL ?? new PerfilAcessoBL();

        private IAcessoPermissaoBL OAcessoPermissaoBL => this._AcessoPermissaoBL = this._AcessoPermissaoBL ?? new AcessoPermissaoBL();
		

		//Apresenta a lista de módulos do sistema com a marcação de quais itens o perfil informado (se houver) tem permissão
		[HttpGet, ActionName("editar-permissao")]
		public ActionResult editarPermissao(int? idPerfil) {

			PermissaoVM ViewModel = new PermissaoVM();

			ViewModel.idPerfilAcesso = UtilNumber.toInt32(idPerfil);

		    var queryGrupos = this.OAcessoRecursoGrupoBL.listar("S");

            var queryRecursos = this.OAcessoRecursoBL.listar(0, 0, "S");			

            if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR) {

                var listaPermissoesUsuario = this.OAcessoPermissaoBL.listar(User.idPerfil(), 0, 0).ToList();

                var idsPermissoesAssociacao = listaPermissoesUsuario.Select(x => x.idRecurso).ToList();
                queryRecursos = queryRecursos.Where(x => idsPermissoesAssociacao.Contains(x.id));

                var idsGruposAssociacao = listaPermissoesUsuario.Select(x => x.AcessoRecurso.idRecursoGrupo).ToList();
                queryGrupos = queryGrupos.Where(x => idsGruposAssociacao.Contains(x.id));
            }

		    ViewModel.listaGrupos = queryGrupos.OrderBy(x => x.ordem).ToList();

            ViewModel.listaRecursos = queryRecursos.ToList();

		    if (ViewModel.idPerfilAcesso > 0) {

                ViewModel.listaPermissoes = this.OAcessoPermissaoBL.listar(ViewModel.idPerfilAcesso, 0, 0).ToList();

		    }

			ViewModel.PerfilAcesso = this.OPerfilAcessoBL.carregar(UtilNumber.toInt32(idPerfil)) ?? new PerfilAcesso();

			return View(ViewModel);
		}

		//Apresentar os menus de cada grupo de módulos (se houver)
		[HttpGet, ActionName("exibir-menus-grupo")]
		public PartialViewResult exibirMenusGrupo(int idRecursoGrupo, int? idPerfilAcesso) {

			int idPerfil = UtilNumber.toInt32(idPerfilAcesso);

			var queryRecursos = this.OAcessoRecursoBL.listar(0, 0, "S");

			var listPermissoes = this.OAcessoPermissaoBL.listar(idPerfil, 0, 0)
														.Where(x => x.idPerfilAcesso == idPerfilAcesso)
														.ToList();

            if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR) {

                var listaPermissoesUsuario = this.OAcessoPermissaoBL.listar(User.idPerfil(), 0, 0).ToList();

                var idsPermissoesAssociacao = listaPermissoesUsuario.Select(x => x.idRecurso).ToList();
                queryRecursos = queryRecursos.Where(x => idsPermissoesAssociacao.Contains(x.id));
            }

			ViewData["idRecursoGrupo"] = idRecursoGrupo;

			ViewData["idRecursoPai"] = 0;
	
			ViewData["listRecursos"] = queryRecursos.ToList();
			
			ViewData["listPermissoes"] = listPermissoes;

			return PartialView();
		}

		//Modal com a possibilidade de edição de informações de um recurso do sistema
		[HttpGet, ActionName("editar-recurso")]
		public ActionResult editarRecurso(int id, int? idRecursoPai, int? idRecursoGrupo, int? idPerfilAcesso) {

			var ViewModel = new AcessoRecursoForm { idRecursoPai = idRecursoPai, idRecursoGrupo = idRecursoGrupo };

			var OAcessoRecurso = this.OAcessoRecursoBL.carregar(UtilNumber.toInt32(id));

			ViewModel = (OAcessoRecurso != null ? Mapper.Map<AcessoRecursoForm>(OAcessoRecurso) : ViewModel);

			if (UtilNumber.toInt32(id) > 0) {
				ViewModel.listRecursoAcao = this.OAcessoRecursoAcaoBL.listar(0, UtilNumber.toInt32(ViewModel.id), "S").ToList();
			} else {
				ViewModel.listRecursoAcao = this.OAcessoRecursoAcaoBL.listar(UtilNumber.toInt32(idRecursoGrupo), 0, "S").ToList();
			}

			ViewModel.idPerfilAcesso = idPerfilAcesso;

			if (idPerfilAcesso > 0) {
				PerfilAcesso OPerfil = this.OPerfilAcessoBL.carregar(UtilNumber.toInt32(idPerfilAcesso));
			
				ViewModel.descricaoPerfil = (OPerfil == null ? "" : OPerfil.descricao);
				
				ViewModel.listaPermissoes = this.OAcessoPermissaoBL.listarPermissoes(UtilNumber.toInt32(idPerfilAcesso), 0).ToList();
			}

			return View(ViewModel);
		}

		//Salvar módulos (menus) do sistema
		[HttpPost, ActionName("salvar-recurso")]
		public ActionResult salvarRecurso(AcessoRecursoForm ViewModel) {

            if (!ModelState.IsValid) {
				return PartialView("editar-recurso", ViewModel);
			}

		    if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Você não tem permissao para realizar essa operação.");

                return PartialView("editar-recurso", ViewModel);
		    }

			AcessoRecurso OAcessoRecurso = this.OAcessoRecursoBL.carregar(UtilNumber.toInt32(ViewModel.id)) ?? new AcessoRecurso();
			Mapper.Map(ViewModel, OAcessoRecurso);
			this.OAcessoRecursoBL.salvar(OAcessoRecurso);

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "As informações foram salvas com sucesso.");

			ViewModel.listRecursoAcao = this.OAcessoRecursoAcaoBL.listar(0, UtilNumber.toInt32(ViewModel.id), "S").ToList();

			return PartialView("editar-recurso", ViewModel);
		}

		// Action usada para reordenar as posições dos menus
		[HttpPost, ActionName("reordenar-recurso")]
		public JsonResult reordenarRecurso() {

		    if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){

                return Json(new { error = true, message = "Você não tem permissao para realizar essa operação." });
		    }

			int idRecurso = UtilNumber.toInt32(UtilRequest.getString("idRecurso").onlyNumber());

			int idRecursoPai = UtilNumber.toInt32(UtilRequest.getString("idRecursoPai").onlyNumber());

			int idRecursoGrupo = UtilNumber.toInt32(UtilRequest.getString("idRecursoGrupo").onlyNumber());

			int position = UtilRequest.getInt32("position");

			this.OAcessoRecursoBL.reordenarRecurso(idRecurso, idRecursoPai, idRecursoGrupo, position);

			return Json(new { error = false });
		}

		//
		[HttpPost, ActionName("carregar-recursos-ajax")]
		public JsonResult carregarRecursosAjax() {
			int idRecursoGrupo = UtilRequest.getInt32("idRecursoGrupo");

			if (idRecursoGrupo == 0) {
				return null;
			}

			var list = this.OAcessoRecursoBL.listar(idRecursoGrupo, 0, "S").ToList()
																		.OrderBy(x => x.nomeDisplay)
																		.ToList();
			return Json(list.Select(x => new { id = x.id, name = x.nomeDisplay }));
		}

		//Editar dados de um action
		[HttpPost, ActionName("editar-action")]
		public ActionResult editarAction(int idRecursoGrupo, int idRecursoPai, string descricaoAcao, string controleAcao, string nomeAcao, string metodoAcao) {
			
			var OAcessoRecursoAcao = new AcessoRecursoAcao { idRecursoGrupo = idRecursoGrupo, idRecursoPai = idRecursoPai, descricao = descricaoAcao, controller = controleAcao, action = nomeAcao, method = metodoAcao };

			var Validacao = this.OAcessoRecursoAcaoBL.validar(OAcessoRecursoAcao);

		    if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){

                return Json(new { error = true, message = "Você não tem permissao para realizar essa operação." });
		    }

			if (!Validacao.flagError){
				this.OAcessoRecursoAcaoBL.salvar(OAcessoRecursoAcao);
			}

			string message = Validacao.listaErros.Count == 0 ? "" : String.Join("<br />", Validacao.listaErros);

			return Json(new { error = Validacao.flagError, message = message });
		}

		//Exibir os actions de um recurso
		[HttpGet, ActionName("exibir-actions")]
		public PartialViewResult exibirActions(int idRecurso, int idRecursoGrupo, int? idPerfilAcesso) {

			AcessoRecursoForm ViewModel = new AcessoRecursoForm { id = idRecurso, idRecursoGrupo = idRecursoGrupo };
			
			if (idRecurso > 0) {
				ViewModel.listRecursoAcao = this.OAcessoRecursoAcaoBL.listar(0, idRecurso, "S").ToList();
			} else {
				ViewModel.listRecursoAcao = this.OAcessoRecursoAcaoBL.listar(idRecursoGrupo, 0,"S").ToList();
			}

			if (idPerfilAcesso > 0) {
				PerfilAcesso OPerfil = this.OPerfilAcessoBL.carregar(UtilNumber.toInt32(idPerfilAcesso));

				ViewModel.descricaoPerfil = (OPerfil == null ? "" : OPerfil.descricao);
				
				ViewModel.listaPermissoes = this.OAcessoPermissaoBL.listarPermissoes(UtilNumber.toInt32(idPerfilAcesso), 0).ToList();
			}

			ViewModel.idPerfilAcesso = idPerfilAcesso;

			return PartialView(ViewModel);
		}

		//Salvar o grupo de permissões para o perfil informado
		[HttpPost, ActionName("salvar-permissoes")]
		public JsonResult salvarPermissoes(int idPerfilAcesso, List<AcessoRecurso> listaRecursos) {

			bool flagError = false;
			
			string message = "As configurações de permissões foram salvas com sucesso!";

			this.OAcessoPermissaoBL.salvarPermissoesRecursos(idPerfilAcesso, listaRecursos);

			return Json(new { error = flagError, message = message });
		}

		//Salvar permissão de acões
		[HttpPost, ActionName("salvar-permissoes-acao")]
		public JsonResult salvarPermissoesAcao(int idPerfil, int idRecursoAcao, bool flagIncluir) {

			bool flagError = false;
	
			string message = "As configurações de permissões foram salvas com sucesso!";

			if (flagIncluir) {
				this.OAcessoPermissaoBL.salvarPermissoesAcoes(idPerfil, idRecursoAcao);
			} else {
				this.OAcessoPermissaoBL.excluir(idPerfil, idRecursoAcao);
			}


			return Json(new { error = flagError, message = message });
		}

		//[HttpPost, ActionName("alterar-status-recurso")]
		//public ActionResult alterarStatusRecurso(int id) {
		//	return Json(this.OAcessoRecursoBL.alterarStatus(id));
		//}

		//[HttpPost, ActionName("alterar-status-action")]
		//public ActionResult alterarStatusAction(int id) {
		//	return Json(this.OAcessoRecursoAcaoBL.alterarStatus(id));
		//}

		[HttpPost, ActionName("excluir-recurso")]
		public ActionResult excluirRecurso(int id) {

		    if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){

                return Json(new { error = true, message = "Você não tem permissao para realizar essa operação." });
		    }

			var Retorno = this.OAcessoRecursoBL.excluir(id); 

			return Json(new JsonMessage{ error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault()});
		}

		[HttpPost, ActionName("excluir-action")]
		public ActionResult excluirAction(int id) {

		    if (User.idPerfil() != PerfilAcessoConst.DESENVOLVEDOR){

                return Json(new { error = true, message = "Você não tem permissao para realizar essa operação." });
		    }

			var Retorno = this.OAcessoRecursoAcaoBL.excluir(id); 

			return Json(new JsonMessage{ error = Retorno.flagError, message = Retorno.listaErros.FirstOrDefault()});
		}
	}
}