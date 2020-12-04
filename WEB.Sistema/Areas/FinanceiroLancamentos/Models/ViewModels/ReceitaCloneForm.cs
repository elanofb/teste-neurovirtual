using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {
     
    //
	[Validator(typeof(ReceitaCloneFormValidator))]
	public class ReceitaCloneForm {
	    
		// Atributos Serviços
		private ITituloReceitaBL _ITituloReceitaBL;
		
		// Propriedades Serviços
		private ITituloReceitaBL OTituloReceitaBL => _ITituloReceitaBL = _ITituloReceitaBL ?? new TituloReceitaPadraoBL();
		
		//Propriedades
        public TituloReceita TituloReceita { get; set; }

		public string idReferenciaPessoa { get; set; }
		
		public int qtdeReplicacoes { get; set; }

		//
		public ReceitaCloneForm() {
			
			this.TituloReceita = new TituloReceita();

			this.qtdeReplicacoes = 1;
		} 
		
		//
		public void carregarReceitaBase(int id) {
			
			this.TituloReceita = this.OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.id == id)
									 .Select(x => new {
										 x.id, 
										 x.descricao, 
										 x.idPessoa, 
										 x.dtVencimento,
										 x.dtCompetencia,
										 x.valorTotal,
										 x.flagCategoriaPessoa
									 }).FirstOrDefault().ToJsonObject<TituloReceita>();

			if (this.TituloReceita == null) {
				return;
			}

			this.idReferenciaPessoa = this.TituloReceita.flagCategoriaPessoa + "#" + this.TituloReceita.idPessoa;

		}

	}
	
}