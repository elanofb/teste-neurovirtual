using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ResultadoFinanceiroVM {

        // Atributos Serviços
        private IReceitasDespesasVWBL _IReceitasDespesasVWBL;

        // Propriedades Serviços
        private IReceitasDespesasVWBL OReceitasDespesasVWBL => _IReceitasDespesasVWBL = _IReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();

        // Propriedades
        public List<ReceitaDespesaVW> listaPagamentos { get; set; }

        //
        public ResultadoFinanceiroVM() {

            this.listaPagamentos = new List<ReceitaDespesaVW>();
            
        }

        //
        public void carregarPagamentos(ResultadoFinanceiroForm Form) {

            var queryPagamentos = this.OReceitasDespesasVWBL.listar();
            
            if (Form.tipoBuscaPeriodo.Equals("dtPagamento")) {
                queryPagamentos = this.filtrarPorPagamento(Form, queryPagamentos);
            }

            if (Form.tipoBuscaPeriodo.Equals("dtCompetencia")) {
                queryPagamentos = this.filtrarPorCompetencia(Form, queryPagamentos);
            }

            if (Form.tipoBuscaPeriodo.Equals("dtVencimento")) {
                queryPagamentos = this.filtrarPorVencimento(Form, queryPagamentos);
            }

            if (Form.idsCentroCusto.Any()) {
                queryPagamentos = queryPagamentos.Where(x => Form.idsCentroCusto.Contains(x.idCentroCusto));
            }

            if (Form.idsMacroConta.Any()) {
                queryPagamentos = queryPagamentos.Where(x => Form.idsMacroConta.Contains(x.idMacroConta));
            }

            if (Form.idsSubConta.Any()) {
                queryPagamentos = queryPagamentos.Where(x => Form.idsSubConta.Contains(x.idSubConta));
            }

            if (!Form.flagTipoTitulo.isEmpty()) {
                queryPagamentos = queryPagamentos.Where(x => x.flagTipoTitulo.Equals(Form.flagTipoTitulo));
            }

            this.listaPagamentos = queryPagamentos.Where(x => x.valorRealizado > 0).OrderBy(x => x.idPagamento).ToList();
            
        }

        //
        private IQueryable<ReceitaDespesaVW> filtrarPorPagamento(ResultadoFinanceiroForm Form, IQueryable<ReceitaDespesaVW> queryPagamentos) {

            queryPagamentos = queryPagamentos.Where(x => x.dtPagamento >= Form.dtInicioPeriodo);

            var dtFiltro = Form.dtFimPeriodo.Value.AddDays(1);
            queryPagamentos = queryPagamentos.Where(x => x.dtPagamento < dtFiltro);

            return queryPagamentos;
        }

        //
        private IQueryable<ReceitaDespesaVW> filtrarPorCompetencia(ResultadoFinanceiroForm Form, IQueryable<ReceitaDespesaVW> queryPagamentos) {

            queryPagamentos = queryPagamentos.Where(x => x.dtCompetencia >= Form.dtInicioPeriodo);

            var dtFiltro = Form.dtFimPeriodo.Value.AddDays(1);
            queryPagamentos = queryPagamentos.Where(x => x.dtCompetencia < dtFiltro);

            return queryPagamentos;
        }

        //
        private IQueryable<ReceitaDespesaVW> filtrarPorVencimento(ResultadoFinanceiroForm Form, IQueryable<ReceitaDespesaVW> queryPagamentos) {

            queryPagamentos = queryPagamentos.Where(x => x.dtVencimento >= Form.dtInicioPeriodo);

            var dtFiltro = Form.dtFimPeriodo.Value.AddDays(1);
            queryPagamentos = queryPagamentos.Where(x => x.dtVencimento < dtFiltro);

            return queryPagamentos;
        }

    }
}
