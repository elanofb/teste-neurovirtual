using System;
using System.Security.Principal;
using BLL.Historicos.Interfaces;
using BLL.Historicos.Services;
using DAL.Associados;
using DAL.Pessoas;
using DAL.Historicos;
using Newtonsoft.Json;
using WEB.Areas.Associados.ViewModels;

namespace WEB.Areas.Associados.Models.ViewModels {
	
    public class PreAtualizacaoDetalheVM {
					
	    //Atributos
	    private IHistoricoAtualizacaoConsultaBL _HistoricoAtualizacaoConsultaBL;
	    private IHistoricoAtualizacaoCadastroBL _HistoricoAtualizacaoCadastroBL;
		
	    //Propriedades
	    private IHistoricoAtualizacaoConsultaBL OHistoricoAtualizacaoConsultaBL => _HistoricoAtualizacaoConsultaBL = _HistoricoAtualizacaoConsultaBL ?? new HistoricoAtualizacaoConsultaBL();
	    private IHistoricoAtualizacaoCadastroBL OHistoricoAtualizacaoCadastroBL => _HistoricoAtualizacaoCadastroBL = _HistoricoAtualizacaoCadastroBL ?? new HistoricoAtualizacaoCadastroBL();
	    
		public HistoricoAtualizacao OHistoricoAtualizacao { get; set;}
	    public Associado OAssociado { get; set;}
	    
	    public AssociadoPreAtualizacaoCadastroPFForm FormAssociadoPF { get; set;}
	    public AssociadoPreAtualizacaoCadastroPJForm FormAssociadoPJ { get; set;}
	    
	    // Constants
	    private IPrincipal User => HttpContextFactory.Current.User;
		
		//Construtor
        public PreAtualizacaoDetalheVM() {
					        
	        this.OHistoricoAtualizacao = new HistoricoAtualizacao();
	        this.OAssociado = new Associado();
        }
	    
	    public void carregarHistorico(int? id){

		    this.OHistoricoAtualizacao = this.OHistoricoAtualizacaoConsultaBL.carregar(id.toInt()) ?? new HistoricoAtualizacao();		    

	    }
	    
	    public void carregarDadosAssociado(){
			
		    if (this.OHistoricoAtualizacao.id == 0 || this.OHistoricoAtualizacao.informacoes.isEmpty()){
			    return;
		    }
		    
		    this.OAssociado =  JsonConvert.DeserializeObject<Associado>(this.OHistoricoAtualizacao.informacoes);		    		    
		    		    
	    }
	    
	    public void salvarHistoricoAssociado(Associado OAssociadoHistorico){
			
		    if (OAssociadoHistorico.id == 0){
			    return;
		    }
			
		    if (this.OHistoricoAtualizacao.id == 0){
			    return ;
		    }
		    
		    if (this.OHistoricoAtualizacao.dtAnalise != null || !this.OHistoricoAtualizacao.informacoesAnteriores.isEmpty()){
			    return;
		    }
		    		    
		    OAssociadoHistorico.Pessoa.limparListas();
            
		    OAssociadoHistorico.limparListas();
		    
		    string infoCadastro = JsonConvert.SerializeObject(OAssociadoHistorico, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
		    
		    if (infoCadastro.isEmpty()){
			    return;
		    }
			
		    HistoricoAtualizacao OHistoricoAtualizacaoUpdate = this.OHistoricoAtualizacao;

		    OHistoricoAtualizacaoUpdate.informacoesAnteriores = infoCadastro;
			
		    this.OHistoricoAtualizacaoCadastroBL.salvar(OHistoricoAtualizacaoUpdate);

	    }
	    
	    public Associado retonarDadosAlterados(){
		    
		    if (this.OHistoricoAtualizacao.id == 0 || OHistoricoAtualizacao.informacoesAnteriores.isEmpty()){
			    return new Associado();
		    }
		    
		    Associado OAssociadoAlteracoes = JsonConvert.DeserializeObject<Associado>(this.OHistoricoAtualizacao.informacoesAnteriores);
		    
		    return OAssociadoAlteracoes;
		    
	    }

    }


}