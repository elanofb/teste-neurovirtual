using System;
     using System.Linq;
     using System.Web.Mvc;
     using BLL.Permissao;
     using BLL.Services;
     using DAL.Permissao;
     using MoreLinq;
     using WEB.App_Infrastructure;
     using WEB.Areas.Permissao.ViewModels;
using DAL.Permissao.Security.Extensions;
     
     namespace WEB.Areas.Permissao.Controllers {
     	[AllowAnonymous]
     	public class MenuController : BaseSistemaController {
     		
     		//Constantes
     
     		//Atributos
     		private IAcessoPermissaoBL _AcessoPermissaoBL;
     
     		//Propriedades
     		private IAcessoPermissaoBL OAcessoPermissaoBL{
     			get{ return ( this._AcessoPermissaoBL = this._AcessoPermissaoBL ?? new AcessoPermissaoBL() );}
     		}
     
     		//responsavel pela montagem do menu principal do sistema
     		[ActionName("exibir-menu-principal")]
     		public PartialViewResult exibirMenuPrincipal() {
     
     			PermissaoVM ViewModel = new PermissaoVM();
     
     			ViewModel.idPerfilAcesso = User.idPerfil();
     
     			ViewModel.listaGrupos  = this.OAcessoPermissaoBL.listar(ViewModel.idPerfilAcesso, 0, 0)
     										.Where(x => x.ativo == "S" && x.AcessoRecursoGrupo.flagMenuLateral == true)
     										.Select(x => new {
     															x.id,
     															x.idGrupo,
     															AcessoRecursoGrupo = new {
     																						x.AcessoRecursoGrupo.id,
     																						x.AcessoRecursoGrupo.descricao,
     																						x.AcessoRecursoGrupo.iconeClasse,
     																						x.AcessoRecursoGrupo.ordem,
     																						x.AcessoRecursoGrupo.controller,
																							x.AcessoRecursoGrupo.area,
																							x.AcessoRecursoGrupo.action,
																							x.AcessoRecursoGrupo.flagMenuLateral,
																							x.AcessoRecursoGrupo.flagMenuTopo,

																					},
															x.idPerfilAcesso,
															x.idRecurso,
															x.idRecursoAcao,
															x.ativo,
															x.flagExcluido
														}).ToListJsonObject<AcessoPermissao>()
														.DistinctBy(x => x.idGrupo)
														.Select(x => x.AcessoRecursoGrupo)
													   .OrderBy(x => x.ordem)
														.ToList();


			int[] idsGrupos = ViewModel.listaGrupos.Where(x => x.flagMenuLateral == true).Select(x => x.id).ToArray();

			ViewModel.listaPermissoes = this.OAcessoPermissaoBL.listar(ViewModel.idPerfilAcesso, 0, 0)
																.Select(x => new {
																					x.id,
																					x.idGrupo,
																					x.idPerfilAcesso,
																					x.idRecurso,
																					x.idRecursoAcao,
																					AcessoRecurso = new {
																											x.AcessoRecurso.id,
																											x.AcessoRecurso.actionPadrao,
																											x.AcessoRecurso.area,
																											x.AcessoRecurso.controller,
																											x.AcessoRecurso.flagAcessoLiberado,
																											x.AcessoRecurso.flagMenu,
																											x.AcessoRecurso.flagExcluido,
																											x.AcessoRecurso.idRecursoGrupo,
																											x.AcessoRecurso.idRecursoPai,
																											x.AcessoRecurso.nomeDisplay,
																											x.AcessoRecurso.ordemExibicao,
																											x.AcessoRecurso.ativo,
																											x.AcessoRecurso.descricao
																										}
																				})
																.Where(x => (x.idRecursoAcao == 0 || x.idRecursoAcao == null) && x.AcessoRecurso.flagMenu == true )
																.ToListJsonObject<AcessoPermissao>()
																.Where(x => idsGrupos.Contains(UtilNumber.toInt32(x.idGrupo)))
																.ToList();


			return PartialView(ViewModel);
		}

        //responsavel pela montagem do menu do topo do sistema
        [ActionName("exibir-menu-topo")]
        public PartialViewResult exibirMenuTopo() {

            PermissaoVM ViewModel = new PermissaoVM();

            ViewModel.idPerfilAcesso = User.idPerfil();

            ViewModel.listaGrupos = this.OAcessoPermissaoBL.listar(ViewModel.idPerfilAcesso, 0, 0)
                                                            .Where(x => x.ativo == "S" && x.AcessoRecursoGrupo.flagMenuTopo == true)
                                                            .ToList()
                                                            .DistinctBy(x => x.idGrupo)
                                                            .Select(x => x.AcessoRecursoGrupo)
                                                            .OrderBy(x => x.ordem)
                                                            .ToList();


            int[] idsGrupos = ViewModel.listaGrupos.Where(x => x.flagMenuTopo == true).Select(x => x.id).ToArray();

            ViewModel.listaPermissoes = this.OAcessoPermissaoBL.listar(ViewModel.idPerfilAcesso, 0, 0)
                                                                .Where(x => (x.idRecursoAcao == 0 || x.idRecursoAcao == null) && x.AcessoRecurso.flagMenu == true)
                                                                .ToList()
                                                                .Where(x => idsGrupos.Contains(UtilNumber.toInt32(x.idGrupo)))
                                                                .ToList();


            return PartialView(ViewModel);
        }
    }
}