using DAL.Financeiro;

namespace WEB.Areas.Financeiro.Extensions {
    
    public static class ReceitaDespesaArquivoExtensions {
        
        //Borda
        public static string exibirBordaStatus(this ReceitaDespesaArquivoVW OLancamento) {

            if (OLancamento.dtPagamento.HasValue) {
                return "border-green";
            }

            return "border-yellow";
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this ReceitaDespesaArquivoVW OLancamento) {

            if (OLancamento.dtPagamento.HasValue) {
                return "fa-check";
            }

            return "fa-exclamation";
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this ReceitaDespesaArquivoVW OLancamento) {

            if (OLancamento.dtPagamento.HasValue) {
                return "text-green";
            }

            return "text-yellow";
        }
        
    }
    
}