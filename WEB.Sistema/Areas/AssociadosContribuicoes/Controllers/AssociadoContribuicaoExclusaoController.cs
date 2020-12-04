using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.App_Infrastructure;

namespace WEB.Areas.AssociadosContribuicoes.Controllers{

    public class AssociadoContribuicaoExclusaoController : BaseSistemaController{

		//Atributos
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL; 
		private IAssociadoContribuicaoExclusaoBL _AssociadoContribuicaoExclusaoBL; 

		//Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => this._AssociadoContribuicaoBL = this._AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();
        private IAssociadoContribuicaoExclusaoBL OAssociadoContribuicaoExclusaoBL => this._AssociadoContribuicaoExclusaoBL = this._AssociadoContribuicaoExclusaoBL ?? new AssociadoContribuicaoExclusaoBL();

		//Abertura do modal para configurar a exclusão da anuidade
		[ActionName("partial-excluir-contribuicao"), HttpGet]
		public PartialViewResult partialExcluirContribuicao(int id) {
            
			AssociadoContribuicao OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(id);

			return PartialView(OAssociadoContribuicao);
		}
	    
		//Exclusão da anuidade
		[ActionName("excluir-contribuicao"), HttpPost]
		public ActionResult excluirContribuicao(AssociadoContribuicao ViewModel, string observacoes) {
            
			AssociadoContribuicao OAssociadoContribuicao = this.OAssociadoContribuicaoBL.carregar(ViewModel.id);

			if (OAssociadoContribuicao == null) { 
				ModelState.AddModelError("", "A cobrança não pôde ser localizado.");
				return PartialView("partial-excluir-contribuicao", ViewModel);
			}

			if (String.IsNullOrEmpty(observacoes)) { 
				ModelState.AddModelError("observacoes", "Informe o motivo para a exclusão.");
				return PartialView("partial-excluir-contribuicao", ViewModel);
			}

            if (observacoes.Length > 100) { 
				ModelState.AddModelError("observacoes", "O motivo para a exclusão não pode ultrapassar 100 caracteres.");
				return PartialView("partial-excluir-contribuicao", ViewModel);
			}

			this.OAssociadoContribuicaoExclusaoBL.excluir(ViewModel.id, observacoes, User.id());

			this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A contribuição foi excluída com sucesso!");

            return Json( new{ flagSucesso = true} );
		}
	    
	    //Abertura do modal para configurar a exclusão da anuidade
	    [ActionName("partial-excluir-contribuicao-lote")]
	    public PartialViewResult partialExcluirContribuicaoLote(List<int> ids) {
            
		    List<AssociadoContribuicao> listaAssociadoContribuicao = this.OAssociadoContribuicaoBL.query(User.idOrganizacao())
			    .Where(x => ids.Contains(x.id))
			    .Select(x => new {
					x.id,
				    x.valorAtual,
				    x.dtVencimentoAtual,
				    Associado = new {
					    x.Associado.id,
					    x.Associado.nroAssociado,
						Pessoa = new {
							x.Associado.Pessoa.nome
						},
					    TipoAssociado = new {
						    x.Associado.TipoAssociado.descricao
					    }
					}
				}).ToListJsonObject<AssociadoContribuicao>();

		    return PartialView(listaAssociadoContribuicao);
	    }
	    
	    
	    //Exclusão da anuidade
	    [ActionName("excluir-contribuicao-lote"), HttpPost]
	    public ActionResult excluirContribuicaoLote(List<int> ids, string observacoes) {

		    ids = ids ?? new List<int>();
            
		    var listaAssociadoContribuicao = this.OAssociadoContribuicaoBL.query(User.idOrganizacao())
																		.Where(x => ids.Contains(x.id))
																		.Select(x => new {
																			x.id,
																			x.valorAtual,
																			x.dtVencimentoAtual,
																			Associado = new {
																				x.Associado.id,
																				x.Associado.nroAssociado,
																				Pessoa = new {
																					x.Associado.Pessoa.nome
																				},
																				TipoAssociado = new {
																					x.Associado.TipoAssociado.descricao
																				}
																			}
																		}).ToListJsonObject<AssociadoContribuicao>();

		    if (!listaAssociadoContribuicao.Any()) { 
			    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha!", "Nenhuma cobrança foi localizada para exclusão."));
			    ModelState.AddModelError("observacoes", "As cobranças não foram localizadas.");
			    return PartialView("partial-excluir-contribuicao-lote", listaAssociadoContribuicao);
		    }

		    if (String.IsNullOrEmpty(observacoes)) { 
			    ModelState.AddModelError("observacoes", "Informe o motivo para a exclusão.");
			    return PartialView("partial-excluir-contribuicao-lote", listaAssociadoContribuicao);
		    }

		    if (observacoes.Length > 100) { 
			    ModelState.AddModelError("observacoes", "O motivo para a exclusão não pode ultrapassar 100 caracteres.");
			    return PartialView("partial-excluir-contribuicao-lote", listaAssociadoContribuicao);
		    }

		    foreach (var OContribuicao in listaAssociadoContribuicao) {
			    this.OAssociadoContribuicaoExclusaoBL.excluir(OContribuicao.id, observacoes, User.id());
		    }

		    this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "As contribuições foram excluídas com sucesso!");

		    return Json( new{ flagSucesso = true} );
	    }
    }
}
