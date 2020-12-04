using DAL.Financeiro;

namespace BLL.FinanceiroLancamentos {

	public interface IDespesaCadastroBL {
        bool salvar(TituloDespesa OTituloDespesa);
	}
}
