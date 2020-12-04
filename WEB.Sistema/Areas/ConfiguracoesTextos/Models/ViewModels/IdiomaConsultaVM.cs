using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.ConfiguracoesTextos;
using DAL.ConfiguracoesTextos;
using DAL.Permissao.Security.Extensions;
using PagedList;
using WEB.Extensions;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels{

	public class IdiomaConsultaVM {

		//Atributos
		private IIdiomaConsultaBL _IdiomaConsultaBL;

		//Propriedades
		private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
		
        //Propriedades
		public IPagedList<Idioma> listaIdiomas { get; set; }
		
		private IPrincipal User => HttpContextFactory.Current.User;

        //Construtor
        public IdiomaConsultaVM() { 
			this.listaIdiomas = new List<Idioma>().ToPagedList(1, 20);
        }

		public void carregar() {

			var query = this.OIdiomaConsultaBL.query(User.idOrganizacao());

			var flagAtivo = UtilRequest.getBool("flagAtivo");
			var valorBusca = UtilRequest.getString("valorBusca");
			
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca) || x.sigla.Contains(valorBusca));
			}
			
			if (!flagAtivo.isEmpty()) {
				query = query.Where(x => x.ativo == flagAtivo);
			}
			
			this.listaIdiomas = query.OrderBy(x => x.id)
				.Select(x => new {
					x.id,
					x.descricao,
					x.sigla,
					x.dtCadastro,
					x.ativo
				}).OrderBy(x => x.dtCadastro)
				.ToPagedListJsonObject<Idioma>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
			
		}
		
    }
}