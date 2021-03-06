﻿using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosOperacoes;
using DAL.Associados.DTO;
using MvcFlashMessages;
using WEB.Areas.AssociadosOperacoes.ViewModels;

namespace WEB.Areas.AssociadosOperacoes.Controllers {

	public class AssociadoExclusaoController : Controller {

		//Atributos
        private IAssociadoBL _IAssociadoBL;
		private IAssociadoExclusaoBL _IAssociadoExclusaoBL;

		//Propriedades
        private IAssociadoBL OAssociadoBL => _IAssociadoBL = _IAssociadoBL ?? new AssociadoBL();
		private IAssociadoExclusaoBL OAssociadoExclusaoBL => _IAssociadoExclusaoBL = _IAssociadoExclusaoBL ?? new AssociadoExclusaoBL();
        
	    //Abertura do modal para configurar a exclusão dos associados
		[HttpPost, ActionName("modal-excluir-associados")]
		public ActionResult modalExcluirAssociados(AssociadoFiltroVM DadosConsulta) {
            
			var ViewModel = new AssociadoExclusaoForm();
            
            ViewModel.listaAssociados = DadosConsulta.montarQuery()
                                                     .Select(x => new ItemListaAssociado {
                                                         id = x.id, nome = x.nome,
                                                         nroAssociado = x.nroAssociado,
                                                         descricaoTipoAssociado = x.descricaoTipoAssociado,
                                                         idPessoa = x.idPessoa
                                                     }).OrderBy(x => x.nome).ToList();

		    if (!ViewModel.listaAssociados.Any()) {

		        return Json(new { error = true, message = "Nenhum associado foi encontrado para realizar o desligamento." }, JsonRequestBehavior.AllowGet);

		    }

		    ViewModel.idsAssociados = ViewModel.listaAssociados.Select(x => x.id).ToList();

			return PartialView(ViewModel);

		}

        //
        [HttpPost, ActionName("excluir-associados")]
        public ActionResult excluirAssociados(AssociadoExclusaoForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.listaAssociados = this.OAssociadoBL.listar(0, "", "", "")
                                                .Where(x => ViewModel.idsAssociados.Contains(x.id))
                                                .Select(x => new ItemListaAssociado {
                                                    id = x.id, nome = x.Pessoa.nome, nroAssociado = x.nroAssociado,
                                                    descricaoTipoAssociado = x.TipoAssociado.nomeDisplay,
                                                }).OrderBy(x => x.nome).ToList();

                return View("modal-excluir-associados", ViewModel);    

            }

            var ORetorno = this.OAssociadoExclusaoBL.excluirAssociados(ViewModel.idsAssociados, ViewModel.idMotivoDesligamento.toInt(), ViewModel.motivoExclusao);

            if (!ORetorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", ORetorno.listaErros.FirstOrDefault()));

                return Json(new { error = false }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() }, JsonRequestBehavior.AllowGet);

        }

        
	}
}
