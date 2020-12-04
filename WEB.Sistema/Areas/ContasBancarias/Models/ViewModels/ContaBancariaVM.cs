using DAL.ContasBancarias;
using DAL.Financeiro.Procedures;
using PagedList;
using System.Collections.Generic;

namespace WEB.Areas.ContasBancarias.ViewModels {
    
    public class ContaBancariaVM {

        public IPagedList<ContaBancaria> listaContaBancaria { get; set; }
        public List<SpContaBancariaSaldoAtual> listaSaldos { get; set; }

    }
}