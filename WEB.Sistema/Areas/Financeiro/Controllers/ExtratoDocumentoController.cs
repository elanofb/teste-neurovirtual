using System.Linq;
using System.Web.Mvc;
using BLL.Arquivos;
using DAL.Entities;
using DAL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

	public class ExtratoDocumentoController : Controller {

		//Constantes

		//Atributos
        private IArquivoUploadBL _ArquivoUploadBL;

        //Propriedades
        private IArquivoUploadBL OArquivoUploadBL { get { return (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL()); } }

        //
        public ExtratoDocumentoController() { 
        }
        
        // Listagem de arquivos diversos formatos 
        [ActionName("listar")]
        public PartialViewResult listarDocumentos(int idTitulo, int idPagamento, string tipoMovimentacao) {

            var VM = new ExtratoDocumentoVM();

            if(tipoMovimentacao == TipoMovimentacaoConst.DESPESA) {
                var listaArquivosTitulo = OArquivoUploadBL.listarDocumentos(idTitulo, EntityTypes.TITULODESPESA).ToList();
                listaArquivosTitulo.ForEach(o => { VM.listaArquivoTitulo.Add(o); });

                var listaArquivosPagamento =
                    OArquivoUploadBL.listarDocumentos(idPagamento, EntityTypes.TITULODESPESAPAGAMENTO).ToList();
                listaArquivosPagamento.ForEach(o => { VM.listaArquivoPagamento.Add(o); });
            }

            if(tipoMovimentacao == TipoMovimentacaoConst.RECEITA) {
                var listaArquivosTitulo = OArquivoUploadBL.listarDocumentos(idTitulo, EntityTypes.TITULORECEITA).ToList();
                listaArquivosTitulo.ForEach(o => { VM.listaArquivoTitulo.Add(o); });

                var listaArquivosPagamento = OArquivoUploadBL.listarDocumentos(idPagamento, EntityTypes.TITULORECEITAPAGAMENTO).ToList();
                listaArquivosPagamento.ForEach(o => { VM.listaArquivoPagamento.Add(o); });
            }

            if(tipoMovimentacao == TipoMovimentacaoConst.TRANSFERENCIA_DEBITO || tipoMovimentacao == TipoMovimentacaoConst.TRANSFERENCIA_RECEITA) {
                var listaArquivosTitulo = OArquivoUploadBL.listarDocumentos(idTitulo, EntityTypes.TRANSFERENCIABANCARIA).ToList();
                listaArquivosTitulo.ForEach(o => { VM.listaArquivoTransferencia.Add(o); });
            }

            return PartialView(VM);
        }
    }
}
