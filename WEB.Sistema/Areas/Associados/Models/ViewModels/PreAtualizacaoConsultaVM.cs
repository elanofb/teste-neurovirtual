using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using BLL.Historicos.Interfaces;
using BLL.Historicos.Services;
using BLL.Services;
using DAL.Historicos;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associados.Models.ViewModels {
	
    public class PreAtualizacaoConsultaVM {
		
	    //Atributos
	    private IHistoricoAtualizacaoConsultaBL _HistoricoAtualizacaoConsultaBL;
		
	    //Propriedades
	    private IHistoricoAtualizacaoConsultaBL OHistoricoAtualizacaoConsultaBL => _HistoricoAtualizacaoConsultaBL = _HistoricoAtualizacaoConsultaBL ?? new HistoricoAtualizacaoConsultaBL();
			    		
		public List<HistoricoAtualizacao> listaHistoricos { get; set;}
	    public List<HistoricoAtualizacao> listaPendentes { get; set;}
	    public List<HistoricoAtualizacao> listaAvaliados { get; set;}
	    	
	    //Filtros
	    public List<int> idsTipoAssociado { get; set; }
	    public DateTime? dtAtualizacaoInicial { get; set; }
	    public DateTime? dtAtualizacaoFinal { get; set; }
	    public string valorBusca { get; set; }
	    public string flagTipoSaida { get; set; }
	    
	    // Constants
	    private IPrincipal User => HttpContextFactory.Current.User;

		//Construtor
        public PreAtualizacaoConsultaVM() {
					        
	        this.listaHistoricos = new List<HistoricoAtualizacao>();
	        this.listaPendentes = new List<HistoricoAtualizacao>();
	        this.listaAvaliados = new List<HistoricoAtualizacao>();
	        
        }
		
	    public void capturarParametros(){
		    this.idsTipoAssociado = UtilRequest.getListInt("idsTipoAssociado");
		    this.dtAtualizacaoInicial = UtilRequest.getDateTime("dtAtualizacaoInicial");
		    this.dtAtualizacaoFinal = UtilRequest.getDateTime("dtAtualizacaoFinal");
		    this.valorBusca = UtilRequest.getString("valorBusca");
		    this.flagTipoSaida = UtilRequest.getString("flagTipoSaida");
	    }
		
	    public void carregarDados(){
			
		    var query = this.OHistoricoAtualizacaoConsultaBL.query(User.idOrganizacao());
			
		    if (this.idsTipoAssociado.Any()){
			    query = query.Where(x => this.idsTipoAssociado.Contains(x.Associado.idTipoAssociado));
		    }
		    
		    if (this.dtAtualizacaoInicial.HasValue){
			    query = query.Where(x => x.dtAtualizacao >= this.dtAtualizacaoInicial);
		    }
			
		    if (this.dtAtualizacaoFinal.HasValue){
			    query = query.Where(x => x.dtAtualizacao <= this.dtAtualizacaoFinal);
		    }
			
		    if (!this.valorBusca.isEmpty()){
			    query = query.Where(x => x.Associado.Pessoa.nome.Contains(this.valorBusca));
		    }
		    
		    this.listaHistoricos = query.Select(x => new{
				    x.id, 
				    x.idAssociado,
				    Associado = new { TipoAssociado = new { x.Associado.TipoAssociado.descricao } },
				    x.idPessoa,
				    Pessoa= new { x.Pessoa.nome },
				    x.dtAtualizacao,
				    x.dtAnalise, 
				    x.flagAprovado,
				    x.idUsuarioAnalise,
				    UsuarioAnalise = new { x.UsuarioAnalise.nome }
			    })
			    .ToListJsonObject<HistoricoAtualizacao>();
		    
		    if (!this.listaHistoricos.Any()){
			    return;
		    }
			
		    this.listaPendentes = this.listaHistoricos.Where(x => x.idAssociado > 0 && x.dtAnalise == null).ToList();
		    this.listaAvaliados = this.listaHistoricos.Where(x => x.idAssociado > 0 && x.dtAnalise != null).ToList();		    

	    }
	    
	    
    }


}