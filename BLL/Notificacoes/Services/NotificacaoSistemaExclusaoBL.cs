using System;
using BLL.Core.Events;
using BLL.Notificacoes.Events;
using BLL.Services;
using DAL.Notificacoes;
using DAL.Permissao.Security.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaExclusaoBL : DefaultBL, INotificacaoSistemaExclusaoBL {

        // Atributos

        // Propriedades

        //Events
        private readonly EventAggregator onNotificacaoExcluida = OnNotificacaoExcluida.getInstance;

        /// <summary>
        /// Excluir um registro pelo ID
        /// </summary>
        public UtilRetorno excluir(int id) {

            NotificacaoSistema ONotificacaoSistema = this.db.NotificacaoSistema.Find(id);

            if (ONotificacaoSistema == null) {
                return UtilRetorno.newInstance(true, "O registro não foi localizado.");
            }

            ONotificacaoSistema.flagExcluido = true;

            ONotificacaoSistema.idUsuarioAlteracao = User.id();

            ONotificacaoSistema.dtAlteracao = DateTime.Now;

            db.SaveChanges();

            return UtilRetorno.newInstance(false, "O registro foi removido com sucesso.");
        }



    }
}
