using System.Collections.Generic;
using DAL.ContasBancarias;

namespace WEB.Areas.ContasBancarias.ViewModels {
    
    public class ListaMovimentacaoContaVM {
    
        public int idContaBancaria { get; set; }
        
        public List<ContaBancariaMovimentacao> listaMovimentacoes { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public ListaMovimentacaoContaVM() {
            
            this.listaMovimentacoes = new List<ContaBancariaMovimentacao>();
            
        }
    }
}
