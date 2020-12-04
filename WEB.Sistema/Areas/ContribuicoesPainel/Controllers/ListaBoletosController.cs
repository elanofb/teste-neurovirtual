using System;
using System.Linq;
using System.Web.Mvc;
using BLL.AssociadosContribuicoes;
using MvcFlashMessages;
using WEB.Areas.ContribuicoesPainel.ViewModels;
using DAL.AssociadosContribuicoes;
using System.Collections.Generic;
using BLL.Financeiro;
using PagedList;

namespace WEB.Areas.ContribuicoesPainel.Controllers {

    [OrganizacaoFilter]
    public class ListaBoletosController : Controller {

        //Atributos
        private IAssociadoContribuicaoBoletoBL _AssociadoContribuicaoBoletoBL;
        private ITituloReceitaPagamentoExclusaoBL _TituloReceitaPagamentoExclusaoBL;

        //Propriedades
        private IAssociadoContribuicaoBoletoBL OAssociadoContribuicaoBoletoBL => _AssociadoContribuicaoBoletoBL = _AssociadoContribuicaoBoletoBL ?? new AssociadoContribuicaoBoletoBL();
        private ITituloReceitaPagamentoExclusaoBL OTituloReceitaPagamentoExclusaoBL => _TituloReceitaPagamentoExclusaoBL = _TituloReceitaPagamentoExclusaoBL ?? new TituloReceitaPagamentoExclusaoBL();

        /// <summary>
        /// Gerar as cobranças
        /// </summary>
        [ActionName("listar")]
        public ActionResult listar() {

            int idContribuicao = UtilRequest.getInt32("idContribuicao");

            int ano = UtilRequest.getInt32("ano");

            var ViewModel = new PainelCobrancaVM();

            if (idContribuicao == 0 || ano == 0) {
                return View(ViewModel);
            }

            ViewModel.carregarDadosContribuicao(idContribuicao, null);

            ViewModel.listaBoletos = ViewModel.carregarBoletos()
                                              .Where(x => x.idMeioPagamento > 0 && x.dtExclusaoParcela == null && !string.IsNullOrEmpty(x.boletoUrl))
                                              .OrderBy(x => x.idTituloReceitaPagamento)
                                              .ToPagedList(UtilRequest.getNroPagina(), 200);

            return View(ViewModel);
        }

        /// <summary>
        /// Exportar Zip
        /// </summary>
        public ActionResult exportar() {

            int idContribuicao = UtilRequest.getInt32("idContribuicao");
            int ano = UtilRequest.getInt32("ano");

            var ViewModel = new PainelCobrancaVM();

            var listaIds = UtilRequest.getListInt("id");

            if ((idContribuicao == 0 || ano == 0) && !listaIds.Any()) {
                return Json(new {error = true, message = "Nenhum boleto foi localizado para ser realizado a exportação" });
            }

            
            var listaUrlBoletos = ViewModel.carregarBoletosExportacao(idContribuicao, listaIds).Where(x => !x.isEmpty()).ToList();
            
            if (!listaUrlBoletos.Any()) {
                return Json(new { error = true, message = "Nenhum boleto foi localizado para ser realizado a exportação" });
            }

            var nomeArquivoZip = "";

            return Json(new { error = false, nomeArquivo = nomeArquivoZip, totalRegistros = listaUrlBoletos.Count }, JsonRequestBehavior.AllowGet);
        }


        [ActionName("download-zip")]
        public FileResult downloadZip(string nomeArquivo) {

            var ODownload = File(UtilConfig.pathAbsTempFiles + nomeArquivo, "application/zip", nomeArquivo);

            return ODownload;
        }

        //Abertura do modal para configurar a exclusão da anuidade
        [ActionName("modal-excluir-boleto"), HttpGet]
        public PartialViewResult modalExcluirBoleto() {

            var listaIds = UtilRequest.getListInt("id");

            var listaAssociadoContribuicao = this.OAssociadoContribuicaoBoletoBL.listar(0).Where(x => listaIds.Contains(x.idTituloReceitaPagamento.Value)).ToList();

            return PartialView(listaAssociadoContribuicao);
        }

        //Exclusão da anuidade
        [ActionName("excluir-boleto"), HttpPost]
        public ActionResult excluirContribuicao(List<AssociadoContribuicaoBoleto> ViewModel, string observacoes) {

            if (String.IsNullOrEmpty(observacoes)) {
                ModelState.AddModelError("observacoes", "Informe o motivo para a exclusão.");
                return PartialView("modal-excluir-boleto", ViewModel);
            }

            if (observacoes.Length > 100) {
                ModelState.AddModelError("observacoes", "O motivo para a exclusão não pode ultrapassar 100 caracteres.");
                return PartialView("modal-excluir-boleto", ViewModel);
            }

            var errors = "";
            foreach (var OItem in ViewModel){

                var ORetorno = this.OTituloReceitaPagamentoExclusaoBL.excluirPagamento(OItem.idTituloReceitaPagamento.toInt(), observacoes);

                if (ORetorno.flagError == true) {
                    errors += $"O boleto #{OItem.id} não pôde ser removido, {ORetorno.listaErros.FirstOrDefault()}.<br/>";
                }
            }

            if (!errors.isEmpty()) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, errors);

                return Json(new { error = false });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os boletos foram removidos com sucesso");

            return Json(new { error = false });
        }
    }
}