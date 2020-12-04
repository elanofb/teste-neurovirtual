using DAL.ContasBancarias;

namespace BLL.ContasBancarias {

    public interface IContaBancariaTransferenciaBL {

        bool registrarLancamentos(ContaBancariaMovimentacao OContaBancariaMovimentacao);
        
    }

}