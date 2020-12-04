using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Notificacoes;
using BLL.Services;
using DAL.Associados;
using DAL.Emails;
using DAL.Notificacoes;
using DAL.Contribuicoes;
using BLL.Core.Events;
using BLL.Notificacoes.Events;

namespace BLL.AssociadosNotificacoes.Services {

    public class AssociadoContribuicaoCobrancaNotificacaoBL : DefaultBL, IAssociadoContribuicaoCobrancaNotificacaoBL {

        // Atributos
        private INotificacaoSistemaCadastroBL _INotificacaoSistemaCadastroBL;

        // Propriedades
        private INotificacaoSistemaCadastroBL ONotificacaoSistemaCadastroBL => this._INotificacaoSistemaCadastroBL = this._INotificacaoSistemaCadastroBL ?? new NotificacaoSistemaCadastroBL();

        // Eventos
        private readonly EventAggregator onNotificacaoCadastrada = OnNotificacaoCadastrada.getInstance;

        //
        public bool registrarEmailsCobrancas(Contribuicao OContribuicao, List<int> idsAssociadoContribuicoes) {

            var ONotificacao = this.gerarNotificacao(OContribuicao);

            if (ONotificacao.id > 0) {

                this.vincularAssociados(ONotificacao, idsAssociadoContribuicoes);

                return true;

            }

            return false;

        }

        //
        private NotificacaoSistema gerarNotificacao(Contribuicao OContribuicao) {

            var ONotificacao = new NotificacaoSistema();

            ONotificacao.idOrganizacao = OContribuicao.idOrganizacao;
            ONotificacao.flagEmail = true;
            ONotificacao.flagTodosAssociados = false;
            ONotificacao.flagAssociadosEspecificos = true;
            ONotificacao.flagSistema = false;
            ONotificacao.flagMobile = false;

            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.COBRANCA_CONTRIBUICAO;
            ONotificacao.titulo = OContribuicao.emailCobrancaTitulo;
            ONotificacao.notificacao = OContribuicao.emailCobrancaHtml;

            this.ONotificacaoSistemaCadastroBL.salvar(ONotificacao);
            
            return ONotificacao;

        }

        //
        private void vincularAssociados(NotificacaoSistema ONotificacao, List<int> idsAssociadoContribuicoes) {

            var listaAssociadosContribuicao = db.AssociadoContribuicao
                                    .Include(x => x.Associado.Pessoa)
                                    .Include(x => x.Associado.Pessoa.listaEmails)
                                    .Where(x => idsAssociadoContribuicoes.Contains(x.id) && x.dtExclusao == null)
                                    .Select(x => new AssociadoDadosBasicos {
                                            id = x.id,
                                            nome = x.Associado.Pessoa.nome,
                                            emailPrincipal = x.Associado.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).FirstOrDefault().email,
                                            emailSecundario = x.Associado.Pessoa.listaEmails.Where(c => !c.email.Equals("") && c.dtExclusao == null).OrderByDescending(c => c.id).Skip(1).FirstOrDefault().email
                                    }).ToList();

            var listaNotificacoesVinculadas = new List<NotificacaoSistemaEnvio>();

            foreach (var OAssociadoContribuicao in listaAssociadosContribuicao) {
                
                var listaEmails = new List<string> { OAssociadoContribuicao.emailPrincipal, OAssociadoContribuicao.emailSecundario };

                foreach (var email in listaEmails) {
                    
                    var OEnvio = new NotificacaoSistemaEnvio();
                    
                    OEnvio.idOrganizacao = ONotificacao.idOrganizacao;
                    
                    OEnvio.idNotificacao = ONotificacao.id;
                    
                    OEnvio.idReferencia = OAssociadoContribuicao.id;
                    
                    OEnvio.nome = OAssociadoContribuicao.nome;
                    
                    OEnvio.email = email;
                    
                    if (!UtilValidation.isEmail(OEnvio.email)) {

                        OEnvio.flagExcluido = true;

                        OEnvio.motivoExclusao = "O e-mail configurado não é válido.";
                    }

                    listaNotificacoesVinculadas.Add(OEnvio);
                    
                }

            }

            using (var ctx = this.db) {

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
