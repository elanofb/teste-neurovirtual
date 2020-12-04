using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;

namespace WEB.Areas.PlanoContas.ViewModels{
    
	public sealed class PlanoContasVM{

		//Atributos
		private IMacroContaBL _MacroContaBL;
		private ICategoriaTituloBL _CategoriaTituloBL;

		//Servicos
		private IMacroContaBL OMacroContaBL => this._MacroContaBL = this._MacroContaBL ?? new MacroContaBL();
		private ICategoriaTituloBL OCategoriaTituloBL => this._CategoriaTituloBL = this._CategoriaTituloBL ?? new CategoriaTituloBL();	
		
		//Propriedades
        public List<MacroConta> listaMacroConta { get; set; }

        public List<CategoriaTitulo> listaSubConta { get; set; }

        public List<CategoriaTitulo> listaSubContaFilha { get; set; }

        //
        public PlanoContasVM(){
			this.listaMacroConta = new List<MacroConta>();
			this.listaSubConta = new List<CategoriaTitulo>();
			this.listaSubContaFilha = new List<CategoriaTitulo>();
		}

		/// <summary>
		/// 
		/// </summary>
		public void carregarDados(){
			
			this.carregarMacroContas();
			
			this.carregarSubContas();
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		public void carregarMacroContas(){
			
			listaMacroConta = OMacroContaBL.listar("",true)
											.Select(x => new {
												x.id, 
												x.codigoFiscal,
												x.descricao,
												x.ativo,
												CentroCustoDRE = new{
													id = x.CentroCustoDRE == null? 0 : x.CentroCustoDRE.id,
													descricao = x.CentroCustoDRE == null? "" : x.CentroCustoDRE.descricao
												}
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
											.Where(x => x.idCategoriaPai == 0 || x.idCategoriaPai == null)
											.Select(x => new {
												x.id, 
												x.codigoFiscal,
												x.descricao,
												x.idMacroConta,
												MacroConta = new {
													id = x.MacroConta == null? 0: x.MacroConta.id,
													descricao = x.MacroConta == null? "": x.MacroConta.descricao,
													codigoFiscal = x.MacroConta == null? "": x.MacroConta.codigoFiscal
												},	
												x.ativo
											})
											.ToListJsonObject<CategoriaTitulo>()
											.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
											.ThenBy(x => x.descricao)
											.ToList();
			
			listaSubContaFilha = OCategoriaTituloBL.listar(0,"","S")
													.Where(x => x.idCategoriaPai != null && x.idCategoriaPai > 0)
													.Select(x => new {
														x.id, 
														x.codigoFiscal,
														x.descricao,
														x.idMacroConta,
														MacroConta = new {
															id = x.MacroConta == null? 0: x.MacroConta.id,
															descricao = x.MacroConta == null? "": x.MacroConta.descricao,
															codigoFiscal = x.MacroConta == null? "": x.MacroConta.codigoFiscal
														},					
														x.idCategoriaPai,
														CategoriaPai = new {
															id = x.CategoriaPai == null? 0: x.CategoriaPai.id,
															descricao = x.CategoriaPai == null? "": x.CategoriaPai.descricao,
															codigoFiscal = x.CategoriaPai == null? "": x.CategoriaPai.codigoFiscal
														},
														x.ativo
													})
													.ToListJsonObject<CategoriaTitulo>()
													.OrderBy(x => UtilString.onlyNumber(x.codigoFiscal).toInt())
													.ThenBy(x => x.descricao)
													.ToList();				
		}		
	}
    
}