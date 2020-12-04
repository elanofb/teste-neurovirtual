using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.PlanoContas.ViewModels{
    
	public sealed class PlanoContasDREVM{

		//Atributos
		private ICentroCustoBL _CentroCustoBL;
		private IMacroContaBL _MacroContaBL;
		private ICategoriaTituloBL _CategoriaTituloBL;

		//Servicos
		private ICentroCustoBL OCentroCustoBL => this._CentroCustoBL = this._CentroCustoBL ?? new CentroCustoBL();
		private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
		private ICategoriaTituloBL OCategoriaTituloBL => this._CategoriaTituloBL = this._CategoriaTituloBL ?? new CategoriaTituloBL();	
		
		//Propriedades
        public List<CentroCusto> listaCentroCusto { get; set; }

        public List<MacroConta> listaMacroConta { get; set; }

        public List<CategoriaTitulo> listaSubConta { get; set; }

        public List<CategoriaTitulo> listaSubContaFilha { get; set; }

        //
        public PlanoContasDREVM(){
			this.listaCentroCusto = new List<CentroCusto>();
			this.listaMacroConta = new List<MacroConta>();
			this.listaSubConta = new List<CategoriaTitulo>();
			this.listaSubContaFilha = new List<CategoriaTitulo>();
		}

		/// <summary>
		/// 
		/// </summary>
		public void carregarDados(){
			
			this.carregarCentrosCustos();

			this.carregarMacroContas();
			
			this.carregarSubContas();
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void carregarCentrosCustos(){
			
			listaCentroCusto = OCentroCustoBL.listar("",true)
											.Select(x => new {
												x.id, 
												x.codigoFiscal,
												x.descricao,
												x.ativo
											})
											.ToListJsonObject<CentroCusto>()
											.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
											.ThenBy(x => x.descricao)
											.ToList();
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void carregarMacroContas(){
			
			listaMacroConta = OMacroContaBL.listar("",true)
                                            .Where(x => x.idCentroCustoDRE != null && x.idCentroCustoDRE > 0)
											.Select(x => new {
												x.id, 
												x.codigoFiscal,
												x.descricao,
												x.ativo,
                                                x.idCentroCustoDRE
											})
											.ToListJsonObject<MacroConta>()
											.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
											.ThenBy(x => x.descricao)
											.ToList();
			
		}		
		
		/// <summary>
		/// 
		/// </summary>
		public void carregarSubContas(){
		
			listaSubConta = OCategoriaTituloBL.listar(0,"","S")
											.Where(x => (x.idCategoriaPai == 0 || x.idCategoriaPai == null) && x.flagExibirDRE == true)
											.Select(x => new {
												x.id, 
												x.codigoFiscal,
												x.descricao,
												x.idMacroConta,
												x.ativo
												
											})
											.ToListJsonObject<CategoriaTitulo>()
											.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
											.ThenBy(x => x.descricao)
											.ToList();
			
			listaSubContaFilha = OCategoriaTituloBL.listar(0,"","S")
													.Where(x => x.idCategoriaPai != null && x.idCategoriaPai > 0 && x.flagExibirDRE == true)
													.Select(x => new {
														x.id, 
														x.codigoFiscal,	
														x.descricao,
														x.idMacroConta,
														x.idCategoriaPai,
														x.ativo
													})
													.ToListJsonObject<CategoriaTitulo>()
													.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
													.ThenBy(x => x.descricao)
													.ToList();				
		}		
	}
    
}