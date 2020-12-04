using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Associados;
using BLL.AssociadosContribuicoes;
using DAL.Contribuicoes;
using DAL.Associados;
using DAL.AssociadosContribuicoes;
using MoreLinq;

namespace WEB.Areas.ContribuicoesPainel.ViewModels {

    public class PainelContribuicaoPadraoVM {

        //Atributos
        private IAssociadoBL _AssociadoBL;
        private IAssociadoContribuicaoBL _AssociadoContribuicaoBL;

        //Servicos
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL();

        //Filtros
        public string flagFiltroAssociados { get; set; }

        public string flagSituacao { get; set; }

        //Propriedades
        public Contribuicao Contribuicao { get; set; }

        public List<AssociadoDadosBasicos> listaAssociados{ get; set; }

        public List<AssociadoContribuicao> listaContribuicoes { get; set; }

        public List<DateTime> listaVencimentos { get; set; }

        public List<DateTime> listaDatasSelecionadas { get; set; }

        //Construtor
        public PainelContribuicaoPadraoVM() {
           
            listaContribuicoes = new List<AssociadoContribuicao>();

            listaVencimentos = new List<DateTime>();

            listaDatasSelecionadas = new List<DateTime>();
        }

        /// <summary>
        /// Carregar as informacoes necessarias para dinamizar o painel
        /// </summary>
        public void carregarDadosContribuicao() {

            this.listaContribuicoes = this.OAssociadoContribuicaoBL.listar(Contribuicao.id, 0, null, null, "").ToList();

            this.carregarVencimentos();

            this.filtrarContribuicoes();

            this.carregarFiltros();

            DateTime minVencimento = listaDatasSelecionadas.Min();

            DateTime maxVencimento = listaDatasSelecionadas.Max();

            this.listaContribuicoes = listaContribuicoes.Where(x => x.dtVencimentoOriginal >= minVencimento && x.dtVencimentoOriginal <= maxVencimento).ToList();

            this.carregarAssociados();

            this.filtrarAssociados();
        }

        /// <summary>
        /// Tratamento de informacoes para filtros
        /// </summary>
        public void filtrarContribuicoes() {

            this.flagSituacao = UtilRequest.getString("flagSituacao");

            if (this.flagSituacao == "quitados") {

                this.listaContribuicoes = this.listaContribuicoes.Where(x => x.dtPagamento != null).ToList();

            }

            if (this.flagSituacao == "isentos") {

                this.listaContribuicoes = this.listaContribuicoes.Where(x => x.flagIsento == true).ToList();

            }

            if (this.flagSituacao == "atrasados") {

                this.listaContribuicoes = this.listaContribuicoes.Where(x => x.flagIsento == false && x.flagEmAtraso()).ToList();

            }

            if (this.flagSituacao == "nao_pagos") {

                this.listaContribuicoes = this.listaContribuicoes.Where(x => x.flagIsento == false && x.dtPagamento == null).ToList();

            }

        }

        /// <summary>
        /// Tratamento de informacoes para filtros
        /// </summary>
        public void carregarFiltros() {

            this.flagFiltroAssociados = UtilRequest.getString("flagFiltroAssociados");

            if (!listaDatasSelecionadas.Any()) {

                this.listaDatasSelecionadas = listaVencimentos.TakeLast(12).ToList();

                return;
            }
            
            this.listaDatasSelecionadas = listaDatasSelecionadas.OrderBy(x => x).TakeLast(12).ToList();
        }

        /// <summary>
        /// Montar a lista de vencimentos disponíveis pra consulta por parte do usuário
        /// </summary>
        public void carregarVencimentos() {

            this.Contribuicao.listaContribuicaoVencimento = this.Contribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            var minAno = this.listaContribuicoes.Select(x => x.dtVencimentoOriginal.Year).Min();

            var maxAno = this.listaContribuicoes.Select(x => x.dtVencimentoOriginal.Year).Max();

            var maxData = this.listaContribuicoes.Select(x => x.dtVencimentoOriginal).Max();

            DateTime data = DateTime.MinValue;

            while (minAno <= maxAno && data < maxData) {

                foreach (var OVencimento in Contribuicao.listaContribuicaoVencimento) {

                    data = new DateTime(minAno, UtilNumber.toInt32(OVencimento.mesVencimento), UtilNumber.toInt32(OVencimento.diaVencimento));

                    if (data > maxData) {
                        break;
                    }

                    listaVencimentos.Add(data);

                }

                minAno++;
            }

            listaVencimentos = listaVencimentos.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// Montar a lista de associados que serao visualizados na tela
        /// </summary>
        public void carregarAssociados() {

            if (flagFiltroAssociados == "todos") {

                this.listaAssociados = this.OAssociadoBL.listar(0, "", "", "S")
                                                        .Select(x => new AssociadoDadosBasicos {
                                                            id = x.id,
                                                            idTipoAssociado = x.idTipoAssociado,
                                                            nome = x.Pessoa.nome
                                                        })
                                                        .ToList();

                return;
            }

            if (string.IsNullOrEmpty(flagSituacao)) {
                
                this.listaAssociados = this.OAssociadoContribuicaoBL.listar(Contribuicao.id, 0, null, null, "")
                                                                    .Select(x => new AssociadoDadosBasicos {
                                                                                    id = x.idAssociado,
                                                                                    idTipoAssociado = x.Associado.idTipoAssociado,
                                                                                    nome = x.Associado.Pessoa.nome
                                                                                })
                                                                                .DistinctBy(x => x.id)
                                                                                .ToList();
                return;
            }

            this.listaAssociados = this.listaContribuicoes
                                        .Select(x => new AssociadoDadosBasicos {
                                            id = x.idAssociado,
                                            idTipoAssociado = x.Associado.idTipoAssociado,
                                            nome = x.Associado.Pessoa.nome
                                        })
                                        .DistinctBy(x => x.id)
                                        .ToList();

        }

        /// <summary>
        /// Filtrar os associados de acordo com o preenchimento dos campos
        /// </summary>
        private void filtrarAssociados() {

            string valorBusca = UtilRequest.getString("valorBusca");

            int valorBuscaInt = UtilNumber.toInt32("valorBusca");

            if (string.IsNullOrEmpty(valorBusca)) {
                return;
            }

            this.listaAssociados = this.listaAssociados.Where(x => x.nome.Contains(valorBusca) || x.id == valorBuscaInt).ToList();
        }

        /// <summary>
        /// Montar o combo com as opcoes de vencimentos para filtro
        /// </summary>
        /// <returns></returns>
        public MultiSelectList multiSelectListVencimentos() {

		    var listaOpcoes = listaVencimentos.OrderByDescending(x => x).Select(x => new {
		        text = x.ToShortDateString(),
                value = x.ToShortDateString()
		    }).ToList();

            var listaSelected = listaDatasSelecionadas.Select(x => x.ToShortDateString()).ToList();

            return new MultiSelectList(listaOpcoes, "value", "text", listaSelected);

        }

    }
}