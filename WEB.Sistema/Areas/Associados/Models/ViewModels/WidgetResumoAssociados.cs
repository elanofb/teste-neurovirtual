using System.Linq;
using BLL.Associados;
using BLL.NaoAssociados;

namespace WEB.Areas.Associados.ViewModels{

	public class WidgetResumoAssociado{

		//Atributos
		private IAssociadoBL _AssociadoBL;
		private INaoAssociadoBL _NaoAssociadoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
		private INaoAssociadoBL ONaoAssociadoBL => _NaoAssociadoBL = _NaoAssociadoBL ?? new NaoAssociadoBL();

        public int qtdeTotalAssociados { get; set; }

		public int qtdeConsumidores { get; set; }

		public int qtdeComerciantes { get; set; }

		public int qtdeConsumidoresInativos { get; set; }

		public int qtdeComerciantesInativos { get; set; }


		//
		public void carregarDados() {

			var listaAssociados = this.OAssociadoBL.listar(0, "", "", "").Where(x => x.ativo != "E").Select(x => new  {x.ativo }).ToList();
			var listaNaoAssociados = this.ONaoAssociadoBL.listar("", "").Where(x => x.ativo != "E").Select(x => new  {x.ativo }).ToList();

            this.qtdeTotalAssociados = listaAssociados.Count();

			this.qtdeConsumidores = listaAssociados.Count(x => x.ativo == "S");

			this.qtdeComerciantes = listaNaoAssociados.Count(x => x.ativo == "S");

		    this.qtdeConsumidoresInativos = listaAssociados.Count(x => x.ativo != "S");

		    this.qtdeComerciantesInativos = listaAssociados.Count(x => x.ativo != "S");

		}

	}


}