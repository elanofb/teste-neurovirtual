using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos {

	public interface IReceitaCadastroBL {
        bool salvar(TituloReceita OTituloReceita);
	}
}
