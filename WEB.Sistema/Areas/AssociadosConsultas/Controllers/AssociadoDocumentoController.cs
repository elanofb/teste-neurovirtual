using System;
using System.Web.Mvc;
using BLL.Arquivos;
using PagedList;
using WEB.App_Infrastructure;
using WEB.Areas.AssociadosConsultas.ViewModels;
using System.Linq;

namespace WEB.Areas.AssociadosConsultas.Controllers{
    public class AssociadoDocumentoController : BaseSistemaController {
        
        private IArquivoAssociadoVWBL _ArquivoAssociadoVWBL;
        private IExportarArquivoAssociadoVWBL _ExportarArquivoAssociadoVWBL;
        
        private IArquivoAssociadoVWBL OArquivoAssociadoVWBL => this._ArquivoAssociadoVWBL = this._ArquivoAssociadoVWBL ?? new ArquivoAssociadoVWBL();
        private IExportarArquivoAssociadoVWBL OExportarArquivoAssociadoVWBL => this._ExportarArquivoAssociadoVWBL = this._ExportarArquivoAssociadoVWBL ?? new ExportarArquivoAssociadoVWBL();

        // GET: AssociadosConsultas/AssociadoDocumento
        public ActionResult Index(){

            var idsTipoAssociado = UtilRequest.getListInt("idsTipoAssociado");
            var flagSituacaoContribuicao = UtilRequest.getString("flagSituacaoContribuicao");
            var valorBuscaAssociado = UtilRequest.getString("valorBuscaAssociado");
            var idEntidadeArquivo = UtilRequest.getInt32("idEntidadeArquivo");
            var formatoArquivo = UtilRequest.getString("formatoArquivo");
            var ativo = UtilRequest.getString("ativo");
            var valorBusca = UtilRequest.getString("valorBusca");

            var listaDocumentos = OArquivoAssociadoVWBL.listar(idsTipoAssociado, flagSituacaoContribuicao, valorBuscaAssociado, idEntidadeArquivo, formatoArquivo, valorBusca, ativo);
            
            var ViewModel = new AssociadoDocumentoVM();

            ViewModel.listaArquivoAssociado = listaDocumentos.OrderByDescending(x => x.dtCadastro).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

            return View(ViewModel);
        }

        /// <summary>
        /// Exportar Zip
        /// </summary>
        public ActionResult exportar() {

            var ViewModel = new AssociadoDocumentoVM();

            var listaIds = UtilRequest.getListInt("id");

            if (!listaIds.Any()) {
                return Json(new { error = true, message = "Nenhum arquivo foi localizado para ser realizado a exportação" });
            }
            
            var listaUrlArquivos = OArquivoAssociadoVWBL.carregarArquivosExportacao(listaIds);

            if (!listaUrlArquivos.Any()) {
                return Json(new { error = true, message = "Nenhum arquivo foi localizado para ser realizado a exportação" });
            }

            var nomeArquivoZip = this.OExportarArquivoAssociadoVWBL.exportar(listaUrlArquivos);

            return Json(new { error = false, nomeArquivo = nomeArquivoZip, totalRegistros = listaUrlArquivos.Count }, JsonRequestBehavior.AllowGet);
        }


        [ActionName("download-zip")]
        public FileResult downloadZip(string nomeArquivo) {

            var ODownload = File(UtilConfig.pathAbsTempFiles + nomeArquivo, "application/zip", nomeArquivo);

            return ODownload;
        }
    }
}