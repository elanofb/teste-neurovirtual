using System;
using BLL.Configuracoes;
using DAL.Notificacoes;
using FluentValidation.Attributes;

namespace WEB.Areas.CorreioInterno.ViewModels {

    [Validator(typeof(ConfiguracaoContaFormValidator))]
    public class ConfiguracaoContaForm {

        //Atributos

        //Propriedades
        public ConfiguracaoEmailUsuario ConfiguracaoEmailUsuario { get; set; }

        //Construtor
        public ConfiguracaoContaForm() {

            ConfiguracaoEmailUsuario = new ConfiguracaoEmailUsuario();

        }

        public void preencherAssinatura() {

            if (!this.ConfiguracaoEmailUsuario.assinaturaEmail.isEmpty()) {
                return;
            }
            
            var OConfigSistema = ConfiguracaoEmailBL.getInstance.carregar();

            this.ConfiguracaoEmailUsuario.assinaturaEmail = OConfigSistema.assinaturaEmail;

        } 

   }
}