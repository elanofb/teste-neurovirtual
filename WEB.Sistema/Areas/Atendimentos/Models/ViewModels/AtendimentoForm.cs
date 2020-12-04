using System.Collections.Generic;
using System.Web;
using DAL.Associados.DTO;
using DAL.Atendimentos;

namespace WEB.Areas.Atendimentos.ViewModels{
    
	public class AtendimentoForm {

        public Atendimento Atendimento { get; set; } 

        public ItemListaAssociado AssociadoVinculado { get; set; }
		
		public List<HttpPostedFileBase> listaArquivo { get; set; }
		
		public AtendimentoForm() {
			
			this.Atendimento = new Atendimento();
			
		}
		
	}
}