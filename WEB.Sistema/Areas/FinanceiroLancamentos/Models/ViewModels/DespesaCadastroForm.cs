using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    [Validator(typeof(DespesaCadastroFormValidator))]
    public class DespesaCadastroForm {

        public TituloDespesa TituloDespesa { get; set; }

        public string idReferenciaPessoa { get; set; }

        public int flagTipoRepeticao { get; set; }

        public string flagValorTotalParcelamento { get; set; }

        public decimal valorParcelas { get; set; }
        
        public decimal valorJuros { get; set; }
        public decimal valorMulta { get; set; }
        public decimal valorDesconto { get; set; }

        public string flagCompleteDtCompetencia { get; set; }
        
        public string urlRetorno { get; set; }

        //Propriedades pra criar uma despesa já paga
        public DateTime? dtDebito { get; set; }
        public bool? flagReembolso { get; set; }
        public byte? idMeioPagamento { get; set; }

        public DespesaCadastroForm() {
            this.TituloDespesa = new TituloDespesa();
        }

        /// <summary>
        /// Gera o Titulo despesa pagamentos para despesas de parcela única
        /// </summary>
        public void gerarPagamento() {

            if (this.flagTipoRepeticao == TipoRepeticaoConst.PARCELAMENTO) {
                this.TituloDespesa.listaTituloDespesaPagamento = this.TituloDespesa.listaTituloDespesaPagamento.OrderBy(x => x.dtVencimento).ToList();

                this.TituloDespesa.valorMulta = this.TituloDespesa.listaTituloDespesaPagamento.Sum(x => x.valorMulta);
                this.TituloDespesa.valorJuros = this.TituloDespesa.listaTituloDespesaPagamento.Sum(x => x.valorJuros);
                this.TituloDespesa.valorDesconto = this.TituloDespesa.listaTituloDespesaPagamento.Sum(x => x.valorDesconto);
                
                if (this.flagValorTotalParcelamento != "S") {
                    this.TituloDespesa.valorTotal = this.TituloDespesa.listaTituloDespesaPagamento.Sum(x => x.valorOriginal);
                }
                return;
            }

            this.TituloDespesa.qtdeRepeticao = 1;

            this.TituloDespesa.listaTituloDespesaPagamento = new List<TituloDespesaPagamento>();

            var OTituloDespesaPagamento = new TituloDespesaPagamento();
            OTituloDespesaPagamento.dtVencimento = this.TituloDespesa.dtVencimento.Value;
            
            OTituloDespesaPagamento.nroNotaFiscal = this.TituloDespesa.nroNotaFiscal;
            OTituloDespesaPagamento.nroDocumento = this.TituloDespesa.nroDocumento;
            OTituloDespesaPagamento.nroContrato = this.TituloDespesa.nroContrato;
            
            OTituloDespesaPagamento.valorOriginal = UtilNumber.toDecimal(this.TituloDespesa.valorTotal);
            OTituloDespesaPagamento.flagReembolso = this.flagReembolso;
            OTituloDespesaPagamento.dtDebito = this.dtDebito ?? OTituloDespesaPagamento.dtVencimento;

            if (this.TituloDespesa.dtQuitacao.HasValue) {
                OTituloDespesaPagamento.dtPagamento = this.TituloDespesa.dtQuitacao;
                OTituloDespesaPagamento.idMeioPagamento = this.idMeioPagamento;
                OTituloDespesaPagamento.valorPago = OTituloDespesaPagamento.valorOriginal;
                OTituloDespesaPagamento.idUsuarioCadastro = HttpContextFactory.Current.User.id();
                OTituloDespesaPagamento.dtBaixa = DateTime.Now;
                OTituloDespesaPagamento.flagPago = "S";
            }

            this.TituloDespesa.listaTituloDespesaPagamento.Add(OTituloDespesaPagamento);
        }

        /// <summary>
        /// Gera o Titulo despesa pagamentos para despesas de parcela única
        /// </summary>
        public void tratarPagamentos() {

            this.TituloDespesa.listaTituloDespesaPagamento = this.TituloDespesa.listaTituloDespesaPagamento.OrderBy(x => x.dtVencimento).ToList();
            this.TituloDespesa.valorTotal = this.TituloDespesa.listaTituloDespesaPagamento.Sum(x => x.valorOriginal);

            this.TituloDespesa.listaTituloDespesaPagamento.ForEach(Item => {
                Item.idCentroCusto = this.TituloDespesa.idCentroCusto;
                Item.idMacroConta = this.TituloDespesa.idMacroConta;
                Item.idCategoria = this.TituloDespesa.idCategoria;
                Item.idStatusPagamento = Item.dtPagamento.HasValue ? StatusPagamentoConst.PAGO : StatusPagamentoConst.ABERTO;
                Item.dtVencimento = Item.dtVencimento;

                Item.dtCompetencia = Item.dtCompetencia ?? Item.dtVencimento;

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
                this.TituloDespesa.flagCategoriaPessoa = array[0];
                this.TituloDespesa.idPessoa = Convert.ToInt32(array[1]);
            }
        }

        public SelectList selectListEspecCompetencia(string selected) {
            var list = new[] {
                    new{value = "", text = "Manualmente"},
                    new{value = "N", text = "Pela Dt. Vencimento"},
                    new{value = "S", text = "Pela Dt. Despesa"}
            };
            return new SelectList(list, "value", "text", selected);
        }
    }
}