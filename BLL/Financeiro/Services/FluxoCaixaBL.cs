using DAL.Financeiro;
using System.Linq;
using BLL.Services;

namespace BLL.Financeiro {

    public class FluxoCaixaBL : DefaultBL, IFluxoCaixaBL {

        // Atributos
        private IReceitasDespesasVWBL _IReceitasDespesasVWBL;

        // Propriedades
        private IReceitasDespesasVWBL OReceitasDespesasVWBL => _IReceitasDespesasVWBL = _IReceitasDespesasVWBL ?? new ReceitasDespesasVWBL();


        public IQueryable<ReceitaDespesaVW> listar() {

            var query = this.OReceitasDespesasVWBL.listar().Where(x => x.dtPagamento.HasValue);
            
            return query;
        }
    }
}
