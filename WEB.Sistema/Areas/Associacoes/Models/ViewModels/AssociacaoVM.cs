using System.Collections.Generic;
using DAL.Configuracoes;
using DAL.Organizacoes;
using PagedList;

namespace WEB.Areas.Associacoes.ViewModels {

    //
    public class AssociacaoVM{

        public List<ConfiguracaoSistema> listaConfiguracaoSistema { get; set; }

        public IPagedList<Organizacao> listaAssociacoes { get; set; }

        public AssociacaoVM() {

            listaAssociacoes = new List<Organizacao>().ToPagedList(1, 20);
            listaConfiguracaoSistema = new List<ConfiguracaoSistema>();
        }
    }
}