using System.Linq;
using BLL.NaoAssociados;

namespace WEB.Areas.NaoAssociados.ViewModels{

	public class WidgetResumoNaoAssociado{

		//Atributos
		private INaoAssociadoBL _NaoAssociadoBL;

		//Propriedades
		private INaoAssociadoBL ONaoAssociadoBL => ( this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL() );

	    public int qtdeAssociadosAtivos { get; set; }

		public int qtdeAssociadosInativos { get; set; }

		//
		public void carregarDados() {

			var query = this.ONaoAssociadoBL.listar("", "");

			this.qtdeAssociadosAtivos = query.Count(x => x.ativo == "S");
			this.qtdeAssociadosInativos = query.Count(x => x.ativo == "N");
		}
	}

}