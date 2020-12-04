using System.Collections.Generic;
using DAL.LogsAlteracoes;

namespace WEB.Areas.Planos.ViewModels{
    
	public class PlanoCarreiraAlteracaoDadosListaVM {
        
	    public int idPlanoCarreira { get; set; }
		
        public string valorBusca { get; set; }
		
        public List<LogAlteracao> listaLogs { get; set; }
		
        public PlanoCarreiraAlteracaoDadosListaVM() {
            
        }

	}

}