using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class FluxoCaixaDiarioVM {

        // Atributos Serviços
        private IFluxoCaixaBL _FluxoCaixaBL;

        // Propriedades Serviços
        private IFluxoCaixaBL OFluxoCaixaBL => _FluxoCaixaBL = _FluxoCaixaBL ?? new FluxoCaixaBL();

        // Propriedades
        public List<ReceitaDespesaVW> listaPagamentos { get; set; }

        //
        public FluxoCaixaDiarioVM() {
            this.listaPagamentos = new List<ReceitaDespesaVW>();
        }

        //
        public void carregarPagamentos(FluxoCaixaDiarioForm Form) {

            if (!Form.dtInicioPeriodo.HasValue && !Form.dtFimPeriodo.HasValue) {

                Form.dtInicioPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                Form.dtFimPeriodo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            }

            if (Form.dtInicioPeriodo.HasValue && !Form.dtFimPeriodo.HasValue) {
                Form.dtFimPeriodo = Form.dtInicioPeriodo.Value.AddDays(31);
            }

            if(Form.dtFimPeriodo.HasValue && !Form.dtInicioPeriodo.HasValue) {
                Form.dtInicioPeriodo = Form.dtFimPeriodo.Value.AddDays(-31);
            }

            if ((Form.dtFimPeriodo.Value - Form.dtInicioPeriodo.Value).TotalDays > 31) {
                Form.dtFimPeriodo = Form.dtInicioPeriodo.Value.AddDays(31);
            }
            
            var queryPagamentos = this.OFluxoCaixaBL.listar().Where(x =>
                x.idContaBancaria == Form.idContaBancaria &&
                ((x.dtPagamento != null && x.dtEfetivacao == null && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) >= Form.dtInicioPeriodo && DbFunctions.TruncateTime(x.dtPrevisaoEfetivacao) <= Form.dtFimPeriodo) || 
                (x.dtPagamento != null && x.dtEfetivacao != null && DbFunctions.TruncateTime(x.dtEfetivacao) >= Form.dtInicioPeriodo && DbFunctions.TruncateTime(x.dtEfetivacao) <= Form.dtFimPeriodo)) 
            );

            this.listaPagamentos = queryPagamentos.OrderBy(x => x.dtVencimento).ThenBy(x => x.dtCadastro).ToList();
            
            foreach (var Item in this.listaPagamentos) {

                Item.descricaoTitulo = $"{Item.nomePessoa} - {Item.descricaoTitulo}";

                Item.dtMovimento = Item.dtEfetivacao.HasValue ? Item.dtEfetivacao : Item.dtPrevisaoEfetivacao;

                if (!Item.dtMovimento.HasValue) {
                    Item.dtMovimento = Item.dtVencimento;
                }
            }

            this.listaPagamentos = this.listaPagamentos.OrderBy(x => x.dtMovimento).ToList();
        }
    }
}
