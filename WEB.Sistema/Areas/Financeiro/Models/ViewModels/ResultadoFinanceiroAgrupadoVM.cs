using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;
using MoreLinq;

namespace WEB.Areas.Financeiro.ViewModels {

    public class ResultadoFinanceiroAgrupadoVM {
        
        // Propriedades
        public List<ReceitaDespesaVW> listaPagamentos { get; set; }

        public List<ResultadoFinanceiroPagamentoAgupadoDTO> listaGrupos { get; set; }

        //
        public ResultadoFinanceiroAgrupadoVM(List<ReceitaDespesaVW> listaPagamentosCarregados) {

            this.listaPagamentos = listaPagamentosCarregados;

            this.listaGrupos = new List<ResultadoFinanceiroPagamentoAgupadoDTO>();
            
        }

        //
        public void agruparPorTipo() {

            this.listaGrupos = this.listaPagamentos.Select(x => new ResultadoFinanceiroPagamentoAgupadoDTO  {
                                    id = x.idTipoTitulo.toInt(), descricao = x.descricaoTipoTitulo ?? "Não Informado",
                                    valor = this.listaPagamentos.Where(y => y.idTipoTitulo == x.idTipoTitulo).Sum(y => y.valor)
                               }).DistinctBy(x => x.id).ToList();

        }

        //
        public void agruparPorCentroCusto() {

            this.listaGrupos = this.listaPagamentos.Select(x => new ResultadoFinanceiroPagamentoAgupadoDTO  {
                                    id = x.idCentroCusto.toInt(), descricao = x.descricaoCentroCusto ?? "Não Informado",
                                    valor = this.listaPagamentos.Where(y => y.idCentroCusto == x.idCentroCusto).Sum(y => y.valor)
                               }).DistinctBy(x => x.id).ToList();

        }

        //
        public void agruparPorMacroConta() {

            this.listaGrupos = this.listaPagamentos.Select(x => new ResultadoFinanceiroPagamentoAgupadoDTO  {
                                    id = x.idMacroConta.toInt(), descricao = x.descricaoMacroConta ?? "Não Informado",
                                    valor = this.listaPagamentos.Where(y => y.idMacroConta == x.idMacroConta).Sum(y => y.valor)
                               }).DistinctBy(x => x.id).ToList();

        }

    }
}
