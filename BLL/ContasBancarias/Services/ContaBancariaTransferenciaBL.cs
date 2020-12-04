using BLL.Financeiro;
using BLL.Services;
using DAL.ContasBancarias;

namespace BLL.ContasBancarias {

    public class ContaBancariaTransferenciaBL : DefaultBL, IContaBancariaTransferenciaBL {
        
        // Atributos
        private ITituloReceitaGeradorBL _ITituloReceitaGeradorBL;
        private ITituloDespesaGeradorBL _ITituloDespesaGeradorBL;
        
        // Propriedades
        private ITituloReceitaGeradorBL OTituloReceitaGeradorBL => _ITituloReceitaGeradorBL = _ITituloReceitaGeradorBL ?? new TituloReceitaGeradorMovimentacaoBL();
        private ITituloDespesaGeradorBL OTituloDespesaGeradorBL => _ITituloDespesaGeradorBL = _ITituloDespesaGeradorBL ?? new TituloDespesaGeradorMovimentacaoBL();

        //
        public ContaBancariaTransferenciaBL(){

        }

        //
        public bool registrarLancamentos(ContaBancariaMovimentacao OContaBancariaMovimentacao) {

            var ORetornoReceita = this.OTituloReceitaGeradorBL.gerar(OContaBancariaMovimentacao);

            var ORetornoDespesa = this.OTituloDespesaGeradorBL.gerar(OContaBancariaMovimentacao);

            return ORetornoReceita.flagError && ORetornoDespesa.flagError;

        }
        
    }

}