using System.Collections.Generic;
using System.Linq;
using BLL.Arquivos;
using DAL.ContasBancarias;
using DAL.Entities;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class GestaoTituloVM {

        //Atributos
        public List<ContaBancaria> listaContas { get; set; }
        public List<GestaoTituloVW> listaExtrato { get; set; }
        public decimal valorTotalReceitas { get; set; }
        public decimal valorTotalDespesas { get; set; }
        public decimal valorTotal { get; set; }
        private IArquivoUploadBL _ArquivoUploadBL;

        //Propriedades
        private IArquivoUploadBL OArquivoUploadBL { get { return (this._ArquivoUploadBL = this._ArquivoUploadBL ?? new ArquivoUploadBL()); } }

        public void validarArquivo() {

            this.listaExtrato.ForEach(x => {

                if(x.tipoMovimentacao == TipoMovimentacaoConst.DESPESA) {
                    x.flagArquivo = OArquivoUploadBL.listarDocumentos(x.idTitulo, EntityTypes.TITULODESPESA).Any();

                    if(!x.flagArquivo) {
                        x.flagArquivo = OArquivoUploadBL.listarDocumentos(x.idPagamento, EntityTypes.TITULODESPESAPAGAMENTO).Any();
                    }
                }

                if(x.tipoMovimentacao == TipoMovimentacaoConst.RECEITA) {
                    x.flagArquivo = OArquivoUploadBL.listarDocumentos(x.idTitulo, EntityTypes.TITULORECEITA).Any();

                    if(!x.flagArquivo) {
                        x.flagArquivo = OArquivoUploadBL.listarDocumentos(x.idPagamento, EntityTypes.TITULORECEITAPAGAMENTO).Any();
                    }
                }
            });
        }
    }
}
