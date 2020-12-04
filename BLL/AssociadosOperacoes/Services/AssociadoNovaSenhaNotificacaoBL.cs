using System;
using System.Collections.Generic;
using System.Linq;
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
using EntityFramework.Extensions;

namespace BLL.AssociadosOperacoes {

    public class AssociadoNovaSenhaNotificacaoBL : DefaultBL, IAssociadoNovaSenhaNotificacaoBL {

        // Atributos
        private INotificacaoSistemaCadastroBL _INotificacaoSistemaCadastroBL;

        // Propriedades
        private INotificacaoSistemaCadastroBL ONotificacaoSistemaCadastroBL => this._INotificacaoSistemaCadastroBL = this._INotificacaoSistemaCadastroBL ?? new NotificacaoSistemaCadastroBL();

        // Eventos
        private readonly EventAggregator onNotificacaoCadastrada = OnNotificacaoCadastrada.getInstance;

        //
        public UtilRetorno registrarNovaSenha(List<ItemListaAssociado> listaAssociados, string senhaProvisoria) {

            if(listaAssociados.Any(x => x.ativo != "S" && x.ativo != "E")) {
                return UtilRetorno.newInstance(true, "Existem membros desativados na lista, eles não podem receber nova senha de acesso. Tente novamente!");
            }

            var idsPessoas = listaAssociados.Select(x => x.idPessoa).ToList();

            var senhaCrypt = UtilCrypt.SHA512(senhaProvisoria);

            db.Pessoa.Where(x => idsPessoas.Contains(x.id)).Update(x => new Pessoa { senha = senhaCrypt });

            return this.registrarEmails(listaAssociados, senhaProvisoria);

        }

        //
        private UtilRetorno registrarEmails(List<ItemListaAssociado> listaAssociados, string senhaProvisoria) {
            
            var ONotificacao = this.gerarNotificacao(senhaProvisoria);

            if(ONotificacao.id > 0) {

                this.vincularAssociados(ONotificacao, listaAssociados, senhaProvisoria);

                return UtilRetorno.newInstance(false, "A nova senha foi enviada para o(s) e-mail(s) do(s) membro(s) com sucesso.");

            }
            
            return UtilRetorno.newInstance(true, "Não foi possível enviar o link de recuperação de senha para os membros.");

        }

        //
        private NotificacaoSistema gerarNotificacao(string senhaProvisoria) {

            var ONotificacao = new NotificacaoSistema();
            
            ONotificacao.flagEmail = true;

            ONotificacao.flagTodosAssociados = false;

            ONotificacao.flagAssociadosEspecificos = true;

            ONotificacao.flagSistema = false;

            ONotificacao.flagMobile = false;

            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.ASSOCIADO_NOVA_SENHA;
                
            ONotificacao.titulo = "Sinctec Digital - Senha de Acesso";

            ONotificacao.notificacao = "Caro #NOME#, <br /><br /> Você está recebendo uma nova senha para acessar as plataformas digitais da SINCTEC.<br />" +
                                       "Estes dados são referentes à conta SINCTEC nº <strong>#NUMERO_CONTA#</strong>" +
                                       "Pedimos que não empreste ou informe seus dados.<br />" +
                                       "Você pode alterar sua senha de acesso através do site na área dos membros SINCTEC ou em nosso aplicativo.<br />" +
                                       "Sua nova senha está logo abaixo: <br /><br /><br />" +
                                       "<strong>#SENHA#</strong>";

            ONotificacao.notificacao = ONotificacao.notificacao.Replace("#SENHA#", senhaProvisoria);

            this.ONotificacaoSistemaCadastroBL.salvar(ONotificacao);

            return ONotificacao;

        }

        //
        private void vincularAssociados(NotificacaoSistema ONotificacao, List<ItemListaAssociado> listaAssociados, string senhaProvisoria) {
            
            var listaNotificacoesVinculadas = new List<NotificacaoSistemaEnvio>();

            foreach(var OAssociado in listaAssociados) {
                
                var listaEmails = new List<string> { OAssociado.email, OAssociado.emailSecundario };

                foreach (var email in listaEmails) {
                    
                    var OEnvio = new NotificacaoSistemaEnvio();

                    OEnvio.idNotificacao = ONotificacao.id;

                    OEnvio.idReferencia = OAssociado.id;

                    OEnvio.nroMembro = OAssociado.nroAssociado;
                    
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

            listaParametros.Add(senhaProvisoria);

            this.onNotificacaoCadastrada.subscribe(new OnNotificacaoCadastradaHandler());

            this.onNotificacaoCadastrada.subscribe(new OnNovaSenhaEnviadaHandler());

            this.onNotificacaoCadastrada.publish((listaParametros as object));

        }

    }

}
