using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.ConfiguracoesTextos;
using BLL.Services;
using DAL.ConfiguracoesTextos;
using DAL.Permissao.Security.Extensions;
using PagedList;
using WEB.Extensions;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels{

	public class ConfiguracaoTextoVM {

		//Atributos
		private IConfiguracaoTextoBL _ConfiguracaoTextoBL;
		private IIdiomaConsultaBL _IdiomaConsultaBL;

		//Propriedades
		private IConfiguracaoTextoBL OConfiguracaoTextoBL => this._ConfiguracaoTextoBL = this._ConfiguracaoTextoBL ?? new ConfiguracaoTextoBL();
		private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
		
        //Propriedades
		public IPagedList<ConfiguracaoTexto> listaConfiguracaoTextosPaged { get; set; }
		public List<ConfiguracaoTexto> listaConfiguracaoTextos { get; set; }

		public List<Idioma> listaIdiomas { get; set; }
		
		private IPrincipal User => HttpContextFactory.Current.User;
		
        //Construtor
        public ConfiguracaoTextoVM() { 
			this.listaConfiguracaoTextosPaged = new List<ConfiguracaoTexto>().ToPagedList(1, 20);
			this.listaConfiguracaoTextos = new List<ConfiguracaoTexto>();
        }

		public void carregar() {
			
			var valorBusca = UtilRequest.getString("valorBusca");

			var query = this.OConfiguracaoTextoBL.query();

			if (!string.IsNullOrEmpty(valorBusca)){
				query = query.Where(x => x.key.Equals(valorBusca) || x.texto.Contains(valorBusca));
			}

			this.listaConfiguracaoTextos = query.Where(x => x.idIdioma > 0)
				.Select(x => new {
					x.id,
					x.idIdioma,
					x.key,
					x.texto
				}).ToListJsonObject<ConfiguracaoTexto>();
			
			this.listaConfiguracaoTextosPaged = query.Where(x => x.idIdioma == null)
				.Select(x => new {
					x.id,
					x.texto,
					x.key,
					x.dtCadastro,
					x.dtAlteracao,
					UsuarioSistema = new {
						x.UsuarioSistema.nome
					}
				})
				.OrderBy(x => x.id).ToPagedListJsonObject<ConfiguracaoTexto>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
			
		}
		
		public void carregarIdiomas() {

			this.listaIdiomas = OIdiomaConsultaBL.query(User.idOrganizacao())
				.Select(x => new {
					x.id,
					x.descricao,
					x.sigla
				}).ToListJsonObject<Idioma>();
	        
		}
		
    }
}