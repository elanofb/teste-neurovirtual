using System;
using System.Collections.Generic;
using System.Linq;
using DAL.AssociadosContribuicoes;
using DAL.Pessoas;
using WEB.Areas.ContribuicoesPainel.ViewModels;

namespace WEB.Areas.AssociadosContribuicoes.ViewModels {

    public class AssociadoContribuicaoItemLista {

        public AssociadoContribuicaoResumoVW AssociadoContribuicao { get; set; }

        public List<AssociadoContribuicaoResumoVW> listaCobrancasDependentes { get; set; }

        public DTOExcelAssociadoContribuicao AssociadoExcel { get; set; }

        public List<PessoaTelefone> listTelefones { get; set; }

        public List<PessoaEmail> listEmails { get; set; }

        public int qtdeDependentes { get; set; }

        public decimal valorAtualFinal { get; set; }

        public decimal valorOriginalFinal { get; set; }

        public string detalheValores { get; set; }

        public string urlBoleto { get; set; }

        public int? idTituloReceitaPagamento { get; set; }

        public bool? flagDescontoAntecipacao { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public AssociadoContribuicaoItemLista() {

            this.AssociadoExcel = new DTOExcelAssociadoContribuicao();

            this.listaCobrancasDependentes = new List<AssociadoContribuicaoResumoVW>();

        }

        /// <summary>
        /// Montar dados do objeto
        /// </summary>
        public AssociadoContribuicaoItemLista carregarDados(AssociadoContribuicaoResumoVW ItemCobranca, List<AssociadoContribuicaoResumoVW> listaDependentes) {

            this.AssociadoContribuicao = ItemCobranca;

            this.detalheValores = String.Concat(ItemCobranca.nomeAssociado, ": ", ItemCobranca.valorAtual.ToString("C"), "<br />");

            if (!listaDependentes.Any()) {

                this.valorAtualFinal = ItemCobranca.valorAtual;

                this.valorOriginalFinal = ItemCobranca.valorOriginal;

                

                return this;
            }

            foreach (var ODependente in listaDependentes) {

                this.detalheValores = String.Concat(this.detalheValores, ODependente.nomeAssociado, ": ", ODependente.valorAtual.ToString("C"), "<br />");

            }


            decimal valorAtualDependentes = listaDependentes.Sum(x => x.valorAtual);

            decimal valorOriginalDependentes = listaDependentes.Sum(x => x.valorOriginal);

            this.valorAtualFinal = decimal.Add(valorAtualDependentes, ItemCobranca.valorAtual);

            this.valorOriginalFinal = decimal.Add(valorOriginalDependentes, ItemCobranca.valorOriginal);

            this.qtdeDependentes = listaDependentes.Count;

            this.listaCobrancasDependentes = listaDependentes;

            return this;
        }
    }
}