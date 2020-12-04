using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Financeiro;
using DAL.Financeiro;
using PagedList;

namespace WEB.Areas.Financeiro.ViewModels{

	public class ArquivosFinanceirosConsultaVM {

		// Atributos Serviços
		private IReceitasDespesasArquivosVWBL _IReceitasDespesasArquivosVWBL;
		
		// Propriedades Serviços
		private IReceitasDespesasArquivosVWBL OReceitasDespesasArquivosVWBL => _IReceitasDespesasArquivosVWBL = _IReceitasDespesasArquivosVWBL ?? new ReceitasDespesasArquivosVWBL();
		
		// Propriedades
		public DateTime? dtCadastroInicio { get; set; }
		public DateTime? dtCadastroFim { get; set; }

		public string flagTipoTitulo { get; set; }

		public List<int?> idsTipoReceita { get; set; }
		
		public string credor { get; set; }	
		
		public string pagador { get; set; }

		public IPagedList<ReceitaDespesaArquivoVW> listaArquivos { get; set; }

		//
	    public ArquivosFinanceirosConsultaVM(){
		    
		    this.idsTipoReceita = new List<int?>();
		    
	        this.listaArquivos = new List<ReceitaDespesaArquivoVW>().ToPagedList(1, 20);
		    
        }
		
		// 
		public void montarLista() {

			var query = this.montarQuery();

			this.listaArquivos = query.OrderByDescending(x => x.id).ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

		}
		
		//
		public IQueryable<ReceitaDespesaArquivoVW> montarQuery() {
			
			var query = this.OReceitasDespesasArquivosVWBL.listar();

			if (this.dtCadastroInicio.HasValue) {
				query = query.Where(x => x.dtCadastro >= this.dtCadastroInicio);
			}
			
			if (this.dtCadastroFim.HasValue) {
				var dtFiltro = this.dtCadastroFim.Value.AddDays(1);
				query = query.Where(x => x.dtCadastro < dtFiltro);
			}

			if (!this.flagTipoTitulo.isEmpty()) {
				query = query.Where(x => x.flagTipoTitulo.Equals(this.flagTipoTitulo));
			}
			
			if (this.idsTipoReceita.Any()) {
				query = query.Where(x => this.idsTipoReceita.Contains(x.idTipoTitulo));
			}
			
			if (!this.credor.isEmpty()) {
				query = query.Where(x => x.nomePessoa.Contains(this.credor) && x.flagTipoTitulo == "D");
			}
			
			if (!this.pagador.isEmpty()) {
				query = query.Where(x => x.nomePessoa.Contains(this.pagador) && x.flagTipoTitulo == "R");
			}

			return query;

		}

	}
	
}