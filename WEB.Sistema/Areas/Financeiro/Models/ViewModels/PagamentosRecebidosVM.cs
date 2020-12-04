using System.Collections.Generic;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.Financeiro.ViewModels {

    public class PagamentosRecebidosVM {

        //Atributos
        public IPagedList<TituloReceitaPagamentoVW> listaPagamentos { get; set; }
        
        public decimal valorTotalRecebido { get; set; }

        public decimal valorTotalTarifas { get; set; }
        
        public PagamentosRecebidosVM() {

            this.listaPagamentos = new List<TituloReceitaPagamentoVW>().ToPagedList(1, 20);

        }

    }
}
