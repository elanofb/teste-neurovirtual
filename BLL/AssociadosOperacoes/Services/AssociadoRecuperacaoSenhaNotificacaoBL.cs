using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosInstitucional.Emails;
using BLL.AssociadosOperacoes.Events;
using BLL.Configuracoes;
using BLL.Core.Events;
using BLL.Notificacoes;
using BLL.Notificacoes.Events;
using BLL.Services;
using DAL.Associados.DTO;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace BLL.AssociadosOperacoes {

    public class AssociadoRecuperacaoSenhaNotificacaoBL : DefaultBL, IAssociadoRecuperacaoSenhaNotificacaoBL {

        // Atributos
        private INotificacaoSistemaCadastroBL _INotificacaoSistemaCadastroBL;

        // Propriedades
        private INotificacaoSistemaCadastroBL ONotificacaoSistemaCadastroBL => this._INotificacaoSistemaCadastroBL = this._INotificacaoSistemaCadastroBL ?? new NotificacaoSistemaCadastroBL();

        // Eventos
        private readonly EventAggregator onNotificacaoCadastrada = OnNotificacaoCadastrada.getInstance;

        //
        public UtilRetorno  registrarEmails(List<ItemListaAssociado> listaAssociados) {
            
            if (listaAssociados.Any(x => x.ativo != "S" && x.ativo != "E")) {
                return UtilRetorno.newInstance(true, "Existem Associados desativados na lista, eles não podem receber o link de recuperação de senha. Tente novamente!");
            }

            var ONotificacao = this.gerarNotificacao();

            if(ONotificacao.id > 0) {

                this.vincularAssociados(ONotificacao, listaAssociados);

                return UtilRetorno.newInstance(false, "O link de reenvio de senha foi enviado para o(s) e-mail(s) do(s) associado(s), através dele será possível criar uma nova senha.");

            }
            
            return UtilRetorno.newInstance(true, "Não foi possível enviar o link de recuperação de senha para os associados.");

        }

        //
        private NotificacaoSistema gerarNotificacao() {

            var OConfigNotificacao = ConfiguracaoNotificacaoBL.getInstance.carregar(User.idOrganizacao());

            var OConfigSistema = ConfiguracaoSistemaBL.getInstance.carregar(User.idOrganizacao());

            var ONotificacao = new NotificacaoSistema();
            
            ONotificacao.flagEmail = true;

            ONotificacao.flagTodosAssociados = false;

            ONotificacao.flagAssociadosEspecificos = true;

            ONotificacao.flagSistema = false;

            ONotificacao.flagMobile = false;

            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.ASSOCIADO_RECUPERACAO_SENHA;
                
            ONotificacao.titulo = OConfigNotificacao.tituloEmailRecuperacaoSenhaAssociado.Replace("#NOME_ORGANIZACAO#", OConfigSistema.tituloSistema);

            ONotificacao.notificacao = OConfigNotificacao.corpoEmailRecuperacaoSenhaAssociado;

            this.ONotificacaoSistemaCadastroBL.salvar(ONotificacao);

            return ONotificacao;

        }

        //
        private void vincularAssociados(NotificacaoSistema ONotificacao, List<ItemListaAssociado> listaAssociados) {
            
            var listaNotificacoesVinculadas = new List<NotificacaoSistemaEnvio>();

            foreach(var OAssociado in listaAssociados) {
                
                var listaEmails = new List<string> { OAssociado.email, OAssociado.emailSecundario };

                foreach (var email in listaEmails) {
                    
                    var OEnvio = new NotificacaoSistemaEnvio();

                    OEnvio.idNotificacao = ONotificacao.id;

                    OEnvio.idReferencia = OAssociado.id;

                    OEnvio.nome = OAssociado.nome;

                    OEnvio.email = email;
                    
                    if (!UtilValidation.isEmail(OEnvio.email)) {

                        OEnvio.flagExcluido = true;

                        OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                    }

                    listaNotificacoesVinculadas.Add(OEnvio);   
                    
                }

            }

            using(var ctx = this.db) {

                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;

                listaNotificacoesVinculadas.ForEach(x => {
                    x.setDefaultInsertValues();
                });

                ctx.NotificacaoSistemaEnvio.AddRange(listaNotificacoesVinculadas);

                ctx.SaveChanges();

            }

            var listaParametros = new List<object>();

            listaParametros.Add(listaNotificacoesVinculadas);

            listaParametros.Add(ONotificacao);

            this.onNotificacaoCadastrada.subscribe(new OnNotificacaoCadastradaHandler());

            this.onNotificacaoCadastrada.publish((listaParametros as object));

        }

    }

}
