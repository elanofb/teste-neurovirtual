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

    [Validator(typeof(ConfiguracaoLabelFormValidator))]
	public class ConfiguracaoLabelForm {

		//Atributos
		private IConfiguracaoLabelBL _ConfiguracaoLabelBL;
		private IIdiomaConsultaBL _IdiomaConsultaBL;

		//Propriedades
		private IConfiguracaoLabelBL OConfiguracaoLabelBL => this._ConfiguracaoLabelBL = this._ConfiguracaoLabelBL ?? new ConfiguracaoLabelBL();
		private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
		
        //Propriedades
		public List<ConfiguracaoLabel> listaConfiguracaoLabel { get; set; }
		public ConfiguracaoLabel ConfiguracaoLabelPadrao { get; set; }
		public List<Idioma> listaIdiomas { get; set; }
		
		public string key { get; set; }

		private IPrincipal User => HttpContextFactory.Current.User;
		
        //Construtor
        public ConfiguracaoLabelForm() { 
			this.ConfiguracaoLabelPadrao = new ConfiguracaoLabel();
			this.listaConfiguracaoLabel = new List<ConfiguracaoLabel>();
			this.listaIdiomas = new List<Idioma>();
        }

		public void carregar(string keyParam) {
			
			this.key = keyParam;
			
			this.carregarIdiomas();
			this.carregarListaLabels();
			
		}

        private void carregarIdiomas() {

	        this.listaIdiomas = OIdiomaConsultaBL.query(User.idOrganizacao())
		        .Select(x => new {
			        x.id,
			        x.descricao,
			        x.sigla
		        }).ToListJsonObject<Idioma>();
	        
        }

		private void carregarListaLabels() {
			
			var query = OConfiguracaoLabelBL.query(User.idOrganizacao()).Where(x => x.key == this.key);

			this.ConfiguracaoLabelPadrao = query.FirstOrDefault(x => x.idIdioma == null) ?? new ConfiguracaoLabel();

			if (!this.listaIdiomas.Any()) {
				return;
			}
			
			var listaLabels = query.Where(x => x.idIdioma > 0)
				.Select(x => new {
					x.id,
					x.idIdioma,
					x.label
				}).ToListJsonObject<ConfiguracaoLabel>();

			var listaConfiguracaoSubmit = this.listaConfiguracaoLabel;
			this.listaConfiguracaoLabel = new List<ConfiguracaoLabel>();
			
			foreach (var OIdioma in this.listaIdiomas) {

				var OLabel = new ConfiguracaoLabel();
				
				if (this.listaConfiguracaoLabel.Any()) {
					OLabel = listaConfiguracaoSubmit.FirstOrDefault(x => x.idIdioma == OIdioma.id) ?? new ConfiguracaoLabel();
				}

				if (OLabel.id == 0) {
					OLabel = listaLabels.FirstOrDefault(x => x.idIdioma == OIdioma.id) ?? new ConfiguracaoLabel();
				}

				OLabel.idIdioma = OIdioma.id;
				OLabel.Idioma = OIdioma;
				
				this.listaConfiguracaoLabel.Add(OLabel);

			}
			
		}
        
    }

}