using DAL.Contatos;
using PagedList;
using System.Collections.Generic;

namespace WEB.Areas.Contatos.ViewModels {

    public class ContatoConsulta {

        public IPagedList<PessoaContatoVW> listaPessoaContato { get; set; }
        
        public List<int> idsTipoAssociado { get; set; }

        public List<int> idsTipoContato { get; set; }

        public string flagSituacaoContribuicao { get; set; }

        public string ativo { get; set; }

        public ContatoConsulta() {

            this.idsTipoAssociado = new List<int>();

            this.idsTipoContato = new List<int>();

        }
    }

}