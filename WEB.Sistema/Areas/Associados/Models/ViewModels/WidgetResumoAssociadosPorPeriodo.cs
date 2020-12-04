using System;
using System.Linq;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

	public class WidgetResumoAssociadosPorPeriodo {

		//Atributos
		private IAssociadoBL _AssociadoBL;

		//Propriedades
		private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();

        public int qtdeTotalAssociados { get; set; }

        public int qtdeAssociadosAtivos { get; set; }

        public int qtdeAssociadosAdmissao { get; set; }

		public int qtdeAssociadosInativos { get; set; }

        //
        public DateTime? dtIniPesquisa { get; set; }

        public DateTime? dtFimPesquisa { get; set; }

		//
		public void carregarDados() {

            this.dtIniPesquisa = UtilRequest.getDateTime("dataInicio");
            this.dtFimPesquisa = UtilRequest.getDateTime("dataFim");

			var queryAssociados = this.OAssociadoBL.listar(0, "", "", "");
            
            if (dtIniPesquisa != null) {
                queryAssociados = queryAssociados.Where(x => x.dtCadastro >= dtIniPesquisa);
            }

            if (dtFimPesquisa != null) {
                var dtFiltro = dtFimPesquisa.Value.AddDays(1);
                queryAssociados = queryAssociados.Where(x => x.dtCadastro < dtFiltro);
            }

            var listaAssociados = queryAssociados.Select(x => new {x.id, x.ativo}).ToList();

            this.qtdeTotalAssociados = listaAssociados.Count();

			this.qtdeAssociadosAtivos = listaAssociados.Count(x => x.ativo == "S");

			this.qtdeAssociadosAdmissao = listaAssociados.Count(x => x.ativo == "E");

			this.qtdeAssociadosInativos = listaAssociados.Count(x => x.ativo == "N");

		}

	}


}