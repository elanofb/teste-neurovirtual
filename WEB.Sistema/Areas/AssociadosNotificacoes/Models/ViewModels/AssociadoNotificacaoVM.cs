using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Associados;
using DAL.Associados;
using DAL.Associados.DTO;

namespace WEB.Areas.AssociadosNotificacoes.ViewModels{
    
	public class AssociadoNotificacaoVM {
        
	    // Atributos Serviços
	    private IAssociadoRelatorioVWBL _AssociadoRelatorioVWBL;

	    // Propriedades Serviços
	    private IAssociadoRelatorioVWBL OAssociadoRelatorioVWBL => _AssociadoRelatorioVWBL = _AssociadoRelatorioVWBL ?? new AssociadoRelatorioVWBL();

		//Propriedades
	    public List<int> idsAssociados { get; set; }

	    public string valorBuscaLote { get; set; }

	    public List<int> idsTipoAssociado { get; set; }

	    public int idTipoAssociado { get; set; }

	    public List<int> idsUnidades { get; set; }

	    public DateTime? dtCadastroInicio { get; set; }
	    public DateTime? dtCadastroFim { get; set; }

	    public DateTime? dtAdmissaoInicio { get; set; }
	    public DateTime? dtAdmissaoFim { get; set; }

	    public string flagSexo { get; set; }

	    public string flagSituacaoContribuicao { get; set; }

	    public string flagTipoPessoa { get; set; }

	    public string ativo { get; set; }

	    public string valorBusca { get; set; }

        //
        public List<ItemListaAssociado> listaAssociados { get; set; }
        
	    //Construtor
		public AssociadoNotificacaoVM(){

		}
        
	    public IQueryable<AssociadoRelatorioVW> montarQuery() {

	        var query = this.OAssociadoRelatorioVWBL.listar(0, this.flagSituacaoContribuicao, this.valorBusca, this.ativo);
            
            if (this.idsAssociados?.Any() == true) {

                query = query.Where(x => this.idsAssociados.Contains(x.id));

                return query;

            }

	        if (this.idTipoAssociado > 0) {
	            query = query.Where(x => x.idTipoAssociado == this.idTipoAssociado);
	        }

	        if (this.idsTipoAssociado?.Any() == true) {
	            query = query.Where(x => this.idsTipoAssociado.Contains(x.idTipoAssociado.Value));
	        }

	        if (this.dtCadastroInicio.HasValue) {
	            query = query.Where(x => x.dtCadastro >= this.dtCadastroInicio);
	        }

	        if (this.dtCadastroFim.HasValue) {
	            var dtFiltro = this.dtCadastroFim.Value.AddDays(1);
	            query = query.Where(x => x.dtCadastro < dtFiltro);
	        }

	        if (this.dtAdmissaoInicio.HasValue) {
	            query = query.Where(x => x.dtAdmissao >= this.dtAdmissaoInicio);
	        }

	        if (this.dtAdmissaoFim.HasValue) {
	            var dtFiltro = this.dtAdmissaoFim.Value.AddDays(1);
	            query = query.Where(x => x.dtAdmissao < dtFiltro);
	        }

	        if(this.idsUnidades?.Any() == true) {
	            query = query.Where(x => this.idsUnidades.Contains(x.idUnidade.Value));
	        }
            
	        if (!this.ativo.isEmpty()) {
	            query = query.Where(x => x.ativo.Equals(this.ativo));
	        }

	        if (!this.flagTipoPessoa.isEmpty()) {
	            query = query.Where(x => x.flagTipoPessoa.Equals(this.flagTipoPessoa));
	        }

	        if (!this.flagSexo.isEmpty()) {
	            query = query.Where(x => x.flagSexo.Equals(this.flagSexo));
	        }
            
	        query = this.montarBuscaLote(query, this.valorBuscaLote);

	        return query;
	    }

	    private IQueryable<AssociadoRelatorioVW> montarBuscaLote(IQueryable<AssociadoRelatorioVW> query, string valorBuscaLote) {
            
	        if (!valorBuscaLote.isEmpty()) {

	            string[] separadores = { "\r\n" };
	            string[] valoresBusca = valorBuscaLote.Split(separadores, StringSplitOptions.None).Where(x => !x.isEmpty()).ToArray();
                
	            var valoresNumericos = valoresBusca.Select(x => (int?) x.toInt()).Where(x => x > 0).ToList();

	            var valoresSoNumeros = valoresBusca.Select(UtilString.onlyNumber).Where(x => !x.isEmpty()).ToList();

	            query = query.Where(x => valoresNumericos.Contains(x.id) || 
	                                     valoresNumericos.Contains(x.nroAssociado) ||
	                                     valoresSoNumeros.Contains(x.nroDocumento));
                
	        }

	        return query;

	    } 

	}

}