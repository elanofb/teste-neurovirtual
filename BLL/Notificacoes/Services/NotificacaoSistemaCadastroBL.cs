using System;
using System.Collections.Generic;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaCadastroBL : DefaultBL, INotificacaoSistemaCadastroBL {

        /// <summary>
        /// Atualizar ou criar uma nova notificacao
        /// </summary>
        public bool salvar(NotificacaoSistema ONotificacaoSistema) {

            ONotificacaoSistema.titulo = ONotificacaoSistema.titulo.abreviar(100);

            if (!(ONotificacaoSistema.idTemplate > 0)) {
                ONotificacaoSistema.notificacao = ONotificacaoSistema.notificacao.abreviar(8000);
            }

            if (ONotificacaoSistema.id == 0) {

                return this.inserir(ONotificacaoSistema);

            }

            return this.atualizar(ONotificacaoSistema);
        }

        /// <summary>
        /// 
        /// </summary>
        public void salvarDetalhesNotificacao(List<NotificacaoSistemaEnvio> listaNotificacoesVinculadas){
            
            using(var ctx = this.db) {
                
                ctx.Configuration.AutoDetectChangesEnabled = false;

                ctx.Configuration.ValidateOnSaveEnabled = false;

                listaNotificacoesVinculadas.ForEach(x => {
                    x.setDefaultInsertValues();
                });

                ctx.NotificacaoSistemaEnvio.AddRange(listaNotificacoesVinculadas);

                ctx.SaveChanges();

            }
        }

        /// <summary>
        /// Persistir o objecto e salvar na base de dados
        /// </summary>
        private bool inserir(NotificacaoSistema ONotificacaoSistema) {

            ONotificacaoSistema.idUnidade = User.idUnidade();

            ONotificacaoSistema.setDefaultInsertValues();

            foreach (var ONotificacaoPessoa in ONotificacaoSistema.listaPessoa) {
                ONotificacaoPessoa.setDefaultInsertValues();
            }

            if (ONotificacaoSistema.idUnidade == 0) {
                ONotificacaoSistema.idUnidade = null;
            }

            db.NotificacaoSistema.Add(ONotificacaoSistema);

            db.SaveChanges();

            return (ONotificacaoSistema.id > 0);
        }

        /// <summary>
        /// Persistir o objecto e atualizar informações
        /// </summary>
        private bool atualizar(NotificacaoSistema ONotificacaoSistema) {

            NotificacaoSistema dbNotificacaoSistema = this.db.NotificacaoSistema.Find(ONotificacaoSistema.id);

            var tipoEntry = db.Entry(dbNotificacaoSistema);

            ONotificacaoSistema.setDefaultUpdateValues();

            if (ONotificacaoSistema.idUnidade == 0) {

                ONotificacaoSistema.idUnidade = null;

            }

            tipoEntry.CurrentValues.SetValues(ONotificacaoSistema);

            tipoEntry.ignoreFields();

            db.SaveChanges();

            return (ONotificacaoSistema.id > 0);
        }

    }
}
