using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Configuracoes;
using DAL.Configuracoes;
using DAL.Permissao.Security.Extensions;
using FluentValidation;
using WebGrease.Css.Extensions;

namespace WEB.Areas.Configuracao.ViewModels {

    //
    public class ConfiguracaoSistemaFormValidator : AbstractValidator<ConfiguracaoSistemaForm> {

        // Atributos
        private List<string> listaDominiosUtilizados;

        // Propriedades
        private IConfiguracaoSistemaBL OConfiguracaoSistemaBL => ConfiguracaoSistemaBL.getInstance;

        private List<ConfiguracaoSistema> listaConfiguracoes => this.OConfiguracaoSistemaBL.listar(0).ToList();

        private IPrincipal User => HttpContextFactory.Current.User;

        //
        public ConfiguracaoSistemaFormValidator() {

            this.listaDominiosUtilizados = new List<string>();

            RuleFor(x => x.ConfiguracaoSistema.codigoOrganizacao)
                .Must((x, codigoOrganizacao) => !this.checkCodigoOrganizacao(x, codigoOrganizacao))
                .WithMessage("O código informado já está sendo utilizado por outra organização.");

            RuleFor(x => x.ConfiguracaoSistema.siglaOrganizacao)
                .Must((x, siglaOrganizacao) => !this.checkSiglaOrganizacao(x, siglaOrganizacao))
                .WithMessage("A sigla informada já está sendo utilizada por outra organização.");
            
            RuleFor(x => x.ConfiguracaoSistema.dominios)
                .Must((x, dominios) => !checkDominios(x, dominios))
                .WithMessage(x => string.Format("O domínio {0} já está sendo utilizado por outra organização.", this.listaDominiosUtilizados.FirstOrDefault()));

        }

        private bool checkCodigoOrganizacao(ConfiguracaoSistemaForm ViewModel, string codigoOrganizacao) {

            var idOrganizacao = User.idOrganizacao() > 0 ? User.idOrganizacao() : ViewModel.ConfiguracaoSistema.idOrganizacao;

            var listaConfiguracoesOrganizacoes = this.listaConfiguracoes.Where(x => x.idOrganizacao != idOrganizacao && x.flagExcluido == false && 
                                                                                    !x.codigoOrganizacao.isEmpty()).ToList();

            var existe = listaConfiguracoesOrganizacoes.Any(x => x.codigoOrganizacao.Equals(codigoOrganizacao));

            return existe;

        }

        private bool checkSiglaOrganizacao(ConfiguracaoSistemaForm ViewModel, string siglaOrganizacao) {

            var idOrganizacao = User.idOrganizacao() > 0 ? User.idOrganizacao() : ViewModel.ConfiguracaoSistema.idOrganizacao;

            var listaConfiguracoesOrganizacoes = this.listaConfiguracoes.Where(x => x.idOrganizacao != idOrganizacao && x.flagExcluido == false && 
                                                                                   !x.siglaOrganizacao.isEmpty()).ToList();

            var existe = listaConfiguracoesOrganizacoes.Any(x => x.siglaOrganizacao.Equals(siglaOrganizacao));

            return existe;

        }

        private bool checkDominios(ConfiguracaoSistemaForm ViewModel, string dominios) {

            if (dominios.isEmpty()) {
                return false;
            }

            this.listaDominiosUtilizados = new List<string>();

            var idOrganizacao = User.idOrganizacao() > 0 ? User.idOrganizacao() : ViewModel.ConfiguracaoSistema.idOrganizacao;

            var listaConfiguracoesOrganizacoes = this.listaConfiguracoes.Where(x => x.idOrganizacao != idOrganizacao && x.flagExcluido == false &&
                                                                                   !x.dominios.isEmpty()).ToList();

            string[] separadores = new string[] { "\r\n" };
            var listaDominios = dominios.Split(separadores, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (!listaDominios.Any()) {
                return false;
            }
            
            listaDominios.ForEach(x => {

                var existe = listaConfiguracoesOrganizacoes.Any(y => y.dominios.Contains(x));

                if (existe) {

                    this.listaDominiosUtilizados.Add(x);

                }

            });

            return this.listaDominiosUtilizados.Any();

        }

    }
}