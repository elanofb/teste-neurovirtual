using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroParcelamentos.ViewModels {

    [Validator(typeof(TituloReceitaParcelamentoValidator))]
    public class TituloReceitaParcelamentoForm {

        //Atributos
        private ITituloReceitaConsultaBL _TituloReceitaConsultaBL;

        //Propriedades
        private ITituloReceitaConsultaBL OTituloReceitaConsultaBL => this._TituloReceitaConsultaBL = this._TituloReceitaConsultaBL ?? new TituloReceitaConsultaBL();

        public TituloReceita TituloReceita { get; set; }

        public List<TituloReceitaPagamento> listaPagamentos { get; set; }

        public int qtdeParcelas { get; set; }

        public bool flagTemPagamento { get; set; }

        //
        public TituloReceitaParcelamentoForm() {

            TituloReceita = new TituloReceita();

            this.listaPagamentos = new List<TituloReceitaPagamento>();

        }

        /// <summary>
        /// Carregar informacoes do titulo
        /// </summary>
	    public void carregarTitulo(int id, int idOrganizacao) {

            TituloReceita = this.OTituloReceitaConsultaBL.query(idOrganizacao, false)
                                                         .Where(x => x.id == id)
                                                         .Select(x => new {
                                                             x.id,
                                                             x.idOrganizacao,
                                                             x.descricao,
                                                             x.idTipoReceita,
                                                             x.valorTotal,
                                                             x.valorJuros,
                                                             x.valorDesconto,
                                                             x.nomePessoa,
                                                             x.limiteParcelamento,
                                                             x.dtVencimento,
                                                             TipoReceita = new { id = x.idTipoReceita, x.TipoReceita.descricao }
                                                         })
                                                         .FirstOrDefault()
                                                         .ToJsonObject<TituloReceita>() ?? new TituloReceita();
        }

        /// <summary>
        /// Calcular valores de parcelas
        /// </summary>
        public void carregarParcelas() {

            decimal valorTotalCobranca = TituloReceita.valorTotalComDesconto();

            decimal valorParcela = decimal.Divide(TituloReceita.valorTotalComDesconto(), new decimal(qtdeParcelas));

            decimal valorSomatorioParcelas = 0;

            for (int i = 0; i < qtdeParcelas; i++) {

                var OParcela = listaPagamentos.Count > i? listaPagamentos[i] : new TituloReceitaPagamento();

                OParcela.valorOriginal = Math.Round(valorParcela, 2);

                OParcela.dtVencimento = OParcela.dtVencimento.HasValue? OParcela.dtVencimento: DateTime.Today.AddDays(1).AddMonths(i);

                valorSomatorioParcelas = Math.Round(decimal.Add(valorSomatorioParcelas, valorParcela), 2);

                if (listaPagamentos.Count > i){

                    continue;

                }

                listaPagamentos.Add(OParcela);
            }

            decimal valorDiferenca = decimal.Subtract(valorTotalCobranca, valorSomatorioParcelas);

            var UltimaParcela = listaPagamentos.LastOrDefault() ?? new TituloReceitaPagamento();

            UltimaParcela.valorOriginal = decimal.Add(UltimaParcela.valorOriginal, valorDiferenca);

        }
    }

}