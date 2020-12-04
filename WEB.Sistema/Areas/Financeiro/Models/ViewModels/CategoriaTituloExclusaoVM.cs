using System;
using System.Linq;
using BLL.Financeiro;

namespace WEB.Areas.Financeiro.ViewModels{
    
	public class CategoriaTituloExclusaoVM{

        //Atributos
	    private ICategoriaTituloBL _CategoriaTituloBL;
	    private ITituloDespesaBL _TituloDespesaBL;
	    private ITituloReceitaBL _TituloReceitaBL;
	    private ITituloDespesaPagamentoBL _TituloDespesaPagamentoBL;
	    private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

	    //Propriedades
	    private ICategoriaTituloBL OCategoriaTituloBL => _CategoriaTituloBL = _CategoriaTituloBL ?? new CategoriaTituloBL();
	    private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
	    private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
	    private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _TituloDespesaPagamentoBL = _TituloDespesaPagamentoBL ?? new TituloDespesaPagamentoBL();
	    private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        //Propriedades
        public int qtdItens { get; set; }
		
        public int idMacroConta { get; set; }
		
        public int idCategoria { get; set; }
		
		public string descricaoCategoria { get; set; }
        
		public int idMacroContaNova { get; set; }
		
		public int idCategoriaNova { get; set; }

        public string nomeSubConta { get; set; }

	    public void carregarDados(int id) {
	        var OCategoriaTitulo = OCategoriaTituloBL.carregar(id);

	        if (OCategoriaTitulo.id > 0) {
	            this.qtdItens = 
                    OTituloDespesaBL.listar("").Count(x => x.idCategoria == OCategoriaTitulo.id) +
                    OTituloReceitaBL.listar(0, 0, 0, "").Count(x => x.idCategoria == OCategoriaTitulo.id);
	            this.qtdItens += 
                    OTituloDespesaPagamentoBL.listar(0).Count(x => x.idCategoria == OCategoriaTitulo.id) + 
                    OTituloReceitaPagamentoBL.listar(0).Count(x => x.idCategoria == OCategoriaTitulo.id);
                
            }

	        this.idCategoria = OCategoriaTitulo.id;
	        this.idMacroConta = OCategoriaTitulo.idMacroConta.toInt();
	        this.nomeSubConta = OCategoriaTitulo.descricao;
	    }
    }
}