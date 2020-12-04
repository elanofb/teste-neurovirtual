using System.Collections.Generic;
using DAL.Arquivos;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ExtratoDocumentoVM {

        //Atributos
        public List<ArquivoUpload> listaArquivoTitulo { get; set; }

        public List<ArquivoUpload> listaArquivoPagamento { get; set; }

        public List<ArquivoUpload> listaArquivoTransferencia { get; set; }

        public ExtratoDocumentoVM() {
            this.listaArquivoTitulo = new List<ArquivoUpload>();
            this.listaArquivoPagamento = new List<ArquivoUpload>();
            this.listaArquivoTransferencia = new List<ArquivoUpload>();
        }
    }
}