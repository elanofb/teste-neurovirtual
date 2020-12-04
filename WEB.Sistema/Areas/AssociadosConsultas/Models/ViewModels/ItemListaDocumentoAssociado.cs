using System.Collections.Generic;
using DAL.Arquivos;
using PagedList;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoDocumentoVM {

        public IPagedList<ArquivoAssociadoVW> listaArquivoAssociado { get; set; }

        public AssociadoDocumentoVM() {
            listaArquivoAssociado = new List<ArquivoAssociadoVW>().ToPagedList<ArquivoAssociadoVW>(1, 100);
        }
    }
}