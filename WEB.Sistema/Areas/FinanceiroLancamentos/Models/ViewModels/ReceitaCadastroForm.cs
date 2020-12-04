using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    [Validator(typeof(ReceitaCadastroFormValidator))]
    public class ReceitaCadastroForm {

        public TituloReceita TituloReceita { get; set; }

        public string idReferenciaPessoa { get; set; }

        public string flagValorTotalParcelamento { get; set; }
        public string flagCompleteDtCompetencia { get; set; }

        public decimal valorParcelas { get; set; }

        public int flagTipoRepeticao { get; set; }

        public string urlRetorno { get; set; }
        
        public DateTime? dtPrevisaoPagamento { get; set; }

        public ReceitaCadastroForm() {
            this.TituloReceita = new TituloReceita();
        }

        /// <summary>
        /// Gera o Titulo despesa pagamentos para despesas de parcela única
        /// </summary>
        public void gerarPagamento() {

            if (this.flagTipoRepeticao == TipoRepeticaoConst.PARCELAMENTO) {
                if (this.flagValorTotalParcelamento != "S") {
                    this.TituloReceita.valorTotal = this.TituloReceita.listaTituloReceitaPagamento.Sum(x => x.valorOriginal);
                }
                return;
            }

            this.TituloReceita.qtdeRepeticao = 1;

            this.TituloReceita.listaTituloReceitaPagamento = new List<TituloReceitaPagamento>();

            var OTituloReceitaPagamento = new TituloReceitaPagamento();
            OTituloReceitaPagamento.dtVencimento = this.TituloReceita.dtVencimento.Value;
            OTituloReceitaPagamento.valorOriginal = UtilNumber.toDecimal(this.TituloReceita.valorTotal);
            OTituloReceitaPagamento.dtPrevisaoPagamento = this.dtPrevisaoPagamento ?? this.TituloReceita.dtVencimento;

            this.TituloReceita.listaTituloReceitaPagamento.Add(OTituloReceitaPagamento);
        }


        /// <summary>
        /// Coloca os valores padrões de cada parcela
        /// </summary>
        public void tratarPagamentos() {

            this.TituloReceita.listaTituloReceitaPagamento = this.TituloReceita.listaTituloReceitaPagamento.OrderBy(x => x.dtVencimento).ToList();
            this.TituloReceita.valorTotal = this.TituloReceita.listaTituloReceitaPagamento.Sum(x => x.valorOriginal);

            this.TituloReceita.listaTituloReceitaPagamento.ForEach(Item => {

                Item.idContaBancaria = this.TituloReceita.idContaBancaria;
                Item.idCentroCusto = this.TituloReceita.idCentroCusto;
                Item.idMacroConta = this.TituloReceita.idMacroConta;
                Item.idCategoria = this.TituloReceita.idCategoria;
                Item.idStatusPagamento = StatusPagamentoConst.ABERTO;
                Item.dtVencimentoOriginal = Item.dtVencimento;

                Item.dtPrevisaoPagamento = Item.dtPrevisaoPagamento ?? Item.dtVencimento;
                Item.dtCompetencia = Item.dtCompetencia ?? (this.flagCompleteDtCompetencia == "S" ? Item.TituloReceita.dtCompetencia : Item.dtVencimento);
                
                Item.anoCompetencia = Convert.ToInt16(Item.dtCompetencia?.Year);
                Item.mesCompetencia = Convert.ToByte(Item.dtCompetencia?.Month);
            });
        }

        /// <summary>
        /// Carrega o id da pessoa de acordo com o hash informado 
        /// </summary>
        public void carregarIdPessoa() {

            if (!string.IsNullOrEmpty(idReferenciaPessoa)) {
                var array = idReferenciaPessoa.Split('#');
                this.TituloReceita.flagCategoriaPessoa = array[0];
                this.TituloReceita.idPessoa = Convert.ToInt32(array[1]);
            }
        }

        public SelectList selectListEspecCompetencia(string selected) {
            var list = new[] {
                    new{value = "", text = "Manualmente"},
                    new{value = "N", text = "Pela Dt. Vencimento"},
                    new{value = "S", text = "Pela Dt. Competência da Receita"}
            };
            return new SelectList(list, "value", "text", selected);
        }
    }
}