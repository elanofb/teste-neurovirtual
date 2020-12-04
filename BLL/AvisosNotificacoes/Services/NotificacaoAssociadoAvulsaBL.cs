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
using DAL.Repository.Base;

namespace BLL.AvisosNotificacoes.Services {

    public class NotificacaoAssociadoAvulsaBL : DefaultBL, INotificacaoAssociadoAvulsaBL {

        // Atributos
        private INotificacaoSistemaCadastroBL _INotificacaoSistemaCadastroBL;

        // Propriedades
        private INotificacaoSistemaCadastroBL ONotificacaoSistemaCadastroBL => this._INotificacaoSistemaCadastroBL = this._INotificacaoSistemaCadastroBL ?? new NotificacaoSistemaCadastroBL();

        // Eventos
        private readonly EventAggregator onNotificacaoCadastrada = OnNotificacaoCadastrada.getInstance;

        //
        public bool salvar(NotificacaoSistema ONotificacao, List<NotificacaoSistemaEnvio> listaEnvios) {

            var ONotificacaoGerada = this.gerarNotificacao(ONotificacao);

            if (ONotificacaoGerada.id == 0) {
                return false;
            }

            if (listaEnvios.Any()) {
                this.vincularAssociados(ONotificacao, listaEnvios);    
            }

            return true;

        }

        //
        private NotificacaoSistema gerarNotificacao(NotificacaoSistema ONotificacao) {

            if (ONotificacao.flagMobile == true) {


            }
            
            ONotificacao.idTipoNotificacao = TipoNotificacaoConst.AVULSA;

            ONotificacao.flagSistema = false;

            ONotificacao.flagTodosUsuarios = false;

            ONotificacao.flagUsuariosEspecificos = false;

            this.ONotificacaoSistemaCadastroBL.salvar(ONotificacao);
            
            return ONotificacao;

        }

        //
        private void vincularAssociados(NotificacaoSistema ONotificacao,  List<NotificacaoSistemaEnvio> listaEnvios) {
            
            foreach (var OEnvio in listaEnvios) {
                OEnvio.idNotificacao = ONotificacao.id;
            }

            using (var ctx = new DataContext()) {

                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;
                
                listaEnvios.ForEach(x => {
                    x.setDefaultInsertValues();
                });

                ctx.NotificacaoSistemaEnvio.AddRange(listaEnvios);

                ctx.SaveChanges();
                
            }

            var listaParametros = new List<object>();
            listaParametros.Add(listaEnvios);
            listaParametros.Add(ONotificacao);
            
            this.onNotificacaoCadastrada.subscribe(new OnNotificacaoCadastradaHandler());

            this.onNotificacaoCadastrada.publish((listaParametros as object));

        }

    }
    
}
