using PagedList;
using System.Collections.Generic;
using DAL.Associados;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoEmailConsultaForm {
        
        public int? idTipoCadastro { get; set; }

        public List<int> idsTipoAssociado { get; set; }

        public string ativo { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string valorBusca { get; set; }

        public int? idTipoEmail { get; set; }
        
        public string flagTipoSaida { get; set; }

        
        public IPagedList<AssociadoEmailVW> listaEmails { get; set; }

        //
        public AssociadoEmailConsultaForm() {
            
            this.listaEmails = new List<AssociadoEmailVW>().ToPagedList(1, 20);

        }

    }
}