using System.Linq;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {
     
    //
	[Validator(typeof(DespesaCloneFormValidator))]
	public class DespesaCloneForm {
	    
		// Atributos Serviços
		private ITituloDespesaBL _TituloDespesaBL;
		
		// Propriedades Serviços
		private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
		
		//Propriedades
        public TituloDespesa TituloDespesa { get; set; }

		public string idReferenciaPessoa { get; set; }
		
		public int qtdeReplicacoes { get; set; }

		//
		public DespesaCloneForm() {
			
			this.TituloDespesa = new TituloDespesa();

			this.qtdeReplicacoes = 1;
		} 
		
		//
		public void carregarDespesaBase(int id) {
			
			this.TituloDespesa = this.OTituloDespesaBL.listar("").Where(x => x.id == id)
									 .Select(x => new {
										 x.id, 
										 x.descricao, 
										 x.idPessoa, 
										 x.dtVencimento,
										 x.dtDespesa,
										 x.valorTotal,
										 x.flagCategoriaPessoa
									 }).FirstOrDefault().ToJsonObject<TituloDespesa>();

			if (this.TituloDespesa == null) {
				return;
			}

			this.idReferenciaPessoa = this.TituloDespesa.flagCategoriaPessoa + "#" + this.TituloDespesa.idPessoa;

		}

	}
	
}