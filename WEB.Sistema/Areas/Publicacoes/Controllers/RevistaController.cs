using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Publicacoes;
using WEB.Areas.Publicacoes.ViewModels;
using DAL.Entities;
using DAL.Publicacoes;
using PagedList;
using BLL.Arquivos;
using System.Json;
using System.Data.Entity;
using DAL.Arquivos.Extensions;
using MvcFlashMessages;

namespace WEB.Areas.Publicacoes.Controllers {

    public class RevistaController : Controller {

        //Atributos
        private INoticiaBL _NoticiaBL;
		private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private INoticiaBL ONoticiaBL => _NoticiaBL = _NoticiaBL ?? new NoticiaBL();
		private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //Construtor
        public RevistaController() {

        }

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            int idPortal = UtilRequest.getInt32("idPortal");
            string ativo = UtilRequest.getString("flagAtivo");

            var listaRevistas = this.ONoticiaBL.listar(descricao, ativo,TipoNoticiaConst.REVISTA,false, idPortal)
											.OrderByDescending(x => x.id)
                                            .ToList();

			int[] idsRevistas = listaRevistas.Select(x => x.id).ToArray();

			var listaArquivo = this.OArquivoUploadFotoBL.listar(0, EntityTypes.NOTICIA, "S")
								   .Where(x => idsRevistas.Contains(x.idReferenciaEntidade) ).AsNoTracking().ToList();

			listaRevistas.ForEach(Item => {
				Item.Foto = listaArquivo.fotoPrincipal(Item.id);
			});

            return View(listaRevistas.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {
            var ViewModel = new RevistaForm();

            ViewModel.Revista = this.ONoticiaBL.carregar(UtilNumber.toInt32(id)) ?? new Noticia();
            
            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(RevistaForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            ViewModel.Revista.idTipoNoticia = TipoNoticiaConst.REVISTA;

            bool flagSucesso = this.ONoticiaBL.salvar(ViewModel.Revista, ViewModel.Foto, ViewModel.Documento);

            if (flagSucesso) {
                 
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                return RedirectToAction("editar", new { id = ViewModel.Revista.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.ONoticiaBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

			var Retorno = new JsonMessage();
			Retorno.error = false;
			Retorno.message = "Os dados foram removidos com sucesso.";
            
			var RetornoItem = new UtilRetorno();

			RetornoItem.flagError = this.ONoticiaBL.excluir(id);

			if (RetornoItem.flagError) { 
				Retorno.error = true;
				Retorno.message = RetornoItem.listaErros.FirstOrDefault();
			}

            return Json(Retorno);
        }

    }
}