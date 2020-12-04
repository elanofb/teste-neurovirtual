using BLL.Arquivos;
using BLL.Diretorias;
using DAL.Diretorias;
using DAL.Entities;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using System;
using System.Data.Entity;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using DAL.Arquivos.Extensions;
using WEB.Areas.Diretorias.ViewModels;

namespace WEB.Areas.Diretorias.Controllers {

    public class DiretoriaMembroController : Controller {
        
        //Constantes

        //Atributos
        private IDiretoriaMembroBL _DiretoriaMembroBL;
        private IDiretoriaBL _DiretoriaBL;
        private IArquivoUploadFotoBL _ArquivoUploadFotoBL;

        //Propriedades
        private IDiretoriaMembroBL ODiretoriaMembroBL => _DiretoriaMembroBL = _DiretoriaMembroBL ?? new DiretoriaMembroBL();
        private IDiretoriaBL ODiretoriaBL => _DiretoriaBL = _DiretoriaBL ?? new DiretoriaBL();
        private IArquivoUploadFotoBL OArquivoUploadFotoBL => _ArquivoUploadFotoBL = _ArquivoUploadFotoBL ?? new ArquivoUploadFotoBL();

        //Construtor
        public DiretoriaMembroController() {
        }

        //Bloco Partial para membros da diretoria
        [HttpGet, ActionName("partial-listar")]
        public PartialViewResult partialListar(int idDiretoria) {

            var lista = this.ODiretoriaMembroBL.listar(idDiretoria, true).ToList();

            int[] idDiretoriaMembros = lista.Select(x => x.id).ToArray();

            var listaFotos = this.OArquivoUploadFotoBL.listar(0, EntityTypes.DIRETORIA_MEMBRO, "S")
                                                        .Where(x => idDiretoriaMembros.Contains(x.idReferenciaEntidade))
                                                        .AsNoTracking().ToList();

            lista.ForEach(Item => {
                Item.Arquivo = listaFotos.fotoPrincipal(Item.id);
            });

            return PartialView(lista);
        }

        //
        public ActionResult listar() {

            string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaDiretoriaMembro = this.ODiretoriaMembroBL.listar(0, ativo).OrderBy(x => x.nomeMembro);

            return View(listaDiretoriaMembro.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new DiretoriaMembroForm();

            var ODiretoriaMembro = this.ODiretoriaMembroBL.carregar(UtilNumber.toInt32(id)) ?? new DiretoriaMembro();

            ViewModel.DiretoriaMembro = ODiretoriaMembro;

            return View(ViewModel);
        }

        //
        [HttpPost, ValidateInput(false)]
        public ActionResult editar(DiretoriaMembroForm ViewModel) {

            if(!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.ODiretoriaMembroBL.salvar(ViewModel.DiretoriaMembro, ViewModel.OImagem);

            if(flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { id = ViewModel.DiretoriaMembro.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
        }

        [HttpGet, ActionName("modal-editar")]
        public ActionResult modalEditar(int? id, int idDiretoria) {

            var ViewModel = new DiretoriaMembroForm();

            ViewModel.DiretoriaMembro = this.ODiretoriaMembroBL.carregar(UtilNumber.toInt32(id)) ?? new DiretoriaMembro();

            ViewModel.DiretoriaMembro.idDiretoria = idDiretoria;

            return PartialView(ViewModel);
        }

        [HttpPost, ActionName("salvar-modal-editar"), ValidateInput(false)]
        public ActionResult salvarModalEditar(DiretoriaMembroForm ViewModel) {
            bool flagSucesso;

            if(!ModelState.IsValid) {
                return PartialView("modal-editar", ViewModel);
            }

            int idDiretoria = ViewModel.DiretoriaMembro.idDiretoria ?? 0;

            Diretoria ODiretoria = this.ODiretoriaBL.carregar(idDiretoria) ?? new Diretoria();

            if(ODiretoria.idOrganizacao != User.idOrganizacao() && User.idOrganizacao() > 0) {
                return Json(new { error = true, flagSucesso = false, ViewModel.DiretoriaMembro.id });
            }

            flagSucesso = this.ODiretoriaMembroBL.salvar(ViewModel.DiretoriaMembro, ViewModel.OImagem);

            return Json(new { error = false, flagSucesso, ViewModel.DiretoriaMembro.id });
        }

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
            return Json(this.ODiretoriaMembroBL.alterarStatus(id));
        }

        //
        [HttpPost]
        public ActionResult excluir(int id) {

            var Retorno = new JsonMessage();
            Retorno.error = false;
            Retorno.message = "Os registros informados foram removidos com sucesso.";

            var RetornoItem = this.ODiretoriaMembroBL.excluir(id, User.id());

            if(RetornoItem.flagError == true) {
                Retorno.error = true;
                Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
            }

            return Json(Retorno);
        }
    }
}