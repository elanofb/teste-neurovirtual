using System;
using System.Collections.Generic;
using BLL.ConfiguracoesTextos;
using DAL.ConfiguracoesTextos;
using FluentValidation.Attributes;
using System.Linq;
using System.Security.Principal;
using BLL.Services;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels{

    [Validator(typeof(ConfiguracaoTextoFormValidator))]
	public class ConfiguracaoTextoForm {

		//Atributos
		private IConfiguracaoTextoBL _ConfiguracaoTextoBL;
		private IIdiomaConsultaBL _IdiomaConsultaBL;

		//Propriedades
		private IConfiguracaoTextoBL OConfiguracaoTextoBL => this._ConfiguracaoTextoBL = this._ConfiguracaoTextoBL ?? new ConfiguracaoTextoBL();
		private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
		
        //Propriedades
		public List<ConfiguracaoTexto> listaConfiguracaoTexto { get; set; }
		public ConfiguracaoTexto ConfiguracaoTextoPadrao { get; set; }
		public List<Idioma> listaIdiomas { get; set; }
		
		public string key { get; set; }

		private IPrincipal User => HttpContextFactory.Current.User;
		
        //Construtor
        public ConfiguracaoTextoForm() { 
			this.ConfiguracaoTextoPadrao = new ConfiguracaoTexto();
			this.listaConfiguracaoTexto = new List<ConfiguracaoTexto>();
			this.listaIdiomas = new List<Idioma>();
        }

		public void carregar(string keyParam) {
			
			this.key = keyParam;
			
			this.carregarIdiomas();
			this.carregarListaTextos();
			
		}

        private void carregarIdiomas() {

	        this.listaIdiomas = OIdiomaConsultaBL.query(User.idOrganizacao())
		        .Select(x => new {
			        x.id,
			        x.descricao,
			        x.sigla
		        }).ToListJsonObject<Idioma>();
	        
        }

		private void carregarListaTextos() {
			
			var query = OConfiguracaoTextoBL.query(User.idOrganizacao()).Where(x => x.key == this.key);

			this.ConfiguracaoTextoPadrao = query.FirstOrDefault(x => x.idIdioma == null) ?? new ConfiguracaoTexto();

			if (!this.listaIdiomas.Any()) {
				return;
			}
			
			var listaTextos = query.Where(x => x.idIdioma > 0)
				.Select(x => new {
					x.id,
					x.idIdioma,
					x.texto
				}).ToListJsonObject<ConfiguracaoTexto>();

			var listaConfiguracaoSubmit = this.listaConfiguracaoTexto;
			this.listaConfiguracaoTexto = new List<ConfiguracaoTexto>();
			
			foreach (var OIdioma in this.listaIdiomas) {

				var OTexto = new ConfiguracaoTexto();
				
				if (this.listaConfiguracaoTexto.Any()) {
					OTexto = listaConfiguracaoSubmit.FirstOrDefault(x => x.idIdioma == OIdioma.id) ?? new ConfiguracaoTexto();
				}

				if (OTexto.id == 0) {
					OTexto = listaTextos.FirstOrDefault(x => x.idIdioma == OIdioma.id) ?? new ConfiguracaoTexto();
				}

				OTexto.idIdioma = OIdioma.id;
				OTexto.Idioma = OIdioma;
				
				this.listaConfiguracaoTexto.Add(OTexto);

			}
			
		}
        
    }

}