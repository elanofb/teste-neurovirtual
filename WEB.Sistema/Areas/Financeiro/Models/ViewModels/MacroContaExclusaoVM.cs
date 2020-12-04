using System.Linq;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{
    
	public class MacroContaExclusaoVM{

	    //Atributos        
	    private IMacroContaBL _MacroContaBL;
	    private ITituloDespesaBL _TituloDespesaBL;
	    private ITituloReceitaBL _TituloReceitaBL;
	    private ITituloDespesaPagamentoBL _TituloDespesaPagamentoBL;
	    private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

	    //Propriedades
	    private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
	    private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
	    private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
	    private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _TituloDespesaPagamentoBL = _TituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();
	    private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        public int qtdItens { get; set; }
        public int idMacroConta { get; set; }
        public int idMacroContaNova { get; set; }
        public string nomeMacroConta { get; set; }

	    public void carregarDados(int id) {
	        var OMacroConta = OMacroContaBL.carregar(id);

	        if (OMacroConta.id > 0) {
	            this.qtdItens = OTituloDespesaBL.listar("").Count(x => x.idMacroConta == OMacroConta.id) + OTituloReceitaBL.listar(0, 0, 0, "").Count(x => x.idMacroConta == OMacroConta.id);
	            this.qtdItens += OTituloDespesaPagamentoBL.listar(0).Count(x => x.idMacroConta == OMacroConta.id) + OTituloReceitaPagamentoBL.listar(0).Count(x => x.idMacroConta == OMacroConta.id);
	        }

	        this.idMacroConta = OMacroConta.id;
	        this.nomeMacroConta = OMacroConta.descricao;
        }

	}
}