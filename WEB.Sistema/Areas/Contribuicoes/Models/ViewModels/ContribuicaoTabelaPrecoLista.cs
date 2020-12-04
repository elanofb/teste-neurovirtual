using System.Collections.Generic;
using System.Linq;
using BLL.Contribuicoes;
using BLL.Associados;
using DAL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels {

    public class ContribuicaoTabelaPrecoLista {

        //Atributos
        private IContribuicaoBL _ContribuicaoBL;
        private IContribuicaoTabelaPrecoBL _ContribuicaoTabelaPrecoBL;
        private IContribuicaoPrecoBL _ContribuicaoPrecoBL;
        private TipoAssociadoBL _TipoAssociadoBL;
        
        //
        private IContribuicaoBL OContribuicaoBL => this._ContribuicaoBL = this._ContribuicaoBL ?? new ContribuicaoPadraoBL();
        private IContribuicaoTabelaPrecoBL OContribuicaoTabelaPrecoBL => this._ContribuicaoTabelaPrecoBL = this._ContribuicaoTabelaPrecoBL ?? new ContribuicaoTabelaPrecoBL();
        private IContribuicaoPrecoBL OContribuicaoPrecoBL => this._ContribuicaoPrecoBL = this._ContribuicaoPrecoBL ?? new ContribuicaoPrecoBL();
        private TipoAssociadoBL OTipoAssociadoBL => this._TipoAssociadoBL = this._TipoAssociadoBL ?? new TipoAssociadoBL();

        //Propriedades
        public Contribuicao Contribuicao { get; set; }

        public ContribuicaoTabelaPreco TabelaPrecoVigente { get; set; }

        public List<ContribuicaoTabelaPreco> listaTabelasAnteriores { get; set; }

        //Construtor
        public ContribuicaoTabelaPrecoLista() {

            this.Contribuicao = new Contribuicao();

            this.TabelaPrecoVigente = new ContribuicaoTabelaPreco();

            this.listaTabelasAnteriores = new List<ContribuicaoTabelaPreco>();
        }

        //
        public void carregarDados(int idContribuicao){

            this.Contribuicao = this.OContribuicaoBL.carregar(idContribuicao);

            var listaTabelas = this.OContribuicaoTabelaPrecoBL.listar(this.Contribuicao.id, true).ToList();

            this.TabelaPrecoVigente = this.Contribuicao.retornarTabelaVigente();

            this.TabelaPrecoVigente.listaPrecos = this.TabelaPrecoVigente.listaPrecos
                                                                         .Where(x => x.TipoAssociado.idOrganizacao == this.Contribuicao.idOrganizacao  && x.flagExcluido == "N")
                                                                         .ToList();

            this.listaTabelasAnteriores = listaTabelas.Where(x => x.id != TabelaPrecoVigente.id).ToList();

        }

    }
}