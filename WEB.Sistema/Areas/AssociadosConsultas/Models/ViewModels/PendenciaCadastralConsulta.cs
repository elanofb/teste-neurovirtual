using DAL.Associados;
using PagedList;
using System.Collections.Generic;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class PendenciaCadastralConsulta {

        public IPagedList<PendenciaCadastralVW> listaPendenciaCadastral { get; set; }
        
        public List<int> idsTipoAssociado { get; set; }

        public int? qtdEmails { get; set; }

        public int? qtdTel { get; set; }

        public int? qtdEnderecos { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        public PendenciaCadastralConsulta() {

            this.idsTipoAssociado = new List<int>();

        }
    }

}