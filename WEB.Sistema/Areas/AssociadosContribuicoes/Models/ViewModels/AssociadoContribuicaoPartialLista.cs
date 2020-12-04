using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosContribuicoes;
using BLL.Financeiro;
using DAL.Associados;
using DAL.Financeiro;

namespace WEB.Areas.AssociadosContribuicoes.ViewModels {

    public class AssociadoContribuicaoPartialLista {

        //Atributos
        private IAssociadoContribuicaoResumoBL _AssociadoContribuicaoResumoBL;
        private ITituloReceitaResumoBL _TituloReceitaResumoBL;

        //
        private IAssociadoContribuicaoResumoBL OAssociadoContribuicaoResumoBL => this._AssociadoContribuicaoResumoBL = this._AssociadoContribuicaoResumoBL ?? new AssociadoContribuicaoResumoBL();
        private ITituloReceitaResumoBL OTituloReceitaResumoBL => this._TituloReceitaResumoBL = this._TituloReceitaResumoBL ?? new TituloReceitaResumoBL();

        //Propriedades
        public TituloReceitaResumoVW TaxaInscricao { get; set; }

        public List<AssociadoContribuicaoItemLista> listaContribuicoes { get; set; }

        public int qtdeCobrancas { get; set; }


        //Construtor
        public AssociadoContribuicaoPartialLista() {

            this.listaContribuicoes = new List<AssociadoContribuicaoItemLista>();

        }

        /// <summary>
        /// Carregamento de itens de contribuicoes para exibicao
        /// </summary>
        public void carregarContribuicoes(Associado OAssociado) {

            var listaGeral = this.OAssociadoContribuicaoResumoBL.listar(0, 0)
                                                                .Where(x => x.idAssociadoEstipulante == OAssociado.id || x.idAssociado == OAssociado.id)
                                                                .OrderByDescending(x => x.dtVencimentoOriginal)
                                                                .ToList();

            var listaCobrancasPrincipais = listaGeral.Where(x => x.idAssociadoContribuicaoPrincipal == null || x.idAssociadoContribuicaoPrincipal == 0)
                                                .ToList();

            foreach (var ItemCobranca in listaCobrancasPrincipais) {

                var listaDependentes = listaGeral.Where(x => x.idAssociadoContribuicaoPrincipal == ItemCobranca.id)
                                                            .ToList();

                var ItemLista = new AssociadoContribuicaoItemLista().carregarDados(ItemCobranca, listaDependentes);

                this.listaContribuicoes.Add(ItemLista);
            }

        }

        /// <summary>
        /// Carregar dados da taxa de inscricao (se houver)
        /// </summary>
        public void carregarInscricoes(Associado OAssociado) {

            this.TaxaInscricao = this.OTituloReceitaResumoBL.listar("")
                                                            .FirstOrDefault(x => x.idTipoReceita == TipoReceitaConst.TAXA_INSCRICAO && x.idReceita == OAssociado.id) ?? new TituloReceitaResumoVW();

        }
    }

}

