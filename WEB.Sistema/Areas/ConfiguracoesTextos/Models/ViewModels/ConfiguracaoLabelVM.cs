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

	public class ConfiguracaoLabelVM {

		//Atributos
		private IConfiguracaoLabelBL _ConfiguracaoLabelBL;
		private IIdiomaConsultaBL _IdiomaConsultaBL;

		//Propriedades
		private IConfiguracaoLabelBL OConfiguracaoLabelBL => this._ConfiguracaoLabelBL = this._ConfiguracaoLabelBL ?? new ConfiguracaoLabelBL();
		private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
		
        //Propriedades
		public IPagedList<ConfiguracaoLabel> listaConfiguracaoLabelsPaged { get; set; }
		public List<ConfiguracaoLabel> listaConfiguracaoLabels { get; set; }

		public List<Idioma> listaIdiomas { get; set; }
		
		private IPrincipal User => HttpContextFactory.Current.User;
		
        //Construtor
        public ConfiguracaoLabelVM() { 
			this.listaConfiguracaoLabelsPaged = new List<ConfiguracaoLabel>().ToPagedList(1, 20);
			this.listaConfiguracaoLabels = new List<ConfiguracaoLabel>();
        }

		public void carregar() {
			
			var valorBusca = UtilRequest.getString("valorBusca");

			var query = this.OConfiguracaoLabelBL.query();

			if (!string.IsNullOrEmpty(valorBusca)){
				query = query.Where(x => x.key.Equals(valorBusca) || x.label.Contains(valorBusca));
			}

			this.listaConfiguracaoLabels = query.Where(x => x.idIdioma > 0)
				.Select(x => new {
					x.id,
					x.idIdioma,
					x.key,
					x.label
				}).ToListJsonObject<ConfiguracaoLabel>();
			
			this.listaConfiguracaoLabelsPaged = query.Where(x => x.idIdioma == null)
				.Select(x => new {
					x.id,
					x.label,
					x.key,
					x.dtCadastro,
					x.dtAlteracao,
					UsuarioSistema = new {
						x.UsuarioSistema.nome
					}
				})
				.OrderBy(x => x.id).ToPagedListJsonObject<ConfiguracaoLabel>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
			
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